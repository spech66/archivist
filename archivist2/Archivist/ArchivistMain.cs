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
		private string dataDirectory;
		private string imageDirectory;
		private string decksDirectory;

        public ArchivistMain()
        {
            InitializeComponent();
			dataDirectory = Path.Combine(Application.StartupPath, "data");
			imageDirectory = Path.Combine(Application.StartupPath, "img");
			decksDirectory = Path.Combine(Application.StartupPath, "decks");
			
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
			UpdateDeckListGetDeckTree(decksDirectory);
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
				lbDeckManagerDeckList.Items.Add(file.Replace(decksDirectory, ""));
			}
		}
		
		public void UpdateLibraryList()
		{
			List<Archivist.MagicObjects.MagicCard> cards = new List<Archivist.MagicObjects.MagicCard>();

			ArchivistDatabase adb = new ArchivistDatabase();
			Archivist.MagicObjects.MagicCard mc = adb.GetCard("Brainbite") as Archivist.MagicObjects.MagicCard;
			if (mc != null)
			{
				cards.Add(mc);
			}

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

			OpenDeck(Path.Combine(decksDirectory, lbDeckManagerDeckList.SelectedItem.ToString()));
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
			deckPage.Controls.Add(deck);

			tabControl1.TabPages.Add(deckPage);
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
			//listBoxSearchExpansion.Items.Clear();
			if (listBoxSearchExpansion.SelectedIndex > 0)
			{
				/*SELECT * FROM CARD WHERE ID IN (
					SELECT CARD_ID FROM CARD_EXTENSION
					JOIN EXTENSION ON CARD_EXTENSION.EXTENSION_ID=EXTENSION.ID
					WHERE EXTENSION.NAME='Nemesis'
				)*/
				string list = "";
				foreach (string sel in listBoxSearchExpansion.SelectedItems)
				{
					list += "'" + sel + "', ";
				}
				list = list.Remove(list.Length - 2, 2);
				
				whereclause += " AND ID IN (SELECT CARD_ID FROM CARD_EXTENSION JOIN EXTENSION ON CARD_EXTENSION.EXTENSION_ID=EXTENSION.ID WHERE EXTENSION.NAME in ("+list+"))";
			}

			// Flavor text
			// TODO: :)

			if (!String.IsNullOrEmpty(whereclause))
			{
				whereclause = " WHERE 1=1 " + whereclause;
			}

			ArchivistDatabase adb = new ArchivistDatabase();
			List<Archivist.MagicObjects.Card> cards = adb.GetCards(whereclause, data.ToArray());
			dgCards.BindDatasource(cards, true);

			// Load image
			string noneimg = System.IO.Path.Combine(imageDirectory, "none.jpg");
			if (System.IO.File.Exists(noneimg))
			{
				pictureBoxCard.Image = Image.FromFile(noneimg);
			}
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
				DisplayImage(card.Multiverseid.ToString());

				linkLabelGatherer.Links.Clear();
				linkLabelGatherer.Links.Add(0, 20, "http://gatherer.wizards.com/Pages/Card/Details.aspx?multiverseid=" + card.Multiverseid.ToString());
			}
			else
			{
				pictureBoxCard.ImageLocation = "";
			}

			IDbCommand cmdEditon = DataBuider.database.CreateCommand();
			cmdEditon.Connection = DataBuider.database.CreateOpenConnection();
			IDbDataParameter p1Editon = cmdEditon.CreateParameter();
			cmdEditon.Parameters.Add(p1Editon);
			p1Editon.Value = card.Name;
			cmdEditon.CommandText = "SELECT RARITY, EXTENSION.NAME FROM CARD JOIN CARD_EXTENSION ON CARD_EXTENSION.CARD_ID = CARD.ID JOIN EXTENSION ON CARD_EXTENSION.EXTENSION_ID=EXTENSION.ID WHERE CARD.NAME = ?";
			IDataReader readerEditon = cmdEditon.ExecuteReader();
			listBoxCardEdition.Items.Clear();
			while (readerEditon.Read())
			{
				listBoxCardEdition.Items.Add(String.Format("{1} ({0})", readerEditon.GetString(0), readerEditon.GetString(1)));
			}
		}

		private void DisplayImage(string multiversid)
		{
			pictureBoxCard.ImageLocation = "";

			string cardimg = Path.Combine(Application.StartupPath, "cardimg");
			if (!Directory.Exists(cardimg))
			{
				Directory.CreateDirectory(cardimg);
			}

			string filename = Path.Combine(cardimg, multiversid + ".jpg");
			if (File.Exists(filename))
			{
				pictureBoxCard.ImageLocation = filename;
			}
			else
			{
				try
				{
					using (System.Net.WebClient client = new System.Net.WebClient())
					{
						string downloadUrl = String.Format("http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid={0}&type=card", multiversid);
						client.DownloadFile(downloadUrl, filename);
					}
					
					pictureBoxCard.ImageLocation = filename;
				}
				catch (Exception e)
				{
					MessageBox.Show(e.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
				}
			}
		}
	}
}