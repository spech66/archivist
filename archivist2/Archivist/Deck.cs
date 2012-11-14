using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Windows.Forms;

namespace Archivist
{
	public partial class Deck : UserControl
	{
		private string deckFilename;

		public Deck(string path = "")
		{
			InitializeComponent();

			if (!String.IsNullOrEmpty(path))
			{
				deckFilename = path;
			}
		}
	}
}
