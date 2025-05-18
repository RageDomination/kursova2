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
        private Label lblNoReviews;
        private int? _editingReviewId = null;
        public Form5(int dishId, int userId)
        {
            InitializeComponent();
            _dishId = dishId;
            _userId = userId;
            textBox1.Height = 50;
            textBox1.Multiline = true;
            lblNoReviews = new Label()
            {
                Text = "Ще не було додано відгуків на цю позицію в меню",
                ForeColor = Color.Gray,
                Font = new Font("Arial", 12, FontStyle.Italic),
                AutoSize = true,
                Visible = false,
                TextAlign = ContentAlignment.MiddleCenter,
                Dock = DockStyle.Fill
            };

            flowLayoutPanel1.Controls.Add(lblNoReviews);
            LoadReviews();
            this.ActiveControl = label2;
        }
        private void LoadReviews()
        {
            flowLayoutPanel1.SuspendLayout();
            flowLayoutPanel1.Controls.Clear();

            try
            {
                using (var connection = Database.GetConnection())
                {
                    connection.Open();

                    string query = @"
                        SELECT r.review_id, r.review_text, r.review_date, r.red, u.user_id, u.name, u.profile_img
                        FROM reviews r
                        JOIN users u ON r.user_id = u.user_id
                        WHERE r.dish_id = @dishId
                        ORDER BY r.review_date DESC";

                    using (var cmd = new MySqlCommand(query, connection))
                    {
                        cmd.Parameters.AddWithValue("@dishId", _dishId);

                        using (var reader = cmd.ExecuteReader())
                        {
                            bool hasReviews = false;

                            while (reader.Read())
                            {
                                hasReviews = true;

                                int reviewId = reader.GetInt32("review_id");
                                string reviewText = reader["review_text"] == DBNull.Value ? "" : reader.GetString("review_text");
                                DateTime reviewDate = reader["review_date"] == DBNull.Value ? DateTime.MinValue : reader.GetDateTime("review_date");
                                bool isEdited = reader["red"] != DBNull.Value && reader.GetInt32("red") == 1;
                                string userName = reader["name"] == DBNull.Value ? "Користувач" : reader.GetString("name");
                                int reviewUserId = reader.GetInt32("user_id");

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
                                        catch
                                        {
                                            userPhoto = GetDefaultUserPhoto();
                                        }
                                    }
                                }
                                if (userPhoto == null)
                                    userPhoto = GetDefaultUserPhoto();

                                var reviewPanel = CreateReviewPanel(userPhoto, userName, reviewDate, reviewText, reviewId, reviewUserId, isEdited);
                                flowLayoutPanel1.Controls.Add(reviewPanel);
                            }

                            if (!hasReviews)
                            {
                                flowLayoutPanel1.Controls.Add(lblNoReviews);
                                lblNoReviews.Visible = true;
                            }
                            else
                            {
                                lblNoReviews.Visible = false;
                            }
                        }
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка завантаження відгуків: " + ex.Message);
            }
            finally
            {
                flowLayoutPanel1.ResumeLayout();
            }
        }

        private Image GetDefaultUserPhoto()
        {
            string defaultAvatarPath = "images/default_avatar.png";
            if (File.Exists(defaultAvatarPath))
            {
                try
                {
                    return Image.FromFile(defaultAvatarPath);
                }
                catch
                {
                    return null;
                }
            }
            return null;
        }

        private const int FixedPanelWidth = 375;
        private Panel CreateReviewPanel(Image userPhoto, string userName, DateTime reviewDate, string reviewText, int reviewId, int reviewUserId, bool isEdited)
        {
            Panel panel = new Panel
            {
                Width = FixedPanelWidth + 10,
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
            if (reviewDate > DateTime.MinValue.AddDays(1))
                reviewDateText = reviewDate.ToString("dd.MM.yyyy HH:mm");

            if (isEdited)
                reviewDateText += " (ред.)";

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

            string singleLineReview = reviewText.Replace("\r", " ").Replace("\n", " ").Trim();
            bool isLongText = singleLineReview.Length > 70;
            string displayedText = isLongText ? singleLineReview.Substring(0, 70) + "..." : singleLineReview;

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
                ScrollBars = ScrollBars.None,
                Cursor = Cursors.Default,
                WordWrap = true
            };

            txtReview.MouseWheel += (s, e) =>
            {
                var parent = flowLayoutPanel1;
                if (parent != null && parent.VerticalScroll.Visible)
                {
                    int newValue = parent.VerticalScroll.Value - e.Delta;
                    newValue = Math.Max(parent.VerticalScroll.Minimum, Math.Min(parent.VerticalScroll.Maximum, newValue));
                    parent.VerticalScroll.Value = newValue;
                    parent.PerformLayout();
                }
            };

            panel.Controls.Add(pic);
            panel.Controls.Add(lblName);
            panel.Controls.Add(lblDate);
            panel.Controls.Add(txtReview);

            Label lblShowMore = null;
            if (isLongText)
            {
                lblShowMore = new Label
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

            if (reviewUserId == _userId)
            {
                Label lblEdit = new Label
                {
                    Text = "редагувати",
                    Font = new Font("Arial", 8, FontStyle.Italic),
                    ForeColor = Color.Gray,
                    Cursor = Cursors.Hand,
                    AutoSize = true,
                    BackColor = Color.Transparent,
                    Location = new Point(
                        panel.Width - 80,
                        lblShowMore != null ? lblShowMore.Top - 2 : txtReview.Bottom - 6)
                };

                lblEdit.Click += (s, e) =>
                {
                    textBox1.Text = reviewText;
                    _editingReviewId = reviewId;
                };

                panel.Controls.Add(lblEdit);
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

                    if (_editingReviewId.HasValue)
                    {
                        string updateQuery = @"
                            UPDATE reviews
                            SET review_text = @reviewText,
                                review_date = @reviewDate,
                                red = 1
                            WHERE review_id = @reviewId
                              AND user_id = @userId";

                        using (var cmd = new MySqlCommand(updateQuery, connection))
                        {
                            cmd.Parameters.AddWithValue("@reviewText", reviewText);
                            cmd.Parameters.AddWithValue("@reviewDate", DateTime.Now);
                            cmd.Parameters.AddWithValue("@reviewId", _editingReviewId.Value);
                            cmd.Parameters.AddWithValue("@userId", _userId);

                            int rows = cmd.ExecuteNonQuery();
                            if (rows == 0)
                            {
                                MessageBox.Show("Не вдалось оновити відгук: ви можете редагувати лише власні відгуки.");
                                return;
                            }
                        }

                        _editingReviewId = null;
                        textBox1.Clear();
                        LoadReviews();
                        MessageBox.Show("Відгук успішно оновлено!");
                    }
                    else
                    {
                        string insertQuery = @"
                            INSERT INTO reviews (dish_id, user_id, review_text, review_date, red)
                            VALUES (@dishId, @userId, @reviewText, @reviewDate, 0)";

                        using (var cmd = new MySqlCommand(insertQuery, connection))
                        {
                            cmd.Parameters.AddWithValue("@dishId", _dishId);
                            cmd.Parameters.AddWithValue("@userId", _userId);
                            cmd.Parameters.AddWithValue("@reviewText", reviewText);
                            cmd.Parameters.AddWithValue("@reviewDate", DateTime.Now);
                            cmd.ExecuteNonQuery();
                        }

                        textBox1.Clear();
                        LoadReviews();
                        MessageBox.Show("Відгук додано!");
                    }
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show("Помилка збереження відгуку: " + ex.Message);
            }
        }
    }
}