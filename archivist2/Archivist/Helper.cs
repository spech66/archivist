using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.IO;
using System.Drawing;

namespace Archivist
{
	public class Helper
	{
		private static readonly Helper _instance;
		public static Helper Instance
		{
			get
            {
				return _instance;
            }
		}

		private string dataDirectory;
		private string imageDirectory;
		private string cardImageDirectory;
		private string decksDirectory;

		static Helper()
		{
			if(_instance == null)
			{
				_instance = new Helper();
			}

			Instance.dataDirectory = Path.Combine(Application.StartupPath, "data");
			Instance.imageDirectory = Path.Combine(Application.StartupPath, "img");
			Instance.cardImageDirectory = Path.Combine(Application.StartupPath, "cardimg");
			Instance.decksDirectory = Path.Combine(Application.StartupPath, "decks");
		}

		public static string DataDirectory { get { return Instance.dataDirectory; } }
		public static string ImageDirectory { get { return Instance.imageDirectory; } }
		public static string CardImageDirectory { get { return Instance.cardImageDirectory; } }
		public static string DecksDirectory { get { return Instance.decksDirectory; } }

		public static Image GetMagicImage(string multiversid = "")
		{
			// Check or create card image directory
			if (!Directory.Exists(CardImageDirectory))
			{
				Directory.CreateDirectory(CardImageDirectory);
			}

			if (!string.IsNullOrEmpty(multiversid) && Properties.Settings.Default.ShowImages)
			{
				// Try to get image from file
				string filename = Path.Combine(CardImageDirectory, multiversid + ".jpg");
				if (File.Exists(filename))
				{
					return Image.FromFile(filename);
				}

				// Download image if requested
				if (Properties.Settings.Default.DownloadImages)
				{
					try
					{
						using (System.Net.WebClient client = new System.Net.WebClient())
						{
							string downloadUrl = String.Format("http://gatherer.wizards.com/Handlers/Image.ashx?multiverseid={0}&type=card", multiversid);
							client.DownloadFile(downloadUrl, filename);
						}

						if (File.Exists(filename))
						{
							return Image.FromFile(filename);
						}
					}
					catch (Exception e)
					{
						MessageBox.Show(e.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
					}
				}
			}

			// Show empty card
			string noneimg = System.IO.Path.Combine(Helper.ImageDirectory, "none.jpg");
			if (System.IO.File.Exists(noneimg))
			{
				return Image.FromFile(noneimg);
			}

			// Nothing was found :-(
			Bitmap img = new Bitmap(1, 1);
			return img;
		}
	}
}
