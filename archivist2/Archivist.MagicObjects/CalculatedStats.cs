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
        private int calculatedManaCost = -1;
        private Dictionary<char, int> calculatedManaSymbols;

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

                    // Only calculate once per card!
                    if (calculatedManaCost != -1)
                        return calculatedManaCost;

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

                    calculatedManaCost = convertedCosts;
					return convertedCosts;
				}

				return 0;
			}
        }

        public Dictionary<char, int> CalculatedManaSymbols
        {
            get
            {
                if (card != null)
                {
                    if (string.IsNullOrEmpty(card.ManaCost))
                        return new Dictionary<char, int>();

                    // Only calculate once per card!
                    if (calculatedManaSymbols != null)
                        return calculatedManaSymbols;

                    calculatedManaSymbols = new Dictionary<char, int>();

                    string pattern = "\\([WURBGP//]*\\)"; // P = B/P
                    Regex rgx = new Regex(pattern);
                    string cleanMana = rgx.Replace(card.ManaCost, "M");
                    cleanMana = cleanMana.Replace("X", ""); // X = 0

                    char[] symbols = new char[] { 'W', 'U', 'R', 'B', 'G', 'M' };
                    foreach (char symb in cleanMana)
                    {
                        if(!symbols.Contains(symb))
                            continue;

                        if (calculatedManaSymbols.ContainsKey(symb))
                        {
                            calculatedManaSymbols[symb]++;
                        }
                        else
                        {
                            calculatedManaSymbols.Add(symb, 1);
                        }
                    }

                    return calculatedManaSymbols;
                }

                return new Dictionary<char, int>();
            }
        }

		public int CalculatedBasePower
		{
			get
			{
				if (card != null && !String.IsNullOrEmpty(card.PowTgh))
				{
					string[] data = card.PowTgh.Replace("(", "").Replace(")", "").Split('/');
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
				if (card != null && !String.IsNullOrEmpty(card.PowTgh))
				{
					string[] data = card.PowTgh.Replace("(", "").Replace(")", "").Split('/');
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
