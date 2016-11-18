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
    }
}
