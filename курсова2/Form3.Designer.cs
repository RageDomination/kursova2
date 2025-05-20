namespace курсова2
{
    partial class Form3
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form3));
            panel1 = new Panel();
            pictureBox3 = new PictureBox();
            flowLayoutPanel2 = new FlowLayoutPanel();
            button2 = new Button();
            button1 = new Button();
            pictureBox1 = new PictureBox();
            labelHidden = new Label();
            flowLayoutPanel1 = new FlowLayoutPanel();
            label1 = new Label();
            label2 = new Label();
            panel1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)pictureBox3).BeginInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).BeginInit();
            SuspendLayout();
            // 
            // panel1
            // 
            panel1.BackColor = Color.White;
            panel1.BorderStyle = BorderStyle.FixedSingle;
            panel1.Controls.Add(pictureBox3);
            panel1.Controls.Add(flowLayoutPanel2);
            panel1.Controls.Add(button2);
            panel1.Controls.Add(button1);
            panel1.Location = new Point(-1, 0);
            panel1.Name = "panel1";
            panel1.Size = new Size(926, 37);
            panel1.TabIndex = 11;
            // 
            // pictureBox3
            // 
            pictureBox3.Image = (Image)resources.GetObject("pictureBox3.Image");
            pictureBox3.Location = new Point(3, -1);
            pictureBox3.Name = "pictureBox3";
            pictureBox3.Size = new Size(52, 40);
            pictureBox3.SizeMode = PictureBoxSizeMode.StretchImage;
            pictureBox3.TabIndex = 29;
            pictureBox3.TabStop = false;
            // 
            // flowLayoutPanel2
            // 
            flowLayoutPanel2.Location = new Point(0, 45);
            flowLayoutPanel2.Name = "flowLayoutPanel2";
            flowLayoutPanel2.Size = new Size(806, 415);
            flowLayoutPanel2.TabIndex = 28;
            // 
            // button2
            // 
            button2.BackColor = Color.DarkSeaGreen;
            button2.Cursor = Cursors.Hand;
            button2.FlatStyle = FlatStyle.Popup;
            button2.Font = new Font("Times New Roman", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            button2.ForeColor = Color.White;
            button2.Location = new Point(182, -1);
            button2.Name = "button2";
            button2.Size = new Size(127, 37);
            button2.TabIndex = 13;
            button2.Text = "Замовлення";
            button2.UseVisualStyleBackColor = false;
            // 
            // button1
            // 
            button1.BackColor = Color.DarkSeaGreen;
            button1.Cursor = Cursors.Hand;
            button1.FlatStyle = FlatStyle.Popup;
            button1.Font = new Font("Times New Roman", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            button1.ForeColor = Color.White;
            button1.Location = new Point(58, -1);
            button1.Name = "button1";
            button1.Size = new Size(125, 37);
            button1.TabIndex = 12;
            button1.Text = "Кабiнет";
            button1.UseVisualStyleBackColor = false;
            // 
            // pictureBox1
            // 
            pictureBox1.BackColor = Color.Transparent;
            pictureBox1.Cursor = Cursors.Hand;
            pictureBox1.Image = (Image)resources.GetObject("pictureBox1.Image");
            pictureBox1.Location = new Point(856, 61);
            pictureBox1.Name = "pictureBox1";
            pictureBox1.Size = new Size(44, 30);
            pictureBox1.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox1.TabIndex = 26;
            pictureBox1.TabStop = false;
            // 
            // labelHidden
            // 
            labelHidden.AutoSize = true;
            labelHidden.BackColor = Color.Transparent;
            labelHidden.ForeColor = SystemColors.ControlText;
            labelHidden.ImageAlign = ContentAlignment.MiddleRight;
            labelHidden.Location = new Point(881, 499);
            labelHidden.Name = "labelHidden";
            labelHidden.Size = new Size(44, 15);
            labelHidden.TabIndex = 24;
            labelHidden.Text = "label11";
            labelHidden.Visible = false;
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.BackColor = Color.DarkSeaGreen;
            flowLayoutPanel1.Cursor = Cursors.Hand;
            flowLayoutPanel1.FlowDirection = FlowDirection.TopDown;
            flowLayoutPanel1.Location = new Point(182, 103);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(590, 354);
            flowLayoutPanel1.TabIndex = 25;
            flowLayoutPanel1.WrapContents = false;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Times New Roman", 14.25F, FontStyle.Bold, GraphicsUnit.Point);
            label1.ForeColor = Color.White;
            label1.Location = new Point(343, 66);
            label1.Name = "label1";
            label1.Size = new Size(258, 22);
            label1.TabIndex = 27;
            label1.Text = "Сформуйте своє замовлення";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.BackColor = Color.Transparent;
            label2.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label2.ForeColor = Color.White;
            label2.Location = new Point(831, 69);
            label2.Name = "label2";
            label2.Size = new Size(17, 19);
            label2.TabIndex = 28;
            label2.Text = "1";
            // 
            // Form3
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(924, 513);
            Controls.Add(label2);
            Controls.Add(label1);
            Controls.Add(pictureBox1);
            Controls.Add(flowLayoutPanel1);
            Controls.Add(labelHidden);
            Controls.Add(panel1);
            Name = "Form3";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Список замовлень";
            panel1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)pictureBox3).EndInit();
            ((System.ComponentModel.ISupportInitialize)pictureBox1).EndInit();
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private Panel panel1;
        private Button button2;
        private Button button1;
        private Label labelHidden;
        private FlowLayoutPanel flowLayoutPanel1;
        private PictureBox pictureBox1;
        private Label label1;
        private FlowLayoutPanel flowLayoutPanel2;
        private Label label2;
        private PictureBox pictureBox3;
    }
}