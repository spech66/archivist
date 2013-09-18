﻿using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Archivist.MagicObjects;
using Archivist.Data;

namespace Archivist
{
	public partial class CardInfo : UserControl
	{

		public CardInfo()
		{
			InitializeComponent();
		}

		#region DataBinding
		private object dataSource;
		[TypeConverter("System.Windows.Forms.Design.DataSourceConverter, System.Design")]
		[Category("Data")]
		[DefaultValue(null)]
		public object DataSource
		{
			get
			{
				return this.dataSource;
			}
			set
			{
				if (this.dataSource != value)
				{
					this.dataSource = value;
					BindDataSource();
				}
			}
		}
		#endregion

		private void BindDataSource()
		{
			if (dataSource == null || !(dataSource is Archivist.MagicObjects.Card))
			{

				return;
			}

			ShowCard();
		}

		private void linkLabelGatherer_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
		{
			System.Diagnostics.Process.Start(e.Link.LinkData.ToString());
		}

		private void listBoxCardEdition_SelectedIndexChanged(object sender, EventArgs e)
		{
			ListBoxItemNameId itm = (ListBoxItemNameId)listBoxCardEdition.SelectedItem;
			if (itm != null)
			{
				ShowImage(itm.Id);
			}
		}
		
		private void ShowCard()
		{
			Card card = dataSource as Card;

			textBoxCardName.Text = card.Name;
			//textBoxCostType.Text = reader.GetString(1);
			textBoxCardPowtgh.Text = card.PowTgh;
            ShowHtml(card);
			textBoxCardType.Text = card.Type;

			ShowImage(card.Multiverseid.ToString());

			// Select all extensions
			listBoxCardEdition.Items.Clear();

			IDbCommand cmdEditon = DataBuider.database.CreateCommand();
			cmdEditon.Connection = DataBuider.database.CreateOpenConnection();
			IDbDataParameter p1Editon = cmdEditon.CreateParameter();
			cmdEditon.Parameters.Add(p1Editon);

            byte[] bytes = System.Text.Encoding.UTF8.GetBytes(card.Name);
            p1Editon.Value = System.Text.Encoding.Default.GetString(bytes);

			cmdEditon.CommandText = "SELECT RARITY, EXTENSION, ID FROM CARD WHERE NAME = ?";
			
			IDataReader readerEditon = cmdEditon.ExecuteReader();
			while (readerEditon.Read())
			{
				string cardid = readerEditon.GetInt32(2).ToString();
				ListBoxItemNameId item = new ListBoxItemNameId(String.Format("{1} ({0})", readerEditon.GetString(0), readerEditon.GetString(1)), cardid);
				listBoxCardEdition.DisplayMember = "Name";
				listBoxCardEdition.ValueMember = "Id";
				listBoxCardEdition.Items.Add(item);

				if (card.Multiverseid.ToString() == cardid)
				{
					listBoxCardEdition.SelectedItem = item;
				}
			}
		}

        private void ShowHtml(Card card)
        {
            StringBuilder builder = new StringBuilder();
            builder.AppendLine("<style>body { margin: 0; font-family:'Microsoft Sans Serif', 'Arial',Times; font-size: 8pt; }</style>");
            builder.AppendLine(card.Rule);

            if (Archivist.Properties.Settings.Default.ShowIconsInRule)
            {
                string imgPath = Helper.ImageDirectory;
                string[] tokenList = { "0", "1", "2", "3", "4", "5", "6", "7", "8", "9", "10", "11", "12",
                                     "13", "14", "16", "T", "X", "W", "U", "R", "B", "G" };
                foreach (string token in tokenList)
                {
                    builder = builder.Replace("{" + token + "}", "<img src=\"file://" + System.IO.Path.Combine(imgPath, token + ".gif") + "\" height=\"15\" width=\"15\">");
                }
            }

            textBoxCardText.Navigate("about:blank");
            textBoxCardText.Document.OpenNew(false);
            textBoxCardText.Document.Write(builder.ToString());
            textBoxCardText.Refresh();
        }

		private void ShowImage(string id)
		{
			if (!String.IsNullOrEmpty(id))
			{
				Image cardImg = Helper.GetMagicImage(id);
				pictureBoxCard.Image = cardImg;

				// Scheme/Archenemy oversize card handling
				if (cardImg.Width < pictureBoxCard.Width && cardImg.Height < pictureBoxCard.Height)
				{
					pictureBoxCard.SizeMode = PictureBoxSizeMode.Normal;
				}
				else
				{
					pictureBoxCard.SizeMode = PictureBoxSizeMode.Zoom;
				}

				linkLabelGatherer.Links.Clear();
				linkLabelGatherer.Links.Add(0, 20, "http://gatherer.wizards.com/Pages/Card/Details.aspx?multiverseid=" + id);
			}
			else
			{
				pictureBoxCard.ImageLocation = "";
			}
		}
	}
}
