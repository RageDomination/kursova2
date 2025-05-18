using System;
using System.Data;
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
            tabPage1.Text = "Історія замовлень";
            tabPage2.Text = "Залишені коментарі";
            this.currentUserID = userID;
            this.currentLogin = login;
            this.buttonSave2.Click += new System.EventHandler(this.buttonSave2_Click);
            this.button2.Click += new System.EventHandler(this.button2_Click);
            LoadUserInfo();
            LoadOrders(currentUserID);
            LoadReviews();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            label3.Text = $"UserID: {currentUserID}";
            this.ActiveControl = labelHidden;
            tabControl1.SelectedIndexChanged += (s, e) =>
            {
                if (tabControl1.SelectedTab == tabPage2)
                {
                    LoadReviews();
                }
            };
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

        private void button2_Click(object sender, EventArgs e)
        {
            Form3 form3 = new Form3(currentUserID, currentLogin);
            form3.FormClosed += (s, args) => this.Show();
            this.Hide();
            form3.Show();
        }
        private void LoadOrders(int userId)
        {
            tabPage1.Controls.Clear();

            string query = "SELECT order_id, created_at FROM orders WHERE user_id = @userId ORDER BY created_at DESC";

            using (MySqlConnection conn = Database.GetConnection())
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@userId", userId);

                try
                {
                    conn.Open();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        int yPos = 10;

                        while (reader.Read())
                        {
                            int orderId = reader.GetInt32("order_id");
                            DateTime createdAt = reader.IsDBNull(reader.GetOrdinal("created_at"))
                                ? DateTime.MinValue
                                : reader.GetDateTime("created_at");

                            string formattedDate = createdAt != DateTime.MinValue
                                ? createdAt.ToString("dd.MM.yyyy HH:mm")
                                : "—";

                            LinkLabel link = new LinkLabel();
                            link.Text = $"Замовлення №{orderId}, {formattedDate}";
                            link.Tag = orderId;
                            link.Location = new Point(10, yPos);
                            link.AutoSize = true;
                            link.LinkClicked += OrderLink_LinkClicked;

                            tabPage1.Controls.Add(link);
                            yPos += 25;
                        }

                        if (yPos == 10)
                        {
                            Label noOrdersLabel = new Label();
                            noOrdersLabel.Text = "Замовлень поки що немає.";
                            noOrdersLabel.Location = new Point(10, yPos);
                            noOrdersLabel.AutoSize = true;
                            tabPage1.Controls.Add(noOrdersLabel);
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Помилка при завантаженні замовлень: " + ex.Message);
                }
            }
        }
        private void OrderLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LinkLabel link = sender as LinkLabel;
            if (link != null && link.Tag != null)
            {
                int orderId = (int)link.Tag;

                string query = "SELECT * FROM orders WHERE order_id = @orderId";

                using (MySqlConnection conn = Database.GetConnection())
                using (MySqlCommand cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@orderId", orderId);

                    try
                    {
                        conn.Open();
                        using (MySqlDataReader reader = cmd.ExecuteReader())
                        {
                            if (reader.Read())
                            {
                                string GetSafeString(string column) => reader[column] != DBNull.Value ? reader[column].ToString() : "—";
                                string GetSafeDate(string column) =>
                                    reader[column] != DBNull.Value ? Convert.ToDateTime(reader[column]).ToString("dd.MM.yyyy HH:mm") : "—";
                                string dishesRaw = GetSafeString("dish_name");
                                string quantitiesRaw = GetSafeString("quantility");
                                string[] dishes = dishesRaw.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                                string[] quantities = quantitiesRaw.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);

                                List<string> combinedList = new List<string>();

                                for (int i = 0; i < dishes.Length; i++)
                                {
                                    string dish = dishes[i].Trim();
                                    string quantity = (i < quantities.Length) ? quantities[i].Trim() : "1";
                                    combinedList.Add($"{dish} ({quantity} шт.)");
                                }

                                string dishesWithQuantities = string.Join(", ", combinedList);

                                string details = $"ID ордера: {GetSafeString("order_id")}\n" +
                                                 $"Дата та час ордера: {GetSafeDate("created_at")}\n" +
                                                 $"Дата та час замовлення: {GetSafeString("order_date")}\n" +
                                                 $"Позиції замовлення: {dishesWithQuantities}\n" +
                                                 $"Ім’я замовника: {GetSafeString("user_name")}\n" +
                                                 $"Прізвище замовника: {GetSafeString("user_surname")}\n" +
                                                 $"Телефон замовника: {GetSafeString("user_phone")}\n" +
                                                 $"Адреса: {GetSafeString("adress")}\n" +
                                                 $"Сума замовлення: {GetSafeString("sum")} грн";

                                MessageBox.Show(details, "Деталі замовлення");
                            }
                            else
                            {
                                MessageBox.Show("Замовлення не знайдено.", "Помилка");
                            }
                        }
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show("Помилка при завантаженні замовлення: " + ex.Message, "Помилка");
                    }
                }
            }
        }
        private void LoadReviews()
        {
            tabPage2.Controls.Clear();

            string query = @"
SELECT r.review_id, r.user_id, r.dish_id, d.dish_name, r.review_date
FROM reviews r
JOIN dishes d ON r.dish_id = d.dish_id
WHERE r.user_id = @userId
ORDER BY r.review_date DESC";

            using (MySqlConnection conn = Database.GetConnection())
            using (MySqlCommand cmd = new MySqlCommand(query, conn))
            {
                cmd.Parameters.AddWithValue("@userId", currentUserID);

                try
                {
                    conn.Open();
                    using (MySqlDataReader reader = cmd.ExecuteReader())
                    {
                        int y = 10;
                        while (reader.Read())
                        {
                            string dishName = reader["dish_name"] == DBNull.Value ? "Без назви" : reader["dish_name"].ToString();
                            int reviewId = Convert.ToInt32(reader["review_id"]);
                            int userId = Convert.ToInt32(reader["user_id"]);
                            int dishId = Convert.ToInt32(reader["dish_id"]);
                            DateTime reviewDate = reader["review_date"] == DBNull.Value ? DateTime.MinValue : Convert.ToDateTime(reader["review_date"]);

                            string linkText = $"Вiдгук №{reviewId} | {dishName} | {reviewDate:dd.MM.yyyy HH:mm:ss}";

                            LinkLabel link = new LinkLabel();
                            link.Text = linkText;
                            link.Tag = new { ReviewId = reviewId, UserId = userId, DishId = dishId };
                            link.Location = new Point(10, y);
                            link.AutoSize = true;
                            link.LinkClicked += ReviewLink_LinkClicked;

                            tabPage2.Controls.Add(link);
                            y += 25;
                        }
                    }
                }
                catch (Exception ex)
                {
                    MessageBox.Show("Помилка при завантаженні коментарів: " + ex.Message, "Помилка");
                }
            }
        }

        private void ReviewLink_LinkClicked(object sender, LinkLabelLinkClickedEventArgs e)
        {
            LinkLabel link = sender as LinkLabel;
            if (link != null && link.Tag != null)
            {
                dynamic data = link.Tag;
                int userId = data.UserId;
                int dishId = data.DishId;
                Form5 form5 = new Form5(dishId, userId);
                form5.Show();
            }
        }
    }
}