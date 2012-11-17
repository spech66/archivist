using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archivist.MagicObjects
{
    public class MagicCard : Card
	{
        // Objects for card information
        private CalculatedStats calculatedStats;
		public string Rule { get; set; }
		public string ManaCost { get; set; }
		public string PowTgh { get; set; }
		public string Name { get; set; }
		public string Type { get; set; }
		public int Multiverseid { get; set; }
		public string Rarity { get; set; }
		public string Extension { get; set; }
		public bool IsCardUp { get; set; }
		public bool IsInSideboard { get; set; }

		public int Amount { get; set; }

        /// <summary>
        /// Constructor for a new card.
        /// </summary>
        /// <param name="suit"></param>
        /// <param name="faceVal"></param>
        /// <param name="isCardUp"></param>
        public MagicCard(bool isCardUp)
        {
			Rule = ManaCost = PowTgh = Name = Type = Rarity = Extension = String.Empty;

			this.IsCardUp = isCardUp;
            this.calculatedStats = new CalculatedStats(this);

        }

        public MagicCard(string name, bool isCardUp)
		{
			Rule = ManaCost = PowTgh = Name = Type = Rarity = Extension = String.Empty;

            this.Name = name;
			this.IsCardUp = isCardUp;
            this.calculatedStats = new CalculatedStats(this);
        }
        
        public int CalculatedManaCost { get { return calculatedStats.CalculatedManaCost; } }
        public Dictionary<char, int> CalculatedManaSymbols { get { return calculatedStats.CalculatedManaSymbols; } }
        public int CalculatedBasePower { get { return calculatedStats.CalculatedBasePower; } }
        public int CalculatedBaseToughness { get { return calculatedStats.CalculatedBaseToughness; } }

        /// <summary>
        /// Return the card as a string (i.e. "The Ace of Spades")
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            return this.Name;
        }

		public Card Duplicate(bool amountCopy = false)
		{
			MagicCard mc = new MagicCard(IsCardUp)
			{
				Rule = Rule,
				ManaCost = ManaCost,
				PowTgh = PowTgh,
				Name = Name,
				Type = Type,
				Multiverseid  = Multiverseid,
				Rarity = Rarity,
				Extension = Extension,
				IsCardUp = IsCardUp,
				IsInSideboard = IsInSideboard,
			};

			if (amountCopy)
				mc.Amount = Amount;
			else
				mc.Amount = 1;

			return mc;
		}
	}
}
