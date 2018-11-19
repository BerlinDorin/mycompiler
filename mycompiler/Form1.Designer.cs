namespace mycompiler
{
    partial class Form1
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
            this.components = new System.ComponentModel.Container();
            this.richTextBox1 = new System.Windows.Forms.RichTextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.richTextBox2 = new System.Windows.Forms.RichTextBox();
            this.richTextBox3 = new System.Windows.Forms.RichTextBox();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.skinButton1 = new CCWin.SkinControl.SkinButton();
            this.skinButton2 = new CCWin.SkinControl.SkinButton();
            this.skinButton3 = new CCWin.SkinControl.SkinButton();
            this.SuspendLayout();
            // 
            // richTextBox1
            // 
            this.richTextBox1.BackColor = System.Drawing.Color.LightCyan;
            this.richTextBox1.Font = new System.Drawing.Font("Consolas", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox1.Location = new System.Drawing.Point(26, 43);
            this.richTextBox1.Name = "richTextBox1";
            this.richTextBox1.Size = new System.Drawing.Size(623, 260);
            this.richTextBox1.TabIndex = 0;
            this.richTextBox1.Text = "";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.RoyalBlue;
            this.label1.Location = new System.Drawing.Point(24, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(106, 21);
            this.label1.TabIndex = 3;
            this.label1.Text = "代码编辑区：";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.Orange;
            this.label2.Location = new System.Drawing.Point(24, 306);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(122, 21);
            this.label2.TabIndex = 4;
            this.label2.Text = "词法分析结果：";
            this.label2.Click += new System.EventHandler(this.label2_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("微软雅黑", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.ForeColor = System.Drawing.Color.OrangeRed;
            this.label3.Location = new System.Drawing.Point(532, 306);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(90, 21);
            this.label3.TabIndex = 5;
            this.label3.Text = "运行信息：";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.FileName = "openFileDialog1";
            // 
            // richTextBox2
            // 
            this.richTextBox2.BackColor = System.Drawing.Color.FloralWhite;
            this.richTextBox2.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox2.Location = new System.Drawing.Point(28, 330);
            this.richTextBox2.Name = "richTextBox2";
            this.richTextBox2.Size = new System.Drawing.Size(471, 278);
            this.richTextBox2.TabIndex = 9;
            this.richTextBox2.Text = "";
            // 
            // richTextBox3
            // 
            this.richTextBox3.BackColor = System.Drawing.Color.MistyRose;
            this.richTextBox3.Font = new System.Drawing.Font("Consolas", 10.5F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.richTextBox3.Location = new System.Drawing.Point(536, 330);
            this.richTextBox3.Name = "richTextBox3";
            this.richTextBox3.Size = new System.Drawing.Size(352, 278);
            this.richTextBox3.TabIndex = 10;
            this.richTextBox3.Text = "";
            // 
            // skinButton1
            // 
            this.skinButton1.AutoSize = true;
            this.skinButton1.BackColor = System.Drawing.Color.Transparent;
            this.skinButton1.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.skinButton1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(192)))), ((int)(((byte)(128)))));
            this.skinButton1.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.skinButton1.DownBack = null;
            this.skinButton1.DownBaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.skinButton1.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinButton1.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(128)))), ((int)(((byte)(0)))));
            this.skinButton1.Location = new System.Drawing.Point(694, 67);
            this.skinButton1.MouseBack = null;
            this.skinButton1.MouseBaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(255)))));
            this.skinButton1.Name = "skinButton1";
            this.skinButton1.NormlBack = null;
            this.skinButton1.Size = new System.Drawing.Size(125, 39);
            this.skinButton1.TabIndex = 12;
            this.skinButton1.Text = "选择文件分析";
            this.skinButton1.UseVisualStyleBackColor = false;
            this.skinButton1.Click += new System.EventHandler(this.skinButton1_Click);
            // 
            // skinButton2
            // 
            this.skinButton2.AutoSize = true;
            this.skinButton2.BackColor = System.Drawing.Color.Transparent;
            this.skinButton2.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.skinButton2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(128)))), ((int)(((byte)(255)))), ((int)(((byte)(128)))));
            this.skinButton2.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.skinButton2.DownBack = null;
            this.skinButton2.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinButton2.GlowColor = System.Drawing.Color.Lime;
            this.skinButton2.Location = new System.Drawing.Point(694, 156);
            this.skinButton2.MouseBack = null;
            this.skinButton2.Name = "skinButton2";
            this.skinButton2.NormlBack = null;
            this.skinButton2.Size = new System.Drawing.Size(125, 42);
            this.skinButton2.TabIndex = 13;
            this.skinButton2.Text = "开始分析";
            this.skinButton2.UseVisualStyleBackColor = false;
            this.skinButton2.Click += new System.EventHandler(this.skinButton2_Click);
            // 
            // skinButton3
            // 
            this.skinButton3.AutoSize = true;
            this.skinButton3.BackColor = System.Drawing.Color.Transparent;
            this.skinButton3.BaseColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.skinButton3.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(255)))), ((int)(((byte)(255)))), ((int)(((byte)(192)))));
            this.skinButton3.ControlState = CCWin.SkinClass.ControlState.Normal;
            this.skinButton3.DownBack = null;
            this.skinButton3.Font = new System.Drawing.Font("微软雅黑", 10.5F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.skinButton3.GlowColor = System.Drawing.Color.Yellow;
            this.skinButton3.Location = new System.Drawing.Point(694, 252);
            this.skinButton3.MouseBack = null;
            this.skinButton3.Name = "skinButton3";
            this.skinButton3.NormlBack = null;
            this.skinButton3.Size = new System.Drawing.Size(125, 40);
            this.skinButton3.TabIndex = 14;
            this.skinButton3.Text = "保存分析结果";
            this.skinButton3.UseVisualStyleBackColor = false;
            this.skinButton3.Click += new System.EventHandler(this.skinButton3_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.AutoSize = true;
            this.BackColor = System.Drawing.Color.Lavender;
            this.CaptionFont = new System.Drawing.Font("Microsoft YaHei UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.ClientSize = new System.Drawing.Size(900, 628);
            this.ControlBoxActive = System.Drawing.Color.MediumPurple;
            this.Controls.Add(this.skinButton3);
            this.Controls.Add(this.skinButton2);
            this.Controls.Add(this.skinButton1);
            this.Controls.Add(this.richTextBox3);
            this.Controls.Add(this.richTextBox2);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.richTextBox1);
            this.ForeColor = System.Drawing.Color.Black;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedToolWindow;
            this.MaximizeBox = false;
            this.Name = "Form1";
            this.Text = "Lexical analysis";
            this.TitleCenter = true;
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.RichTextBox richTextBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.RichTextBox richTextBox2;
        private System.Windows.Forms.RichTextBox richTextBox3;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private CCWin.SkinControl.SkinButton skinButton1;
        private CCWin.SkinControl.SkinButton skinButton2;
        private CCWin.SkinControl.SkinButton skinButton3;
    }
}