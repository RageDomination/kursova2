using MySql.Data.MySqlClient;
using System;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace курсова2
{
    public partial class Form5 : Form
    {
        private int _dishId;
        private int _userId;

        public Form5(int dishId, int userId)
        {
            InitializeComponent();
            _dishId = dishId;
            _userId = userId;
            LoadReviews();
        }

        private void LoadReviews()
        {
            flowLayoutPanel1.Controls.Clear();

            try
            {
                using (var connection = Database.GetConnection())
                {
                    connection.Open();

                    string query = @"
                        SELECT r.review_text, r.review_date, u.name, u.profile_img
                        FROM reviews r
                        JOIN users u ON r.user_id = u.user_id
                        WHERE r.dish_id = @dishId
                        ORDER BY r.review_date DESC";

                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@dishId", _dishId);

                        using (var reader = cmd.ExecuteReader())
                        {
                            while (reader.Read())
                            {
                                string reviewText = reader["review_text"] == DBNull.Value ? "" : reader.GetString("review_text");
                                DateTime reviewDate = reader["review_date"] == DBNull.Value ? DateTime.MinValue : reader.GetDateTime("review_date");
                                string userName = reader["name"] == DBNull.Value ? "Користувач" : reader.GetString("name");

                                Image userPhoto = null;
                                if (reader["profile_img"] != DBNull.Value)
                                {
                                    string photoPath = reader.GetString("profile_img");
                                    if (!string.IsNullOrEmpty(photoPath) && File.Exists(photoPath))
                                    {
                                        try
                                        {
                                            userPhoto = Image.FromFile(photoPath);
                                        }
                                        catch { }
                                    }
                                }

                                if (userPhoto == null && File.Exists("images/default_avatar.png"))
                                {
                                    userPhoto = Image.FromFile("images/default_avatar.png");
                                }

                                var reviewPanel = CreateReviewPanel(userPhoto, userName, reviewDate, reviewText);
                                flowLayoutPanel1.Controls.Add(reviewPanel);
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка завантаження відгуків: " + ex.Message);
            }
        }
        private const int FixedPanelWidth = 375;

        private Panel CreateReviewPanel(Image userPhoto, string userName, object reviewDateObj, string reviewText)
        {
            Panel panel = new Panel
            {
                Width = FixedPanelWidth,
                Height = 90,
                Margin = new Padding(0, 5, 0, 5),
                BackColor = Color.White,
                BorderStyle = BorderStyle.FixedSingle,
                AutoSize = false
            };

            PictureBox pic = new PictureBox
            {
                Size = new Size(70, 70),
                Location = new Point(10, 10),
                SizeMode = PictureBoxSizeMode.Zoom,
                Image = userPhoto,
                BackColor = Color.White
            };

            string reviewDateText = "невідома дата";
            if (reviewDateObj is DateTime dt && dt > DateTime.MinValue.AddDays(1))
                reviewDateText = dt.ToString("dd.MM.yyyy HH:mm");

            Label lblName = new Label
            {
                Text = $"{userName},",
                Font = new Font("Arial", 10, FontStyle.Bold),
                Location = new Point(pic.Right + 10, 10),
                AutoSize = true,
                BackColor = Color.Transparent
            };

            Label lblDate = new Label
            {
                Text = reviewDateText,
                Font = new Font("Arial", 8, FontStyle.Italic),
                ForeColor = Color.Gray,
                Location = new Point(lblName.Left + TextRenderer.MeasureText(lblName.Text, lblName.Font).Width + 3, 12),
                AutoSize = true,
                BackColor = Color.Transparent
            };

            string displayedText = reviewText.Length > 70 ? reviewText.Substring(0, 70) + "..." : reviewText;

            TextBox txtReview = new TextBox
            {
                Text = displayedText,
                Location = new Point(pic.Right + 10, lblName.Bottom - 4),
                Width = FixedPanelWidth - pic.Width - 40,
                Height = 40,
                Multiline = true,
                ReadOnly = true,
                BorderStyle = BorderStyle.None,
                BackColor = Color.White,
                ScrollBars = ScrollBars.Vertical
            };

            panel.Controls.Add(pic);
            panel.Controls.Add(lblName);
            panel.Controls.Add(lblDate);
            panel.Controls.Add(txtReview);

            if (reviewText.Length > 70)
            {
                Label lblShowMore = new Label
                {
                    Text = "переглянути повнiстю",
                    ForeColor = Color.Gray,
                    Font = new Font("Arial", 8, FontStyle.Italic),
                    Cursor = Cursors.Hand,
                    AutoSize = true,
                    Location = new Point(txtReview.Left, txtReview.Bottom + 2),
                    BackColor = Color.Transparent
                };

                lblShowMore.Click += (s, e) =>
                {
                    MessageBox.Show(reviewText, "Повний коментар", MessageBoxButtons.OK, MessageBoxIcon.Information);
                };

                panel.Controls.Add(lblShowMore);
            }

            return panel;
        }

        private void buttonAddReview_Click(object sender, EventArgs e)
        {
            string reviewText = textBox1.Text.Trim();
            if (string.IsNullOrEmpty(reviewText))
            {
                MessageBox.Show("Відгук не може бути порожнім");
                return;
            }

            try
            {
                using (var connection = Database.GetConnection())
                {
                    connection.Open();
                    string insertQuery = @"
                        INSERT INTO reviews (dish_id, user_id, review_text, review_date)
                        VALUES (@dishId, @userId, @reviewText, @reviewDate)";

                    using (var cmd = new MySqlCommand(insertQuery, connection))
                    {
                        cmd.Parameters.AddWithValue("@dishId", _dishId);
                        cmd.Parameters.AddWithValue("@userId", _userId);
                        cmd.Parameters.AddWithValue("@reviewText", reviewText);
                        cmd.Parameters.AddWithValue("@reviewDate", DateTime.Now);
                        cmd.ExecuteNonQuery();
                    }
                }

                textBox1.Clear();
                LoadReviews();
                MessageBox.Show("Відгук успішно залишено!");
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка при додаванні відгуку: " + ex.Message);
            }
        }

        private void Form5_Load(object sender, EventArgs e) { }
    }
}