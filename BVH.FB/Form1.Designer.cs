namespace BVH.FB
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
            this.gridAccInfor = new System.Windows.Forms.DataGridView();
            this.lbCountSelected = new System.Windows.Forms.Label();
            this.lbCountAll = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.txtProfilePath = new System.Windows.Forms.TextBox();
            this.label6 = new System.Windows.Forms.Label();
            this.txtImagePath = new System.Windows.Forms.TextBox();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.txtWait = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.btnStop = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.gridAccInfor)).BeginInit();
            this.groupBox1.SuspendLayout();
            this.SuspendLayout();
            // 
            // gridAccInfor
            // 
            this.gridAccInfor.ColumnHeadersHeightSizeMode = System.Windows.Forms.DataGridViewColumnHeadersHeightSizeMode.AutoSize;
            this.gridAccInfor.Location = new System.Drawing.Point(2, 100);
            this.gridAccInfor.Name = "gridAccInfor";
            this.gridAccInfor.Size = new System.Drawing.Size(1054, 438);
            this.gridAccInfor.TabIndex = 0;
            this.gridAccInfor.CellPainting += new System.Windows.Forms.DataGridViewCellPaintingEventHandler(this.grdAccount_CellPainting);
            this.gridAccInfor.RowPostPaint += new System.Windows.Forms.DataGridViewRowPostPaintEventHandler(this.grdAccount_RowPostPaint);
            this.gridAccInfor.SelectionChanged += new System.EventHandler(this.grdAccount_SelectionChanged);
            this.gridAccInfor.KeyDown += new System.Windows.Forms.KeyEventHandler(this.grdAccount_KeyDown);
            this.gridAccInfor.MouseClick += new System.Windows.Forms.MouseEventHandler(this.grdAccount_MouseClick);
            // 
            // lbCountSelected
            // 
            this.lbCountSelected.AutoSize = true;
            this.lbCountSelected.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbCountSelected.ForeColor = System.Drawing.Color.Green;
            this.lbCountSelected.Location = new System.Drawing.Point(195, 541);
            this.lbCountSelected.Name = "lbCountSelected";
            this.lbCountSelected.Size = new System.Drawing.Size(18, 20);
            this.lbCountSelected.TabIndex = 12;
            this.lbCountSelected.Text = "0";
            // 
            // lbCountAll
            // 
            this.lbCountAll.AutoSize = true;
            this.lbCountAll.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.lbCountAll.ForeColor = System.Drawing.Color.DarkOrange;
            this.lbCountAll.Location = new System.Drawing.Point(45, 541);
            this.lbCountAll.Name = "lbCountAll";
            this.lbCountAll.Size = new System.Drawing.Size(18, 20);
            this.lbCountAll.TabIndex = 11;
            this.lbCountAll.Text = "0";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.Location = new System.Drawing.Point(130, 546);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(59, 13);
            this.label3.TabIndex = 10;
            this.label3.Text = "Đã chọn:";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Verdana", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(-1, 545);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(40, 13);
            this.label2.TabIndex = 9;
            this.label2.Text = "Tổng:";
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.btnStop);
            this.groupBox1.Controls.Add(this.txtProfilePath);
            this.groupBox1.Controls.Add(this.label6);
            this.groupBox1.Controls.Add(this.txtImagePath);
            this.groupBox1.Controls.Add(this.label5);
            this.groupBox1.Controls.Add(this.label4);
            this.groupBox1.Controls.Add(this.txtWait);
            this.groupBox1.Controls.Add(this.label1);
            this.groupBox1.Location = new System.Drawing.Point(2, 13);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(1054, 81);
            this.groupBox1.TabIndex = 13;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Cấu hình";
            // 
            // txtProfilePath
            // 
            this.txtProfilePath.Location = new System.Drawing.Point(113, 43);
            this.txtProfilePath.Name = "txtProfilePath";
            this.txtProfilePath.Size = new System.Drawing.Size(289, 20);
            this.txtProfilePath.TabIndex = 16;
            this.txtProfilePath.Text = "C:\\ChromeProfiles";
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Location = new System.Drawing.Point(10, 46);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(97, 13);
            this.label6.TabIndex = 15;
            this.label6.Text = "Thư mục lưu profile";
            // 
            // txtImagePath
            // 
            this.txtImagePath.Location = new System.Drawing.Point(113, 17);
            this.txtImagePath.Name = "txtImagePath";
            this.txtImagePath.Size = new System.Drawing.Size(289, 20);
            this.txtImagePath.TabIndex = 14;
            this.txtImagePath.Text = "C:\\Images";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Location = new System.Drawing.Point(10, 20);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(70, 13);
            this.label5.TabIndex = 3;
            this.label5.Text = "Thư mục ảnh";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(801, 20);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(0, 13);
            this.label4.TabIndex = 2;
            // 
            // txtWait
            // 
            this.txtWait.Location = new System.Drawing.Point(1016, 32);
            this.txtWait.Name = "txtWait";
            this.txtWait.Size = new System.Drawing.Size(27, 20);
            this.txtWait.TabIndex = 1;
            this.txtWait.Text = "15";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(946, 16);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(97, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Thời gian đợi (giây)";
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(532, 19);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 17;
            this.btnStop.Text = "Dừng chạy";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1057, 563);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.lbCountSelected);
            this.Controls.Add(this.lbCountAll);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.gridAccInfor);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "FB by HỘI TAY VỊN";
            ((System.ComponentModel.ISupportInitialize)(this.gridAccInfor)).EndInit();
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.DataGridView gridAccInfor;
        private System.Windows.Forms.Label lbCountSelected;
        private System.Windows.Forms.Label lbCountAll;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.TextBox txtWait;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtProfilePath;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.TextBox txtImagePath;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Button btnStop;
    }
}

