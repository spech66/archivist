using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
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
        public ArchivistMain()
        {
            InitializeComponent();
			
			InitSearch();
			UpdateCardList();
			UpdateLibraryList();
			UpdateDeckList();
        }

		private void InitSearch()
		{
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

			listBoxSearchExpansion.Items.Add("(All)"); listBoxSearchExpansion.SelectedIndex = 0;
            Database database = DatabaseCreatorFactory.CreateDatabase();
			IDbConnection connection = database.CreateConnection();
			if (connection.State != ConnectionState.Open)
			{
				connection.Open();
			}

            IDbCommand cmd = database.CreateCommand();
            cmd.Connection = connection;
			cmd.CommandText = "SELECT NAME FROM EXTENSION ORDER BY NAME";
			IDataReader reader = cmd.ExecuteReader();
			while (reader.Read())
				listBoxSearchExpansion.Items.Add(reader.GetString(0));
			reader.Close();

			listBoxSearchType.Items.Add("(All)"); listBoxSearchType.SelectedIndex = 0;
			cmd.CommandText = "SELECT distinct(TYPE) as TYPE FROM CARD ORDER BY TYPE";
			reader = cmd.ExecuteReader();
			while (reader.Read())
			{
				listBoxSearchType.Items.Add(reader.GetString(0));
			}
			reader.Close();
		}

		private void UpdateDeckList()
		{
			UpdateDeckListGetDeckTree(Helper.DecksDirectory);
		}

		private void UpdateDeckListGetDeckTree(string dir)
		{
			string[] subdirs = Directory.GetDirectories(dir);
			foreach(string subdir in subdirs)
			{
				UpdateDeckListGetDeckTree(subdir);
			}

			string[] files = Directory.GetFiles(dir);
			foreach (string file in files)
			{
				lbDeckManagerDeckList.Items.Add(file.Replace(Helper.DecksDirectory, "").Substring(1));
			}
		}
		
		public void UpdateLibraryList()
		{
			List<Archivist.MagicObjects.MagicCard> cards = new List<Archivist.MagicObjects.MagicCard>();

			ArchivistDatabase adb = new ArchivistDatabase();
			// TEST
			Archivist.MagicObjects.MagicCard mc = adb.GetCard("Archivist") as Archivist.MagicObjects.MagicCard; if (mc != null) cards.Add(mc);
			Archivist.MagicObjects.MagicCard mc2 = adb.GetCard("Time Walk") as Archivist.MagicObjects.MagicCard; if (mc2 != null) cards.Add(mc2);
			Archivist.MagicObjects.MagicCard mc3 = adb.GetCard("Black Lotus") as Archivist.MagicObjects.MagicCard; if (mc3 != null) cards.Add(mc3);
			Archivist.MagicObjects.MagicCard mc4 = adb.GetCard("Fog") as Archivist.MagicObjects.MagicCard; if (mc4 != null) cards.Add(mc4);

			//dgLibrary.DataSource = cards;
			dgLibrary.BindDatasource(cards, false);
		}

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
			ShowCard();
		}

		private void linkLabelGatherer_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start(e.Link.LinkData.ToString());
		}

		private void btnDeckManagerNewDeck_Click(object sender, EventArgs e)
		{
			OpenDeck();
		}

		private void lbDeckManagerDeckList_DoubleClick(object sender, EventArgs e)
		{
			if (lbDeckManagerDeckList.SelectedItem == null)
				return;

			OpenDeck(Path.Combine(Helper.DecksDirectory, lbDeckManagerDeckList.SelectedItem.ToString()));
		}

		private void listBoxCardEdition_SelectedIndexChanged(object sender, EventArgs e)
		{
			//TODO: Select set specific multiverseid and display image
		}

		#endregion

		private void OpenDeck(string path = "")
		{
			Deck deck = new Deck(path);
			deck.Dock = DockStyle.Fill;

			string name = "Deck - New";
			if (path != "")
			{
				int pathIdx = path.LastIndexOf("\\");
				if (pathIdx > -1)
				{
					name = "Deck - " + path.Substring(pathIdx + 1);
				}
				else
				{
					name = "Deck - " + path;
				}
			}

			TabPage deckPage = new TabPage(name);
			tabControl1.TabPages.Add(deckPage);

			// Add controls after page added to tabControl to make layout update working
			deckPage.Controls.Add(deck);

			tabControl1.SelectedTab = deckPage;
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
				foreach(string sel in listBoxSearchType.SelectedItems)
				{
					list += "'" + sel + "', ";
				}
				list = list.Remove(list.Length - 2, 2);

				whereclause += " AND TYPE IN (" + list + ")";
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

			if (!String.IsNullOrEmpty(whereclause))
			{
				whereclause = " WHERE 1=1 " + whereclause;
			}

			ArchivistDatabase adb = new ArchivistDatabase();
			List<Archivist.MagicObjects.Card> cards = adb.GetCards(whereclause, data.ToArray());
			dgCards.BindDatasource(cards, true);

			// Load image
			pictureBoxCard.Image = Helper.GetMagicImage();
        }

		private void ShowCard()
		{
			if (dgCards.SelectedRows.Count < 1)
				return;

			var list = ((List<Archivist.MagicObjects.Card>)dgCards.DataSource);
			Archivist.MagicObjects.Card card = list[dgCards.SelectedRows[0].Index];

			textBoxCardName.Text = card.Name;
			//textBoxCostType.Text = reader.GetString(1);
			textBoxCardPowtgh.Text = card.PowTgh;
			textBoxCardText.Text = card.Rule;
			textBoxCardType.Text = card.Type;

			if (card.Multiverseid > 0)
			{
				pictureBoxCard.Image = Helper.GetMagicImage(card.Multiverseid.ToString(), true);

				linkLabelGatherer.Links.Clear();
				linkLabelGatherer.Links.Add(0, 20, "http://gatherer.wizards.com/Pages/Card/Details.aspx?multiverseid=" + card.Multiverseid.ToString());
			}
			else
			{
				pictureBoxCard.ImageLocation = "";
			}

			// Select all extensions
			IDbCommand cmdEditon = DataBuider.database.CreateCommand();
			cmdEditon.Connection = DataBuider.database.CreateOpenConnection();
			IDbDataParameter p1Editon = cmdEditon.CreateParameter();
			cmdEditon.Parameters.Add(p1Editon);
			p1Editon.Value = card.Name;
			cmdEditon.CommandText = "SELECT RARITY, EXTENSION FROM CARD WHERE NAME = ?";
			IDataReader readerEditon = cmdEditon.ExecuteReader();
			listBoxCardEdition.Items.Clear();
			while (readerEditon.Read())
			{
				listBoxCardEdition.Items.Add(String.Format("{1} ({0})", readerEditon.GetString(0), readerEditon.GetString(1)));
			}
		}
	}
}