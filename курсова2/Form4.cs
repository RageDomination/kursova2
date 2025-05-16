using MySql.Data.MySqlClient;
using System;
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

            this.button1.Click += button1_Click; // Кнопка назад на Form2
            this.button2.Click += button2_Click; // Кнопка назад на Form3
        }

        private void LoadCartItems()
        {
            flowLayoutPanel1.Controls.Clear();
            decimal total = 0;

            using (var conn = Database.GetConnection())
            {
                conn.Open();

                string query = @"
            SELECT d.dish_id, d.dish_name, d.description, d.price, d.image, c.quantility
            FROM cart c
            JOIN dishes d ON c.dish_id = d.dish_id
            WHERE c.user_id = @userID";

                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@userID", userID);

                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int dishId = reader.GetInt32("dish_id");
                            string name = reader.GetString("dish_name");
                            string description = reader.GetString("description");
                            decimal price = reader.GetDecimal("price");
                            int quantility = reader.GetInt32("quantility");

                            Image image = null;
                            if (!reader.IsDBNull(reader.GetOrdinal("image")))
                            {
                                byte[] imageBytes = (byte[])reader["image"];
                                using (MemoryStream ms = new MemoryStream(imageBytes))
                                {
                                    image = Image.FromStream(ms);
                                }
                            }

                            var panel = CreateCartItemPanel(dishId, name, description, price, image, quantility);
                            flowLayoutPanel1.Controls.Add(panel);

                            total += price * quantility;
                        }
                    }
                }
            }

            label2.Text = $"Всього: {total} грн";
        }


        private Panel CreateCartItemPanel(int dishId, string name, string description, decimal price, Image image, int quantity)
        {
            Panel panel = new Panel
            {
                Width = 500,
                Height = 140,
                Margin = new Padding(10),
                BorderStyle = BorderStyle.FixedSingle
            };

            PictureBox pic = new PictureBox
            {
                Image = image,
                SizeMode = PictureBoxSizeMode.Zoom,
                Location = new Point(10, 10),
                Size = new Size(100, 100)
            };

            Label lblName = new Label
            {
                Text = name,
                Font = new Font("Segoe UI", 10, FontStyle.Bold),
                Location = new Point(120, 10),
                AutoSize = true
            };

            Label lblDescription = new Label
            {
                Text = description,
                Location = new Point(120, 35),
                Size = new Size(250, 30)
            };

            Label lblPrice = new Label
            {
                Text = $"Ціна: {price} грн",
                Location = new Point(120, 70),
                AutoSize = true
            };

            Label lblQuantity = new Label
            {
                Text = $"Кількість: {quantity}",
                Location = new Point(120, 95),
                AutoSize = true
            };

            Label lblSum = new Label
            {
                Text = $"Сума: {price * quantity} грн",
                Location = new Point(220, 95),
                AutoSize = true
            };

            Button btnPlus = new Button
            {
                Text = "+",
                Size = new Size(30, 25),
                Location = new Point(350, 90)
            };

            Button btnMinus = new Button
            {
                Text = "-",
                Size = new Size(30, 25),
                Location = new Point(390, 90)
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
            using (var conn = Database.GetConnection())
            {
                conn.Open();
                string query = "UPDATE cart SET quantility = @quantity WHERE user_id = @userID AND dish_id = @dishId";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@quantity", quantity);
                    cmd.Parameters.AddWithValue("@userID", userID);
                    cmd.Parameters.AddWithValue("@dishId", dishId);
                    cmd.ExecuteNonQuery();
                }
            }
        }

        private void RemoveFromCart(int dishId)
        {
            using (var conn = Database.GetConnection())
            {
                conn.Open();
                string query = "DELETE FROM cart WHERE user_id = @userID AND dish_id = @dishId";
                using (var cmd = new MySqlCommand(query, conn))
                {
                    cmd.Parameters.AddWithValue("@userID", userID);
                    cmd.Parameters.AddWithValue("@dishId", dishId);
                    cmd.ExecuteNonQuery();
                }
            }
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
                        {
                            total += sum;
                        }
                    }
                }
            }
            label2.Text = $"Всього: {total} грн";
        }

        private void button1_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form2 form2 = new Form2(userID, login);
            form2.Show();
        }

        private void button2_Click(object sender, EventArgs e)
        {
            this.Hide();
            Form3 form3 = new Form3(userID, login);
            form3.Show();
        }
    }
}
