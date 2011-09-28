using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

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
            get { return 0; }
        }

        public int CalculatedBasePower { get {return 0;} }
        public int CalculatedBaseToughness { get { return 0; } }
    }
}
