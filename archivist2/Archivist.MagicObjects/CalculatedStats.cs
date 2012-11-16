using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;

namespace Archivist.MagicObjects
{
    public class CalculatedStats
    {
        private Card card;

        public CalculatedStats(Card card)
        {
            this.card = card;
        }

        public int CalculatedManaCost
        {
            get
			{
				if (card != null)
				{
					if (string.IsNullOrEmpty(card.ManaCost))
						return 0;

					string pattern = "\\([WURBGP//]*\\)"; // P = B/P
					Regex rgx = new Regex(pattern);
					string cleanMana = rgx.Replace(card.ManaCost, "M");
					cleanMana = cleanMana.Replace("X", ""); // X = 0

					int convertedCosts = 0;
					char[] symbols = new char[] { 'W', 'U', 'R', 'B', 'G', 'M' };
					foreach (char symb in symbols)
					{
						convertedCosts += cleanMana.Count(x => x == symb);
						cleanMana = cleanMana.Replace(symb.ToString(), "");
					}

					// Rest must be numeric
					if (!string.IsNullOrEmpty(cleanMana))
					{
						convertedCosts += Convert.ToInt32(cleanMana);
					}

					return convertedCosts;
				}

				return 0;
			}
        }

		public int CalculatedBasePower
		{
			get
			{
				if (card != null)
				{
					string[] data = card.PowTgh.Split('/');
					if (data.Count() > 1)
					{
						return Convert.ToInt32(data[0]);
					}
				}

				return 0;
			}
		}

		public int CalculatedBaseToughness
		{
			get
			{
				if (card != null)
				{
					string[] data = card.PowTgh.Split('/');
					if (data.Count() > 1)
					{
						return Convert.ToInt32(data[1]);
					}
				}

				return 0;
			}
		}
    }
}
