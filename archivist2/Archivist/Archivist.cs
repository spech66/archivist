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
    public partial class Archivist : Form
    {
		private string dataDirectory;
		private string imageDirectory;

        public Archivist()
        {
            InitializeComponent();
			dataDirectory = Path.Combine(Application.StartupPath, "data");
			imageDirectory = Path.Combine(Application.StartupPath, "img");
			
			InitSearch();
			UpdateCardList();
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

		}

		private void deckToolStripMenuItem_Click(object sender, EventArgs e)
		{

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

		#endregion

		private void UpdateCardList()
        {
			dgCards.Rows.Clear();

            Database database = DatabaseCreatorFactory.CreateDatabase();
            IDbConnection connection = database.CreateConnection();

            IDbCommand cmd = database.CreateCommand();
            cmd.Connection = connection;			
			string sqlcmd = "SELECT NAME, COST FROM CARD";
			string whereclause = "";

			// Card name
			if (textBoxSearchName.Text != "")
			{
				whereclause += " AND NAME LIKE ?";
				IDbDataParameter p1 = cmd.CreateParameter();
				cmd.Parameters.Add(p1);
				p1.Value = "%" + textBoxSearchName.Text + "%";
			}

			// Rule text
			if (textBoxSearchText.Text != "")
			{
				whereclause += " AND RULE LIKE ?";
				IDbDataParameter p1 = cmd.CreateParameter();
				cmd.Parameters.Add(p1);
				p1.Value = "%" + textBoxSearchText.Text + "%";
			}

			// Card cost
			object[] comboBoxes = { comboBoxSearchU, comboBoxSearchB, comboBoxSearchW, comboBoxSearchR, comboBoxSearchG };
			foreach (ComboBox cb in comboBoxes)
			{
				string color = cb.Tag.ToString();

				if (cb.Text == "Must")
				{
					whereclause += " AND COST LIKE ?";
					IDbDataParameter p1 = cmd.CreateParameter();
					cmd.Parameters.Add(p1);
					p1.Value = "%" + color + "%";
				}
				else if (cb.Text == "Must not")
				{
					whereclause += " AND COST NOT LIKE ?";
					IDbDataParameter p1 = cmd.CreateParameter();
					cmd.Parameters.Add(p1);
					p1.Value = "%" + color + "%";
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
				sqlcmd += " WHERE 1=1 " + whereclause;
			}

			cmd.CommandText = sqlcmd;
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
			IDataReader reader = cmd.ExecuteReader();
			while (reader.Read())
			{
				//listBox1.Items.Add(reader.GetString(0));

				dgCards.Rows.Add(reader.GetString(0), reader.GetString(1));
			}

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

			string cardName = dgCards.SelectedRows[0].Cells[0].Value.ToString();

			Database database = DatabaseCreatorFactory.CreateDatabase();

			IDbCommand cmd = database.CreateCommand();
			cmd.Connection = database.CreateOpenConnection();
			IDbDataParameter p1 = cmd.CreateParameter();
			cmd.Parameters.Add(p1);
			p1.Value = cardName;
			cmd.CommandText = "SELECT NAME, COST, POWTGH, RULE, TYPE, MULTIVERSEID FROM CARD WHERE NAME = ?";
			IDataReader reader = cmd.ExecuteReader();
			while (reader.Read())
			{
				textBoxCardName.Text = reader.GetString(0);
				//textBoxCostType.Text = reader.GetString(1);
				textBoxCardPowtgh.Text = reader.GetString(2);

				if (!reader.IsDBNull(3))
					textBoxCardText.Text = reader.GetString(3).Replace("\n", "\r\n");
				else
					textBoxCardText.Text = "";

				if (!reader.IsDBNull(4))
					textBoxCardType.Text = reader.GetString(4);
				else
					textBoxCardType.Text = "";

				if (!reader.IsDBNull(5))
				{
					DisplayImage(reader.GetValue(5).ToString());

					linkLabelGatherer.Links.Clear();
					linkLabelGatherer.Links.Add(0, 20, "http://gatherer.wizards.com/Pages/Card/Details.aspx?multiverseid=" + reader.GetValue(5));
				}
				else
				{
					pictureBoxCard.ImageLocation = "";
				}
			}

			IDbCommand cmdEditon = database.CreateCommand();
			cmdEditon.Connection = database.CreateOpenConnection();
			IDbDataParameter p1Editon = cmdEditon.CreateParameter();
			cmdEditon.Parameters.Add(p1Editon);
			p1Editon.Value = cardName;
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

		private void dgCards_CellFormatting(object sender, DataGridViewCellFormattingEventArgs e)
		{
			string costs = dgCards.Rows[e.RowIndex].Cells[1].Value.ToString();
			int colorCount = 0;
			string color = "";

			string[] colors = { "U", "B", "W", "R", "G" };
			foreach(string c in colors)
			{
				if (costs.Contains(c))
				{
					colorCount++;
					color = c;
				}
			}

			if (colorCount == 0) // Artifacts and lands, ...
			{
				dgCards.Rows[e.RowIndex].Cells[1].Style.BackColor = Color.Gray;
			}
			else if (colorCount == 1) // Mono color cards
			{
				if (color == "U")
				{
					dgCards.Rows[e.RowIndex].Cells[1].Style.BackColor = Color.Blue;
					dgCards.Rows[e.RowIndex].Cells[1].Style.ForeColor = Color.White;
				}
				else if (color == "W")
				{
					dgCards.Rows[e.RowIndex].Cells[1].Style.BackColor = Color.White;
				}
				else if (color == "R")
				{
					dgCards.Rows[e.RowIndex].Cells[1].Style.BackColor = Color.Red;
				}
				else if (color == "G")
				{
					dgCards.Rows[e.RowIndex].Cells[1].Style.BackColor = Color.DarkGreen;
					dgCards.Rows[e.RowIndex].Cells[1].Style.ForeColor = Color.White;
				}
				else if (color == "B")
				{
					dgCards.Rows[e.RowIndex].Cells[1].Style.BackColor = Color.Black;
					dgCards.Rows[e.RowIndex].Cells[1].Style.ForeColor = Color.White;
				}
			}
			else // Multicolor cards
			{
				dgCards.Rows[e.RowIndex].Cells[1].Style.BackColor = Color.Gold;
			}				
		}
	}
}