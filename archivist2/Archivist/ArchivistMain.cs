using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Linq;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.Common;
using Archivist.Data;
using System.IO;

namespace Archivist
{
    public partial class ArchivistMain : Form
	{
		private SortableBindingList<Archivist.MagicObjects.MagicCard> cardsLibrary = new SortableBindingList<Archivist.MagicObjects.MagicCard>();
		private string libraryFile;

        /// <summary>
        /// Constructor
        /// </summary>
        public ArchivistMain()
        {
            InitializeComponent();

			dgCards.SetGridType(CardDataGrid.GridType.Cards);
			dgLibrary.SetGridType(CardDataGrid.GridType.Library);

			InitSearch();
			UpdateCardList();
			UpdateLibraryList();
			UpdateDeckList();
        }

		#region Init
		private void InitSearch()
		{
            comboBoxSearchFormat.DataSource = TournamentFormats.Instance.Formats;
            comboBoxSearchFormat.DisplayMember = "GroupName";

			textBoxSearchName.Text = "";
			comboBoxSearchU.Items.Clear();
			comboBoxSearchB.Items.Clear();
			comboBoxSearchW.Items.Clear();
			comboBoxSearchR.Items.Clear();
			comboBoxSearchG.Items.Clear();
			textBoxSearchText.Text = "";
			textBoxSearchFlavor.Text = "";
			listBoxSearchExpansion.Items.Clear();
			listBoxSearchType.Items.Clear();

			string[] searchlist = new string[] { "May", "Must", "Must not" };
			comboBoxSearchU.Items.AddRange(searchlist); comboBoxSearchU.SelectedIndex = 0;
			comboBoxSearchB.Items.AddRange(searchlist); comboBoxSearchB.SelectedIndex = 0;
			comboBoxSearchW.Items.AddRange(searchlist); comboBoxSearchW.SelectedIndex = 0;
			comboBoxSearchR.Items.AddRange(searchlist); comboBoxSearchR.SelectedIndex = 0;
			comboBoxSearchG.Items.AddRange(searchlist); comboBoxSearchG.SelectedIndex = 0;

            InitSearchExpansionList();


            Database database = DatabaseCreatorFactory.CreateDatabase();
            IDbConnection connection = database.CreateConnection();
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
			listBoxSearchType.Items.Add("(All)"); listBoxSearchType.SelectedIndex = 0;
            IDbCommand cmd = database.CreateCommand();
            cmd.Connection = connection;
			cmd.CommandText = "SELECT distinct(TYPE) as TYPE FROM CARD ORDER BY TYPE";
			IDataReader reader = cmd.ExecuteReader();
			while (reader.Read())
			{
				listBoxSearchType.Items.Add(reader.GetString(0));
			}
			reader.Close();
		}

        private void InitSearchExpansionList()
        {
            listBoxSearchExpansion.Items.Clear();
            listBoxSearchExpansion.Items.Add("(All)"); listBoxSearchExpansion.SelectedIndex = 0;
            Database database = DatabaseCreatorFactory.CreateDatabase();
            IDbConnection connection = database.CreateConnection();
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }

            IDbCommand cmd = database.CreateCommand();
            cmd.Connection = connection;
            cmd.CommandText = "SELECT NAME FROM EXTENSION";

            if (comboBoxSearchFormat.SelectedIndex > 0)
            {
                TournamentFormat format = comboBoxSearchFormat.SelectedItem as TournamentFormat;

                if (format.Set.Count() > 0)
                {
                    string list = "";
                    foreach (string sel in format.Set)
                    {
                        list += "'" + sel + "', ";
                    }
                    list = list.Remove(list.Length - 2, 2);

                    cmd.CommandText += " WHERE NAME IN (" + list + ")";
                }
            }

            cmd.CommandText += " ORDER BY NAME";

            IDataReader reader = cmd.ExecuteReader();
            while (reader.Read())
                listBoxSearchExpansion.Items.Add(reader.GetString(0));
            reader.Close();
        }
		#endregion

		#region UpdateLists
        /// <summary>
        /// Add files and directories to the tree
        /// </summary>
		private void UpdateDeckList()
		{
            tvDeckManagerDeckList.Nodes.Clear();

			UpdateDeckListGetDeckTree(Helper.DecksDirectory);
            
            tvDeckManagerDeckList.ExpandAll();
		}

        /// <summary>
        /// Add files and directories to the tree
        /// </summary>
        /// <param name="dir">The path</param>
        /// <param name="parent">Parent nod to add items</param>
		private void UpdateDeckListGetDeckTree(string dir, TreeNode parent = null)
		{
            // Add directories
			string[] subdirs = Directory.GetDirectories(dir);
			foreach(string subdir in subdirs)
			{
                TreeNode dirNode = new TreeNode(Path.GetFileName(subdir), 0, 0);
                if (parent != null)
                {
                    parent.Nodes.Add(dirNode);
                }
                else
                {
                    tvDeckManagerDeckList.Nodes.Add(dirNode);
                }
				UpdateDeckListGetDeckTree(subdir, dirNode);
			}

            // Add files
			string[] files = Directory.GetFiles(dir);
			foreach (string file in files)
            {
                TreeNode dirNode = new TreeNode(Path.GetFileNameWithoutExtension(file), 1, 1);
                dirNode.Tag = file.Replace(Helper.DecksDirectory, "").Substring(1); // Remove leading / to not confuse Path.Combine
                if (parent != null)
                {
                    parent.Nodes.Add(dirNode);
                }
                else
                {
                    tvDeckManagerDeckList.Nodes.Add(dirNode);
                }
			}
		}
		
        /// <summary>
        /// Load cards from library
        /// </summary>
		public void UpdateLibraryList()
		{
			libraryFile = Path.Combine(Helper.DataDirectory, "Library.dat");

			bwUpdateLibrary.RunWorkerAsync(libraryFile);
		}
		#endregion

		#region Event handler
		private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox aboutBox = new AboutBox();
            aboutBox.ShowDialog();
		}

		private void button2_Click(object sender, EventArgs e)
		{
			InitSearch();
			UpdateCardList();
		}

		private void buttonSearch_Click(object sender, EventArgs e)
		{
			UpdateCardList();
		}

		private void textBoxSearchName_KeyDown(object sender, KeyEventArgs e)
		{
			if(e.KeyCode == Keys.Enter)
				UpdateCardList();
		}

		private void libraryToolStripMenuItem_Click(object sender, EventArgs e)
		{
			tabControl1.SelectedTab = tpLibrary;
		}

		private void cardSearchToolStripMenuItem_Click(object sender, EventArgs e)
		{
			tabControl1.SelectedTab = tpCardSearch;
		}

		private void deckToolStripMenuItem_Click(object sender, EventArgs e)
		{
			tabControl1.SelectedTab = tpDeckManager;
		}

		private void updateToolStripMenuItem_Click(object sender, EventArgs e)
		{
			UpdateDatabase ud = new UpdateDatabase();
			ud.ShowDialog();
			InitSearch();
			UpdateCardList();
		}

		private void dgCards_SelectionChanged(object sender, EventArgs e)
		{
			if (dgCards.SelectedRows.Count < 1)
				return;

			var list = ((SortableBindingList<Archivist.MagicObjects.Card>)dgCards.DataSource);
			Archivist.MagicObjects.Card card = list[dgCards.SelectedRows[0].Index];

			cardInfoCards.DataSource = card;
		}

		private void dgLibrary_SelectionChanged(object sender, EventArgs e)
		{
			if (dgLibrary.SelectedRows.Count < 1)
				return;

			Archivist.MagicObjects.Card card = cardsLibrary[dgLibrary.SelectedRows[0].Index];

			cardInfoLibrary.DataSource = card;
		}

        /// <summary>
        /// Select deck to open
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void tvDeckManagerDeckList_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag == null || !(e.Node.Tag is string))
                return;

            OpenDeck(Path.Combine(Helper.DecksDirectory, e.Node.Tag as string));
        }

		private void optionsToolStripMenuItem_Click(object sender, EventArgs e)
		{
			OptionDialog dlg = new OptionDialog();
			dlg.ShowDialog();
		}

		private void addToLibraryToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (dgCards.SelectedRows.Count < 1)
				return;

			var list = ((SortableBindingList<Archivist.MagicObjects.Card>)dgCards.DataSource);
			Archivist.MagicObjects.Card card = list[dgCards.SelectedRows[0].Index];

			Archivist.MagicObjects.MagicCard findCard = cardsLibrary.FirstOrDefault(sel => sel.Multiverseid == card.Multiverseid);
			if (findCard != null)
			{
				findCard.Amount++;
			}
			else
			{
				cardsLibrary.Add(card.Duplicate() as Archivist.MagicObjects.MagicCard);
			}
		}

		private void toolStripMenuItem3_Click(object sender, EventArgs e)
		{
			if (dgLibrary.SelectedRows.Count < 1)
				return;

			var list = ((SortableBindingList<Archivist.MagicObjects.MagicCard>)dgLibrary.DataSource);
			Archivist.MagicObjects.MagicCard card = list[dgLibrary.SelectedRows[0].Index];

			Archivist.MagicObjects.MagicCard findCard = cardsLibrary.FirstOrDefault(sel => sel.Multiverseid == card.Multiverseid);
			if (findCard != null)
			{
				cardsLibrary.Remove(findCard);
			}
		}

		private void ArchivistMain_FormClosed(object sender, FormClosedEventArgs e)
		{
			using (StreamWriter writer = new StreamWriter(libraryFile))
			{
				foreach(Archivist.MagicObjects.MagicCard card in cardsLibrary)
				{
					writer.WriteLine(String.Format("{0};{1}", card.Multiverseid, card.Amount));
				}
			}
		}

        private void ArchivistMain_FormClosing(object sender, FormClosingEventArgs e)
        {
            foreach (TabPage page in tabControl1.TabPages)
            {
                if (page.Controls[0] is Deck)
                {
                    Deck deck = (Deck)page.Controls[0];
                    if (deck.IsModified)
                    {
                        if (MessageBox.Show("There are unsaved changes. Would you really like to quit?", "Unsaved changes", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.No)
                        {
                            e.Cancel = true;
                            return;
                        }
                    }
                }
            }
        }

		private void DynamicAddCardToDeck_Click(object sender, EventArgs e)
		{
			ToolStripMenuItem item = (ToolStripMenuItem)sender;
			if (item == null)
				return;

			TabPage deckPage = (TabPage)item.Tag;
			if (deckPage == null)
				return;

			var list = ((SortableBindingList<Archivist.MagicObjects.Card>)dgCards.DataSource);
			Archivist.MagicObjects.Card card = list[dgCards.SelectedRows[0].Index];
			((Deck)deckPage.Controls[0]).AddCard(card);
        }

        private void newDeckToolStripMenuItem_Click(object sender, EventArgs e)
        {
            OpenDeck();
        }

        private void openDeckToolStripMenuItem_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog ofd = new OpenFileDialog())
            {
                ofd.Filter = "Decks (*.dec;*.txt)|*.dec;*.txt|All files (*.*)|*.*";
                ofd.RestoreDirectory = true;

                if (ofd.ShowDialog() == DialogResult.OK)
                {
                    OpenDeck(ofd.FileName);
                }
            }
        }

        private void tabControl1_Selected(object sender, TabControlEventArgs e)
        {
            if (e.TabPage.Controls[0] is Deck)
            {
                saveDeckToolStripMenuItem.Enabled = true;
                saveDeckAsToolStripMenuItem.Enabled = true;
                closeDeckToolStripMenuItem.Enabled = true;
                deckListToolStripMenuItem.Enabled = true;
                proxyDeckToolStripMenuItem.Enabled = true;

                tspSaveDeck.Enabled = true;
                tspCloseDeck.Enabled = true;
                tpPrintDeckList.Enabled = true;
                tpPrintProxyDeck.Enabled = true;
            }
            else
            {
                saveDeckToolStripMenuItem.Enabled = false;
                saveDeckAsToolStripMenuItem.Enabled = false;
                closeDeckToolStripMenuItem.Enabled = false;
                deckListToolStripMenuItem.Enabled = false;
                proxyDeckToolStripMenuItem.Enabled = false;

                tspSaveDeck.Enabled = false;
                tspCloseDeck.Enabled = false;
                tpPrintDeckList.Enabled = false;
                tpPrintProxyDeck.Enabled = false;
            }
        }

        private void saveDeckToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab.Controls[0] is Deck)
            {
                Deck deck = (Deck)tabControl1.SelectedTab.Controls[0];
                if (deck.SaveDeck())
                {
                    UpdateDeckList();
                }
            }
        }

        private void saveDeckAsToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab.Controls[0] is Deck)
            {
                Deck deck = (Deck)tabControl1.SelectedTab.Controls[0];
                if (deck.SaveDeck(true))
                {
                    UpdateDeckList();
                }
            }
        }

        private void saveAllToolStripMenuItem_Click(object sender, EventArgs e)
        {
            foreach (TabPage page in tabControl1.TabPages)
            {
                if (page.Controls[0] is Deck)
                {
                    Deck deck = (Deck)page.Controls[0];
                    deck.SaveDeck();
                }
            }
        }

        private void closeDeckToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab.Controls[0] is Deck)
            {
                Deck deck = (Deck)tabControl1.SelectedTab.Controls[0];
                if (deck.IsModified)
                {
                    if (MessageBox.Show("There are unsaved changes. Would you like to save before?", "Save changes", MessageBoxButtons.YesNo, MessageBoxIcon.Question) == System.Windows.Forms.DialogResult.Yes)
                    {
                        if (!deck.SaveDeck())
                        {
                            return;
                        }
                    }
                }

                RemoveDeck(tabControl1.SelectedTab);
            }
        }

        private void proxyDeckToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab.Controls[0] is Deck)
            {
                Deck deck = (Deck)tabControl1.SelectedTab.Controls[0];                
                PrintDeck.PrintProxyDeck(deck.GenerateProxyList());
            }
        }

        private void deckListToolStripMenuItem_Click(object sender, EventArgs e)
        {
            if (tabControl1.SelectedTab.Controls[0] is Deck)
            {
                Deck deck = (Deck)tabControl1.SelectedTab.Controls[0];
                PrintDeck.PrintDeckList(deck.GenerateDeckList());
            }
        }

		private void bwUpdateCardList_DoWork(object sender, DoWorkEventArgs e)
		{
			object[] args = e.Argument as object[];
			string whereclause = args[0] as string;
			List<object> data = args[1] as List<object>;

			ArchivistDatabase adb = new ArchivistDatabase();
			SortableBindingList<Archivist.MagicObjects.Card> cards = new SortableBindingList<MagicObjects.Card>(adb.GetCards(whereclause, data.ToArray()));
			e.Result = cards;
		}

		private void bwUpdateCardList_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			dgCards.BindDatasource(e.Result);

			groupBoxCards.Text = String.Format("Cards ({0})", (e.Result as SortableBindingList<Archivist.MagicObjects.Card>).Count);
		}

		private void bwUpdateLibrary_DoWork(object sender, DoWorkEventArgs e)
		{
			SortableBindingList<Archivist.MagicObjects.MagicCard> tempLib = new SortableBindingList<Archivist.MagicObjects.MagicCard>();

			if (File.Exists(libraryFile))
			{
				ArchivistDatabase adb = new ArchivistDatabase();

				using (StreamReader reader = new StreamReader(libraryFile))
				{
					while (!reader.EndOfStream)
					{
						string[] split = reader.ReadLine().Split(';');
						Archivist.MagicObjects.MagicCard card = adb.GetCard(Convert.ToInt32(split[0])) as Archivist.MagicObjects.MagicCard;
						if (card != null)
						{
							card.Amount = Convert.ToInt32(split[1]);
							tempLib.Add(card);
						}
					}
				}
			}

			e.Result = tempLib;
		}

		private void bwUpdateLibrary_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			cardsLibrary = e.Result as SortableBindingList<Archivist.MagicObjects.MagicCard>;

			dgLibrary.BindDatasource(cardsLibrary);
		}
        
        private void comboBoxSearchFormat_SelectedIndexChanged(object sender, EventArgs e)
        {
            InitSearchExpansionList();
        }

		#endregion

		#region Helper
		private void OpenDeck(string path = "")
		{
			Deck deck = new Deck(path);
			deck.Dock = DockStyle.Fill;
			
			TabPage deckPage = new TabPage("Loading...");
			tabControl1.TabPages.Add(deckPage);

			// Add controls after page added to tabControl to make layout update working
			deckPage.Controls.Add(deck);

			tabControl1.SelectedTab = deckPage;

			// Update Menu
			ToolStripMenuItem item = new ToolStripMenuItem("Add to " + deck.Title, null, DynamicAddCardToDeck_Click);
			item.Tag = deckPage;
			cmCards.Items.Add(item);

			SetDeckTitle(deckPage, deck.Title);
		}

		private void UpdateCardList()
        {
			dgCards.DataSource = null;
			
			string whereclause = "";
			List<object> data = new List<object>();

			// Card name
			if (textBoxSearchName.Text != "")
			{
				whereclause += " AND NAME LIKE ?";
				data.Add("%" + textBoxSearchName.Text + "%");
			}

			// Rule text
			if (textBoxSearchText.Text != "")
			{
				whereclause += " AND RULE LIKE ?";
				data.Add("%" + textBoxSearchText.Text + "%");
			}

			// Card cost
			object[] comboBoxes = { comboBoxSearchU, comboBoxSearchB, comboBoxSearchW, comboBoxSearchR, comboBoxSearchG };
			foreach (ComboBox cb in comboBoxes)
			{
				string color = cb.Tag.ToString();

				if (cb.Text == "Must")
				{
					whereclause += " AND COST LIKE ?";
					data.Add("%" + color + "%");
				}
				else if (cb.Text == "Must not")
				{
					whereclause += " AND COST NOT LIKE ?";
					data.Add("%" + color + "%");
				}
			}

			// Type
			if (listBoxSearchType.SelectedIndex > 0)
			{
				string list = "";
				foreach (string sel in listBoxSearchType.SelectedItems)
				{
					list += "'" + sel + "', ";
				}
				list = list.Remove(list.Length - 2, 2);

				whereclause += " AND TYPE IN (" + list + ")";
			}

			// Type text
			if (textBoxSearchType.Text != "")
			{
				whereclause += " AND TYPE LIKE ?";
				data.Add("%" + textBoxSearchType.Text + "%");
			}

			// Expansion
			if (listBoxSearchExpansion.SelectedIndex > 0)
			{
				string list = "";
				foreach (string sel in listBoxSearchExpansion.SelectedItems)
				{
					list += "'" + sel + "', ";
				}
				list = list.Remove(list.Length - 2, 2);

				whereclause += " AND EXTENSION IN (" + list + ")";
			}

			// Flavor text

            // Format
            if (comboBoxSearchFormat.SelectedIndex > 0)
            {
                TournamentFormat format = comboBoxSearchFormat.SelectedItem as TournamentFormat;

                // Set list
                if (format.Set.Count() > 0)
                {
                    string list = "";
                    foreach (string sel in format.Set)
                    {
                        list += "'" + sel + "', ";
                    }
                    list = list.Remove(list.Length - 2, 2);

                    whereclause += " AND EXTENSION IN (" + list + ")";
                }

                // Banned card list
                if (format.Banned.Count() > 0)
                {
                    string list = "";
                    foreach (string sel in format.Banned)
                    {
                        list += "'" + sel + "', ";
                    }
                    list = list.Remove(list.Length - 2, 2);

                    whereclause += " AND NAME NOT IN (" + list + ")";
                }
            }

            // Build the whereclause, 1=1 to allow every statement start with AND
			if (!String.IsNullOrEmpty(whereclause))
			{
				whereclause = " WHERE 1=1 " + whereclause;
			}

			bwUpdateCardList.RunWorkerAsync(new object[] { whereclause, data });
        }


		internal void RemoveDeck(TabPage tabPage)
		{
			tabControl1.TabPages.Remove(tabPage);

			ToolStripMenuItem menuItem = null;
			foreach (ToolStripMenuItem itm in cmCards.Items)
			{
				if (itm.Tag == tabPage)
				{
					menuItem = itm;
					break;
				}
			}

			if (menuItem != null)
			{
				cmCards.Items.Remove(menuItem);
			}
		}

		internal void SetDeckTitle(TabPage tabPage, string name)
		{
			tabPage.Text = name;

			// Update context menu name
			foreach (ToolStripMenuItem itm in cmCards.Items)
			{
				if (itm.Tag == tabPage)
				{
					itm.Text = "Add to " + name;
					break;
				}
			}
		}
		#endregion
	}
}
