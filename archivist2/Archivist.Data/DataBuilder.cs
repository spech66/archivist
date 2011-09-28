

using System;
namespace Archivist.Data
{
    public class DataBuider
    {
        private static Database _database = null;

        static DataBuider()
        {
            try
            {
                _database = DatabaseCreatorFactory.CreateDatabase();
            }
            catch (Exception excep)
            {
                throw excep;
            }
        }

        public static Database database
        {
            get { return _database; }
        }
    }
}
