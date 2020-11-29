using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Data.SQLite;
using System.IO;
using System.Data;
using System.Data.SqlClient;

namespace testTask
{
    public class DataBase
    {
        private const string ConnectionString = @".\database.sqlite3";

        public DataSet GenerateDocumentsData()
        {
            using (var databaseConnection = new SQLiteConnection($"Data Source={ConnectionString}"))
            {
                DataSet dataSet = new DataSet();

                using (var departmentsDataAdapter = new SQLiteDataAdapter("SELECT departmentId, Name from departments", databaseConnection) { MissingSchemaAction = MissingSchemaAction.AddWithKey })
                using( var positionsDataAdapter = new SQLiteDataAdapter("SELECT id,Name from position", databaseConnection) { MissingSchemaAction = MissingSchemaAction.AddWithKey })
                using (var linesDataAdapter =
                    new SQLiteDataAdapter(
                        "SELECT id,departmentsId,position,salary,allowance1,allowance2,allowance3,staffListNumber,something,count FROM line",
                        databaseConnection) {MissingSchemaAction = MissingSchemaAction.AddWithKey})
                {
                    departmentsDataAdapter.Fill(dataSet, "departments");
                    positionsDataAdapter.Fill(dataSet, "positions");
                    linesDataAdapter.Fill(dataSet, "lines");
                }

                dataSet.Relations.Add("LinesDepartments", dataSet.Tables["departments"].Columns["departmentId"],
                    dataSet.Tables["lines"].Columns["departmentsId"]);
                dataSet.Relations.Add("LinesPositions", dataSet.Tables["positions"].Columns["id"],
                    dataSet.Tables["lines"].Columns["position"]);

                var sum = new DataColumn("Сумма", typeof(float), "count * ( salary + allowance1 + allowance2 + allowance3 )");
                // var departmentName = new DataColumn("Poop 1", typeof(string), "Child(LinesDepartments).departmentsId");

                dataSet.Tables["lines"].Columns.AddRange(new[] { sum});

                return dataSet;
            }
        }

        public DataTable GenerateDocumentList()
        {
            using (var databaseConnection = new SQLiteConnection($"Data Source={ConnectionString}"))
            using (var adapter = new SQLiteDataAdapter("SELECT number,date,signatory,amounted,copy FROM staffList", databaseConnection))
            {
                var documents = new DataTable();
                adapter.Fill(documents);
                return documents;
            }
            
        }

        public void CommitLinesChanges(DataTable lines)
        {
            using (var databaseConnection = new SQLiteConnection($"Data Source={ConnectionString}"))
            using (var adapter =
                new SQLiteDataAdapter(
                    "SELECT id,departmentsId,position,salary,allowance1,allowance2,allowance3,staffListNumber,something,count FROM line",
                    databaseConnection) {MissingSchemaAction = MissingSchemaAction.AddWithKey})
            {
                var builder = new SQLiteCommandBuilder(adapter);
                adapter.UpdateCommand = builder.GetUpdateCommand();
                adapter.Update(lines);
            }
        }

        public void CommitDocumentsChanges(DataTable documents)
        {
            using (var databaseConnection = new SQLiteConnection($"Data Source={ConnectionString}"))
            using (var adapter = new SQLiteDataAdapter("SELECT number,date,signatory,amounted,copy FROM staffList", databaseConnection) { MissingSchemaAction = MissingSchemaAction.AddWithKey })
            {
                var builder = new SQLiteCommandBuilder(adapter);
                adapter.UpdateCommand = builder.GetUpdateCommand();
                adapter.Update(documents);
            }
        }
        public DataTable GetTable(string command)
        {
            using (var databaseConnection = new SQLiteConnection($"Data Source={ConnectionString}"))
            {
                DataTable table = new DataTable {Locale = System.Globalization.CultureInfo.InvariantCulture};

                using (SQLiteCommand cmd = new SQLiteCommand(command, databaseConnection))
                using (SQLiteDataAdapter adapter = new SQLiteDataAdapter())
                {
                    adapter.SelectCommand = cmd;
                    adapter.Fill(table);
                }
                return table;
            }
        }

        public enum AccessRight
        {
            WriteEdit,
            ReadPrint,
            Denied
        }

        public AccessRight GetUserAccessRight(string user)
        {
            using (var databaseConnection = new SQLiteConnection($"Data Source={ConnectionString}"))
            {
                databaseConnection.Open();

                AccessRight status;
                using (SQLiteCommand cmd =
                    new SQLiteCommand($"select name, accessRight from users where name=\"{user}\"", databaseConnection))
                using(var reader = cmd.ExecuteReader()){

                    if (!reader.Read())
                        status = AccessRight.Denied;
                    else status = (AccessRight) reader.GetInt32(1);
                }
                return status;
            }
        }

    }
}
