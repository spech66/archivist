using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.IO;
using System.Xml;

namespace Archivist
{
    class TournamentFormats
    {
        private static readonly TournamentFormats _instance;
        public static TournamentFormats Instance
        {
            get
            {
                return _instance;
            }
        }

        private List<TournamentFormat> formats = new List<TournamentFormat>();

        static TournamentFormats()
		{
			if(_instance == null)
			{
                _instance = new TournamentFormats();
			}
		}

        public TournamentFormats()
        {
            string formatsFile = Path.Combine(Helper.DataDirectory, "formats.xml");

            string group = string.Empty;
            TournamentFormat format = null;
            using (XmlReader reader = XmlReader.Create(formatsFile))
            {
                while (reader.Read())
                {
                    XmlNodeType type = reader.NodeType;
                    if (type == XmlNodeType.Element && reader.Name == "group")
                    {
                        group = reader.GetAttribute("name");
                    }
                    else if (type == XmlNodeType.Element && reader.Name == "format")
                    {
                        string name = reader.GetAttribute("name");
                        format = new TournamentFormat(group, name);
                        formats.Add(format);
                    }
                    else if (type == XmlNodeType.Element && reader.Name == "block")
                    {
                        string name = reader.GetAttribute("name");
                        format = new TournamentFormat(group, /*"Block -" +*/ name);
                        formats.Add(format);
                    }
                    // Sub elements for sets
                    else if (type == XmlNodeType.Element && reader.Name == "set")
                    {
                        reader.Read();
                        if (reader.NodeType == XmlNodeType.Text)
                        {
                            format.AddSet(reader.Value);
                        }
                    }
                    else if (type == XmlNodeType.Element && reader.Name == "banned")
                    {
                        reader.Read();
                        if (reader.NodeType == XmlNodeType.Text)
                        {
                            format.AddBanned(reader.Value);
                        }
                    }
                    else if (type == XmlNodeType.Element && reader.Name == "restricted")
                    {
                        reader.Read();
                        if (reader.NodeType == XmlNodeType.Text)
                        {
                            format.AddRestricted(reader.Value);
                        }
                    }
                }
            }
        }
    }

    class TournamentFormat
    {
        private List<string> set = new List<string>();
        private List<string> banned = new List<string>();
        private List<string> restricted = new List<string>();

        public string Group { get; set; }
        public string Name { get; set; }
        public IEnumerable<string> Set { get { return set; } }
        public IEnumerable<string> Banned { get { return banned; } }
        public IEnumerable<string> Restricted { get { return restricted; } }

        public TournamentFormat(string group, string name)
        {
            Group = group;
            Name = name;
        }

        public void AddSet(string setName)
        {
            set.Add(setName);
        }

        public void AddBanned(string bannedName)
        {
            banned.Add(bannedName);
        }

        public void AddRestricted(string restrictedName)
        {
            restricted.Add(restrictedName);
        }
    }
}
