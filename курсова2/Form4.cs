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

        public Form4(int userID, string login)
        {
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
                    byte[] bytes = (byte[])reader["image"];
                    using MemoryStream ms = new MemoryStream(bytes);
                    image = Image.FromStream(ms);
                }
                var panel = CreateCartItemPanel(dishId, name, description, price, image, quantity);
                flowLayoutPanel1.Controls.Add(panel);
                total += price * quantity;
            }
            label2.Text = $"Всього: {total} грн";
        }

        private Panel CreateCartItemPanel(int dishId, string name, string description, decimal price, Image image, int quantity)
        {
            Panel panel = new Panel { Width = 500, Height = 140, Margin = new Padding(10), BorderStyle = BorderStyle.FixedSingle };
            PictureBox pic = new PictureBox { Image = image, SizeMode = PictureBoxSizeMode.Zoom, Location = new Point(10, 10), Size = new Size(100, 100) };
            Label lblName = new Label { Text = name, Font = new Font("Segoe UI", 10, FontStyle.Bold), Location = new Point(120, 10), AutoSize = true };
            Label lblDescription = new Label { Text = description, Location = new Point(120, 35), Size = new Size(250, 30) };
            Label lblPrice = new Label { Text = $"Ціна: {price} грн", Location = new Point(120, 70), AutoSize = true };
            Label lblQuantity = new Label { Text = $"Кількість: {quantity}", Location = new Point(120, 95), AutoSize = true };
            Label lblSum = new Label { Text = $"Сума: {price * quantity} грн", Location = new Point(220, 95), AutoSize = true };
            Button btnPlus = new Button { Text = "+", Size = new Size(30, 25), Location = new Point(350, 90) };
            Button btnMinus = new Button { Text = "-", Size = new Size(30, 25), Location = new Point(390, 90) };

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

        private void Button3_Click(object sender, EventArgs e)
        {
            if (flowLayoutPanel1.Controls.Count == 0)
            {
                MessageBox.Show("Ваша корзина пуста.");
                return;
            }

            string userName = textBox1.Text.Trim();
            string userSurname = textBox2.Text.Trim();
            string userPhone = textBox4.Text.Trim();
            string address = textBox3.Text.Trim();
            DateTime orderDate = dateTimePicker1.Value;

            if (string.IsNullOrEmpty(userName) || string.IsNullOrEmpty(userSurname) || string.IsNullOrEmpty(userPhone) || string.IsNullOrEmpty(address))
            {
                MessageBox.Show("Будь ласка, заповніть всі поля.");
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
            (user_id, dish_id, order_date, quantility, dish_name, user_name, user_surname, user_phone, adress, sum)
            VALUES
            (@user_id, @dish_id, @order_date, @quantility, @dish_name, @user_name, @user_surname, @user_phone, @adress, @sum)";

                using var insertCmd = new MySqlCommand(insertQuery, conn, transaction);
                insertCmd.Parameters.AddWithValue("@user_id", userID);
                insertCmd.Parameters.AddWithValue("@dish_id", dishIdsStr);
                insertCmd.Parameters.AddWithValue("@order_date", orderDate);
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

                transaction.Commit();

                MessageBox.Show("Замовлення успішно оформлено!");
                LoadCartItems();
            }
            catch (Exception ex)
            {
                transaction.Rollback();
                MessageBox.Show("Помилка при оформленні замовлення: " + ex.Message);
            }
        }
    }
}
