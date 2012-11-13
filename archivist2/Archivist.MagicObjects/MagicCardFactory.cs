using System;
using System.Collections.Generic;
using System.Reflection;
using System.Collections;

namespace Archivist.MagicObjects
{
    public sealed class MagicCardFactory
    {
		public static Card BuildCard(string cardName, string cost, string powTgh, string rulesText, string type, int multiverseid)
        {
            MagicCard card = new MagicCard(cardName, true);
            card.ManaCost = cost;
            card.PowTgh = powTgh;
            card.Rule = rulesText;
			card.Type = type;
			card.Multiverseid = multiverseid;
            return card;
        }

        /// <summary>
        /// method to read in an enum and return a string array
        /// </summary>
        /// <param name="type">enum to convert</param>
        /// <returns></returns>
        public string[] GetStringArray(Type type)
        {
            //use Reflection to get the fields in our enum
            FieldInfo[] info = type.GetFields();
            //new ArrayList to hold the values (will convert later)
            ArrayList fields = new ArrayList();
         
            //loop through all the fields
            foreach (FieldInfo fInfo in info)
            {
                //add each to our ArrayList
                fields.Add(fInfo.Name);
            }
         
            //now we convert to string array and return
            return (string[])fields.ToArray(typeof(string));
        } 

  }
}
