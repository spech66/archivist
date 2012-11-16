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
		private BindingList<Card> cards = new BindingList<Card>();
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

				UpdateGraphManaCurve();
				UpdateGraphDistribution();
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
					ParseFormatDEC(path, ref cards);
				}

				dgDeck.BindDatasource(cards);
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
		private void ParseFormatDEC(string path, ref BindingList<Card> cards)
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
			pane.Title.Text = "Mana Curve";

			PointPairList points = new PointPairList();
			points.AddRange(cards.GroupBy(grp => grp.CalculatedManaCost)
				.OrderBy(ord => ord.Key) // Order x-Axis
				.Where(sel => sel.Key > 0) // Remove lands, and cards with 0 mana for scaling reasons
				.Select(sel => new PointPair(sel.Key, sel.Sum(sum => sum.Amount))));

			BarItem barChart = pane.AddBar("", points, Color.Blue);
			barChart.Bar.Fill.Type = FillType.Solid;

			pane.XAxis.Scale.Min = 0;
			pane.XAxis.Title.IsVisible = false;

			pane.YAxis.Scale.Min = 0;
			pane.YAxis.Title.IsVisible = false;

			zgManaCurve.AxisChange();
			//zgManaCurve.Invalidate();
			//zgManaCurve.Refresh();
		}

		private void UpdateGraphDistribution()
		{
			// http://zedgraph.dariowiz.com/index77e8.html?title=Pie_Charts

			zgDistribution.GraphPane.CurveList.Clear();
			GraphPane pane = zgDistribution.GraphPane;
			pane.Title.Text = "Distribution";

			var qry = cards.GroupBy(grp => grp.Type.Substring(0, grp.Type.IndexOf('-') >= 0 ? grp.Type.IndexOf('-') : grp.Type.Length).Trim())
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
				pane.AddPieSlice(item.Value, c, 0, item.Key);
			}

			zgDistribution.AxisChange();
		}

		private void btnClose_Click(object sender, EventArgs e)
		{
			TabPage tabPage = (TabPage)this.Parent;
			TabControl tabControl = (TabControl)tabPage.Parent;
			ArchivistMain main = (ArchivistMain)tabControl.Parent;
			main.RemoveDeck(tabPage);
		}

		private void btnSave_Click(object sender, EventArgs e)
		{
			SaveDeck();
		}

		private void btnSaveAs_Click(object sender, EventArgs e)
		{
			SaveDeck(true);
		}

		private void SaveDeck(bool saveas = false)
		{
			if (String.IsNullOrEmpty(deckFilename) || saveas)
			{
				using (SaveFileDialog sfd = new SaveFileDialog())
				{
					sfd.Filter = "Decks (*.dec)|*.dec|All files (*.*)|*.*";
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
			}
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
			
			UpdateGraphManaCurve();
			UpdateGraphDistribution();
			dgDeck.BindDatasource(cards);
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

				UpdateGraphManaCurve();
				UpdateGraphDistribution();
			}
		}

		private void dgDeck_CellValueChanged(object sender, DataGridViewCellEventArgs e)
		{
			if (dgDeck.Columns[e.ColumnIndex].Name.Contains("Amount"))
			{
				IsModified = true;

				UpdateGraphManaCurve();
				UpdateGraphDistribution();
			}
		}
	}
}
