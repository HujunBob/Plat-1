namespace plat.function1
{
    partial class Stationmanage
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Stationmanage));
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txt_zsh = new System.Windows.Forms.RichTextBox();
            this.txt_ysh = new System.Windows.Forms.RichTextBox();
            this.txt_lsh = new System.Windows.Forms.RichTextBox();
            this.txt_xj = new System.Windows.Forms.RichTextBox();
            this.xiangjiang = new System.Windows.Forms.Panel();
            this.BtnAddStation = new System.Windows.Forms.Button();
            this.BtnRemoveStation = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.panel2 = new System.Windows.Forms.Panel();
            this.rivercb = new System.Windows.Forms.ComboBox();
            this.zishui = new System.Windows.Forms.Panel();
            this.pictureBox2 = new System.Windows.Forms.PictureBox();
            this.yuanshui = new System.Windows.Forms.Panel();
            this.pictureBox3 = new System.Windows.Forms.PictureBox();
            this.lishui = new System.Windows.Forms.Panel();
            this.pictureBox4 = new System.Windows.Forms.PictureBox();
            this.River1 = new System.Windows.Forms.Panel();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.panel1.SuspendLayout();
            this.groupBox1.SuspendLayout();
            this.xiangjiang.SuspendLayout();
            this.panel2.SuspendLayout();
            this.zishui.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).BeginInit();
            this.yuanshui.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).BeginInit();
            this.lishui.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).BeginInit();
            this.River1.SuspendLayout();
            this.SuspendLayout();
            // 
            // pictureBox1
            // 
            this.pictureBox1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(0, 0);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(887, 511);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox1.TabIndex = 14;
            this.pictureBox1.TabStop = false;
            this.pictureBox1.MouseDown += new System.Windows.Forms.MouseEventHandler(this.pictureBox1_MouseDown);
            this.pictureBox1.Resize += new System.EventHandler(this.panel3_Resize);
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.groupBox1);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Left;
            this.panel1.Location = new System.Drawing.Point(0, 0);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(230, 574);
            this.panel1.TabIndex = 25;
            // 
            // groupBox1
            // 
            this.groupBox1.BackColor = System.Drawing.SystemColors.ActiveCaption;
            this.groupBox1.Controls.Add(this.txt_zsh);
            this.groupBox1.Controls.Add(this.txt_ysh);
            this.groupBox1.Controls.Add(this.txt_lsh);
            this.groupBox1.Controls.Add(this.txt_xj);
            this.groupBox1.Dock = System.Windows.Forms.DockStyle.Right;
            this.groupBox1.Font = new System.Drawing.Font("微软雅黑", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.groupBox1.Location = new System.Drawing.Point(4, 0);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(226, 574);
            this.groupBox1.TabIndex = 2;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "流域简介";
            // 
            // txt_zsh
            // 
            this.txt_zsh.BackColor = System.Drawing.Color.AliceBlue;
            this.txt_zsh.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txt_zsh.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_zsh.Location = new System.Drawing.Point(3, 23);
            this.txt_zsh.Name = "txt_zsh";
            this.txt_zsh.ReadOnly = true;
            this.txt_zsh.Size = new System.Drawing.Size(220, 548);
            this.txt_zsh.TabIndex = 2;
            this.txt_zsh.Text = resources.GetString("txt_zsh.Text");
            // 
            // txt_ysh
            // 
            this.txt_ysh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(245)))), ((int)(((byte)(248)))));
            this.txt_ysh.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txt_ysh.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_ysh.Location = new System.Drawing.Point(3, 23);
            this.txt_ysh.Name = "txt_ysh";
            this.txt_ysh.ReadOnly = true;
            this.txt_ysh.Size = new System.Drawing.Size(220, 548);
            this.txt_ysh.TabIndex = 1;
            this.txt_ysh.Text = resources.GetString("txt_ysh.Text");
            // 
            // txt_lsh
            // 
            this.txt_lsh.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(245)))), ((int)(((byte)(248)))));
            this.txt_lsh.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txt_lsh.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_lsh.Location = new System.Drawing.Point(3, 23);
            this.txt_lsh.Name = "txt_lsh";
            this.txt_lsh.ReadOnly = true;
            this.txt_lsh.Size = new System.Drawing.Size(220, 548);
            this.txt_lsh.TabIndex = 0;
            this.txt_lsh.Text = resources.GetString("txt_lsh.Text");
            // 
            // txt_xj
            // 
            this.txt_xj.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(229)))), ((int)(((byte)(245)))), ((int)(((byte)(248)))));
            this.txt_xj.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txt_xj.Dock = System.Windows.Forms.DockStyle.Fill;
            this.txt_xj.Font = new System.Drawing.Font("微软雅黑", 11F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txt_xj.Location = new System.Drawing.Point(3, 23);
            this.txt_xj.Name = "txt_xj";
            this.txt_xj.ReadOnly = true;
            this.txt_xj.Size = new System.Drawing.Size(220, 548);
            this.txt_xj.TabIndex = 3;
            this.txt_xj.Text = resources.GetString("txt_xj.Text");
            // 
            // xiangjiang
            // 
            this.xiangjiang.Controls.Add(this.pictureBox1);
            this.xiangjiang.Dock = System.Windows.Forms.DockStyle.Fill;
            this.xiangjiang.Location = new System.Drawing.Point(0, 0);
            this.xiangjiang.Name = "xiangjiang";
            this.xiangjiang.Size = new System.Drawing.Size(887, 511);
            this.xiangjiang.TabIndex = 24;
            // 
            // BtnAddStation
            // 
            this.BtnAddStation.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("BtnAddStation.BackgroundImage")));
            this.BtnAddStation.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BtnAddStation.Location = new System.Drawing.Point(577, 13);
            this.BtnAddStation.Name = "BtnAddStation";
            this.BtnAddStation.Size = new System.Drawing.Size(140, 45);
            this.BtnAddStation.TabIndex = 21;
            this.BtnAddStation.UseVisualStyleBackColor = true;
            this.BtnAddStation.Click += new System.EventHandler(this.BtnAddStation_Click);
            // 
            // BtnRemoveStation
            // 
            this.BtnRemoveStation.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("BtnRemoveStation.BackgroundImage")));
            this.BtnRemoveStation.BackgroundImageLayout = System.Windows.Forms.ImageLayout.Stretch;
            this.BtnRemoveStation.Location = new System.Drawing.Point(739, 13);
            this.BtnRemoveStation.Name = "BtnRemoveStation";
            this.BtnRemoveStation.Size = new System.Drawing.Size(136, 45);
            this.BtnRemoveStation.TabIndex = 22;
            this.BtnRemoveStation.UseVisualStyleBackColor = true;
            this.BtnRemoveStation.Click += new System.EventHandler(this.BtnRemoveStation_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.label1.Font = new System.Drawing.Font("楷体", 26.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.OrangeRed;
            this.label1.Location = new System.Drawing.Point(38, 19);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(239, 37);
            this.label1.TabIndex = 15;
            this.label1.Text = "基础数据管理";
            // 
            // panel2
            // 
            this.panel2.BackColor = System.Drawing.Color.SkyBlue;
            this.panel2.Controls.Add(this.rivercb);
            this.panel2.Controls.Add(this.BtnAddStation);
            this.panel2.Controls.Add(this.BtnRemoveStation);
            this.panel2.Controls.Add(this.label1);
            this.panel2.Dock = System.Windows.Forms.DockStyle.Top;
            this.panel2.Location = new System.Drawing.Point(230, 0);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(887, 63);
            this.panel2.TabIndex = 26;
            // 
            // rivercb
            // 
            this.rivercb.Font = new System.Drawing.Font("宋体", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.rivercb.FormattingEnabled = true;
            this.rivercb.Location = new System.Drawing.Point(340, 23);
            this.rivercb.Name = "rivercb";
            this.rivercb.Size = new System.Drawing.Size(134, 27);
            this.rivercb.TabIndex = 23;
            this.rivercb.SelectedIndexChanged += new System.EventHandler(this.rivercb_SelectedIndexChanged);
            // 
            // zishui
            // 
            this.zishui.Controls.Add(this.pictureBox2);
            this.zishui.Dock = System.Windows.Forms.DockStyle.Fill;
            this.zishui.Location = new System.Drawing.Point(0, 0);
            this.zishui.Name = "zishui";
            this.zishui.Size = new System.Drawing.Size(887, 511);
            this.zishui.TabIndex = 25;
            // 
            // pictureBox2
            // 
            this.pictureBox2.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox2.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox2.Image")));
            this.pictureBox2.Location = new System.Drawing.Point(0, 0);
            this.pictureBox2.Name = "pictureBox2";
            this.pictureBox2.Size = new System.Drawing.Size(887, 511);
            this.pictureBox2.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox2.TabIndex = 14;
            this.pictureBox2.TabStop = false;
            // 
            // yuanshui
            // 
            this.yuanshui.Controls.Add(this.pictureBox3);
            this.yuanshui.Dock = System.Windows.Forms.DockStyle.Fill;
            this.yuanshui.Location = new System.Drawing.Point(0, 0);
            this.yuanshui.Name = "yuanshui";
            this.yuanshui.Size = new System.Drawing.Size(887, 511);
            this.yuanshui.TabIndex = 26;
            // 
            // pictureBox3
            // 
            this.pictureBox3.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox3.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox3.Image")));
            this.pictureBox3.Location = new System.Drawing.Point(0, 0);
            this.pictureBox3.Name = "pictureBox3";
            this.pictureBox3.Size = new System.Drawing.Size(887, 511);
            this.pictureBox3.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox3.TabIndex = 14;
            this.pictureBox3.TabStop = false;
            // 
            // lishui
            // 
            this.lishui.Controls.Add(this.pictureBox4);
            this.lishui.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lishui.Location = new System.Drawing.Point(0, 0);
            this.lishui.Name = "lishui";
            this.lishui.Size = new System.Drawing.Size(887, 511);
            this.lishui.TabIndex = 27;
            // 
            // pictureBox4
            // 
            this.pictureBox4.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pictureBox4.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox4.Image")));
            this.pictureBox4.Location = new System.Drawing.Point(0, 0);
            this.pictureBox4.Name = "pictureBox4";
            this.pictureBox4.Size = new System.Drawing.Size(887, 511);
            this.pictureBox4.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.pictureBox4.TabIndex = 14;
            this.pictureBox4.TabStop = false;
            // 
            // River1
            // 
            this.River1.Controls.Add(this.xiangjiang);
            this.River1.Controls.Add(this.lishui);
            this.River1.Controls.Add(this.yuanshui);
            this.River1.Controls.Add(this.zishui);
            this.River1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.River1.Location = new System.Drawing.Point(230, 63);
            this.River1.Name = "River1";
            this.River1.Size = new System.Drawing.Size(887, 511);
            this.River1.TabIndex = 15;
            // 
            // Stationmanage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1117, 574);
            this.Controls.Add(this.River1);
            this.Controls.Add(this.panel2);
            this.Controls.Add(this.panel1);
            this.Name = "Stationmanage";
            this.Text = "Stationmanage";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Stationmanage_FormClosing);
            this.Load += new System.EventHandler(this.Stationmanage_Load);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.panel1.ResumeLayout(false);
            this.groupBox1.ResumeLayout(false);
            this.xiangjiang.ResumeLayout(false);
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            this.zishui.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox2)).EndInit();
            this.yuanshui.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox3)).EndInit();
            this.lishui.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox4)).EndInit();
            this.River1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.RichTextBox txt_zsh;
        private System.Windows.Forms.RichTextBox txt_ysh;
        private System.Windows.Forms.RichTextBox txt_lsh;
        private System.Windows.Forms.RichTextBox txt_xj;
        private System.Windows.Forms.Panel xiangjiang;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.Button BtnAddStation;
        private System.Windows.Forms.Button BtnRemoveStation;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.ComboBox rivercb;
        private System.Windows.Forms.Panel zishui;
        private System.Windows.Forms.PictureBox pictureBox2;
        private System.Windows.Forms.Panel yuanshui;
        private System.Windows.Forms.PictureBox pictureBox3;
        private System.Windows.Forms.Panel lishui;
        private System.Windows.Forms.PictureBox pictureBox4;
        private System.Windows.Forms.Panel River1;

    }
}