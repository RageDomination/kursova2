using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;
using MySql.Data.MySqlClient;

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

            this.button1.Click += button1_Click;
            this.Load += Form3_Load;
            this.pictureBox1.Click += pictureBox1_Click;
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            this.ActiveControl = labelHidden;

            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.Controls.Clear();
            flowLayoutPanel1.FlowDirection = FlowDirection.TopDown; // вертикальный поток
            flowLayoutPanel1.WrapContents = false;                 // не переносить на следующую строку


            try
            {
                using (var connection = Database.GetConnection())
                {
                    connection.Open();

                    string query = @"
                        SELECT d.dish_id, d.dish_name, d.price, d.description, d.image,
                            IF(c.quantility IS NULL, 0, c.quantility) AS quantility_in_cart
                        FROM dishes d
                        LEFT JOIN cart c ON d.dish_id = c.dish_id AND c.user_id = @userID
                        ORDER BY d.dish_id";

                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@userID", userID);
                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                int dishId = reader.GetInt32("dish_id");
                                string title = reader.GetString("dish_name");
                                decimal priceValue = reader.GetDecimal("price");
                                string description = reader.GetString("description");
                                int quantilityInCart = reader.GetInt32("quantility_in_cart");

                                Image dishImage = null;
                                if (!reader.IsDBNull(reader.GetOrdinal("image")))
                                {
                                    byte[] imgBytes = (byte[])reader["image"];
                                    using (var ms = new MemoryStream(imgBytes))
                                    {
                                        try
                                        {
                                            dishImage = Image.FromStream(ms);
                                        }
                                        catch
                                        {
                                            dishImage = null;
                                        }
                                    }
                                }

                                var card = CreateCard(dishId, title, priceValue, description, dishImage, quantilityInCart > 0);
                                flowLayoutPanel1.Controls.Add(card);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка завантаження даних з бази: " + ex.Message);
            }
        }

        private Panel CreateCard(int dishId, string title, decimal priceValue, string description, Image dishImage, bool inCart)
        {
            Panel panel = new Panel
            {
                Width = 550,
                Height = 160,
                Margin = new Padding(10),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            PictureBox pictureBox = new PictureBox
            {
                Size = new Size(140, 140),
                Location = new Point(10, 10),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = dishImage == null ? Color.LightGray : Color.Transparent,
                BorderStyle = dishImage == null ? BorderStyle.FixedSingle : BorderStyle.None,
                Image = dishImage
            };

            Label lblTitle = new Label
            {
                Text = title,
                Font = new Font("Arial", 13, FontStyle.Bold),
                AutoSize = false,
                Size = new Size(380, 30),
                TextAlign = ContentAlignment.MiddleCenter,
                Location = new Point(160, 10)
            };

            Label lblDescription = new Label
            {
                Text = description,
                Font = new Font("Arial", 10, FontStyle.Regular),
                Location = new Point(160, 45),
                Size = new Size(370, 60),
                AutoEllipsis = true
            };

            Label lblPrice = new Label
            {
                Text = $"Цiна: {priceValue} грн",
                Font = new Font("Arial", 11, FontStyle.Regular),
                Location = new Point(160, 110),
                AutoSize = true
            };

            CheckBox checkBox = new CheckBox
            {
                Text = "До кошику",
                Font = new Font("Arial", 10, FontStyle.Regular),
                AutoSize = true,
                BackColor = Color.Transparent,
                Checked = inCart
            };

            Button btnReviews = new Button
            {
                Text = "Переглянути відгуки",
                Size = new Size(160, 30)
            };

            panel.Controls.Add(pictureBox);
            panel.Controls.Add(lblTitle);
            panel.Controls.Add(lblDescription);
            panel.Controls.Add(lblPrice);
            panel.Controls.Add(checkBox);
            panel.Controls.Add(btnReviews);

            checkBox.Location = new Point(160, lblPrice.Bottom + 5);
            btnReviews.Location = new Point(panel.Width - btnReviews.Width - 20, panel.Height - btnReviews.Height - 15);
            checkBox.CheckedChanged += (s, e) =>
            {
                if (checkBox.Checked)
                    AddToCart(dishId);
                else
                    RemoveFromCart(dishId);
            };

            btnReviews.Click += (s, e) =>
            {
                MessageBox.Show($"Показати відгуки для блюда ID: {dishId}");
            };

            pictureBox.Click += (s, e) =>
            {
                using (OpenFileDialog ofd = new OpenFileDialog())
                {
                    ofd.Filter = "Image Files|*.jpg;*.jpeg;*.png;*.bmp;*.gif";
                    if (ofd.ShowDialog() == DialogResult.OK)
                    {
                        try
                        {
                            Image img = Image.FromFile(ofd.FileName);
                            pictureBox.Image = img;
                            pictureBox.BackColor = Color.Transparent;
                            pictureBox.BorderStyle = BorderStyle.None;

                            SaveImageToDatabase(dishId, img);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Не вдалося завантажити зображення: " + ex.Message);
                        }
                    }
                }
            };

            return panel;
        }

        private void AddToCart(int dishId)
        {
            try
            {
                using (var connection = Database.GetConnection())
                {
                    connection.Open();
                    string query = @"
                        INSERT INTO cart (user_id, dish_id, quantility)
                        VALUES (@userID, @dishId, 1)
                        ON DUPLICATE KEY UPDATE quantility = quantility + 1";
                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@userID", userID);
                        cmd.Parameters.AddWithValue("@dishId", dishId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка додавання до кошика: " + ex.Message);
            }
        }

        private void RemoveFromCart(int dishId)
        {
            try
            {
                using (var connection = Database.GetConnection())
                {
                    connection.Open();
                    string query = "DELETE FROM cart WHERE user_id = @userID AND dish_id = @dishId";
                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@userID", userID);
                        cmd.Parameters.AddWithValue("@dishId", dishId);
                        cmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка видалення з кошика: " + ex.Message);
            }
        }

        private void SaveImageToDatabase(int dishId, Image image)
        {
            try
            {
                using (var connection = Database.GetConnection())
                {
                    connection.Open();

                    byte[] imgBytes;
                    using (MemoryStream ms = new MemoryStream())
                    {
                        image.Save(ms, System.Drawing.Imaging.ImageFormat.Png);
                        imgBytes = ms.ToArray();
                    }

                    string updateQuery = @"UPDATE dishes SET image = @image WHERE dish_id = @dishId";
                    using (var updateCmd = new MySqlCommand(updateQuery, connection))
                    {
                        updateCmd.Parameters.AddWithValue("@dishId", dishId);
                        updateCmd.Parameters.AddWithValue("@image", imgBytes);
                        updateCmd.ExecuteNonQuery();
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка збереження зображення в базу: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 f2 = new Form2(userID, login);
            f2.Show();
            this.Hide();
        }

        private void pictureBox1_Click(object sender, EventArgs e)
        {
            Form4 form4 = new Form4(userID, login);
            form4.Show();
            this.Hide();
        }
    }
}
