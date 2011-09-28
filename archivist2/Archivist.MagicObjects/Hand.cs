using System;
using System.Collections.Generic;
using Archivist.MagicObjects;

namespace Archivist.MagicObjects
{
    public class Hand
    {
        // Creates a list of cards
        protected List<Card> cards = new List<Card>();
        public int NumCards { get { return cards.Count; } }
        public List<Card> Cards { get { return cards; } }

        /// <summary>
        /// Checks to see if the hand contains a card of a certain face value
        /// </summary>
        /// <param name="item"></param>
        /// <returns></returns>
        public bool ContainsCard(string name)
        {
            foreach (Card c in cards)
            {
                if (c.Name == name)
                {
                    return true;
                }
            }
            return false;
        }
    }
}
