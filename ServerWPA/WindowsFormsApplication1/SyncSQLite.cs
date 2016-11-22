using System;
using System.Collections.Generic;
using System.Data.SQLite;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WindowsFormsApplication1
{
    class SyncSQLite
    {


        private const string DEFAULT_DATABASE_FILE = "MyDatabase.sqlite";
        private string databaseFile;
        private SQLiteConnection connection;

        public SyncSQLite() : this(DEFAULT_DATABASE_FILE) { }
        public SyncSQLite(string databaseFile)
        {
            bool newDatabase = false;
            this.databaseFile = databaseFile;
            if (!File.Exists(this.databaseFile))
            {
                SQLiteConnection.CreateFile(this.databaseFile);
                newDatabase = true;
            }
            connection = new SQLiteConnection("Data Source=" + this.databaseFile + ";Version=3;");
            connection.Open();

            if (newDatabase)
            {
                this.initDatabaseStructure();
            }
        }

        private int executeQuery(string query)
        {
            SQLiteCommand command = new SQLiteCommand(query, connection);
            return command.ExecuteNonQuery();
        }

        private int executeQuery(string query, Object param1)
        {
            // Example: "SELECT something FROM tabletop WHERE color = @param1"
            SQLiteCommand command = new SQLiteCommand(query, connection);
            command.Parameters.AddWithValue("param1", param1);
            return command.ExecuteNonQuery();
        }

        private void initDatabaseStructure()
        {
            this.executeQuery("CREATE TABLE users (id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, username TEXT NOT NULL UNIQUE, password TEXT NOT NULL, user_dir TEXT NOT NULL);");
        }

        public class UserVersions
        {
            public Int64 userId { get; set; }
            public string username { get; set; }
            public Int64 versionCount { get; set; }
        }

        public Int64 authenticateUser(string username, string password)
        {
            Int64 userId = -1;
            SQLiteCommand command = new SQLiteCommand("SELECT * FROM users WHERE username = @username AND password = @password", connection);
            command.Parameters.AddWithValue("username", username);
            command.Parameters.AddWithValue("password", password);
            SQLiteDataReader reader = command.ExecuteReader();
            if (reader.Read())
            {
                // there at least a row
                userId = (Int64)reader["id"];
            }
            reader.Close();
            return userId;
        }

        public Int64 newUser(string username, string password, string directory)
        {
            // test if there is an user with the same username
            bool usernameAlereadyUsed;
            SQLiteCommand command = new SQLiteCommand("SELECT * FROM users WHERE username = @username", connection);
            command.Parameters.AddWithValue("username", username);
            SQLiteDataReader reader = command.ExecuteReader();
            usernameAlereadyUsed = reader.Read();
            reader.Close();
            if (usernameAlereadyUsed)
            {
                return -1;
            }
            // create a new user
            command = new SQLiteCommand("INSERT INTO users (username, password, user_dir) VALUES (@username, @password, @directory)", connection);
            command.Parameters.AddWithValue("username", username);
            command.Parameters.AddWithValue("password", password);
            command.Parameters.AddWithValue("directory", directory);
            command.ExecuteNonQuery();
            command = new SQLiteCommand("select last_insert_rowid()", connection);
            Int64 lastId = (Int64)command.ExecuteScalar();

            // todo create a table with the right name
            this.executeQuery("CREATE TABLE user_" + lastId + " (id INTEGER NOT NULL PRIMARY KEY AUTOINCREMENT, version INTEGER NOT NULL, server_file TEXT NOT NULL, client_file TEXT NOT NULL, checksum BLOB NOT NULL, timestamp TEXT NOT NULL);");
            return lastId;
        }
    }
}
