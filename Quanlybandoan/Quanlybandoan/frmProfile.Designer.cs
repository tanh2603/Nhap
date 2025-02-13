namespace Quanlybandoan
{
    partial class frmProfile
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
            System.Windows.Forms.Label tenhienthiLabel;
            System.Windows.Forms.Label tendangnhapLabel;
            System.Windows.Forms.Label matkhauLabel;
            System.Windows.Forms.Label label1;
            System.Windows.Forms.Label label2;
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmProfile));
            this.lblProfile = new System.Windows.Forms.Label();
            this.pictureBox1 = new System.Windows.Forms.PictureBox();
            this.btnHuy = new System.Windows.Forms.Button();
            this.btnCapnhat = new System.Windows.Forms.Button();
            this.txtTenhienthi = new System.Windows.Forms.TextBox();
            this.txtTendangnhap = new System.Windows.Forms.TextBox();
            this.txtMatkhau = new System.Windows.Forms.TextBox();
            this.txtMkmoi = new System.Windows.Forms.TextBox();
            this.txtXacnhanmk = new System.Windows.Forms.TextBox();
            tenhienthiLabel = new System.Windows.Forms.Label();
            tendangnhapLabel = new System.Windows.Forms.Label();
            matkhauLabel = new System.Windows.Forms.Label();
            label1 = new System.Windows.Forms.Label();
            label2 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).BeginInit();
            this.SuspendLayout();
            // 
            // tenhienthiLabel
            // 
            tenhienthiLabel.AutoSize = true;
            tenhienthiLabel.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            tenhienthiLabel.Location = new System.Drawing.Point(290, 75);
            tenhienthiLabel.Name = "tenhienthiLabel";
            tenhienthiLabel.Size = new System.Drawing.Size(130, 22);
            tenhienthiLabel.TabIndex = 11;
            tenhienthiLabel.Text = "Tên hiển thị";
            // 
            // tendangnhapLabel
            // 
            tendangnhapLabel.AutoSize = true;
            tendangnhapLabel.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            tendangnhapLabel.Location = new System.Drawing.Point(290, 112);
            tendangnhapLabel.Name = "tendangnhapLabel";
            tendangnhapLabel.Size = new System.Drawing.Size(140, 22);
            tendangnhapLabel.TabIndex = 14;
            tendangnhapLabel.Text = "Tên đăng nhập";
            // 
            // matkhauLabel
            // 
            matkhauLabel.AutoSize = true;
            matkhauLabel.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            matkhauLabel.Location = new System.Drawing.Point(290, 150);
            matkhauLabel.Name = "matkhauLabel";
            matkhauLabel.Size = new System.Drawing.Size(90, 22);
            matkhauLabel.TabIndex = 16;
            matkhauLabel.Text = "Mật khẩu";
            // 
            // label1
            // 
            label1.AutoSize = true;
            label1.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            label1.Location = new System.Drawing.Point(290, 191);
            label1.Name = "label1";
            label1.Size = new System.Drawing.Size(130, 22);
            label1.TabIndex = 22;
            label1.Text = "Mật khẩu mới";
            // 
            // label2
            // 
            label2.AutoSize = true;
            label2.Font = new System.Drawing.Font("Consolas", 14.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            label2.Location = new System.Drawing.Point(290, 233);
            label2.Name = "label2";
            label2.Size = new System.Drawing.Size(180, 22);
            label2.TabIndex = 24;
            label2.Text = "Xác nhận mật khẩu";
            // 
            // lblProfile
            // 
            this.lblProfile.AutoSize = true;
            this.lblProfile.Font = new System.Drawing.Font("Consolas", 27.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblProfile.ForeColor = System.Drawing.Color.MediumSlateBlue;
            this.lblProfile.Location = new System.Drawing.Point(318, 9);
            this.lblProfile.Name = "lblProfile";
            this.lblProfile.Size = new System.Drawing.Size(399, 43);
            this.lblProfile.TabIndex = 20;
            this.lblProfile.Text = "Thông tin tài khoản";
            // 
            // pictureBox1
            // 
            this.pictureBox1.Image = ((System.Drawing.Image)(resources.GetObject("pictureBox1.Image")));
            this.pictureBox1.Location = new System.Drawing.Point(12, 25);
            this.pictureBox1.Name = "pictureBox1";
            this.pictureBox1.Size = new System.Drawing.Size(263, 276);
            this.pictureBox1.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.pictureBox1.TabIndex = 19;
            this.pictureBox1.TabStop = false;
            // 
            // btnHuy
            // 
            this.btnHuy.BackColor = System.Drawing.Color.MediumPurple;
            this.btnHuy.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnHuy.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnHuy.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnHuy.Location = new System.Drawing.Point(609, 275);
            this.btnHuy.Name = "btnHuy";
            this.btnHuy.Size = new System.Drawing.Size(130, 50);
            this.btnHuy.TabIndex = 17;
            this.btnHuy.Text = "Hủy";
            this.btnHuy.UseVisualStyleBackColor = false;
            this.btnHuy.Click += new System.EventHandler(this.btnHuy_Click);
            // 
            // btnCapnhat
            // 
            this.btnCapnhat.BackColor = System.Drawing.Color.MediumPurple;
            this.btnCapnhat.Font = new System.Drawing.Font("Consolas", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCapnhat.ForeColor = System.Drawing.SystemColors.ControlLightLight;
            this.btnCapnhat.Location = new System.Drawing.Point(478, 275);
            this.btnCapnhat.Name = "btnCapnhat";
            this.btnCapnhat.Size = new System.Drawing.Size(130, 50);
            this.btnCapnhat.TabIndex = 18;
            this.btnCapnhat.Text = "Cập nhật";
            this.btnCapnhat.UseVisualStyleBackColor = false;
            this.btnCapnhat.Click += new System.EventHandler(this.btnCapnhat_Click);
            // 
            // txtTenhienthi
            // 
            this.txtTenhienthi.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTenhienthi.Location = new System.Drawing.Point(478, 73);
            this.txtTenhienthi.Name = "txtTenhienthi";
            this.txtTenhienthi.Size = new System.Drawing.Size(261, 26);
            this.txtTenhienthi.TabIndex = 12;
            // 
            // txtTendangnhap
            // 
            this.txtTendangnhap.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtTendangnhap.Location = new System.Drawing.Point(478, 110);
            this.txtTendangnhap.Name = "txtTendangnhap";
            this.txtTendangnhap.Size = new System.Drawing.Size(261, 26);
            this.txtTendangnhap.TabIndex = 13;
            // 
            // txtMatkhau
            // 
            this.txtMatkhau.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMatkhau.Location = new System.Drawing.Point(478, 148);
            this.txtMatkhau.Name = "txtMatkhau";
            this.txtMatkhau.Size = new System.Drawing.Size(261, 26);
            this.txtMatkhau.TabIndex = 15;
            this.txtMatkhau.UseSystemPasswordChar = true;
            // 
            // txtMkmoi
            // 
            this.txtMkmoi.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtMkmoi.Location = new System.Drawing.Point(478, 189);
            this.txtMkmoi.Name = "txtMkmoi";
            this.txtMkmoi.Size = new System.Drawing.Size(261, 26);
            this.txtMkmoi.TabIndex = 21;
            this.txtMkmoi.UseSystemPasswordChar = true;
            // 
            // txtXacnhanmk
            // 
            this.txtXacnhanmk.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.txtXacnhanmk.Location = new System.Drawing.Point(478, 231);
            this.txtXacnhanmk.Name = "txtXacnhanmk";
            this.txtXacnhanmk.Size = new System.Drawing.Size(261, 26);
            this.txtXacnhanmk.TabIndex = 23;
            this.txtXacnhanmk.UseSystemPasswordChar = true;
            // 
            // frmProfile
            // 
            this.AcceptButton = this.btnCapnhat;
            this.AutoScaleDimensions = new System.Drawing.SizeF(96F, 96F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi;
            this.BackColor = System.Drawing.Color.Lavender;
            this.CancelButton = this.btnHuy;
            this.ClientSize = new System.Drawing.Size(779, 341);
            this.Controls.Add(label2);
            this.Controls.Add(this.txtXacnhanmk);
            this.Controls.Add(label1);
            this.Controls.Add(this.txtMkmoi);
            this.Controls.Add(this.lblProfile);
            this.Controls.Add(this.pictureBox1);
            this.Controls.Add(this.btnHuy);
            this.Controls.Add(this.btnCapnhat);
            this.Controls.Add(tenhienthiLabel);
            this.Controls.Add(this.txtTenhienthi);
            this.Controls.Add(tendangnhapLabel);
            this.Controls.Add(this.txtTendangnhap);
            this.Controls.Add(matkhauLabel);
            this.Controls.Add(this.txtMatkhau);
            this.DoubleBuffered = true;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MaximizeBox = false;
            this.Name = "frmProfile";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Thông tin tài khoản";
            ((System.ComponentModel.ISupportInitialize)(this.pictureBox1)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Label lblProfile;
        private System.Windows.Forms.PictureBox pictureBox1;
        private System.Windows.Forms.Button btnHuy;
        private System.Windows.Forms.Button btnCapnhat;
        private System.Windows.Forms.TextBox txtTenhienthi;
        private System.Windows.Forms.TextBox txtTendangnhap;
        private System.Windows.Forms.TextBox txtMatkhau;
        private System.Windows.Forms.TextBox txtMkmoi;
        private System.Windows.Forms.TextBox txtXacnhanmk;
    }
}