using MySql.Data.MySqlClient;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace курсова2
{
    public partial class Form4 : Form
    {
        private int userID;
        private string login;
        private bool isPaymentConfirmed = false;

        public Form4(int userID, string login)
        {
            this.FormBorderStyle = FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            InitializeComponent();
            this.userID = userID;
            this.login = login;
            LoadCartItems();
            LoadUserData();
            button1.Click += Button1_Click;
            button2.Click += Button2_Click;
            button3.Click += Button3_Click;
            dateTimePicker1.Format = DateTimePickerFormat.Custom;
            dateTimePicker1.CustomFormat = "dd.MM.yyyy HH:mm";
            dateTimePicker1.ShowUpDown = true;
            pictureBox1.Click += pictureBox1_Click;
            pictureBox2.Click += pictureBox2_Click;
            this.ActiveControl = labelHidden;
        }

        private void LoadUserData()
        {
            using var conn = Database.GetConnection();
            conn.Open();
            string query = "SELECT name, surname, phone_number FROM users WHERE user_id = @userID";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@userID", userID);
            using var reader = cmd.ExecuteReader();
            if (reader.Read())
            {
                textBox1.Text = reader.IsDBNull(0) ? "" : reader.GetString(0);
                textBox2.Text = reader.IsDBNull(1) ? "" : reader.GetString(1);
                textBox4.Text = reader.IsDBNull(2) ? "" : reader.GetString(2);
            }
        }

        private void LoadCartItems()
        {
            flowLayoutPanel1.Controls.Clear();
            decimal total = 0;
            using var conn = Database.GetConnection();
            conn.Open();
            string query = @"
        SELECT d.dish_id, d.dish_name, d.description, d.price, d.image, c.quantility
        FROM cart c
        JOIN dishes d ON c.dish_id = d.dish_id
        WHERE c.user_id = @userID";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@userID", userID);
            using var reader = cmd.ExecuteReader();
            while (reader.Read())
            {
                int dishId = reader.GetInt32("dish_id");
                string name = reader.GetString("dish_name");
                string description = reader.GetString("description");
                decimal price = reader.GetDecimal("price");
                int quantity = reader.GetInt32("quantility");

                Image image = null;
                if (!reader.IsDBNull(reader.GetOrdinal("image")))
                {
                    string imagePath = reader.GetString("image");
                    if (!string.IsNullOrEmpty(imagePath) && File.Exists(imagePath))
                    {
                        try
                        {
                            image = Image.FromFile(imagePath);
                        }
                        catch
                        {
                            image = null;
                        }
                    }
                }

                var panel = CreateCartItemPanel(dishId, name, description, price, image, quantity);
                flowLayoutPanel1.Controls.Add(panel);
                total += price * quantity;
            }
            label2.Text = $"Всього: {total} грн";
        }

        private Panel CreateCartItemPanel(int dishId, string name, string description, decimal price, Image image, int quantity)
        {
            Panel panel = new Panel
            {
                Width = 550,
                Height = 162,
                Margin = new Padding(10),
                BorderStyle = BorderStyle.FixedSingle,
                BackColor = Color.White
            };

            PictureBox pic = new PictureBox
            {
                Image = image,
                SizeMode = PictureBoxSizeMode.StretchImage,
                Location = new Point(10, 10),
                Size = new Size(140, 140),
                BackColor = image == null ? Color.LightGray : Color.Transparent,
                BorderStyle = image == null ? BorderStyle.FixedSingle : BorderStyle.None
            };

            Label lblName = new Label
            {
                Text = name,
                Font = new Font("Times New Roman", 14, FontStyle.Bold),
                Location = new Point(160, 10),
                AutoSize = true
            };

            Label lblDescription = new Label
            {
                Text = description,
                Font = new Font("Times New Roman", 11, FontStyle.Regular),
                Location = new Point(160, 40),
                Size = new Size(370, 40),
                AutoEllipsis = true
            };

            Label lblPrice = new Label
            {
                Text = $"Ціна: {price} грн",
                Font = new Font("Times New Roman", 11, FontStyle.Regular),
                Location = new Point(160, 112),
                AutoSize = true
            };

            Label lblQuantity = new Label
            {
                Text = $"Кількість: {quantity}",
                Font = new Font("Times New Roman", 11, FontStyle.Regular),
                Location = new Point(160, 132),
                AutoSize = true
            };

            Label lblSum = new Label
            {
                Text = $"Сума: {price * quantity} грн",
                Font = new Font("Times New Roman", 11, FontStyle.Regular),
                Location = new Point(270, 132),
                AutoSize = true
            };

            Button btnPlus = new Button
            {
                Text = "+",
                Font = new Font("Times New Roman", 11, FontStyle.Regular),
                Size = new Size(30, 25),
                Location = new Point(400, 127)
            };

            Button btnMinus = new Button
            {
                Text = "-",
                Font = new Font("Times New Roman", 10, FontStyle.Regular),
                Size = new Size(30, 25),
                Location = new Point(440, 127)
            };

            btnPlus.Click += (s, e) =>
            {
                quantity++;
                lblQuantity.Text = $"Кількість: {quantity}";
                lblSum.Text = $"Сума: {price * quantity} грн";
                UpdateCartQuantity(dishId, quantity);
                RecalculateTotal();
            };

            btnMinus.Click += (s, e) =>
            {
                if (quantity > 1)
                {
                    quantity--;
                    lblQuantity.Text = $"Кількість: {quantity}";
                    lblSum.Text = $"Сума: {price * quantity} грн";
                    UpdateCartQuantity(dishId, quantity);
                    RecalculateTotal();
                }
                else
                {
                    RemoveFromCart(dishId);
                    LoadCartItems();
                }
            };

            panel.Controls.Add(pic);
            panel.Controls.Add(lblName);
            panel.Controls.Add(lblDescription);
            panel.Controls.Add(lblPrice);
            panel.Controls.Add(lblQuantity);
            panel.Controls.Add(lblSum);
            panel.Controls.Add(btnPlus);
            panel.Controls.Add(btnMinus);

            return panel;
        }


        private void UpdateCartQuantity(int dishId, int quantity)
        {
            using var conn = Database.GetConnection();
            conn.Open();
            string query = "UPDATE cart SET quantility = @quantity WHERE user_id = @userID AND dish_id = @dishId";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@quantity", quantity);
            cmd.Parameters.AddWithValue("@userID", userID);
            cmd.Parameters.AddWithValue("@dishId", dishId);
            cmd.ExecuteNonQuery();
        }

        private void RemoveFromCart(int dishId)
        {
            using var conn = Database.GetConnection();
            conn.Open();
            string query = "DELETE FROM cart WHERE user_id = @userID AND dish_id = @dishId";
            using var cmd = new MySqlCommand(query, conn);
            cmd.Parameters.AddWithValue("@userID", userID);
            cmd.Parameters.AddWithValue("@dishId", dishId);
            cmd.ExecuteNonQuery();
        }

        private void RecalculateTotal()
        {
            decimal total = 0;
            foreach (Control ctrl in flowLayoutPanel1.Controls)
            {
                foreach (Control sub in ctrl.Controls)
                {
                    if (sub is Label lbl && lbl.Text.StartsWith("Сума:"))
                    {
                        string sumText = lbl.Text.Replace("Сума:", "").Replace("грн", "").Trim();
                        if (decimal.TryParse(sumText, out decimal sum))
                            total += sum;
                    }
                }
            }
            label2.Text = $"Всього: {total} грн";
        }

        private void Button1_Click(object sender, EventArgs e)
        {
            Hide();
            new Form2(userID, login).Show();
        }

        private void Button2_Click(object sender, EventArgs e)
        {
            Hide();
            new Form3(userID, login).Show();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            isPaymentConfirmed = true;
            MessageBox.Show("Успiх! Оплата успiшно проведена.", "Оплата", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }

        private void pictureBox2_Click(object sender, EventArgs e)
        {
            isPaymentConfirmed = true;
            MessageBox.Show("Успiх! Оплата успiшно проведена.", "Оплата", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
        private void Button3_Click(object sender, EventArgs e)
        {
            if (flowLayoutPanel1.Controls.Count == 0)
            {
                MessageBox.Show("Ваша корзина пуста.", "Увага", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            string userName = textBox1.Text.Trim();
            string userSurname = textBox2.Text.Trim();
            string userPhone = textBox4.Text.Trim();
            string address = textBox3.Text.Trim();
            DateTime orderDate = dateTimePicker1.Value;
            DateTime createdAt = DateTime.Now;
            if ((orderDate - createdAt).TotalMinutes < 30)
            {
                MessageBox.Show("Замовлення повинно бути зроблене щонайменше за 30 хвилин до бажаного часу доставки.", "Увага", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(userSurname) || string.IsNullOrEmpty(userPhone) || string.IsNullOrEmpty(address))
            {
                MessageBox.Show("Будь ласка, заповніть всі поля.", "Увага", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }

            using var conn = Database.GetConnection();
            conn.Open();

            string cartQuery = @"
SELECT d.dish_id, d.dish_name, d.price, c.quantility
FROM cart c
JOIN dishes d ON c.dish_id = d.dish_id
WHERE c.user_id = @userID";

            using var cartCmd = new MySqlCommand(cartQuery, conn);
            cartCmd.Parameters.AddWithValue("@userID", userID);

            using var reader = cartCmd.ExecuteReader();

            List<int> dishIds = new();
            List<string> dishNames = new();
            List<int> quantities = new();
            List<decimal> sums = new();

            while (reader.Read())
            {
                dishIds.Add(reader.GetInt32("dish_id"));
                dishNames.Add(reader.GetString("dish_name"));
                int quantity = reader.GetInt32("quantility");
                quantities.Add(quantity);
                decimal price = reader.GetDecimal("price");
                sums.Add(price * quantity);
            }
            reader.Close();

            if (dishIds.Count == 0)
            {
                MessageBox.Show("Ваша корзина пуста.");
                return;
            }

            decimal totalSum = 0;
            foreach (var sum in sums)
                totalSum += sum;

            using var transaction = conn.BeginTransaction();
            try
            {
                string dishIdsStr = string.Join(",", dishIds);
                string dishNamesStr = string.Join(",", dishNames);
                string quantitiesStr = string.Join(",", quantities);

                string insertQuery = @"
INSERT INTO orders
(user_id, dish_id, order_date, created_at, quantility, dish_name, user_name, user_surname, user_phone, adress, sum)
VALUES
(@user_id, @dish_id, @order_date, @created_at, @quantility, @dish_name, @user_name, @user_surname, @user_phone, @adress, @sum)";

                using var insertCmd = new MySqlCommand(insertQuery, conn, transaction);
                insertCmd.Parameters.AddWithValue("@user_id", userID);
                insertCmd.Parameters.AddWithValue("@dish_id", dishIdsStr);
                insertCmd.Parameters.AddWithValue("@order_date", orderDate);
                insertCmd.Parameters.AddWithValue("@created_at", createdAt);
                insertCmd.Parameters.AddWithValue("@quantility", quantitiesStr);
                insertCmd.Parameters.AddWithValue("@dish_name", dishNamesStr);
                insertCmd.Parameters.AddWithValue("@user_name", userName);
                insertCmd.Parameters.AddWithValue("@user_surname", userSurname);
                insertCmd.Parameters.AddWithValue("@user_phone", userPhone);
                insertCmd.Parameters.AddWithValue("@adress", address);
                insertCmd.Parameters.AddWithValue("@sum", totalSum);

                insertCmd.ExecuteNonQuery();

                string clearCartQuery = "DELETE FROM cart WHERE user_id = @userID";
                using var clearCmd = new MySqlCommand(clearCartQuery, conn, transaction);
                clearCmd.Parameters.AddWithValue("@userID", userID);
                clearCmd.ExecuteNonQuery();

                if (!isPaymentConfirmed)
                {
                    MessageBox.Show("Перед оформленням замовлення необхідно провести оплату зручним для вас варiантом: Apple Pay ибо Google Pay.", "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    return;
                }

                transaction.Commit();

                MessageBox.Show("Замовлення успішно оформлено!", "Успіх", MessageBoxButtons.OK, MessageBoxIcon.Information);
                LoadCartItems();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                MessageBox.Show("Помилка при оформленні замовлення: " + ex.Message, "Помилка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }
    }
}