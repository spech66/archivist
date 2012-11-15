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
		BindingList<Card> cards = new BindingList<Card>();

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

				UpdateGraphManaCurve();
				UpdateGraphDistribution();
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

		private void ParseFormatDEC(string path, ref BindingList<Card> cards)
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
				name = text.Substring(blank).Trim();
			}
			else
			{
				amount = 1;
				name = text.Trim();
			}

			ArchivistDatabase adb = new ArchivistDatabase();
			Card card = adb.GetCard(name);
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
			((TabControl)((TabPage)this.Parent).Parent).TabPages.Remove((TabPage)this.Parent);
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
						writer.WriteLine(String.Format("{0} {1}", card.Amount, card.Name));
					}
				}

				MessageBox.Show("Deck saved to file:\n" + deckFilename, "Saved", MessageBoxButtons.OK, MessageBoxIcon.Information);
			}
		}
	}
}
