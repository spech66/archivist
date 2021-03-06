﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archivist.MagicObjects
{
    public interface Card
    {
        string Name { get;}
        bool IsCardUp { get; set;  }
        bool IsInSideboard { get; set; }
        string ManaCost { get; set; }
        int CalculatedManaCost { get; }
        Dictionary<char, int> CalculatedManaSymbols { get; }
        string PowTgh { get; set; }
        int CalculatedBasePower {get;}
        int CalculatedBaseToughness { get; }
		string Rule { get; }
		string Type { get; }
		string Rarity { get; }
		string Extension { get; }
		int Multiverseid { get; }

		int Amount { get; set; }

		Card Duplicate(bool amountCopy = false);
	}    
}
