using System;
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
            pictureBox1.Click += pictureBox1_Click;
            currentUserID = userID;
            currentLogin = login;
            LoadUserInfo();
        }

        private void Form2_Load(object sender, EventArgs e)
        {
            label3.Text = $"UserID: {currentUserID}";
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
            string query = "SELECT login, profile_img, registration_date FROM users WHERE user_id = @userID";

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
                                label4.Text = $"Registration date: {regDate.ToString("yyyy-MM-dd HH:mm:ss")}";
                            }
                            else
                            {
                                label4.Text = "Registration date: N/A";
                            }
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
    }
}
