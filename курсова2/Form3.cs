using System;
using System.Windows.Forms;

namespace курсова2
{
    public partial class Form3 : Form
    {
        private int userID;
        private string login;

        public Form3(int userID, string login)
        {
            InitializeComponent();
            this.userID = userID;
            this.login = login;
            this.button1.Click += new EventHandler(this.button1_Click);

            this.Load += Form3_Load;
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            this.ActiveControl = labelHidden;
        }

        private void button2_Click(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2(userID, login);
            form2.FormClosed += (s, args) => this.Close();
            this.Hide();
            form2.Show();
        }
    }
}
