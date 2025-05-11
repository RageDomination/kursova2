using System;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace курсова2
{
    public partial class Form1 : Form
    {
        // Флаг, щоб відстежувати, чи знаходимося ми в режимі реєстрації або авторизації
        private bool isRegistrationMode = true;

        public Form1()
        {
            InitializeComponent();
        }

        // Перемикання на режим реєстрації
        private void SwitchToRegistrationMode()
        {
            isRegistrationMode = true;
            linkLabel1.Text = "Зареєструватися"; // Текст для посилання реєстрації
            button1.Text = "Зареєструватися"; // Кнопка для реєстрації
            label1.Text = "РЕЄСТРАЦІЯ"; // Текст в label1 для режиму реєстрації
        }

        // Перемикання на режим авторизації
        private void SwitchToLoginMode()
        {
            isRegistrationMode = false;
            linkLabel2.Text = "Авторизація"; // Текст для посилання авторизації
            button1.Text = "Увійти"; // Кнопка для авторизації
            label1.Text = "АВТОРИЗАЦІЯ"; // Текст в label1 для режиму авторизації
        }

        // Обробник для кнопки (реєстрація/авторизація)
        private void button1_Click(object sender, EventArgs e)
        {
            string login = textBox1.Text;  // Логін з textbox1
            string password = textBox2.Text;  // Пароль з textbox2

            // Строка підключення до бази даних MySQL
            string connectionString = "Server=localhost;Database=kursova;User=root;Password=15011989;";

            try
            {
                using (MySqlConnection conn = new MySqlConnection(connectionString))
                {
                    conn.Open();

                    if (isRegistrationMode)
                    {
                        // Режим реєстрації
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
                            // Додаємо нового користувача
                            string insertQuery = "INSERT INTO users (login, password) VALUES (@login, @password)";
                            MySqlCommand insertCmd = new MySqlCommand(insertQuery, conn);
                            insertCmd.Parameters.AddWithValue("@login", login);
                            insertCmd.Parameters.AddWithValue("@password", password);
                            insertCmd.ExecuteNonQuery();

                            MessageBox.Show("Реєстрація пройшла успішно!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);

                            // Після реєстрації перемикаємо на авторизацію
                            SwitchToLoginMode();
                        }
                    }
                    else
                    {
                        // Режим авторизації
                        string query = "SELECT COUNT(*) FROM users WHERE login = @login AND password = @password";
                        MySqlCommand cmd = new MySqlCommand(query, conn);
                        cmd.Parameters.AddWithValue("@login", login);
                        cmd.Parameters.AddWithValue("@password", password);

                        int userCount = Convert.ToInt32(cmd.ExecuteScalar());

                        if (userCount > 0)
                        {
                            // Успішна авторизація
                            Form2 form2 = new Form2();
                            form2.Show();
                            this.Hide(); // Приховуємо поточну форму
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

        // Обробник для кліка по посиланню "Реєстрація"
        private void linkLabel1_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SwitchToRegistrationMode();  // Перемикаємо на режим реєстрації
        }

        // Обробник для кліка по посиланню "Авторизація"
        private void linkLabel2_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            SwitchToLoginMode();  // Перемикаємо на режим авторизації
        }

        // Завантаження форми (за замовчуванням - авторизація)
        private void Form1_Load(object sender, EventArgs e)
        {
            SwitchToLoginMode();

            // Встановлюємо AutoSize для LinkLabel, щоб вони підстраювались під розмір тексту
            linkLabel1.AutoSize = true;
            linkLabel2.AutoSize = true;
        }
    }
}
