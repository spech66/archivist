using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archivist.MagicObjects
{

        
    /// <summary>
    /// Card suit values
    /// </summary>
    public enum CardType
    {
        Wall, Aura, Angel, Kithkin, Scout, Bear, AssemblyWorker, Skeleton, Plainswalker, Avatar, Creature, Land, LegendaryLand, Instant, Sorcery, Enchantment, Artifact, Token, Legendary, Tribal, Sliver, Human, Druid, Gargoyle, Knight, Soldier, Wizard, Rebel, Assassin, Cat, Cleric, Illusion, Goblin, Warrior, Shapeshifter, Serpent, Dragon, Homonculous
    }

    public interface Card
    {
        // Objects for card information
        string Name { get;}
        CardType[] CardTypes { get; }
        bool IsCardUp { get; set;  }
        string ManaCost { get; set; }
        int CalculatedManaCost { get;}
        string PowTgh { get; set; }
        int CalculatedBasePower {get;}
        int CalculatedBaseToughness { get; }
        string Rule { get; }

    }
    
}
