using System;
using System.Collections.Generic;
using System.Reflection;
using System.Collections;

namespace Archivist.MagicObjects
{
    public sealed class MagicCardFactory
    {
        public static Card BuildCard(string cardName, string cost, string powTgh, string rulesText, CardType[] types)
        {
            MagicCard card = new MagicCard(cardName, types, true);
            card.ManaCost = cost;
            card.PowTgh = powTgh;
            card.Rule = rulesText;

            //string CardTypes = CardTypesAsString(types);
            return card;
        }

        public static string CardTypesAsString(CardType[] cardtypes)
        {
            string types = string.Empty;
            foreach (CardType s in cardtypes)
            {
                types += ", " + s.ToString("G");
            }
            
            return types.Substring(1).Trim();
        }

        public static CardType[] CardTypes(string cardtypenames)
        {
            string typename = cardtypenames;
            typename = typename.Replace("-", "");
            typename = typename.Replace("//", "");
            string[] types = typename.Split(new string[]{" "},StringSplitOptions.RemoveEmptyEntries);
            Array.Sort(types);
            int symbol_table_length = types.Length;
            for (int i = 1; i < symbol_table_length; i++)
            {
                // Check if two consecutive variables are the same. Since the list is sorted, any duplicates will always be consecutive.
                if (types[i] == types[i - 1])
                {
                    // If we're not at the end of the list, shift all variables down
                    if (i != symbol_table_length - 1)
                        for (int j = i; j < symbol_table_length; j++)
                            types[j] = types[j + 1];
                    // Adjust the table length and shift the index to account for the lost variable
                    symbol_table_length--;
                    i--;
                }
            }

            List<string> typelist = new List<string>(types);
            List<CardType> cardtypes = new List<CardType>();
            foreach (string item in typelist)
            {
                CardType cardtype = new CardType();
                try
                {
                     cardtype = (CardType)Enum.Parse(typeof(CardType), item);
                }
                catch (ArgumentException ae)
                {
                    System.Diagnostics.Debug.WriteLine(ae.Message);
                }
                cardtypes.Add(cardtype);
            }
            return cardtypes.ToArray();
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
