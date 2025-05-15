using System;
using System.Collections.Generic;
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

        private readonly Dictionary<int, (string title, string description, int price)> additionalDishes = new()
        {
            {11, ("Паста Карбонара", "Кремова паста з беконом і сиром Пармезан", 120)},
            {12, ("Лазанья Болоньєзе", "Шарувата італійська лазанья з м’ясним соусом", 140)},
            {13, ("Ризотто з грибами", "Ароматне рисове ризотто з білими грибами", 130)},
            {14, ("Спагеті з морепродуктами", "Спагеті з міксом морепродуктів у томатному соусі", 160)},
            {15, ("Піцца Маргарита", "Класична піцца з томатним соусом, моцарелою і базиліком", 110)},
            {16, ("Піцца Капрічоза", "Піцца з шинкою, грибами, артишоками і оливками", 150)},
            {17, ("Каннеллоні з рікотою і шпинатом", "Трубочки з пасти, фаршировані сиром і шпинатом", 135)},
            {18, ("Оссо буко з полентою", "Тушкована теляча голяшка з кукурудзяною кашею", 190)},
            {19, ("Салат Капрезе", "Помідори, моцарела і базилік з оливковою олією", 90)},
            {20, ("Фокачча з розмарином", "Свіжа італійська булочка з розмарином і сіллю", 70)},
            {21, ("Тірамісу", "Класичний італійський десерт з маскарпоне і кавою", 85)},
            {22, ("Панна Котта з полуничним соусом", "Ніжний вершковий десерт з ягідним топінгом", 80)},
            {23, ("Біг Мак", "Класичний бургер з двома котлетами і спеціальним соусом", 95)},
            {25, ("Чізбургер", "Бургер з сиром чеддер і маринованими огірками", 70)},
            {26, ("Дабл Чізбургер", "Подвійний чізбургер для справжніх гурманів", 110)},
            {27, ("Філе-О-Фіш", "Рибний бургер з хрусткою котлетою і соусом тартар", 85)},
            {28, ("Маффін з яйцем і беконом", "Смачний маффін з яйцем, беконом і сиром", 60)},
            {29, ("Нагетси курячі", "Хрусткі курячі нагетси з соусом", 70)},
            {30, ("Картопля фрі", "Класична смажена картопля", 50)},
            {31, ("Макфлурі з Oreo", "Морозиво з шоколадним печивом Oreo", 55)},
            {32, ("Чікенбургер", "Бургер з курячою котлетою і свіжими овочами", 80)},
            {33, ("Апельсиновий сік", "Свіжовижатий апельсиновий сік", 40)},
            {34, ("Кока-Кола", "Освіжаючий газований напій", 35)},
        };

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

            using (var connection = Database.GetConnection())
            {
                connection.Open();

                // 1) Отримуємо існуючі страви з бази
                string query = "SELECT dish_id, dish_name, price, description, image FROM dishes ORDER BY dish_id";
                HashSet<int> existingDishIds = new HashSet<int>();

                using (var cmd = new MySqlCommand(query, connection))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int dishId = reader.GetInt32("dish_id");
                        existingDishIds.Add(dishId);

                        string title = reader.GetString("dish_name");
                        int priceValue = reader.GetInt32("price");
                        string price = $"Ціна: {priceValue} грн";
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

                        var card = CreateCard(dishId, title, price, description, dishImage);
                        flowLayoutPanel1.Controls.Add(card);
                    }
                }

                // 2) Перевіряємо new dishes (additionalDishes), які відсутні у базі, і додаємо їх
                foreach (var kvp in additionalDishes)
                {
                    int dishId = kvp.Key;
                    if (!existingDishIds.Contains(dishId))
                    {
                        string title = kvp.Value.title;
                        string description = kvp.Value.description;
                        int priceValue = kvp.Value.price;

                        string insertQuery = @"INSERT INTO dishes (dish_id, dish_name, price, description) VALUES (@dishId, @dishName, @price, @description)";
                        using (var insertCmd = new MySqlCommand(insertQuery, connection))
                        {
                            insertCmd.Parameters.AddWithValue("@dishId", dishId);
                            insertCmd.Parameters.AddWithValue("@dishName", title);
                            insertCmd.Parameters.AddWithValue("@price", priceValue);
                            insertCmd.Parameters.AddWithValue("@description", description);
                            insertCmd.ExecuteNonQuery();
                        }

                        // Додаємо нову карточку в UI після додавання до бази
                        string priceText = $"Ціна: {priceValue} грн";
                        var card = CreateCard(dishId, title, priceText, description, null);
                        flowLayoutPanel1.Controls.Add(card);
                    }
                    else
                    {
                        // Якщо страва вже є, додамо її до UI, щоб всі були видимі
                        string title = kvp.Value.title;
                        string description = kvp.Value.description;
                        int priceValue = kvp.Value.price;
                        string price = $"Ціна: {priceValue} грн";
                        var card = CreateCard(dishId, title, price, description, null);
                        flowLayoutPanel1.Controls.Add(card);
                    }
                }
            }
        }

        private void SaveDishDataToDatabase(int dishId, string title, string priceText, string description)
        {
            int priceValue = ExtractPrice(priceText);

            try
            {
                using (var connection = Database.GetConnection())
                {
                    connection.Open();

                    string checkQuery = "SELECT COUNT(*) FROM dishes WHERE dish_id = @dishId";
                    using (var checkCmd = new MySqlCommand(checkQuery, connection))
                    {
                        checkCmd.Parameters.AddWithValue("@dishId", dishId);
                        long count = (long)checkCmd.ExecuteScalar();

                        if (count == 0)
                        {
                            string insertQuery = @"INSERT INTO dishes (dish_id, dish_name, price, description) VALUES (@dishId, @dishName, @price, @description)";
                            using (var insertCmd = new MySqlCommand(insertQuery, connection))
                            {
                                insertCmd.Parameters.AddWithValue("@dishId", dishId);
                                insertCmd.Parameters.AddWithValue("@dishName", title);
                                insertCmd.Parameters.AddWithValue("@price", priceValue);
                                insertCmd.Parameters.AddWithValue("@description", description);
                                insertCmd.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            string updateQuery = @"UPDATE dishes SET dish_name = @dishName, price = @price, description = @description WHERE dish_id = @dishId";
                            using (var updateCmd = new MySqlCommand(updateQuery, connection))
                            {
                                updateCmd.Parameters.AddWithValue("@dishId", dishId);
                                updateCmd.Parameters.AddWithValue("@dishName", title);
                                updateCmd.Parameters.AddWithValue("@price", priceValue);
                                updateCmd.Parameters.AddWithValue("@description", description);
                                updateCmd.ExecuteNonQuery();
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка збереження даних страви: " + ex.Message);
            }
        }

        private int ExtractPrice(string priceText)
        {
            int price = 0;
            string digitsOnly = "";
            foreach (char c in priceText)
            {
                if (char.IsDigit(c))
                    digitsOnly += c;
            }
            int.TryParse(digitsOnly, out price);
            return price;
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
                            string insertQuery = @"INSERT INTO dishes (dish_id, image) VALUES (@dishId, @image)";
                            using (var insertCmd = new MySqlCommand(insertQuery, connection))
                            {
                                insertCmd.Parameters.AddWithValue("@dishId", dishId);
                                insertCmd.Parameters.AddWithValue("@image", imgBytes);
                                insertCmd.ExecuteNonQuery();
                            }
                        }
                        else
                        {
                            string updateQuery = @"UPDATE dishes SET image = @image WHERE dish_id = @dishId";
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
