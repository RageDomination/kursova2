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
            this.button1.Click += button1_Click;
            this.button2.Click += button2_Click;
        }

        private void LoadCartItems()
        {
            flowLayoutPanel1.Controls.Clear();
            decimal total = 0;

            using (var conn = Database.GetConnection())
            {
                conn.Open();
                string query = "SELECT dish_id, dish_name, description, price, image FROM dishes WHERE in_cart = 'yes'";

                using (var cmd = new MySqlCommand(query, conn))
                using (var reader = cmd.ExecuteReader())
                {
                    while (reader.Read())
                    {
                        int dishId = reader.GetInt32("dish_id");
                        string name = reader.GetString("dish_name");
                        string description = reader.GetString("description");
                        decimal price = reader.GetDecimal("price");

                        Image image = null;
                        if (!reader.IsDBNull(reader.GetOrdinal("image")))
                        {
                            byte[] imageBytes = (byte[])reader["image"];
                            using (MemoryStream ms = new MemoryStream(imageBytes))
                            {
                                image = Image.FromStream(ms);
                            }
                        }

                        var panel = CreateCartItemPanel(dishId, name, description, price, image);
                        flowLayoutPanel1.Controls.Add(panel);

                        total += price;
                    }
                }
            }

            label2.Text = $"Загальна сума: {total} грн";
        }

        private Panel CreateCartItemPanel(int dishId, string name, string description, decimal price, Image image)
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
                Text = "Кількість: 1",
                Location = new Point(120, 95),
                AutoSize = true
            };

            Label lblSum = new Label
            {
                Text = $"Сума: {price} грн",
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

            int quantity = 1;

            btnPlus.Click += (s, e) =>
            {
                quantity++;
                lblQuantity.Text = $"Кількість: {quantity}";
                lblSum.Text = $"Сума: {price * quantity} грн";
                RecalculateTotal();
            };

            btnMinus.Click += (s, e) =>
            {
                if (quantity > 1)
                {
                    quantity--;
                    lblQuantity.Text = $"Кількість: {quantity}";
                    lblSum.Text = $"Сума: {price * quantity} грн";
                    RecalculateTotal();
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
            label2.Text = $"Загальна сума: {total} грн";
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
