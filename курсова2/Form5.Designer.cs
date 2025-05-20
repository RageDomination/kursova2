namespace курсова2
{
    partial class Form5
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form5));
            flowLayoutPanel1 = new FlowLayoutPanel();
            textBox1 = new TextBox();
            buttonAddReview = new Button();
            label1 = new Label();
            labelHidden = new Label();
            SuspendLayout();
            // 
            // flowLayoutPanel1
            // 
            flowLayoutPanel1.AutoScroll = true;
            flowLayoutPanel1.BackColor = Color.White;
            flowLayoutPanel1.Location = new Point(12, 12);
            flowLayoutPanel1.Name = "flowLayoutPanel1";
            flowLayoutPanel1.Size = new Size(407, 396);
            flowLayoutPanel1.TabIndex = 0;
            // 
            // textBox1
            // 
            textBox1.Location = new Point(29, 435);
            textBox1.Name = "textBox1";
            textBox1.Size = new Size(371, 23);
            textBox1.TabIndex = 1;
            // 
            // buttonAddReview
            // 
            buttonAddReview.BackColor = Color.DarkSeaGreen;
            buttonAddReview.FlatStyle = FlatStyle.Popup;
            buttonAddReview.Font = new Font("Times New Roman", 11.25F, FontStyle.Regular, GraphicsUnit.Point);
            buttonAddReview.ForeColor = Color.White;
            buttonAddReview.Location = new Point(97, 490);
            buttonAddReview.Name = "buttonAddReview";
            buttonAddReview.Size = new Size(242, 36);
            buttonAddReview.TabIndex = 2;
            buttonAddReview.Text = "Додати вiдгук";
            buttonAddReview.UseVisualStyleBackColor = false;
            buttonAddReview.Click += buttonAddReview_Click;
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.BackColor = Color.Transparent;
            label1.Font = new Font("Times New Roman", 12F, FontStyle.Regular, GraphicsUnit.Point);
            label1.ForeColor = Color.White;
            label1.Location = new Point(57, 411);
            label1.Name = "label1";
            label1.Size = new Size(300, 19);
            label1.TabIndex = 3;
            label1.Text = "Хочете залишити вiдгук? Залиште його тут.";
            // 
            // labelHidden
            // 
            labelHidden.AutoSize = true;
            labelHidden.BackColor = Color.Transparent;
            labelHidden.Location = new Point(393, 511);
            labelHidden.Name = "labelHidden";
            labelHidden.Size = new Size(38, 15);
            labelHidden.TabIndex = 4;
            labelHidden.Text = "label2";
            labelHidden.Visible = false;
            // 
            // Form5
            // 
            AutoScaleDimensions = new SizeF(7F, 15F);
            AutoScaleMode = AutoScaleMode.Font;
            BackgroundImage = (Image)resources.GetObject("$this.BackgroundImage");
            BackgroundImageLayout = ImageLayout.Stretch;
            ClientSize = new Size(428, 538);
            Controls.Add(labelHidden);
            Controls.Add(label1);
            Controls.Add(buttonAddReview);
            Controls.Add(textBox1);
            Controls.Add(flowLayoutPanel1);
            Name = "Form5";
            StartPosition = FormStartPosition.CenterScreen;
            Text = "Вiдгуки";
            ResumeLayout(false);
            PerformLayout();
        }

        #endregion

        private FlowLayoutPanel flowLayoutPanel1;
        private TextBox textBox1;
        private Button buttonAddReview;
        private Label label1;
        private Label labelHidden;
    }
}