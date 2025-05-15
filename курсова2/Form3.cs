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
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            this.ActiveControl = labelHidden;

            flowLayoutPanel1.FlowDirection = FlowDirection.LeftToRight;
            flowLayoutPanel1.WrapContents = false;
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.Controls.Clear();

            try
            {
                using (var connection = Database.GetConnection())
                {
                    connection.Open();

                    string query = "SELECT dish_id, dish_name, price, description, image FROM dishes ORDER BY dish_id";

                    using (var cmd = new MySqlCommand(query, connection))
                    using (var reader = cmd.ExecuteReader())
                    {
                        while (reader.Read())
                        {
                            int dishId = reader.GetInt32("dish_id");
                            string title = reader.GetString("dish_name");
                            int priceValue = reader.GetInt32("price");
                            string description = reader.GetString("description");
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

                            var card = CreateCard(dishId, title, priceValue, description, dishImage);
                            flowLayoutPanel1.Controls.Add(card);
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка завантаження даних з бази: " + ex.Message);
            }
        }

        private Panel CreateCard(int dishId, string title, int priceValue, string description, Image dishImage)
        {
            Panel panel = new Panel
            {
                Size = new Size(180, 230),
                Margin = new Padding(10),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle
            };

            PictureBox pictureBox = new PictureBox
            {
                Size = new Size(160, 90),
                Location = new Point(10, 10),
                SizeMode = PictureBoxSizeMode.Zoom,
                BackColor = dishImage == null ? Color.LightGray : Color.Transparent,
                BorderStyle = dishImage == null ? BorderStyle.FixedSingle : BorderStyle.None,
                Image = dishImage
            };

            if (dishImage == null)
            {
                pictureBox.Paint += (s, e) =>
                {

                };
            }

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
                            pictureBox.Paint -= null;

                            SaveImageToDatabase(dishId, img);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Не вдалося завантажити зображення: " + ex.Message);
                        }
                    }
                }
            };

            Label lblTitle = new Label
            {
                Text = title,
                Font = new Font("Arial", 11, FontStyle.Bold),
                Location = new Point(10, 105),
                Size = new Size(160, 20),
                AutoEllipsis = true
            };

            Label lblDescription = new Label
            {
                Text = description,
                Font = new Font("Arial", 9, FontStyle.Regular),
                Location = new Point(10, 125),
                Size = new Size(160, 40),
                AutoEllipsis = true
            };

            Label lblPrice = new Label
            {
                Text = $"{priceValue} грн",
                Font = new Font("Arial", 10, FontStyle.Regular),
                Location = new Point(10, 170),
                AutoSize = true
            };

            CheckBox checkBox = new CheckBox
            {
                Text = "До кошику",
                Font = new Font("Arial", 9, FontStyle.Regular),
                AutoSize = true,
                Location = new Point(panel.Width - 90, 168),
                BackColor = Color.Transparent
            };

            Button btnReviews = new Button
            {
                Text = "Переглянути відгуки",
                Size = new Size(160, 30),
                Location = new Point(10, 190)
            };

            btnReviews.Click += (s, e) =>
            {
                MessageBox.Show($"Показати відгуки для блюда ID: {dishId}");
            };

            panel.Controls.Add(pictureBox);
            panel.Controls.Add(lblTitle);
            panel.Controls.Add(lblDescription);
            panel.Controls.Add(lblPrice);
            panel.Controls.Add(checkBox);
            panel.Controls.Add(btnReviews);

            return panel;
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
                        int rowsAffected = updateCmd.ExecuteNonQuery();

                        if (rowsAffected == 0)
                        {
                            string insertQuery = @"INSERT INTO dishes (dish_id, image) VALUES (@dishId, @image)";
                            using (var insertCmd = new MySqlCommand(insertQuery, connection))
                            {
                                insertCmd.Parameters.AddWithValue("@dishId", dishId);
                                insertCmd.Parameters.AddWithValue("@image", imgBytes);
                                insertCmd.ExecuteNonQuery();
                            }
                        }
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
    }
}
