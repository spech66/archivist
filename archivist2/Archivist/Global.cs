using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Security.Cryptography;

namespace Archivist
{
	public static class Global
    {
        /// <summary>
        /// Shuffle using http://en.wikipedia.org/wiki/Fisher-Yates_shuffle
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="list"></param>
        public static void Shuffle<T>(this IList<T> list)
        {
            RNGCryptoServiceProvider provider = new RNGCryptoServiceProvider();
            int n = list.Count;
            while (n > 1)
            {
                byte[] box = new byte[1];
                do provider.GetBytes(box);
                while (!(box[0] < n * (Byte.MaxValue / n)));
                int k = (box[0] % n);
                n--;
                T value = list[k];
                list[k] = list[n];
                list[n] = value;
            }
        }
	}

	public class ListBoxItemNameId
	{
		public ListBoxItemNameId(string name, string id)
		{
			Name = name;
			Id = id;
		}

		public string Name { get; set; }
		public string Id { get; set; }
	}
}
