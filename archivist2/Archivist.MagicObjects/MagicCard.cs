using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archivist.MagicObjects
{
    public class MagicCard : Card
    {
            // Objects for card information
            private readonly CardType[] cardTypes;
            private bool isCardUp;
            private string name;
            private string rule;
            private string manaCost;
            private string powTgh;
            private CalculatedStats calculatedStats;
            public string Rule { get { return rule; } set { rule = value;} }
            public string ManaCost { get { return manaCost; } set { manaCost = value; } }
            public string PowTgh { get {return powTgh;} set {powTgh = value; }}
            public string Name { get { return name; } set { name = value; } }
            public CardType[] CardTypes { get { return cardTypes; } }
            public bool IsCardUp { get { return isCardUp; } set { isCardUp = value; } }
            
            /// <summary>
            /// Constructor for a new card.  Assign the card a suit, face value, and if the card is facing up or down
            /// </summary>
            /// <param name="suit"></param>
            /// <param name="faceVal"></param>
            /// <param name="isCardUp"></param>
            public MagicCard(CardType[] cardTypes, bool isCardUp)
            {
                this.cardTypes = cardTypes;
                this.isCardUp = isCardUp;
                this.calculatedStats = new CalculatedStats(this);
            }

            public MagicCard(string name, CardType[] cardTypes, bool isCardUp)
            {
                this.name = name;
                this.cardTypes = cardTypes;
                this.isCardUp = isCardUp;
                this.calculatedStats = new CalculatedStats(this);
            }


            public int CalculatedManaCost
            {
                get { return calculatedStats.CalculatedManaCost;}
            }

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

    }   
}
