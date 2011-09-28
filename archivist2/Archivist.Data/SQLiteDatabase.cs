
using System.Data.SQLite;
using System.Data;

namespace Archivist.Data
{
    public class SQLiteDatabase : Database
    {
        public override IDbConnection CreateConnection()
        {
            return new SQLiteConnection(connectionString);
        }

        public override IDbCommand CreateCommand()
        {
            return new SQLiteCommand();
        }

        public override IDbConnection CreateOpenConnection()
        {
            SQLiteConnection connection = (SQLiteConnection)CreateConnection();
            connection.Open();

            return connection;
        }

        public override IDbCommand CreateCommand(string commandText, IDbConnection connection)
        {
            SQLiteCommand command = (SQLiteCommand)CreateCommand();

            command.CommandText = commandText;
            command.Connection = (SQLiteConnection)connection;
            command.CommandType = CommandType.Text;

            return command;
        }

        public override IDbCommand CreateStoredProcCommand(string procName, IDbConnection connection)
        {
            SQLiteCommand command = (SQLiteCommand)CreateCommand();

            command.CommandText = procName;
            command.Connection = (SQLiteConnection)connection;
            command.CommandType = CommandType.StoredProcedure;

            return command;
        }

        public override IDataParameter CreateParameter(string parameterName, object parameterValue)
        {
            return new SQLiteParameter(parameterName, parameterValue);
        }

    }
}
