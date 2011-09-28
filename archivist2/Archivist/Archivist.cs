using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Text;
using System.Windows.Forms;
using System.Data.Common;
using Archivist.Data;

namespace Archivist
{
    public partial class Archivist : Form
    {
		private string dataDirectory;
		private string imageDirectory;

        public Archivist()
        {
            InitializeComponent();
			dataDirectory = Application.StartupPath + "\\data";
			imageDirectory = Application.StartupPath + "\\img";
			//string userHomePath = Environment.GetFolderPath(Environment.SpecialFolder.Personal).ToString();
			//string appUserHomePath = System.IO.Path.Combine(userHomePath, "Archivist");



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

            IDbCommand cmd = database.CreateCommand();
            cmd.Connection = connection;			
            cmd.CommandText = "SELECT NAME FROM EXTENSIONS ORDER BY NAME";
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
			IDataReader reader = cmd.ExecuteReader();
			while (reader.Read())
				listBoxSearchExpansion.Items.Add(reader.GetString(0));
			reader.Close();

			listBoxSearchType.Items.Add("(All)"); listBoxSearchType.SelectedIndex = 0;
			/*cmd.CommandText = "SELECT distinct(TYPE) as TYPE FROM CARDS ORDER BY TYPE";
			reader = cmd.ExecuteReader();
			while (reader.Read())
				listBoxSearchType.Items.Add(reader.GetString(0));*/
		}

        private void exitToolStripMenuItem_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void aboutToolStripMenuItem_Click(object sender, EventArgs e)
        {
            AboutBox aboutBox = new AboutBox();
            aboutBox.ShowDialog();
        }

        private void toolStripButton3_Click(object sender, EventArgs e)
        {
            AboutBox aboutBox = new AboutBox();
            aboutBox.ShowDialog();
        }

        private void UpdateCardList()
        {
			listBox1.Items.Clear();
            Database database = DatabaseCreatorFactory.CreateDatabase();
            IDbConnection connection = database.CreateConnection();

            IDbCommand cmd = database.CreateCommand();
            cmd.Connection = connection;			
			string sqlcmd = "SELECT NAME FROM CARDS";
			if (textBoxSearchName.Text != "")
			{
				sqlcmd += " WHERE NAME LIKE ?";
				IDbDataParameter p1 = cmd.CreateParameter();
				cmd.Parameters.Add(p1);
				p1.Value = "%" + textBoxSearchName.Text + "%";
			}
			cmd.CommandText = sqlcmd;
            if (connection.State != ConnectionState.Open)
            {
                connection.Open();
            }
			IDataReader reader = cmd.ExecuteReader();
			while (reader.Read())
				listBox1.Items.Add(reader.GetString(0));

			string noneimg = System.IO.Path.Combine(imageDirectory, "none.jpg");
			if (System.IO.File.Exists(noneimg))
			{
				pictureBoxCard.Image = Image.FromFile(noneimg);
			}
        }

        private void updateToolStripMenuItem_Click(object sender, EventArgs e)
        {
            UpdateDatabase ud = new UpdateDatabase();
            ud.ShowDialog();
			InitSearch();
			UpdateCardList();
        }

		private void listBox1_SelectedIndexChanged(object sender, EventArgs e)
		{
            Database database = DatabaseCreatorFactory.CreateDatabase();
			IDbCommand cmd = database.CreateCommand();
            cmd.Connection = database.CreateOpenConnection();
			IDbDataParameter p1 = cmd.CreateParameter();
			cmd.Parameters.Add(p1);
			p1.Value = listBox1.Text;
			cmd.CommandText = "SELECT NAME, COST, POWTGH, RULE, TYPE, ID FROM CARDS WHERE NAME = ?";
			IDataReader reader = cmd.ExecuteReader();
			while (reader.Read())
			{
				textBoxCardName.Text = reader.GetString(0);
				//textBoxCostType.Text = reader.GetString(1);
				textBoxCardPowtgh.Text = reader.GetString(2);

				if (!reader.IsDBNull(3))
					textBoxCardText.Text = reader.GetString(3);
				else
					textBoxCardText.Text = "";

				if (!reader.IsDBNull(4))
					textBoxCardType.Text = reader.GetString(4);
				else
					textBoxCardType.Text = "";
				if(!reader.IsDBNull(5))
				{
					//pictureBoxCard.ImageLocation = "http://resources.wizards.com/Magic/Cards/4e/en-us/Card" + reader.GetValue(5) + ".jpg";
					pictureBoxCard.ImageLocation = "";
				} else
				{
					pictureBoxCard.ImageLocation = "";
				}
			}
				//("//edition");
				//listBoxCardEdition.Items.Add(entry);
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

        private void pictureBoxCard_Click(object sender, EventArgs e)
        {

        }
    }
}