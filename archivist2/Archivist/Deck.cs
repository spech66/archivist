using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Archivist.MagicObjects;
using System.IO;

namespace Archivist
{
	public partial class Deck : UserControl
	{
		private string deckFilename;
		List<Card> cards = new List<Card>();

		public Deck(string path = "")
		{
			InitializeComponent();

			if (!String.IsNullOrEmpty(path))
			{
				deckFilename = path;
			}
		}

		private void Deck_Load(object sender, EventArgs e)
		{
			if (!String.IsNullOrEmpty(deckFilename))
			{
				LoadDeck(deckFilename);
			}
		}

		public void LoadDeck(string path)
		{
			deckFilename = path;
			cards.Clear();

			//CHECK file exists

			if (path.ToLower().EndsWith(".dec"))
			{
				ParseFormatDEC(path, ref cards);
			}

			cardDataGrid1.BindDatasource(cards, false);
		}

		private void ParseFormatDEC(string path, ref List<Card> cards)
		{
			using (StreamReader sr = new StreamReader(path))
			{
				string line;
				Card card;

				while (!sr.EndOfStream)
				{
					line = sr.ReadLine().Trim();
					card = null;
					
					if (line.StartsWith("//")) // Comment, may contain "Name:"
					{
					}
					else if (line.StartsWith("SB:"))
					{
						card = GetCardFromText(line.Replace("SB:", "").Trim());
					}
					else
					{
						card = GetCardFromText(line);
					}

					if (card != null)
					{
						cards.Add(card);
					}
				}
			}
		}

		private Card GetCardFromText(string text)
		{
			int blank = text.IndexOf(" ");
			int amount;
			string name;

			if (blank > 0)
			{
				amount = Convert.ToInt32(text.Substring(0, blank).Trim());
				name = text.Substring(blank);
			}
			else
			{
				amount = 1;
				name = text.Trim();
			}

			MagicCard card = new MagicCard(true)
			{
				Name = name,
				Amount = amount
			};
			return card;
		}
	}
}
