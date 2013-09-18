
using System.Data;
using Archivist.MagicObjects;
using System;
using System.Collections.Generic;
namespace Archivist.Data
{
    public class ArchivistDatabase : DataBuider
    {
        public void DeleteExtensions()
        {
			string sqlcmd = "DELETE FROM EXTENSION";

            using (IDbConnection connection = database.CreateOpenConnection())
            {
                using (IDbCommand command = database.CreateCommand(sqlcmd, connection))
                {
                    command.ExecuteNonQuery();
                }
            }
        }

        public Dictionary<int, string> GetExtensions()
		{
            Dictionary<int, string> extensions = new Dictionary<int, string>();

			string sqlcmd = "SELECT ID, NAME FROM EXTENSION";

			using (IDbConnection connection = database.CreateOpenConnection())
			{
				using (IDbCommand command = database.CreateCommand(sqlcmd, connection))
				{
					IDataReader reader = command.ExecuteReader();
					while (reader.Read())
					{
						extensions.Add(Convert.ToInt32(reader["id"].ToString()), reader["name"].ToString());
					}
				}
			}

			return extensions;
		}

        public void InsertExtension(int paramID, string paramEXT, string paramNAME)
        {
			string sqlcmd = "INSERT INTO EXTENSION (ID, EXT, NAME) VALUES (?, ?, ?)";

            using (IDbConnection connection = database.CreateOpenConnection())
            {
                using (IDbCommand command = database.CreateCommand(sqlcmd, connection))
                {
                    IDbDataParameter p1 = command.CreateParameter(); command.Parameters.Add(p1);
                    IDbDataParameter p2 = command.CreateParameter(); command.Parameters.Add(p2);
                    IDbDataParameter p3 = command.CreateParameter(); command.Parameters.Add(p3);
                    p1.Value = paramID;
                    p2.Value = paramEXT;
                    p3.Value = paramNAME;
                    command.ExecuteNonQuery();
                }
            }
        }

		public MagicCard GetCard(int id)
		{
			string sqlcmd = "SELECT * FROM CARD";
			string whereclause = "WHERE ID = ?";

			using (IDbConnection connection = database.CreateOpenConnection())
			{
				using (IDbCommand command = database.CreateCommand(sqlcmd + " " + whereclause, connection))
				{
					IDbDataParameter p1 = command.CreateParameter(); command.Parameters.Add(p1);
					p1.Value = id;
					using (IDataReader reader = command.ExecuteReader())
					{
						if (!reader.Read())
							return null;

						MagicCard card = new MagicCard(true);
						card.Name = reader["name"].ToString();
						card.ManaCost = reader["cost"].ToString();
						card.PowTgh = reader["PowTgh"].ToString();
						card.Multiverseid = Convert.ToInt32(reader["Id"].ToString());
						card.Rule = reader["Rule"].ToString();
						card.Type = reader["Type"].ToString();
						card.Extension = reader["Extension"].ToString();
						card.Rarity = reader["Rarity"].ToString();
						return card;
					}
				}
			}
		}

        public Card GetCard(string paramNAME)
        {
            string sqlcmd = "SELECT * FROM CARD";
            string whereclause ="WHERE NAME = ?";

            using (IDbConnection connection = database.CreateOpenConnection())
            {
                using (IDbCommand command = database.CreateCommand(sqlcmd + " " + whereclause, connection))
                {
                    IDbDataParameter p1 = command.CreateParameter(); command.Parameters.Add(p1);
                    p1.Value = paramNAME;
                    using (IDataReader reader = command.ExecuteReader())
                    {
						if (!reader.Read())
							return null;

                        MagicCard card = new MagicCard(true);
                        card.Name= reader["name"].ToString();
                        card.ManaCost = reader["cost"].ToString();
                        card.PowTgh = reader["PowTgh"].ToString();
						card.Multiverseid = Convert.ToInt32(reader["Id"].ToString());
						card.Rule = reader["Rule"].ToString();
						card.Type = reader["Type"].ToString();
						card.Extension = reader["Extension"].ToString();
						card.Rarity = reader["Rarity"].ToString();
                        return card;
                    }
                }
            }
        }

		public List<Card> GetCards(string whereclause = "", params object[] values)
		{
			List<Card> cards = new List<Card>();

			string sqlcmd = "SELECT * FROM CARD";
			string orderby = " ORDER BY NAME";

			using (IDbConnection connection = database.CreateOpenConnection())
			{
				using (IDbCommand command = database.CreateCommand(String.Format("{0} {1} {2}", sqlcmd, whereclause, orderby), connection))
				{
					for (int i = 0; i < values.Length; i++)
					{
						IDbDataParameter p1 = command.CreateParameter();
						command.Parameters.Add(p1);
						p1.Value = values[i];
					}

					IDataReader reader = command.ExecuteReader();
					while (reader.Read())
					{
						MagicCard card = new MagicCard(true);
                        card.Name = reader["name"].ToString();
						card.ManaCost = reader["cost"].ToString();
						card.PowTgh = reader["PowTgh"].ToString();
						card.Multiverseid = Convert.ToInt32(reader["Id"].ToString());
						card.Rule = reader["Rule"].ToString();
						card.Type = reader["Type"].ToString();
						card.Extension = reader["Extension"].ToString();
						card.Rarity = reader["Rarity"].ToString();
						cards.Add(card);
					}
				}
			}

			return cards;
		}

        public string InsertCard(Card card)
        {
            string sparaCardName = card.Name;
            string sparaCost = card.ManaCost;
            string sparaPowTgh = card.PowTgh;
            string sparaRulesText = card.Rule;
			string sparaType = card.Type;
			string sparaRarity = card.Rarity;
			string sparaExtension = card.Extension;
			int sparaMultiverseid = card.Multiverseid;
			return InsertCard(sparaCardName, sparaCost, sparaPowTgh, sparaRulesText, sparaType, sparaRarity, sparaExtension, sparaMultiverseid);            
        }

		public string InsertCard(string sparaCardName, string sparaCost, string sparaPowTgh, string sparaRulesText, string sparaType, string sparaRarity, string sparaExtension, int sparaMultiverseid)
        {
			string sqlcmd = "INSERT OR IGNORE INTO CARD (NAME, COST, POWTGH, RULE, TYPE, RARITY, EXTENSION, ID)" +
							" VALUES (?, ?, ?, ?, ?, ?, ?, ?)";//; SELECT ID FROM CARD WHERE NAME = ?";

            using (IDbConnection connection = database.CreateOpenConnection())
            {
                using (IDbCommand cmdCard = database.CreateCommand(sqlcmd, connection))
                {
                    IDbDataParameter paraCardName = cmdCard.CreateParameter(); cmdCard.Parameters.Add(paraCardName); paraCardName.Value = sparaCardName;
                    IDbDataParameter paraCost = cmdCard.CreateParameter(); cmdCard.Parameters.Add(paraCost); paraCost.Value = sparaCost;
                    IDbDataParameter paraPowTgh = cmdCard.CreateParameter(); cmdCard.Parameters.Add(paraPowTgh); paraPowTgh.Value = sparaPowTgh;
                    IDbDataParameter paraRulesText = cmdCard.CreateParameter(); cmdCard.Parameters.Add(paraRulesText); paraRulesText.Value = sparaRulesText;
					IDbDataParameter paraType = cmdCard.CreateParameter(); cmdCard.Parameters.Add(paraType); paraType.Value = sparaType;
					IDbDataParameter paraRarity = cmdCard.CreateParameter(); cmdCard.Parameters.Add(paraRarity); paraRarity.Value = sparaRarity;
					IDbDataParameter paraExtension = cmdCard.CreateParameter(); cmdCard.Parameters.Add(paraExtension); paraExtension.Value = sparaExtension;
					IDbDataParameter paraMultiversid = cmdCard.CreateParameter(); cmdCard.Parameters.Add(paraMultiversid); paraMultiversid.Value = sparaMultiverseid;

					//IDbDataParameter paraCardName2 = cmdCard.CreateParameter(); cmdCard.Parameters.Add(paraCardName2); paraCardName2.Value = sparaCardName;

                    //string cid = cmdCard.ExecuteScalar().ToString();
					cmdCard.ExecuteScalar();
					return sparaMultiverseid.ToString();
                    //return cid;
                }
            }
        }
	}
}
