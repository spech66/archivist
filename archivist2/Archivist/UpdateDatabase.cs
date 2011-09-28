using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
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
			UpdateListText("Checking directories...");
			
			dataDirectory = Application.StartupPath + "\\data";
			imageDirectory = Application.StartupPath + "\\img";
			tempDirectory = Application.StartupPath + "\\tmp";

			System.IO.Directory.CreateDirectory(dataDirectory);
			System.IO.Directory.CreateDirectory(imageDirectory);
			System.IO.Directory.CreateDirectory(tempDirectory);

            UpdateListText("Starting update...");
            System.Threading.Thread updThrad = new System.Threading.Thread(new System.Threading.ThreadStart(UpdateDB));
            updThrad.Start();
        }

        public void UpdateDB()
        {
            try
            {
                WebClient client = new WebClient();
				string downloadUrl;

				// ------------------------------------------------------------
				// Download set list
				downloadUrl = "http://ww2.wizards.com/gatherer/";
				UpdateListText("Downloading Set list...");
				byte[] setlistBytes = client.DownloadData(downloadUrl);
				UpdateTotalStatus(1, 100);

				// ------------------------------------------------------------
				// Parse download for setlist
				UpdateListText("Analyzing file...");
				UTF8Encoding utf8 = new UTF8Encoding();
				string setlist = utf8.GetString(setlistBytes);

				string subsetlist = setlist.Substring(setlist.IndexOf("<select name=\"_ddlFilterFormat\""));
				subsetlist = subsetlist.Substring(subsetlist.IndexOf(">") + 1);
				subsetlist = subsetlist.Substring(0, subsetlist.IndexOf("</select>"));

				Regex r = new Regex("<option value=\"(?<id>[0-9]*)\">(?<name>.+)</option>", RegexOptions.IgnoreCase | RegexOptions.Compiled);
				MatchCollection mcl = r.Matches(subsetlist);
				ArrayList extlist = new ArrayList();
				foreach (Match m in mcl)
				{
					if (m.Groups["id"].ToString() != "1")
						extlist.Add(m.Groups["name"].ToString());
				}
				UpdateListText("Found " + extlist.Count + " Sets.");

				// ------------------------------------------------------------
				// Download spoiler lists
				int i = 0;
				foreach (string ext in extlist)
				{
				    if (i < 10) { i++; continue; } // Skipping Standard, vintage, ... (types)
					downloadUrl = "http://ww2.wizards.com/gatherer/Index.aspx?setfilter=" + ext.Replace(" ", "%20") + "&output=Text%20Spoiler";
					string extoutfile = tempDirectory +  "\\" + System.Web.HttpUtility.UrlEncode(ext) + ".dat";
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
	
					UpdateTaskStatus(i, extlist.Count);
					UpdateTotalStatus(i + 1, 2 * extlist.Count + 2);
					i++;
				}


				// ------------------------------------------------------------
				// Update extensions table
				UpdateListText("Writing extensions to database...");
				int id = 1;
                ArchivistDatabase adb = new ArchivistDatabase();
                adb.DeleteExtensions();
				foreach (string ext in extlist)
				{
					adb.InsertExtension(id,"", ext);
					id++;
				}

                // ------------------------------------------------------------
                // Analyze spoiler lists and update db
                string paraCardName = string.Empty, paraCost = string.Empty, paraPowTgh = string.Empty, paraRulesText = string.Empty, paraType = string.Empty, paraCardName2 = string.Empty;
                string paraCardExtCID = string.Empty,  paraCardExtRar = string.Empty;
                int paraCardExtEID;
				id = 1;
				foreach (string ext in extlist)
				{
                    string extoutfile = tempDirectory + "\\" + System.Web.HttpUtility.UrlEncode(ext) + ".dat";
					UpdateListText("Analyzing extension file for " + ext + "...");

					if (!System.IO.File.Exists(extoutfile))
					{
						UpdateListText("File not found. Skipping.");
						continue;
					}

					string cardlist = utf8.GetString(File.ReadAllBytes(extoutfile));

					string subcardlist = cardlist.Substring(cardlist.IndexOf("<table class=\"TextResultsTable\""));
					subcardlist = subcardlist.Substring(subcardlist.IndexOf(">") + 1);
					subcardlist = subcardlist.Substring(0, subcardlist.IndexOf("</table>"));

					Regex cardregex = new Regex("<tr><td class=\"TextResultsRowHeader\">(?<key>.+?):</td><td class=\"TextResultsRowValue\">(?<value>.*?)</td></tr>", RegexOptions.IgnoreCase | RegexOptions.Singleline | RegexOptions.Compiled);
					MatchCollection cardmcl = cardregex.Matches(subcardlist);
					string cardSetRarity = "";
					string currentCardName = "";
					foreach (Match m in cardmcl)
					{
						if (m.Groups["key"].ToString() == "CardName" && paraCardName != null && paraCardName != currentCardName)
						{
							currentCardName = paraCardName;
							paraCardName2 = paraCardName;

                            Card card = MagicCardFactory.BuildCard(paraCardName, paraCost, paraPowTgh, paraRulesText, MagicCardFactory.CardTypes(paraType));
                            //string cid = adb.InsertCard(paraCardName,paraCost,paraPowTgh,paraRulesText,paraType,paraCardName2);
                            string cid = adb.InsertCard(card);

							string[] setrlist = cardSetRarity.Split(',');
							foreach(string setr in setrlist)
							{
								int split = setr.LastIndexOf(" ");
                                if (split > 0)
                                {
                                    string set = setr.Substring(0, split).Trim();
                                    string rarity = setr.Substring(split).Trim();
                                    int eid = extlist.IndexOf(set);
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

							paraCardName = null; paraCost = null; paraType = null;
							paraPowTgh = null; paraRulesText = null; cardSetRarity = null;
							currentCardName = "";
						}
						
						if (m.Groups["key"].ToString() == "CardName") paraCardName = m.Groups["value"].ToString();
						if (m.Groups["key"].ToString() == "Cost") paraCost = m.Groups["value"].ToString();
						if (m.Groups["key"].ToString() == "Type") paraType = m.Groups["value"].ToString();
						if (m.Groups["key"].ToString() == "Pow/Tgh") paraPowTgh = m.Groups["value"].ToString();
						if (m.Groups["key"].ToString() == "Rules Text") paraRulesText = m.Groups["value"].ToString();
						if (m.Groups["key"].ToString() == "Set/Rarity") cardSetRarity = m.Groups["value"].ToString();
					}

					UpdateTotalStatus(extlist.Count + id + 1, 2 * extlist.Count + 2);
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

		private string ReadBlock(StreamReader reader)
		{
			string line = "";
			string text = "";
			bool blockComplete = false;
			while (!reader.EndOfStream && !blockComplete)
			{
				line = reader.ReadLine().Trim();
				text += line + " ";

				if (line == "")
					blockComplete = true;
			}

			text = text.Replace("CardName:", "");
			text = text.Replace("Cost:", "\n");
			text = text.Replace("Type:", "\n");
			text = text.Replace("Pow/Tgh:", "\n");
			text = text.Replace("Rules Text:", "\n");
			text = text.Replace("Set/Rarity:", "\n");

			string[] fields = text.Split(new char[]{'\n'});
			for (int i = 0; i < fields.Length; i++ )
				fields[i] = GetNiceValue(fields[i]);

			string xml = "";
			xml += "<card";
			xml += " name=\"" + fields[0] + "\"";
			xml += " cost=\"" + fields[1] + "\"";
			xml += " type=\"" + fields[2] + "\"";
			xml += " powtgh=\"" + fields[3] + "\"";
			xml += ">\n";
			xml += "<rules>" + fields[4] + "</rules>\n";
			string[] eds = fields[5].Split(new char[] { ',' });
			foreach (string ed in eds)
			{
				string edname = ed.Substring(0, ed.LastIndexOf(' ')).Trim();
				string rarity = ed.Substring(ed.LastIndexOf(' ') + 1).Trim();
				xml += "<edition name=\"" + edname + "\" rarity=\"" + rarity + "\"/>\n";
			}
			xml += "</card>\n";
			return xml;
		}

		private string GetNiceValue(string text)
		{
			text = text.Trim();
			text = text.Replace("&", "&amp;");
			text = text.Replace("\"", "&quot;");
			return text;
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
    }
}