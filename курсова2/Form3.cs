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
            this.button1.Click += new EventHandler(this.button1_Click);
            this.Load += Form3_Load;
        }

        private void Form3_Load(object sender, EventArgs e)
        {
            this.ActiveControl = labelHidden;
            flowLayoutPanel1.FlowDirection = FlowDirection.LeftToRight;
            flowLayoutPanel1.WrapContents = false;
            flowLayoutPanel1.AutoScroll = true;

            flowLayoutPanel1.Controls.Clear();

            var dishes = new[]
            {
        new { Title = "Суші Мікс", Price = "Ціна: 250 грн", Description = "Асорті свіжих суші з лососем, тунцем і креветками" },
        new { Title = "Рамен Тонкоцу", Price = "Ціна: 180 грн", Description = "Насичений бульйон з свининою, локшиною та овочами" },
        new { Title = "Темпура", Price = "Ціна: 200 грн", Description = "Хрусткі смажені овочі та морепродукти у легкому клярі" },
        new { Title = "Якитори", Price = "Ціна: 150 грн", Description = "Шашлички з курки, мариновані в соусі теріякі" },
        new { Title = "Унагі Дон", Price = "Ціна: 300 грн", Description = "Рис з смаженим вугрем у солодкому соусі" },
        new { Title = "Тонкацу", Price = "Ціна: 220 грн", Description = "Хрустка панірована свинина, смажена до золотистої скоринки" },
        new { Title = "Гьоза", Price = "Ціна: 140 грн", Description = "Парові або смажені японські пельмені з м’ясом" },
        new { Title = "Місо Суп", Price = "Ціна: 90 грн", Description = "Класичний суп з місо пастою, тофу та водоростями" },
        new { Title = "Сашимі", Price = "Ціна: 280 грн", Description = "Тонко нарізані свіжі шматочки сирої риби" },
        new { Title = "Оякодон", Price = "Ціна: 160 грн", Description = "Рис з куркою і яйцем у ніжному соусі" }
    };

            for (int i = 0; i < dishes.Length; i++)
            {
                int dishId = i + 1;
                string title = dishes[i].Title;
                string price = dishes[i].Price;
                string description = dishes[i].Description;
                Image dishImage = null;

                var card = CreateCard(dishId, title, price, description, dishImage);
                flowLayoutPanel1.Controls.Add(card);
            }
        }

        private Image LoadImageFromDatabase(int dishId)
        {
            try
            {
                using (var connection = Database.GetConnection())
                {
                    connection.Open();
                    string query = "SELECT image FROM dishes WHERE dish_id = @dishId";
                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@dishId", dishId);
                        var result = cmd.ExecuteScalar();
                        if (result != DBNull.Value && result != null)
                        {
                            byte[] imgBytes = (byte[])result;
                            using (MemoryStream ms = new MemoryStream(imgBytes))
                            {
                                return Image.FromStream(ms);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка завантаження зображення з бази: " + ex.Message);
            }
            return null;
        }

        private Panel CreateCard(int dishId, string title, string price, string description, Image dishImage)
        {
            Panel panel = new Panel();
            panel.Size = new Size(180, 230);
            panel.Margin = new Padding(10);
            panel.BackColor = Color.White;
            panel.BorderStyle = BorderStyle.FixedSingle;

            PictureBox pictureBox = new PictureBox();
            pictureBox.Size = new Size(160, 90);
            pictureBox.Location = new Point(10, 10);
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;

            void PictureBox_Paint(object sender, PaintEventArgs e)
            {
                var pb = sender as PictureBox;
                if (pb.Image == null)
                {
                    string placeholderText = "Натисніть для\nзавантаження фото";
                    using (Font font = new Font("Arial", 10, FontStyle.Bold))
                    using (SolidBrush brush = new SolidBrush(Color.DarkGray))
                    {
                        SizeF textSize = e.Graphics.MeasureString(placeholderText, font);
                        float x = (pb.Width - textSize.Width) / 2;
                        float y = (pb.Height - textSize.Height) / 2;
                        e.Graphics.DrawString(placeholderText, font, brush, x, y);
                    }
                }
            }

            if (dishImage != null)
            {
                pictureBox.Image = dishImage;
                pictureBox.BackColor = Color.Transparent;
                pictureBox.BorderStyle = BorderStyle.None;
            }
            else
            {
                pictureBox.BackColor = Color.LightGray;
                pictureBox.BorderStyle = BorderStyle.FixedSingle;
                pictureBox.Paint += PictureBox_Paint;
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
                            pictureBox.Paint -= PictureBox_Paint;
                            SaveImageToDatabase(dishId, img);
                        }
                        catch (Exception ex)
                        {
                            MessageBox.Show("Не вдалося завантажити зображення: " + ex.Message);
                        }
                    }
                }
            };

            Label lblTitle = new Label();
            lblTitle.Text = title;
            lblTitle.Font = new Font("Arial", 11, FontStyle.Bold);
            lblTitle.Location = new Point(10, 105);
            lblTitle.Size = new Size(160, 20);
            lblTitle.AutoEllipsis = true;

            Label lblDescription = new Label();
            lblDescription.Text = description;
            lblDescription.Font = new Font("Arial", 9, FontStyle.Regular);
            lblDescription.Location = new Point(10, 125);
            lblDescription.Size = new Size(160, 40);
            lblDescription.AutoEllipsis = true;

            Label lblPrice = new Label();
            lblPrice.Text = price;
            lblPrice.Font = new Font("Arial", 10, FontStyle.Italic);
            lblPrice.Location = new Point(10, 170);
            lblPrice.AutoSize = true;

            Button btnReviews = new Button();
            btnReviews.Text = "Переглянути відгуки";
            btnReviews.Size = new Size(160, 30);
            btnReviews.Location = new Point(10, 190);
            btnReviews.Visible = true;

            btnReviews.Click += (s, e) =>
            {
                MessageBox.Show($"Показати відгуки для блюда ID: {dishId}");
            };

            panel.Controls.Add(pictureBox);
            panel.Controls.Add(lblTitle);
            panel.Controls.Add(lblDescription);
            panel.Controls.Add(lblPrice);
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

                    string checkQuery = "SELECT COUNT(*) FROM dishes WHERE dish_id = @dishId";
                    using (var checkCmd = new MySqlCommand(checkQuery, connection))
                    {
                        checkCmd.Parameters.AddWithValue("@dishId", dishId);
                        long count = (long)checkCmd.ExecuteScalar();

                        if (count == 0)
                        {
                            string insertQuery = "INSERT INTO dishes (dish_id, image) VALUES (@dishId, @image)";
                            using (var insertCmd = new MySqlCommand(insertQuery, connection))
                            {
                                insertCmd.Parameters.AddWithValue("@dishId", dishId);
                                insertCmd.Parameters.AddWithValue("@image", imgBytes);
                                insertCmd.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            string updateQuery = "UPDATE dishes SET image = @image WHERE dish_id = @dishId";
                            using (var updateCmd = new MySqlCommand(updateQuery, connection))
                            {
                                updateCmd.Parameters.AddWithValue("@dishId", dishId);
                                updateCmd.Parameters.AddWithValue("@image", imgBytes);
                                updateCmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка при збереженні зображення в базу: " + ex.Message);
            }
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Form2 form2 = new Form2(userID, login);
            form2.FormClosed += (s, args) => this.Close();
            this.Hide();
            form2.Show();
        }

        private void flowLayoutPanel1_Paint(object sender, PaintEventArgs e)
        {

        }
    }
}