using System;
using System.Collections.Generic;
using System.Reflection;
using System.Collections;

namespace Archivist.MagicObjects
{
	public sealed class MagicCardFactory
	{
		public static Card BuildCard(string cardName, string cost, string powTgh, string rulesText, string type, string rarity, string extension, int multiverseid)
		{
			MagicCard card = new MagicCard(cardName, true);
			card.ManaCost = cost;
			card.PowTgh = powTgh;
			card.Rule = rulesText;
			card.Type = type;
			card.Rarity = rarity;
			card.Extension = extension;
			card.Multiverseid = multiverseid;
			return card;
		}
	}
}
