using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using System.Net; // WebClient Download
using System.IO; // Directory, Path
using System.Collections;
using System.Text.RegularExpressions;
using System.Data.Common;
using Archivist.Data;
using Archivist.MagicObjects;

namespace Archivist
{
    public partial class UpdateDatabase : Form
    {
		private string dataDirectory;
		private string imageDirectory;
		private string tempDirectory;

        delegate void UpdateListTextCallback(string text, bool enableButton);
		delegate void UpdateTaskStatusCallback(long value, long max);
		delegate void UpdateTotalStatusCallback(long value, long max);

        public UpdateDatabase()
        {
            InitializeComponent();
        }

        private void UpdateDatabase_Load(object sender, EventArgs e)
        {
			UpdateListText("Checking directories...", true);
			
			dataDirectory = Path.Combine(Application.StartupPath, "data");
			imageDirectory = Path.Combine(Application.StartupPath, "img");
			tempDirectory = Path.Combine(Application.StartupPath, "tmp");

			Directory.CreateDirectory(dataDirectory);
			Directory.CreateDirectory(imageDirectory);
			Directory.CreateDirectory(tempDirectory);

			UpdateListText("Please select one of the update options below!", true);
        }

        public void UpdateDB()
        {
            try
            {
				List<string> setList = DownloadSetList().ToList();
				/* TEST
				List<string> setList = new List<string>();
				setList.Add("Limited Edition Alpha");
				setList.Add("Magic 2011");*/
				
				UpdateExtensions(ref setList);

				DownloadSpoilerList(ref setList);

				GenerateCards(ref setList);

				// ------------------------------------------------------------
                // We are done!
				UpdateTotalStatus(100, 100);
                UpdateListText("Update completed!", true);
            }
            catch (Exception e)
            {
                UpdateListText("Error updating database!");
                UpdateListText(e.Message, true);
            }
        }

		/// <summary>
		/// Download all sets
		/// </summary>
		/// <returns></returns>
		private IEnumerable<string> DownloadSetList()
		{
			using (WebClient client = new WebClient())
			{
				// ------------------------------------------------------------
				// Download set list
				string downloadUrl = "http://gatherer.wizards.com/Pages/Default.aspx";

				UpdateListText("Downloading Set list...");
				byte[] setlistBytes = client.DownloadData(downloadUrl);
				UpdateTotalStatus(1, 100);

				// ------------------------------------------------------------
				// Parse download for setlist
				UpdateListText("Analyzing file...");
				UTF8Encoding utf8 = new UTF8Encoding();
				string setlist = utf8.GetString(setlistBytes);
				string subsetlist = setlist.Substring(setlist.IndexOf("<select name=\"ctl00$ctl00$MainContent$Content$SearchControls$setAddText\""));
				subsetlist = subsetlist.Substring(subsetlist.IndexOf(">") + 1);
				subsetlist = subsetlist.Substring(0, subsetlist.IndexOf("</select>"));
				Regex r = new Regex("<option value=\"(?<id>.+)\">(?<name>.+)</option>", RegexOptions.IgnoreCase | RegexOptions.Compiled);
				MatchCollection mcl = r.Matches(subsetlist);

				List<string> extlist = new List<string>();
				foreach (Match m in mcl)
				{
					if (m.Groups["id"].ToString() != "")
					{
						extlist.Add(m.Groups["name"].ToString());
					}
				}
				UpdateListText(String.Format("Found {0} Sets.", extlist.Count));
				return extlist;
			}
		}

		/// <summary>
		/// Download spoiler lists
		/// </summary>
		/// <param name="setList"></param>
		private void DownloadSpoilerList(ref List<string> setList)
		{
			using (WebClient client = new WebClient())
			{
				int i = 0;
				foreach (string ext in setList)
				{
					string downloadUrl = String.Format("http://gatherer.wizards.com/Pages/Search/Default.aspx?output=spoiler&method=text&action=advanced&set=[%22{0}%22]&special=true",
						ext.Replace(" ", "+").Replace("&quot;", "%22"));

					string extoutfile = tempDirectory + "\\" + System.Web.HttpUtility.UrlEncode(ext) + ".dat";
					UpdateListText(String.Format("Getting extension {0}...", ext));

					if (System.IO.File.Exists(extoutfile))
					{
						UpdateListText("Already downloaded. Skipping.");
					}
					else
					{
						System.Diagnostics.Debug.WriteLine(String.Format("Getting extension {0}...", ext));
						client.DownloadFile(downloadUrl, extoutfile);
					}

					UpdateTaskStatus(i, setList.Count);
					UpdateTotalStatus(i + 1, 2 * setList.Count + 2);
					i++;
				}
			}
		}

		/// <summary>
		/// Update extensions table
		/// </summary>
		/// <param name="setList"></param>
		/// <param name="id"></param>
		/// <param name="adb"></param>
		private void UpdateExtensions(ref List<string> setList)
		{
			UpdateListText("Writing extensions to database...");

			ArchivistDatabase adb = new ArchivistDatabase();
			List<string> dbExtensions = adb.GetExtensions();

			foreach (string ext in dbExtensions)
			{
				//string cleanext = ext.Replace("&quot;", "\"");
				string uncleanext = ext.Replace("\"", "&quot;");
				if (setList.Contains(uncleanext))
				{
					UpdateListText(String.Format("Extension {0} exists in database. Skipping.", ext));
					setList.Remove(uncleanext);
				}
			}
		}

		/// <summary>
		/// Generate card data from files
		/// </summary>
		/// <param name="setList"></param>
		private void GenerateCards(ref List<string> setList)
		{
			ArchivistDatabase adb = new ArchivistDatabase();
			
			string currentCardName = string.Empty;
			string paraCardName = string.Empty, paraCost = string.Empty, paraPowTgh = string.Empty, paraRulesText = string.Empty, paraType = string.Empty;
			string paraCardExtCID = string.Empty, paraCardExtRar = string.Empty, paraMultiverseidString = string.Empty;
			int /*paraCardExtEID,*/ paraMultiverseid = 0;
			int id = 1;
			int extId = setList.Count + 1;

			foreach (string ext in setList)
			{
				string extoutfile = String.Format("{0}\\{1}.dat", tempDirectory, System.Web.HttpUtility.UrlEncode(ext));
				UpdateListText(String.Format("Analyzing extension file for {0}...", ext));

				if (!System.IO.File.Exists(extoutfile))
				{
					UpdateListText("File not found. Skipping.");
					continue;
				}

				HtmlAgilityPack.HtmlWeb web = new HtmlAgilityPack.HtmlWeb();
				HtmlAgilityPack.HtmlDocument doc = web.Load(extoutfile);
				HtmlAgilityPack.HtmlNode textspoilerNode = doc.DocumentNode.SelectSingleNode("//div[@class=\"textspoiler\"]/table");

				foreach (HtmlAgilityPack.HtmlNode rows in textspoilerNode.SelectNodes("tr"))
				{
					HtmlAgilityPack.HtmlNodeCollection cols = rows.SelectNodes("td");
					if (cols.Count == 2)
					{
						string key = cols[0].InnerText.Replace(":", "").Trim();
						string value = cols[1].InnerText.TrimStart().TrimEnd();

						if (key == "Name")
						{
							currentCardName = value;
							paraCardName = value;
							string href = cols[1].SelectSingleNode("a").GetAttributeValue("href", ""); //../Card/Details.aspx?multiverseid=201281
							paraMultiverseidString = href.Substring(href.LastIndexOf("=") + 1); //201281
							paraMultiverseid = Convert.ToInt32(paraMultiverseidString);
						}
						if (key == "Cost") paraCost = value;
						if (key == "Type") paraType = value.Replace("â€”", "-").Replace("  ", " ");
						if (key == "Pow/Tgh") paraPowTgh = value;
						if (key == "Rules Text") paraRulesText = value;
						if (key == "Set/Rarity") paraCardExtRar = value.Replace("\"", "&quot;");
					}
					else
					{
						if (currentCardName != "")
						{
							string[] setrlist = paraCardExtRar.Split(',');
							string cid = string.Empty;
							foreach (string setr in setrlist)
							{
								if (setr.Contains(ext))
								{
									string set = ext.Replace("&quot;", "\"").Trim();
									string rarity = setr.Replace(ext, "").Trim(); // Might be Common/Uncomm/Rare/Mythic Rare

									Card card = MagicCardFactory.BuildCard(paraCardName, paraCost, paraPowTgh, paraRulesText, paraType, rarity, set, paraMultiverseid);
									cid = adb.InsertCard(card);
									break;
								}
							}

							if (string.IsNullOrEmpty(cid))
							{
								UpdateListText("Error inserting card: " + currentCardName);
							}
						}

						// New card
						paraCardName = null; paraCost = null; paraType = null;
						paraPowTgh = null; paraRulesText = null; paraCardExtRar = null;
						paraMultiverseidString = null;
						currentCardName = "";
					}

				}

				// Insert extension to mark it completed
				adb.InsertExtension(extId, "", ext.Replace("&quot;", "\""));
				UpdateListText(String.Format("Added extension {0} to database.", ext));

				UpdateTotalStatus(setList.Count + id + 1, 2 * setList.Count + 2);
				id++;
			}
		}


		/// <summary>
		/// Add info text
		/// </summary>
		/// <param name="text"></param>
		/// <param name="enableButton"></param>
        private void UpdateListText(string text, bool enableButton = false)
        {
            if(this.listStatus.InvokeRequired)
            {
                UpdateListTextCallback d = new UpdateListTextCallback(UpdateListText);
                this.Invoke(d, new object[] { text, enableButton });
            }
            else
            {
                listStatus.Items.Add(text);
				listStatus.TopIndex = listStatus.Items.Count - 1;
                button1.Enabled = enableButton;
				btnGatherer.Enabled = enableButton;
				btnSoftware.Enabled = enableButton;
            }
        }

		private void UpdateTaskStatus(long value, long max)
		{
			if (this.listStatus.InvokeRequired)
			{
				UpdateTaskStatusCallback d = new UpdateTaskStatusCallback(UpdateTaskStatus);
				this.Invoke(d, new object[] { value, max });
			}
			else
			{
				this.progressBarTaskStatus.Maximum = Convert.ToInt32(max);
				this.progressBarTaskStatus.Value = Convert.ToInt32(value);
			}
		}

		private void UpdateTotalStatus(long value, long max)
		{
			if (this.listStatus.InvokeRequired)
			{
				UpdateTotalStatusCallback d = new UpdateTotalStatusCallback(UpdateTotalStatus);
				this.Invoke(d, new object[] { value, max });
			} else
			{
				this.progressBarStatus.Maximum = Convert.ToInt32(max);
				this.progressBarStatus.Value = Convert.ToInt32(value);
			}
		}

		private void button1_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void btnSoftware_Click(object sender, EventArgs e)
		{
			UpdateListText("Sorry not yet implemented. Please check https://sourceforge.net/projects/archivist/");
		}

		private void btnGatherer_Click(object sender, EventArgs e)
		{
			if (MessageBox.Show("Updating cards from Gatherer might take a very long time and could damage your existing cardlist file.\r\nContinue?",
				"Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == System.Windows.Forms.DialogResult.Yes)
			{
				UpdateListText("Starting update...");
				System.Threading.Thread updThrad = new System.Threading.Thread(new System.Threading.ThreadStart(UpdateDB));
				updThrad.Start();
			}
		}
    }
}