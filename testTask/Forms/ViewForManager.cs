using System;
using System.Data;
using System.Data.SQLite;
using System.Linq;
using System.Windows.Forms;

namespace testTask
{
    public partial class ViewForManager : Form
    {
        private readonly string documentId;
        private readonly DataBase.AccessRight access;
        
        private DataBase db;
        private DataSet documentsData;

        public ViewForManager(string documentId, DataBase.AccessRight access)
        {
            this.documentId = documentId;
            this.access = access;
            InitializeComponent();
        }

        private void dataGridView1_CellContentClick(object sender, DataGridViewCellEventArgs e)
        {

        }

        private void ViewForManager_Load(object sender, EventArgs e)
        {

            db = new DataBase();
            documentsData = db.GenerateDocumentsData();

            var lines = documentsData.Tables["lines"];


            lines.Columns["id"].AutoIncrement = true;
            lines.Columns["staffListNumber"].DefaultValue = documentId;
            var view = new DataView(lines) { RowFilter = $"staffListNumber = \'{documentId}\'"};

            departmentsId.DataSource = documentsData.Tables["departments"];
            departmentsId.DisplayMember = "Name";
            departmentsId.ValueMember = "departmentId";

            position.DataSource = documentsData.Tables["positions"];
            position.DisplayMember = "Name";
            position.ValueMember = "id";

            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = view;
            dataGridView1.ReadOnly = access == DataBase.AccessRight.ReadPrint;

            saveMenuItem.Enabled = access == DataBase.AccessRight.WriteEdit;
            printMenuItem.Enabled = access == DataBase.AccessRight.ReadPrint;
        }

        private void printMenuItem_Click(object sender, EventArgs e)
        {
            //TODO
        }

        private void saveMenuItem_Click(object sender, EventArgs e)
        {
            db.CommitLinesChanges(((DataView)dataGridView1.DataSource).Table);
        }
    }
}
