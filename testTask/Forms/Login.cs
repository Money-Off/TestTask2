using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace testTask
{
    public partial class Login : Form
    {

        public Login()
        {
            InitializeComponent();
        }

        private void button1_Click(object sender, EventArgs e)
        {
            var db = new DataBase();
            var access = db.GetUserAccessRight(user.Text);
            if ((DataBase.AccessRight) access == DataBase.AccessRight.Denied)
                MessageBox.Show("Пользователь не найден", "Доступ запрещен");
            else
            {
                var form = new ListOfDocuments(access);
                form.FormClosed += (_, __) => this.Close();
                form.Show();
                this.Hide();
            }
        }

        private void Login_Load(object sender, EventArgs e)
        {

        }
    }
}
