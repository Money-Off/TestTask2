using System;
using System.Data;
using System.Windows.Forms;

namespace testTask
{
    public partial class ListOfDocuments : Form
    {
        private readonly DataBase.AccessRight access;

        public ListOfDocuments(DataBase.AccessRight accessRight)
        {
            access = accessRight;
            InitializeComponent();
        }



        private void ListOfDocuments_Load(object sender, EventArgs e)
        {
            var db = new DataBase();
            var table = db.GenerateDocumentList();
            dataGridView1.AutoGenerateColumns = false;
            dataGridView1.DataSource = table;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var documentId = string.Empty;
            if (dataGridView1.SelectedRows.Count > 0)
                documentId = (string)dataGridView1.SelectedRows[0].Cells[0].Value;
            else if (dataGridView1.SelectedCells.Count > 0)
            {
                var rowIndex = dataGridView1.SelectedCells[0].RowIndex;
                if (rowIndex != -1)
                    documentId = (string)dataGridView1.Rows[rowIndex].Cells[0].Value;
            }
            ViewForManager f = new ViewForManager(documentId, access);
            this.FormClosed += (_, __) => f.Close();
            f.Show();
        }

        private void CommitChanges()
        {
            var db = new DataBase();
            db.CommitDocumentsChanges((DataTable)dataGridView1.DataSource);

        }

        private void button_Save_Click(object sender, EventArgs e)
        {
            CommitChanges();
        }
    }
}
