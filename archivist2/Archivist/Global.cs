using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Archivist
{
	public class Global
	{
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
