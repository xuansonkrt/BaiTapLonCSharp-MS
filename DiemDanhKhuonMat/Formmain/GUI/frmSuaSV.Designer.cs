namespace Formmain.GUI
{
    partial class frmSuaSV
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
            this.label1 = new System.Windows.Forms.Label();
            this.radNu = new System.Windows.Forms.RadioButton();
            this.radNam = new System.Windows.Forms.RadioButton();
            this.txtsNgaySinh = new System.Windows.Forms.TextBox();
            this.txtLop = new System.Windows.Forms.TextBox();
            this.txtSHoTen = new System.Windows.Forms.TextBox();
            this.txtSMaSV = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.button1 = new System.Windows.Forms.Button();
            this.btnXacNhan1 = new System.Windows.Forms.Button();
            this.imgSV = new Emgu.CV.UI.ImageBox();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox2 = new System.Windows.Forms.GroupBox();
            ((System.ComponentModel.ISupportInitialize)(this.imgSV)).BeginInit();
            this.groupBox2.SuspendLayout();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(315, 293);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(92, 17);
            this.label1.TabIndex = 11;
            this.label1.Text = "(dd/MM/yyyy)";
            // 
            // radNu
            // 
            this.radNu.AutoSize = true;
            this.radNu.Location = new System.Drawing.Point(276, 347);
            this.radNu.Name = "radNu";
            this.radNu.Size = new System.Drawing.Size(47, 21);
            this.radNu.TabIndex = 10;
            this.radNu.TabStop = true;
            this.radNu.Text = "Nữ";
            this.radNu.UseVisualStyleBackColor = true;
            // 
            // radNam
            // 
            this.radNam.AutoSize = true;
            this.radNam.Location = new System.Drawing.Point(173, 347);
            this.radNam.Name = "radNam";
            this.radNam.Size = new System.Drawing.Size(58, 21);
            this.radNam.TabIndex = 9;
            this.radNam.TabStop = true;
            this.radNam.Text = "Nam";
            this.radNam.UseVisualStyleBackColor = true;
            // 
            // txtsNgaySinh
            // 
            this.txtsNgaySinh.Location = new System.Drawing.Point(173, 290);
            this.txtsNgaySinh.Name = "txtsNgaySinh";
            this.txtsNgaySinh.Size = new System.Drawing.Size(136, 22);
            this.txtsNgaySinh.TabIndex = 8;
            // 
            // txtLop
            // 
            this.txtLop.Location = new System.Drawing.Point(173, 227);
            this.txtLop.Name = "txtLop";
            this.txtLop.Size = new System.Drawing.Size(292, 22);
            this.txtLop.TabIndex = 7;
            // 
            // txtSHoTen
            // 
            this.txtSHoTen.Location = new System.Drawing.Point(173, 167);
            this.txtSHoTen.Name = "txtSHoTen";
            this.txtSHoTen.Size = new System.Drawing.Size(292, 22);
            this.txtSHoTen.TabIndex = 6;
            // 
            // txtSMaSV
            // 
            this.txtSMaSV.Location = new System.Drawing.Point(173, 101);
            this.txtSMaSV.Name = "txtSMaSV";
            this.txtSMaSV.Size = new System.Drawing.Size(136, 22);
            this.txtSMaSV.TabIndex = 5;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(50, 347);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(60, 17);
            this.label6.TabIndex = 4;
            this.label6.Text = "Giới tính";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(50, 287);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(71, 17);
            this.label5.TabIndex = 3;
            this.label5.Text = "Ngày sinh";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(50, 227);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(32, 17);
            this.label4.TabIndex = 2;
            this.label4.Text = "Lớp";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(50, 167);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(50, 17);
            this.label3.TabIndex = 1;
            this.label3.Text = "Họ tên";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(47, 352);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(130, 45);
            this.button1.TabIndex = 35;
            this.button1.Text = "Hủy ";
            this.button1.UseVisualStyleBackColor = true;
            // 
            // btnXacNhan1
            // 
            this.btnXacNhan1.Location = new System.Drawing.Point(47, 279);
            this.btnXacNhan1.Name = "btnXacNhan1";
            this.btnXacNhan1.Size = new System.Drawing.Size(130, 45);
            this.btnXacNhan1.TabIndex = 34;
            this.btnXacNhan1.Text = "Xác nhận";
            this.btnXacNhan1.UseVisualStyleBackColor = true;
            this.btnXacNhan1.Click += new System.EventHandler(this.btnXacNhan1_Click);
            // 
            // imgSV
            // 
            this.imgSV.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.imgSV.Location = new System.Drawing.Point(17, 12);
            this.imgSV.Name = "imgSV";
            this.imgSV.Size = new System.Drawing.Size(200, 200);
            this.imgSV.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.imgSV.TabIndex = 32;
            this.imgSV.TabStop = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(50, 107);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(87, 17);
            this.label2.TabIndex = 0;
            this.label2.Text = "Mã sinh viên";
            // 
            // groupBox2
            // 
            this.groupBox2.Controls.Add(this.label1);
            this.groupBox2.Controls.Add(this.radNu);
            this.groupBox2.Controls.Add(this.radNam);
            this.groupBox2.Controls.Add(this.txtsNgaySinh);
            this.groupBox2.Controls.Add(this.txtLop);
            this.groupBox2.Controls.Add(this.txtSHoTen);
            this.groupBox2.Controls.Add(this.txtSMaSV);
            this.groupBox2.Controls.Add(this.label6);
            this.groupBox2.Controls.Add(this.label5);
            this.groupBox2.Controls.Add(this.label4);
            this.groupBox2.Controls.Add(this.label3);
            this.groupBox2.Controls.Add(this.label2);
            this.groupBox2.Location = new System.Drawing.Point(294, 12);
            this.groupBox2.Name = "groupBox2";
            this.groupBox2.Size = new System.Drawing.Size(497, 409);
            this.groupBox2.TabIndex = 33;
            this.groupBox2.TabStop = false;
            this.groupBox2.Text = "Thông tin sinh viên";
            // 
            // frmSuaSV
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(807, 431);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnXacNhan1);
            this.Controls.Add(this.imgSV);
            this.Controls.Add(this.groupBox2);
            this.Name = "frmSuaSV";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterParent;
            this.Text = "Sửa thông tin sinh viên";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.frmSuaSV_FormClosed);
            this.Load += new System.EventHandler(this.frmSuaSV_Load);
            ((System.ComponentModel.ISupportInitialize)(this.imgSV)).EndInit();
            this.groupBox2.ResumeLayout(false);
            this.groupBox2.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label1;
        public System.Windows.Forms.RadioButton radNu;
        public System.Windows.Forms.RadioButton radNam;
        public System.Windows.Forms.TextBox txtsNgaySinh;
        public System.Windows.Forms.TextBox txtLop;
        public System.Windows.Forms.TextBox txtSHoTen;
        public System.Windows.Forms.TextBox txtSMaSV;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.Button btnXacNhan1;
        public Emgu.CV.UI.ImageBox imgSV;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox2;
    }
}