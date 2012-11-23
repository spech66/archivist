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
using Archivist.Data;
using ZedGraph;

namespace Archivist
{
	public partial class Deck : UserControl
	{
		private string deckFilename;
		private SortableBindingList<Card> cards = new SortableBindingList<Card>();
		private bool modified;

		public bool IsModified
		{
			get { return modified; }
			private set
			{
				modified = value;

				TabPage tabPage = (TabPage)this.Parent;
				if (tabPage == null) return;
				TabControl tabControl = (TabControl)tabPage.Parent;
				ArchivistMain main = (ArchivistMain)tabControl.Parent;

				main.SetDeckTitle(tabPage, Title);
			}
		}

		public string Title { get { return String.Format("Deck - {0}{1}", String.IsNullOrEmpty(deckFilename) ? "New" : Path.GetFileNameWithoutExtension(deckFilename), (modified ? "*" : "")); } }

		public Deck(string path = "")
		{
			InitializeComponent();

			dgDeck.SetGridType(CardDataGrid.GridType.Deck);

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

		private void LoadDeck(string path)
		{
			deckFilename = path;
			cards.Clear();

			//CHECK file exists
			if (File.Exists(path))
			{

				if (path.ToLower().EndsWith(".dec") || path.ToLower().EndsWith(".txt"))
				{
					bwLoadDeck.RunWorkerAsync(path);
				}
			}
			else
			{
				MessageBox.Show("File does not exist:\n" + path, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
			}
		}

		/// <summary>
		/// DEC Format or Text files of form: Amount Name
		/// </summary>
		/// <param name="path"></param>
		/// <param name="cards"></param>
		private void ParseFormatDEC(string path, ref SortableBindingList<Card> cards)
		{
			using (StreamReader sr = new StreamReader(path))
			{
				string line;
				MagicCard card;

				while (!sr.EndOfStream)
				{
					line = sr.ReadLine().Trim();
					if (string.IsNullOrEmpty(line))
						continue;

					card = null;
					
					if (line.StartsWith("//")) // Comment, may contain "Name:"
					{
					}
					else if (line.StartsWith("SB:"))
					{
						card = GetCardFromText(line.Replace("SB:", "").Trim());
						card.IsInSideboard = true;
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

		private MagicCard GetCardFromText(string text)
		{
			int blank = text.IndexOf(" ");
			int amount;
			string name;

			if (blank > 0)
			{
				amount = Convert.ToInt32(text.Substring(0, blank).Trim());
				name = text.Substring(blank).Trim();
			}
			else
			{
				amount = 1;
				name = text.Trim();
			}

			ArchivistDatabase adb = new ArchivistDatabase();
			MagicCard card = adb.GetCard(name) as MagicCard;
			if (card == null)
			{
				card = new MagicCard(true)
				{
					Name = name,
					Amount = amount
				};
			}
			else
			{
				card.Amount = amount;
			}

			return card;
		}

		private void UpdateGraphManaCurve()
		{
			zgManaCurve.GraphPane.CurveList.Clear();
			GraphPane pane = zgManaCurve.GraphPane;

			int cardAmount = cards.Sum(sum => sum.Amount);
			pane.Title.Text = String.Format("Mana Curve - {0} Cards", cardAmount);

			PointPairList points = new PointPairList();
            points.AddRange(cards.Where(wh => !wh.IsInSideboard)
                .GroupBy(grp => grp.CalculatedManaCost)
				.OrderBy(ord => ord.Key) // Order x-Axis
				.Where(sel => sel.Key > 0) // Remove lands, and cards with 0 mana for scaling reasons
				.Select(sel => new PointPair(sel.Key, sel.Sum(sum => sum.Amount))));

			BarItem barChart = pane.AddBar("", points, Color.Blue);
			barChart.Bar.Fill.Type = FillType.Solid;

			pane.XAxis.Scale.Min = 0;
			pane.XAxis.Scale.MinorStep = 1;
			pane.XAxis.Title.IsVisible = false;

			pane.YAxis.Scale.Min = 0;
			pane.YAxis.Scale.MinorStep = 1;
			pane.YAxis.Title.IsVisible = false;

			zgManaCurve.AxisChange();
			//zgManaCurve.Invalidate();
			//zgManaCurve.Refresh();
		}

        private void UpdateGraphManaSymbols()
        {
            zgManaSymbols.GraphPane.CurveList.Clear();
            GraphPane pane = zgManaSymbols.GraphPane;
            pane.Legend.IsVisible = false;

			int cardAmount = cards.Sum(sum => sum.Amount);
			pane.Title.Text = String.Format("Mana Symbols - {0} Cards", cardAmount);
            
            Dictionary<char, int> calculatedManaSymbols = new Dictionary<char,int>();
            foreach (Card c in cards)
            {
                if (c.IsInSideboard)
                    continue;

                foreach (KeyValuePair<char, int> symb in c.CalculatedManaSymbols)
                {
                    if (calculatedManaSymbols.ContainsKey(symb.Key))
                    {
                        calculatedManaSymbols[symb.Key] += symb.Value * c.Amount;
                    }
                    else
                    {
                        calculatedManaSymbols.Add(symb.Key, symb.Value * c.Amount);
                    }
                }
            }

            foreach (var item in calculatedManaSymbols)
            {
                Color c = Color.Gold; // Other
                string name = "Multicolor and others";
                if (item.Key == 'W') { c = Color.White; name = "White"; }
                else if (item.Key == 'U') { c = Color.Blue; name = "Blue"; }
                else if (item.Key == 'R') { c = Color.Red; name = "Red"; }
                else if (item.Key == 'B') { c = Color.Black; name = "Black"; }
                else if (item.Key == 'G') { c = Color.DarkGreen; name = "Green"; }
                PieItem pi = pane.AddPieSlice(item.Value, c, 0, name + " - " + item.Value);
            }

            zgManaSymbols.AxisChange();
        }

		private void UpdateGraphDistribution()
		{
			// http://zedgraph.dariowiz.com/index77e8.html?title=Pie_Charts

			zgDistribution.GraphPane.CurveList.Clear();
            GraphPane pane = zgDistribution.GraphPane;
			pane.Legend.IsVisible = false;

			int cardAmount = cards.Sum(sum => sum.Amount);
			pane.Title.Text = String.Format("Distribution - {0} Cards", cardAmount);

			var qry = cards.Where(wh => !wh.IsInSideboard).GroupBy(grp => grp.Type.Substring(0, grp.Type.IndexOf('-') >= 0 ? grp.Type.IndexOf('-') : grp.Type.Length).Trim())
				.Select(sel => new KeyValuePair<string, int>(sel.Key, sel.Sum(sum => sum.Amount)));
			foreach (var item in qry)
			{
				Color c = Color.Pink; // Other
				if (item.Key == "Creature") c = Color.DarkRed;
				else if (item.Key == "Basic Land") c = Color.Goldenrod;
				else if (item.Key == "Land") c = Color.Goldenrod;
				else if (item.Key == "Artifact Land") c = Color.Goldenrod;
				else if (item.Key == "Enchantment") c = Color.Green;
				else if (item.Key == "Instant") c = Color.Navy;
				else if (item.Key == "Sorcery") c = Color.DeepSkyBlue;
				else if (item.Key == "Artifact") c = Color.LightGray;
                PieItem pi = pane.AddPieSlice(item.Value, c, 0, item.Key + " - " + item.Value);
			}

			zgDistribution.AxisChange();
		}
        
		public bool SaveDeck(bool saveas = false)
		{
			if (String.IsNullOrEmpty(deckFilename) || saveas)
			{
				using (SaveFileDialog sfd = new SaveFileDialog())
				{
					sfd.Filter = "Decks (*.dec)|*.dec|All files (*.*)|*.*";
                    sfd.InitialDirectory = Helper.DecksDirectory;
					sfd.RestoreDirectory = true;

					if (sfd.ShowDialog() == DialogResult.OK)
					{
						deckFilename = sfd.FileName;
					}
				}
			}

			if (!String.IsNullOrEmpty(deckFilename))
			{
				using (StreamWriter writer = new StreamWriter(deckFilename))
				{
					foreach (MagicCard card in cards)
					{
						if (card.IsInSideboard)
						{
							writer.WriteLine(String.Format("SB: {0} {1}", card.Amount, card.Name));
						}
						else
						{
							writer.WriteLine(String.Format("{0} {1}", card.Amount, card.Name));
						}
					}
				}

				IsModified = false;
				MessageBox.Show("Deck saved to file:\n" + deckFilename, "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);

                return true;
			}

            return false;
		}

		public void AddCard(Card card)
		{
			if (card == null)
				return;

			Card findCard = cards.FirstOrDefault(sel => sel.Multiverseid == card.Multiverseid);
			if (findCard != null)
			{
				findCard.Amount++;
			}
			else
			{
				cards.Add(card.Duplicate());
			}

			IsModified = true;

			dgDeck.BindDatasource(cards);
			
			UpdateAll();
		}

		private void removeToolStripMenuItem_Click(object sender, EventArgs e)
		{
			if (dgDeck.SelectedRows.Count < 1)
				return;

			Card findCard = cards[dgDeck.SelectedRows[0].Index];
			if (findCard != null)
			{
				cards.Remove(findCard);

				IsModified = true;

				UpdateAll();
			}
		}

		private void dgDeck_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			if (dgDeck.Columns[e.ColumnIndex].Name.Contains("Amount")
                || dgDeck.Columns[e.ColumnIndex].Name.Contains("Sideboard"))
			{
				IsModified = true;

				int cardAmount = cards.Sum(sum => sum.Amount);
				tpCards.Text = String.Format("Cards ({0})", cardAmount);

				UpdateAll();
            }
		}

        private void btnDrawMulligan_Click(object sender, EventArgs e)
        {
            if (lbDrawStartingHand.Items.Count > 1)
            {
                GenerateDraw(lbDrawStartingHand.Items.Count - 1);
            }
            else
            {
                GenerateDraw(6);
            }
        }

        private void btnDrawNewHand_Click(object sender, EventArgs e)
        {
            GenerateDraw(7);
        }

        private void GenerateDraw(int drawCards)
        {
            List<ListBoxItemNameId> deckList = new List<ListBoxItemNameId>();
            foreach (Card c in cards)
            {
                for (int i = 0; i < c.Amount; i++)
                {
                    deckList.Add(new ListBoxItemNameId(c.Name, c.Multiverseid.ToString()));
                }
            }

            if (deckList.Count < 8)
            {
                lbDrawLibrary.Items.Add("Not enough cards!");
                lbDrawStartingHand.Items.Add("Not enough cards!");
                return;
            }

            deckList.Shuffle();
            List<ListBoxItemNameId> handList = deckList.Take(drawCards).ToList();
            deckList.RemoveRange(0, drawCards);

            lbDrawLibrary.DisplayMember = "Name";
            lbDrawLibrary.ValueMember = "Id";
            lbDrawLibrary.DataSource = deckList;

            lbDrawStartingHand.DisplayMember = "Name";
            lbDrawStartingHand.ValueMember = "Id";
            lbDrawStartingHand.DataSource = handList;
        }

        private void lbDrawStartingHand_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBoxItemNameId itm = (ListBoxItemNameId)lbDrawStartingHand.SelectedItem;
            if (itm != null)
            {
                pbDrawImage.Image = Helper.GetMagicImage(itm.Id);
            }
        }

        private void lbDrawLibrary_SelectedIndexChanged(object sender, EventArgs e)
        {
            ListBoxItemNameId itm = (ListBoxItemNameId)lbDrawLibrary.SelectedItem;
            if (itm != null)
            {
                pbDrawImage.Image = Helper.GetMagicImage(itm.Id);
            }
        }

        internal List<int> GenerateProxyList()
        {
            //return cards.Select(sel => new KeyValuePair<int, int>(sel.Multiverseid, sel.Amount)).ToDictionary(t => t.Key, t => t.Value);
            List<int> ids = new List<int>();
            foreach (Card c in cards)
            {
                for (int i = 0; i < c.Amount; i++)
                {
                    ids.Add(c.Multiverseid);
                }
            }
            return ids;
        }

        internal List<Card> GenerateDeckList()
        {
            return cards.ToList();
        }

		private void bwLoadDeck_DoWork(object sender, DoWorkEventArgs e)
		{
			SortableBindingList<Card> tempList = new SortableBindingList<Card>();

			ParseFormatDEC(e.Argument.ToString(), ref tempList);

			e.Result = tempList;
		}

		private void bwLoadDeck_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
		{
			cards = e.Result as SortableBindingList<Card>;

			dgDeck.BindDatasource(cards);

			UpdateAll();
		}

		private void UpdateAll()
		{
			UpdateGraphManaCurve();
			UpdateGraphManaSymbols();
			UpdateGraphDistribution();

			int cardAmount = cards.Sum(sum => sum.Amount);
			tpCards.Text = String.Format("Cards ({0})", cardAmount);
		}

		private void dgDeck_SelectionChanged(object sender, EventArgs e)
		{
			if (dgDeck.SelectedRows.Count < 1)
				return;

			Archivist.MagicObjects.Card card = cards[dgDeck.SelectedRows[0].Index];

			cardInfoDeck.DataSource = card;
		}
    }
}
