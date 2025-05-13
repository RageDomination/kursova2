using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace курсова2
{
    public partial class Form1 : Form
    {
        private bool isRegistrationMode = true;

        public Form1()
        {
            InitializeComponent();
        }

        private void SwitchToRegistrationMode()
        {
            isRegistrationMode = true;
            linkLabel1.Text = "Зареєструватися";
            button1.Text = "Зареєструватися";
            label1.Text = "РЕЄСТРАЦІЯ";
            label1.Left = (this.ClientSize.Width - label1.Width) / 2;
        }

        private void SwitchToLoginMode()
        {
            isRegistrationMode = false;
            linkLabel2.Text = "Авторизація";
            button1.Text = "Увійти";
            label1.Text = "АВТОРИЗАЦІЯ";
            label1.Left = (this.ClientSize.Width - label1.Width) / 2;
        }

        private void button1_Click(object sender, EventArgs e)
        {
            string login = textBox1.Text;
            string password = textBox2.Text;
            string connectionString = "Server=localhost;Database=kursova;User=root;Password=15011989;";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    if (isRegistrationMode)
                    {
                        string checkUserQuery = "SELECT COUNT(*) FROM users WHERE login = @login";
                        MySqlCommand checkCmd = new MySqlCommand(checkUserQuery, conn);
                        checkCmd.Parameters.AddWithValue("@login", login);
                        int userExists = Convert.ToInt32(checkCmd.ExecuteScalar());

                        if (userExists > 0)
                        {
                            MessageBox.Show("Користувач з таким логіном вже існує", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                        else
                        {
                            string insertQuery = "INSERT INTO users (login, password) VALUES (@login, @password)";
                            MySqlCommand insertCmd = new MySqlCommand(insertQuery, conn);
                            insertCmd.Parameters.AddWithValue("@login", login);
                            insertCmd.Parameters.AddWithValue("@password", password);
                            insertCmd.ExecuteNonQuery();

                            MessageBox.Show("Реєстрація пройшла успішно!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            SwitchToLoginMode();
                        }
                    }
                    else
                    {
                        string query = "SELECT user_id, login FROM users WHERE login = @login AND password = @password";
                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@login", login);
                        cmd.Parameters.AddWithValue("@password", password);

                        MySqlDataReader reader = cmd.ExecuteReader();

                        if (reader.Read())
                        {
                            int userID = reader.GetInt32("user_id");
                            string userLogin = reader.GetString("login");
                            Form2 form2 = new Form2(userID, userLogin);
                            form2.Show();
                            this.Hide();
                        }
                        else
                        {
                            MessageBox.Show("Невірний логін або пароль", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        }
                    }
                }
            }
            catch (MySqlException ex)
            {
                MessageBox.Show("Помилка при підключенні до бази даних: " + ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SwitchToRegistrationMode();
        }

        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SwitchToLoginMode();
        }

        private void Form1_Load(object sender, EventArgs e)
        {
            SwitchToLoginMode();
            linkLabel1.AutoSize = true;
            linkLabel2.AutoSize = true;
        }
    }
}
