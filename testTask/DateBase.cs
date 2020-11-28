using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using System.Data;

namespace testTask
{
    public class DataBase
    {
        private const string ConnectionString = @".\database.sqlite3";
        public SQLiteConnection myConnection;

        public DataBase()
        {
            using (myConnection = new SQLiteConnection("Data Source="+ConnectionString))
            {
                SQLiteCommand cmd = myConnection.CreateCommand();
                string cmdText = "Select * from line";
                cmd.CommandText = cmdText;
                try
                {
                    cmd.ExecuteNonQuery();
                    Console.WriteLine(123);
                }
                catch(SQLiteException ex)
                {
                    Console.WriteLine(ex.Message);
                }
            }

            
                
        }

        

    }
}
