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

        #region Gatherer update
        public void UpdateDB()
        {
            try
            {
				List<string> setList = DownloadSetList().ToList();
				/* TEST
				List<string> setList = new List<string>();
				setList.Add("Limited Edition Alpha");
				setList.Add("Magic 2011");*/
				
                Dictionary<int, string> updateSetList = UpdateExtensions(setList);

                DownloadSpoilerList(ref updateSetList);

                GenerateCards(ref updateSetList);

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
        private void DownloadSpoilerList(ref Dictionary<int, string> setList)
		{
			using (WebClient client = new WebClient())
			{
				int i = 0;
				foreach (string ext in setList.Values)
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
        private Dictionary<int, string> UpdateExtensions(List<string> setList)
		{
			UpdateListText("Writing extensions to database...");

			ArchivistDatabase adb = new ArchivistDatabase();
            Dictionary<int, string> dbExtensions = adb.GetExtensions();
            Dictionary<int, string> resultList = new Dictionary<int, string>();

			foreach (KeyValuePair<int, string> ext in dbExtensions)
			{
                //string cleanext = ext.Value.Replace("&quot;", "\"");
				string uncleanext = ext.Value.Replace("\"", "&quot;");
                if (setList.Contains(uncleanext))
                {
                    setList.Remove(uncleanext);
                    UpdateListText(String.Format("Extension {0} exists in database. Skipping.", uncleanext));
                }
                else
                {
                    resultList.Add(ext.Key, ext.Value);
                }
			}

            int newId = dbExtensions.Max(sel => sel.Key) + 1;
            foreach (string ext in setList)
            {
                adb.InsertExtension(newId, "", ext.Replace("&quot;", "\""));
                resultList.Add(newId, ext);
                UpdateListText(String.Format("Added extension {0} to database.", ext));

                newId++;
            }

            return resultList;
		}

		/// <summary>
		/// Generate card data from files
		/// </summary>
		/// <param name="setList"></param>
        private void GenerateCards(ref Dictionary<int, string> setList)
		{
			ArchivistDatabase adb = new ArchivistDatabase();
			
			string currentCardName = string.Empty;
			string paraCardName = string.Empty, paraCost = string.Empty, paraPowTgh = string.Empty, paraRulesText = string.Empty, paraType = string.Empty;
			string paraCardExtCID = string.Empty, paraCardExtRar = string.Empty, paraMultiverseidString = string.Empty;
			int /*paraCardExtEID,*/ paraMultiverseid = 0;
			int id = 1;
			int extId = setList.Count + 1;

			foreach (KeyValuePair<int, string> ext in setList)
			{
				string extoutfile = String.Format("{0}\\{1}.dat", tempDirectory, System.Web.HttpUtility.UrlEncode(ext.Value));
                UpdateListText(String.Format("Analyzing extension file for {0}...", ext.Value));

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
								if (setr.Contains(ext.Value))
								{
                                    string set = ext.Value.Replace("&quot;", "\"").Trim();
                                    string rarity = setr.Replace(ext.Value, "").Trim(); // Might be Common/Uncomm/Rare/Mythic Rare

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
                
				UpdateTotalStatus(setList.Count + id + 1, 2 * setList.Count + 2);
				id++;
			}
		}
        #endregion

        #region Check sourceforge
        /// <summary>
        /// Check for new version.
        /// </summary>
        public void CheckSourceforge()
        {
            UpdateListText("Checking version...");

            bool needsUpdate = true;

            try
            {
                string versionLatest;

                // Check for latest version
                string downloadUrl = "https://sourceforge.net/projects/archivist/files/archivist2/";
                UpdateTotalStatus(10, 100);

                HtmlAgilityPack.HtmlWeb web = new HtmlAgilityPack.HtmlWeb();
                HtmlAgilityPack.HtmlDocument doc = web.Load(downloadUrl);
                UpdateTotalStatus(90, 100);
                HtmlAgilityPack.HtmlNode textspoilerNode = doc.DocumentNode.SelectSingleNode("//div[@class=\"download-bar\"]/strong/a/span");
                if (textspoilerNode != null)
                {
                    versionLatest = textspoilerNode.InnerText.Substring(textspoilerNode.InnerText.IndexOf("_") + 1);
                    int lastPos = -1;
                    if ((lastPos = versionLatest.LastIndexOf(".rar")) > 0) versionLatest = versionLatest.Substring(0, lastPos);
                    if ((lastPos = versionLatest.LastIndexOf(".exe")) > 0) versionLatest = versionLatest.Substring(0, lastPos);
                }
                else
                {
                    throw new Exception("Error reading file list.");
                }
                UpdateTotalStatus(95, 100);
                
                string versionCurrent = System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString();
                UpdateListText("Current Version: " + versionCurrent);
                UpdateListText("Latest Version: " + versionLatest);

                needsUpdate = versionCurrent != versionLatest;
                UpdateTotalStatus(100, 100);
            }
            catch (Exception e)
            {
                UpdateTotalStatus(0, 100);
                UpdateListText(e.ToString(), true);
            }
            
            if (needsUpdate)
            {
                UpdateListText("There is a new version available.");
                UpdateListText("Opening https://sourceforge.net/projects/archivist/", true);
                System.Diagnostics.Process.Start("https://sourceforge.net/projects/archivist/");
            }
            else
            {
                UpdateListText("You have the latest version installed.", true);
            }
        }
        #endregion

        #region Update controls
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
        #endregion

        #region Buttons
        private void button1_Click(object sender, EventArgs e)
		{
			this.Close();
		}

		private void btnSoftware_Click(object sender, EventArgs e)
		{
            System.Threading.Thread updThrad = new System.Threading.Thread(new System.Threading.ThreadStart(CheckSourceforge));
            updThrad.Start();
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
        #endregion
    }
}