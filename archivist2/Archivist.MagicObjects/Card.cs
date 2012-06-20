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
        string ManaCost { get; set; }
        int CalculatedManaCost { get;}
        string PowTgh { get; set; }
        int CalculatedBasePower {get;}
        int CalculatedBaseToughness { get; }
        string Rule { get; }
	}    
}
