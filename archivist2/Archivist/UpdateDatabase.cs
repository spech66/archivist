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
        }

        public void UpdateDB()
        {
            try
            {
				List<string> setList = DownloadSetList().ToList();
				DownloadSpoilerList(setList);
								
				// ------------------------------------------------------------
				// Update extensions table
				UpdateListText("Writing extensions to database...");
				int id = 1;
				ArchivistDatabase adb = new ArchivistDatabase();
				adb.DeleteExtensions();
				foreach (string ext in setList)
				{
					adb.InsertExtension(id,"", ext.Replace("&quot;", "\""));
					id++;
				}
				
				// ------------------------------------------------------------
				// Analyze spoiler lists and update db
				string currentCardName = string.Empty;
				string paraCardName = string.Empty, paraCost = string.Empty, paraPowTgh = string.Empty, paraRulesText = string.Empty, paraType = string.Empty;
				string paraCardExtCID = string.Empty, paraCardExtRar = string.Empty, paraMultiverseidString = string.Empty;
				int paraCardExtEID, paraMultiverseid = 0;
				id = 1;
				foreach (string ext in setList)
				{
					string extoutfile = tempDirectory + "\\" + System.Web.HttpUtility.UrlEncode(ext) + ".dat";
					UpdateListText("Analyzing extension file for " + ext + "...");

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
							string value = cols[1].InnerText.TrimStart().TrimEnd().Replace("—", "-");
							if (key == "Name")
							{
								currentCardName = value;
								paraCardName = value;
								string href = cols[1].SelectSingleNode("a").GetAttributeValue("href", ""); //../Card/Details.aspx?multiverseid=201281
								paraMultiverseidString = href.Substring(href.LastIndexOf("=") + 1); //201281
								paraMultiverseid = Convert.ToInt32(paraMultiverseidString);
							}
							if (key == "Cost") paraCost = value;
							if (key == "Type") paraType = value;
							if (key == "Pow/Tgh") paraPowTgh = value;
							if (key == "Rules Text") paraRulesText = value;
							if (key == "Set/Rarity") paraCardExtRar = value;
						}
						else
						{
							if (currentCardName != "")
							{								
								Card card = MagicCardFactory.BuildCard(paraCardName, paraCost, paraPowTgh, paraRulesText, paraType, paraMultiverseid);
								string cid = adb.InsertCard(card);

								string[] setrlist = paraCardExtRar.Split(',');
								foreach(string setr in setrlist)
								{
									int split = setr.LastIndexOf(" ");
									if (split > 0)
									{
										string set = setr.Substring(0, split).Trim();
										string rarity = setr.Substring(split).Trim();
										int eid = setList.IndexOf(set);
										paraCardExtCID = cid;
										paraCardExtEID = eid;
										paraCardExtRar = rarity;
										try
										{
											adb.InsertCardExtension(paraCardExtCID, paraCardExtEID, paraCardExtRar);
										}
										catch
										{
											// Clunky way of catching an exception that seems to go if we retry...
											adb.InsertCardExtension(paraCardExtCID, paraCardExtEID, paraCardExtRar);
										}
									}
									else
									{
									}
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
		private void DownloadSpoilerList(List<string> setList)
		{
			using (WebClient client = new WebClient())
			{
				int i = 0;
				foreach (string ext in setList)
				{
					string downloadUrl = "http://gatherer.wizards.com/Pages/Search/Default.aspx?output=spoiler&method=text&action=advanced&set=[%22" + ext.Replace(" ", "+").Replace("&quot;", "%22") + "%22]&special=true";

					string extoutfile = tempDirectory + "\\" + System.Web.HttpUtility.UrlEncode(ext) + ".dat";
					UpdateListText("Getting extension " + ext + "...");

					if (System.IO.File.Exists(extoutfile))
					{
						UpdateListText("Already downloaded. Skipping.");
					}
					else
					{
						System.Diagnostics.Debug.WriteLine("Getting extension " + ext + "...");
						client.DownloadFile(downloadUrl, extoutfile);
					}

					UpdateTaskStatus(i, setList.Count);
					UpdateTotalStatus(i + 1, 2 * setList.Count + 2);
					i++;
				}
			}
		}

        private void UpdateListText(string text)
        {
            UpdateListText(text, false);
        }

        private void UpdateListText(string text, bool enableButton)
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