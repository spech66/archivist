﻿
using System.Data;
using Archivist.MagicObjects;
using System;
using System.Collections.Generic;
namespace Archivist.Data
{
    public class ArchivistDatabase : DataBuider
    {
        public void LoadCards()
        {
            /*using (IDbConnection connection = database.CreateOpenConnection())
            {
                using (IDbCommand command = database.CreateCommand("SELECT * FROM X", connection))
                {
                    using (IDataReader reader = command.ExecuteReader())
                    {
                        // read flowers and process ...
                    }
                }
            }*/
        }

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

        public Card GetCard(string paramNAME)
        {
            string sqlcmd = "SELECT ID FROM CARD";
            string whereclause ="WHERE NAME = ?";

            using (IDbConnection connection = database.CreateOpenConnection())
            {
                using (IDbCommand command = database.CreateCommand(sqlcmd + " " + whereclause, connection))
                {
                    IDbDataParameter p1 = command.CreateParameter(); command.Parameters.Add(p1);
                    p1.Value = paramNAME;
                    using (IDataReader reader = command.ExecuteReader())
                    {
                        string typename = reader["type"].ToString();
                        //typename.Replace("-", "");
                        //string[] types = typename.Split(' ');
                        //List<string> typelist = new List<string>(types);
                        //List<CardType> cardtypes = new List<CardType>();
                        //foreach (string item in typelist)
                        //{
                        //CardType cardtype = (CardType)Enum.Parse(typeof(CardType), reader["name"].ToString());
                        //cardtypes.Add(cardtype);
                        //}

                        MagicCard card = new MagicCard(true);
                        card.Name= reader["name"].ToString();
                        card.ManaCost = reader["cost"].ToString();
                        card.PowTgh = reader["PowTgh"].ToString();
                        return card;
                    }
                }
            }
        }

        public string InsertCard(Card card)
        {
            string sparaCardName = card.Name;
            string sparaCost = card.ManaCost;
            string sparaPowTgh = card.PowTgh;
            string sparaRulesText = card.Rule;
			string sparaType = card.Type;
			int sparaMultiverseid = card.Multiverseid;
            return InsertCard(sparaCardName, sparaCost, sparaPowTgh, sparaRulesText, sparaType, sparaMultiverseid);            
        }

        public string InsertCard(string sparaCardName, string sparaCost, string sparaPowTgh, string sparaRulesText, string sparaType, int sparaMultiverseid)
        {
            string sqlcmd = "INSERT OR IGNORE INTO CARD (NAME, COST, POWTGH, RULE, TYPE, MULTIVERSEID)" +
                            " VALUES (?, ?, ?, ?, ?, ?); SELECT ID FROM CARD WHERE NAME = ?";

            using (IDbConnection connection = database.CreateOpenConnection())
            {
                using (IDbCommand cmdCard = database.CreateCommand(sqlcmd, connection))
                {
                    IDbDataParameter paraCardName = cmdCard.CreateParameter(); cmdCard.Parameters.Add(paraCardName); paraCardName.Value = sparaCardName;
                    IDbDataParameter paraCost = cmdCard.CreateParameter(); cmdCard.Parameters.Add(paraCost); paraCost.Value = sparaCost;
                    IDbDataParameter paraPowTgh = cmdCard.CreateParameter(); cmdCard.Parameters.Add(paraPowTgh); paraPowTgh.Value = sparaPowTgh;
                    IDbDataParameter paraRulesText = cmdCard.CreateParameter(); cmdCard.Parameters.Add(paraRulesText); paraRulesText.Value = sparaRulesText;
					IDbDataParameter paraType = cmdCard.CreateParameter(); cmdCard.Parameters.Add(paraType); paraType.Value = sparaType;
					IDbDataParameter paraMultiversid = cmdCard.CreateParameter(); cmdCard.Parameters.Add(paraMultiversid); paraMultiversid.Value = sparaMultiverseid;

					IDbDataParameter paraCardName2 = cmdCard.CreateParameter(); cmdCard.Parameters.Add(paraCardName2); paraCardName2.Value = sparaCardName;

                    string cid = cmdCard.ExecuteScalar().ToString();
                    return cid;
                }
            }
        }

        public void InsertCardExtension(string sparaCardExtCID, int sparaCardExtEID, string sparaCardExtRar)
        {
            string sqlcmd = "INSERT OR IGNORE INTO CARD_EXTENSION (CARD_ID, EXTENSION_ID, RARITY) VALUES (?, ?, ?)";

            using (IDbConnection connection = database.CreateOpenConnection())
            {
                using (IDbCommand cmdCardExt = database.CreateCommand(sqlcmd, connection))
                {
                    IDbDataParameter paraCardExtCID = cmdCardExt.CreateParameter(); cmdCardExt.Parameters.Add(paraCardExtCID); paraCardExtCID.Value = sparaCardExtCID;
                    IDbDataParameter paraCardExtEID = cmdCardExt.CreateParameter(); cmdCardExt.Parameters.Add(paraCardExtEID); paraCardExtEID.Value = sparaCardExtEID;
                    IDbDataParameter paraCardExtRar = cmdCardExt.CreateParameter(); cmdCardExt.Parameters.Add(paraCardExtRar); paraCardExtRar.Value = sparaCardExtRar;

                    cmdCardExt.ExecuteNonQuery();
                }
            }
        }
    }
}
