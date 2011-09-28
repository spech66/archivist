using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archivist.MagicObjects
{
    /// <summary>
    /// This class is game-specific.  Creates a BlackJack hand that inherits from class hand
    /// </summary>
    public class MagicHand : Hand
    {
        /// <summary>
        /// This method compares two BlackJack hands
        /// </summary>
        /// <param name="otherHand"></param>
        /// <returns></returns>
        public int CompareFaceValue(object otherHand)
        {
            MagicHand aHand = otherHand as MagicHand;
            if (aHand != null)
            {
                return this.GetSumOfHand().CompareTo(aHand.GetSumOfHand());
            }
            else
            {
                throw new ArgumentException("Argument is not a Hand");
            }
        }

        /// <summary>
        /// Gets the total ManaCost value of a hand 
        /// </summary>
        /// <returns>int</returns>
        public int GetSumOfHand()
        {
            int val = 0;

            foreach (Card c in cards)
            {
                val += (int)c.CalculatedManaCost;
            }

            return val;
        }
    }
}
