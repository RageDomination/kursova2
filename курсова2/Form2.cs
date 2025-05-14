using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

namespace курсова2
{
    public partial class Form2 : Form
    {
        private string selectedImagePath = "";
        private string currentLogin = "";
        private int currentUserID;

        public Form2(int userID, string login)
        {
            InitializeComponent();
            this.FormBorderStyle = FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            pictureBox1.Click += pictureBox1_Click;
            pictureBox2.Click += pictureBox2_Click;
            currentUserID = userID;
            currentLogin = login;
            this.buttonSave2.Click += new System.EventHandler(this.buttonSave2_Click);
            LoadUserInfo();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            label3.Text = $"UserID: {currentUserID}";
            this.ActiveControl = labelHidden;
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            OpenFileDialog openFileDialog = new OpenFileDialog
            {
                Filter = "Image Files (*.jpg; *.jpeg; *.png)|*.jpg;*.jpeg;*.png",
                Title = "Оберіть зображення профілю"
            };

            if (openFileDialog.ShowDialog() == DialogResult.OK)
            {
                selectedImagePath = openFileDialog.FileName;

                pictureBox1.Image = Image.FromFile(selectedImagePath);
                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;

                SaveImagePathToDatabase(selectedImagePath);
            }
        }

        private void LoadUserInfo()
        {
            string query = "SELECT login, profile_img, registration_date, name, surname, phone_number, email FROM users WHERE user_id = @userID";

            using (MySqlConnection conn = Database.GetConnection())
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@userID", currentUserID);

                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            currentLogin = reader["login"].ToString();
                            label2.Text = $"Login: {currentLogin}";

                            string profileImgPath = reader["profile_img"].ToString();
                            if (!string.IsNullOrEmpty(profileImgPath) && File.Exists(profileImgPath))
                            {
                                pictureBox1.Image = Image.FromFile(profileImgPath);
                                pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
                            }

                            if (reader["registration_date"] != DBNull.Value)
                            {
                                DateTime regDate = Convert.ToDateTime(reader["registration_date"]);
                                label4.Text = $"Registration date: {regDate:yyyy-MM-dd HH:mm:ss}";
                            }
                            else
                            {
                                label4.Text = "Registration date: xxxx-xx-xx **:**:**";
                            }

                            textBox1.Text = reader["name"]?.ToString() ?? "";
                            textBox2.Text = reader["surname"]?.ToString() ?? "";
                            textBox3.Text = reader["phone_number"]?.ToString() ?? "";
                            textBox4.Text = reader["email"]?.ToString() ?? "";
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Помилка при завантаженні даних: " + ex.Message);
                }
            }
        }


        private void SaveImagePathToDatabase(string imagePath)
        {
            string query = "UPDATE users SET profile_img = @imagePath WHERE user_id = @userID";

            using (MySqlConnection conn = Database.GetConnection())
            {
                try
                {
                    conn.Open();
                    MySqlCommand cmd = new MySqlCommand(query, conn);
                    cmd.Parameters.AddWithValue("@imagePath", imagePath);
                    cmd.Parameters.AddWithValue("@userID", currentUserID);
                    cmd.ExecuteNonQuery();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Помилка при збереженні зображення: " + ex.Message);
                }
            }
        }

        private void pictureBox1_Click_1(object sender, EventArgs e)
        {

        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            Application.Exit();
        }

        private void buttonSave_Click(object sender, EventArgs e)
        {
            string name = textBox1.Text;
            string surname = textBox2.Text;
            string phoneNumber = textBox3.Text;
            string email = textBox4.Text;

            string selectQuery = "SELECT name, surname, phone_number, email FROM users WHERE user_id = @userID";
            string updateQuery = "UPDATE users SET name = @name, surname = @surname, phone_number = @phone, email = @mail WHERE user_id = @userID";

            using (MySqlConnection conn = Database.GetConnection())
            {
                try
                {
                    conn.Open();
                    MySqlCommand selectCmd = new MySqlCommand(selectQuery, conn);
                    selectCmd.Parameters.AddWithValue("@userID", currentUserID);

                    using (MySqlDataReader reader = selectCmd.ExecuteReader())
                    {
                        if (reader.Read())
                        {
                            string currentName = reader["name"]?.ToString() ?? "";
                            string currentSurname = reader["surname"]?.ToString() ?? "";
                            string currentPhone = reader["phone_number"]?.ToString() ?? "";
                            string currentEmail = reader["email"]?.ToString() ?? "";
                            if (name == currentName && surname == currentSurname && phoneNumber == currentPhone && email == currentEmail)
                            {
                                MessageBox.Show("Жодних змін не було внесено.", "Інформація", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                                return;
                            }
                        }
                    }
                    MySqlCommand updateCmd = new MySqlCommand(updateQuery, conn);
                    updateCmd.Parameters.AddWithValue("@name", name);
                    updateCmd.Parameters.AddWithValue("@surname", surname);
                    updateCmd.Parameters.AddWithValue("@phone", phoneNumber);
                    updateCmd.Parameters.AddWithValue("@mail", email);
                    updateCmd.Parameters.AddWithValue("@userID", currentUserID);

                    updateCmd.ExecuteNonQuery();
                    MessageBox.Show("Дані успішно оновлено!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Помилка при збереженні даних: " + ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }
        private void buttonSave2_Click(object sender, EventArgs e)
        {
            string oldPassword = textBox5.Text;
            string newPassword = textBox6.Text;

            if (string.IsNullOrWhiteSpace(oldPassword) || string.IsNullOrWhiteSpace(newPassword))
            {
                MessageBox.Show("Будь ласка, заповніть обидва поля для зміни паролю.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string selectQuery = "SELECT password FROM users WHERE user_id = @userID";
            string updateQuery = "UPDATE users SET password = @newPassword WHERE user_id = @userID";

            using (MySqlConnection conn = Database.GetConnection())
            {
                try
                {
                    conn.Open();

                    MySqlCommand selectCmd = new MySqlCommand(selectQuery, conn);
                    selectCmd.Parameters.AddWithValue("@userID", currentUserID);

                    string currentPassword = selectCmd.ExecuteScalar()?.ToString();

                    if (currentPassword != oldPassword)
                    {
                        MessageBox.Show("Старий пароль введено неправильно.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                        return;
                    }

                    MySqlCommand updateCmd = new MySqlCommand(updateQuery, conn);
                    updateCmd.Parameters.AddWithValue("@newPassword", newPassword);
                    updateCmd.Parameters.AddWithValue("@userID", currentUserID);
                    updateCmd.ExecuteNonQuery();

                    MessageBox.Show("Пароль успішно змінено!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    textBox5.Clear();
                    textBox6.Clear();
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Помилка при зміні паролю: " + ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                }
            }
        }

    }
}
