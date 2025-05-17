namespace курсова2
{
    partial class Form2
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form2));
            label1 = new Label();
            pictureBox1 = new PictureBox();
            label2 = new Label();
            label3 = new Label();
            label4 = new Label();
            panel1 = new Panel();
            button2 = new Button();
            button1 = new Button();
            pictureBox2 = new PictureBox();
            label5 = new Label();
            label6 = new Label();
            textBox1 = new TextBox();
            textBox2 = new TextBox();
            textBox3 = new TextBox();
            textBox4 = new TextBox();
            label7 = new Label();
            label8 = new Label();
            label9 = new Label();
            label10 = new Label();
            buttonSave = new Button();
            labelHidden = new Label();
            label11 = new Label();
            textBox5 = new TextBox();
            textBox6 = new TextBox();
            label12 = new Label();
            label13 = new Label();
            buttonSave2 = new Button();
            tabControl1 = new TabControl();
            tabPage1 = new TabPage();
            tabPage2 = new TabPage();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox2).BeginInit();
            tabControl1.SuspendLayout();
            SuspendLayout();
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new Font("Arial", 14.25F, FontStyle.Regular, GraphicsUnit.Point);
            label1.Location = new Point(309, 57);
            label1.Name = "label1";
            label1.Size = new Size(171, 22);
            label1.TabIndex = 0;
            label1.Text = "Особистий кабінет";
            // 
            // pictureBox1
            // 
            pictureBox1.Cursor = Cursors.Hand;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(46, 144);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(158, 107);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 1;
            pictureBox1.TabStop = false;
            pictureBox1.Click += pictureBox1_Click_1;
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            label2.Location = new Point(147, 254);
            label2.Name = "label2";
            label2.Size = new Size(40, 17);
            label2.TabIndex = 2;
            label2.Text = "Login";
            // 
            // label3
            // 
            label3.AutoSize = true;
            label3.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            label3.Location = new Point(46, 254);
            label3.Name = "label3";
            label3.Size = new Size(54, 17);
            label3.TabIndex = 3;
            label3.Text = "UserID: ";
            // 
            // label4
            // 
            label4.AutoSize = true;
            label4.Location = new Point(28, 281);
            label4.Name = "label4";
            label4.Size = new Size(199, 15);
            label4.TabIndex = 4;
            label4.Text = "Registration date: xxxx-xx-xx **:**:**";
            // 
            // panel1
            // 
            panel1.BackColor = SystemColors.ActiveCaption;
            panel1.Controls.Add(button2);
            panel1.Controls.Add(button1);
            panel1.Controls.Add(pictureBox2);
            panel1.Location = new Point(-4, -3);
            panel1.Name = "panel1";
            panel1.Size = new Size(809, 47);
            panel1.TabIndex = 10;
            // 
            // button2
            // 
            button2.Location = new Point(183, 3);
            button2.Name = "button2";
            button2.Size = new Size(127, 44);
            button2.TabIndex = 13;
            button2.Text = "Замовлення";
            button2.UseVisualStyleBackColor = true;
            // 
            // button1
            // 
            button1.Location = new Point(66, 3);
            button1.Name = "button1";
            button1.Size = new Size(125, 44);
            button1.TabIndex = 12;
            button1.Text = "Кабiнет";
            button1.UseVisualStyleBackColor = true;
            // 
            // pictureBox2
            // 
            pictureBox2.Cursor = Cursors.Hand;
            pictureBox2.Image = (Image)resources.GetObject("pictureBox2.Image");
            pictureBox2.Location = new Point(743, 6);
            pictureBox2.Name = "pictureBox2";
            pictureBox2.Size = new Size(38, 38);
            pictureBox2.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox2.TabIndex = 11;
            pictureBox2.TabStop = false;
            pictureBox2.Click += pictureBox2_Click;
            // 
            // label5
            // 
            label5.AutoSize = true;
            label5.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label5.Location = new Point(46, 111);
            label5.Name = "label5";
            label5.Size = new Size(159, 21);
            label5.TabIndex = 12;
            label5.Text = "Основна iнформацiя";
            // 
            // label6
            // 
            label6.AutoSize = true;
            label6.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label6.Location = new Point(483, 114);
            label6.Name = "label6";
            label6.Size = new Size(174, 21);
            label6.TabIndex = 13;
            label6.Text = "Додаткова iнформацiя";
            // 
            // textBox1
            // 
            textBox1.Location = new Point(387, 155);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(174, 23);
            textBox1.TabIndex = 14;
            // 
            // textBox2
            // 
            textBox2.Location = new Point(387, 203);
            textBox2.Name = "textBox2";
            textBox2.Size = new Size(174, 23);
            textBox2.TabIndex = 15;
            // 
            // textBox3
            // 
            textBox3.Location = new Point(590, 155);
            textBox3.Name = "textBox3";
            textBox3.Size = new Size(174, 23);
            textBox3.TabIndex = 16;
            // 
            // textBox4
            // 
            textBox4.Location = new Point(590, 203);
            textBox4.Name = "textBox4";
            textBox4.Size = new Size(174, 23);
            textBox4.TabIndex = 17;
            // 
            // label7
            // 
            label7.AutoSize = true;
            label7.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            label7.Location = new Point(452, 135);
            label7.Name = "label7";
            label7.Size = new Size(30, 17);
            label7.TabIndex = 18;
            label7.Text = "Iм'я";
            // 
            // label8
            // 
            label8.AutoSize = true;
            label8.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            label8.Location = new Point(442, 181);
            label8.Name = "label8";
            label8.Size = new Size(66, 17);
            label8.TabIndex = 19;
            label8.Text = "Прізвище";
            // 
            // label9
            // 
            label9.AutoSize = true;
            label9.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            label9.Location = new Point(627, 135);
            label9.Name = "label9";
            label9.Size = new Size(109, 17);
            label9.TabIndex = 20;
            label9.Text = "Номер телефону";
            // 
            // label10
            // 
            label10.AutoSize = true;
            label10.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            label10.Location = new Point(655, 182);
            label10.Name = "label10";
            label10.Size = new Size(44, 17);
            label10.TabIndex = 21;
            label10.Text = "E-mail";
            // 
            // buttonSave
            // 
            buttonSave.Location = new Point(500, 242);
            buttonSave.Name = "buttonSave";
            buttonSave.Size = new Size(147, 29);
            buttonSave.TabIndex = 22;
            buttonSave.Text = "Зберегти змiни";
            buttonSave.UseVisualStyleBackColor = true;
            buttonSave.Click += buttonSave_Click;
            // 
            // labelHidden
            // 
            labelHidden.AutoSize = true;
            labelHidden.Location = new Point(779, 439);
            labelHidden.Name = "labelHidden";
            labelHidden.Size = new Size(44, 15);
            labelHidden.TabIndex = 23;
            labelHidden.Text = "label11";
            labelHidden.Visible = false;
            // 
            // label11
            // 
            label11.AutoSize = true;
            label11.Font = new Font("Segoe UI", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label11.Location = new Point(523, 302);
            label11.Name = "label11";
            label11.Size = new Size(106, 21);
            label11.TabIndex = 24;
            label11.Text = "Зміна пароля";
            // 
            // textBox5
            // 
            textBox5.Location = new Point(387, 343);
            textBox5.Name = "textBox5";
            textBox5.Size = new Size(174, 23);
            textBox5.TabIndex = 25;
            // 
            // textBox6
            // 
            textBox6.Location = new Point(590, 343);
            textBox6.Name = "textBox6";
            textBox6.Size = new Size(174, 23);
            textBox6.TabIndex = 26;
            // 
            // label12
            // 
            label12.AutoSize = true;
            label12.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            label12.Location = new Point(422, 323);
            label12.Name = "label12";
            label12.Size = new Size(114, 17);
            label12.TabIndex = 27;
            label12.Text = "Поточний пароль";
            // 
            // label13
            // 
            label13.AutoSize = true;
            label13.Font = new Font("Segoe UI", 9.75F, FontStyle.Regular, GraphicsUnit.Point);
            label13.Location = new Point(627, 323);
            label13.Name = "label13";
            label13.Size = new Size(102, 17);
            label13.TabIndex = 28;
            label13.Text = "  Новий пароль";
            // 
            // buttonSave2
            // 
            buttonSave2.Location = new Point(500, 382);
            buttonSave2.Name = "buttonSave2";
            buttonSave2.Size = new Size(147, 29);
            buttonSave2.TabIndex = 29;
            buttonSave2.Text = "Зберегти змiни";
            buttonSave2.UseVisualStyleBackColor = true;
            // 
            // tabControl1
            // 
            tabControl1.Controls.Add(tabPage1);
            tabControl1.Controls.Add(tabPage2);
            tabControl1.Location = new Point(12, 302);
            tabControl1.Name = "tabControl1";
            tabControl1.SelectedIndex = 0;
            tabControl1.Size = new Size(369, 131);
            tabControl1.TabIndex = 30;
            // 
            // tabPage1
            // 
            tabPage1.Location = new Point(4, 24);
            tabPage1.Name = "tabPage1";
            tabPage1.Padding = new Padding(3);
            tabPage1.Size = new Size(361, 103);
            tabPage1.TabIndex = 0;
            tabPage1.Text = "tabPage1";
            tabPage1.UseVisualStyleBackColor = true;
            // 
            // tabPage2
            // 
            tabPage2.Location = new Point(4, 24);
            tabPage2.Name = "tabPage2";
            tabPage2.Padding = new Padding(3);
            tabPage2.Size = new Size(274, 103);
            tabPage2.TabIndex = 1;
            tabPage2.Text = "tabPage2";
            tabPage2.UseVisualStyleBackColor = true;
            // 
            // Form2
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            ClientSize = new Size(800, 445);
            Controls.Add(tabControl1);
            Controls.Add(buttonSave2);
            Controls.Add(label13);
            Controls.Add(label12);
            Controls.Add(textBox6);
            Controls.Add(textBox5);
            Controls.Add(label11);
            Controls.Add(labelHidden);
            Controls.Add(buttonSave);
            Controls.Add(label10);
            Controls.Add(label9);
            Controls.Add(label8);
            Controls.Add(label7);
            Controls.Add(textBox4);
            Controls.Add(textBox3);
            Controls.Add(textBox2);
            Controls.Add(textBox1);
            Controls.Add(label6);
            Controls.Add(label5);
            Controls.Add(panel1);
            Controls.Add(label4);
            Controls.Add(label3);
            Controls.Add(label2);
            Controls.Add(pictureBox1);
            Controls.Add(label1);
            Name = "Form2";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Особистий кабінет";
            Load += Form2_Load;
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox2).EndInit();
            tabControl1.ResumeLayout(false);
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Label label1;
        private PictureBox pictureBox1;
        private Label label2;
        private Label label3;
        private Label label4;
        private Panel panel1;
        private PictureBox pictureBox2;
        private Label label5;
        private Label label6;
        private TextBox textBox1;
        private TextBox textBox2;
        private TextBox textBox3;
        private TextBox textBox4;
        private Label label7;
        private Label label8;
        private Label label9;
        private Label label10;
        private Button buttonSave;
        private Label labelHidden;
        private Label label11;
        private TextBox textBox5;
        private TextBox textBox6;
        private Label label12;
        private Label label13;
        private Button buttonSave2;
        private TabControl tabControl1;
        private TabPage tabPage1;
        private TabPage tabPage2;
        private Button button2;
        private Button button1;
    }
}