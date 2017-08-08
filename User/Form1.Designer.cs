namespace User
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.lb_3 = new DevExpress.XtraEditors.LabelControl();
            this.lb_timeStart = new DevExpress.XtraEditors.LabelControl();
            this.lb_timeworking = new DevExpress.XtraEditors.LabelControl();
            this.btn_workStop = new DevExpress.XtraEditors.SimpleButton();
            this.lb1 = new DevExpress.XtraEditors.LabelControl();
            this.btn_workStart = new DevExpress.XtraEditors.SimpleButton();
            this.lb_today = new DevExpress.XtraEditors.LabelControl();
            this.lb_2 = new DevExpress.XtraEditors.LabelControl();
            this.SuspendLayout();
            // 
            // lb_3
            // 
            this.lb_3.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lb_3.Location = new System.Drawing.Point(12, 106);
            this.lb_3.Name = "lb_3";
            this.lb_3.Size = new System.Drawing.Size(126, 19);
            this.lb_3.TabIndex = 4;
            this.lb_3.Text = "Thời gian đã làm:";
            this.lb_3.Click += new System.EventHandler(this.lb_3_Click);
            // 
            // lb_timeStart
            // 
            this.lb_timeStart.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lb_timeStart.Location = new System.Drawing.Point(149, 81);
            this.lb_timeStart.Name = "lb_timeStart";
            this.lb_timeStart.Size = new System.Drawing.Size(68, 19);
            this.lb_timeStart.TabIndex = 3;
            this.lb_timeStart.Text = "HH : MM";
            this.lb_timeStart.Click += new System.EventHandler(this.lb_timeStart_Click);
            // 
            // lb_timeworking
            // 
            this.lb_timeworking.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lb_timeworking.Location = new System.Drawing.Point(149, 106);
            this.lb_timeworking.Name = "lb_timeworking";
            this.lb_timeworking.Size = new System.Drawing.Size(62, 19);
            this.lb_timeworking.TabIndex = 5;
            this.lb_timeworking.Text = "0 : 0 : 0";
            this.lb_timeworking.Click += new System.EventHandler(this.lb_timeworking_Click);
            // 
            // btn_workStop
            // 
            this.btn_workStop.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btn_workStop.Appearance.Options.UseFont = true;
            this.btn_workStop.Enabled = false;
            this.btn_workStop.Image = ((System.Drawing.Image)(resources.GetObject("btn_workStop.Image")));
            this.btn_workStop.Location = new System.Drawing.Point(12, 285);
            this.btn_workStop.Name = "btn_workStop";
            this.btn_workStop.Size = new System.Drawing.Size(241, 40);
            this.btn_workStop.TabIndex = 1;
            this.btn_workStop.Text = "Kết Thúc Làm Việc";
            this.btn_workStop.Click += new System.EventHandler(this.btn_workStop_Click);
            // 
            // lb1
            // 
            this.lb1.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lb1.Location = new System.Drawing.Point(12, 56);
            this.lb1.Name = "lb1";
            this.lb1.Size = new System.Drawing.Size(105, 19);
            this.lb1.TabIndex = 6;
            this.lb1.Text = "Ngày làm việc:";
            this.lb1.Click += new System.EventHandler(this.lb1_Click);
            // 
            // btn_workStart
            // 
            this.btn_workStart.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.btn_workStart.Appearance.Options.UseFont = true;
            this.btn_workStart.Image = ((System.Drawing.Image)(resources.GetObject("btn_workStart.Image")));
            this.btn_workStart.Location = new System.Drawing.Point(12, 237);
            this.btn_workStart.Name = "btn_workStart";
            this.btn_workStart.Size = new System.Drawing.Size(241, 42);
            this.btn_workStart.TabIndex = 0;
            this.btn_workStart.Text = "Bắt Đầu Làm Việc";
            this.btn_workStart.Click += new System.EventHandler(this.btn_workStart_Click);
            // 
            // lb_today
            // 
            this.lb_today.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lb_today.Location = new System.Drawing.Point(149, 56);
            this.lb_today.Name = "lb_today";
            this.lb_today.Size = new System.Drawing.Size(114, 19);
            this.lb_today.TabIndex = 7;
            this.lb_today.Text = "DD/MM/YYYY";
            this.lb_today.Click += new System.EventHandler(this.lb_today_Click);
            // 
            // lb_2
            // 
            this.lb_2.Appearance.Font = new System.Drawing.Font("Tahoma", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(163)));
            this.lb_2.Location = new System.Drawing.Point(12, 81);
            this.lb_2.Name = "lb_2";
            this.lb_2.Size = new System.Drawing.Size(131, 19);
            this.lb_2.TabIndex = 2;
            this.lb_2.Text = "Thời gian bắt đầu:";
            this.lb_2.Click += new System.EventHandler(this.lb_2_Click);
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(280, 337);
            this.Controls.Add(this.lb_today);
            this.Controls.Add(this.lb1);
            this.Controls.Add(this.lb_timeworking);
            this.Controls.Add(this.lb_3);
            this.Controls.Add(this.lb_timeStart);
            this.Controls.Add(this.lb_2);
            this.Controls.Add(this.btn_workStop);
            this.Controls.Add(this.btn_workStart);
            this.Name = "Form1";
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.Form1_Load);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl lb_3;
        private DevExpress.XtraEditors.LabelControl lb_timeStart;
        private DevExpress.XtraEditors.LabelControl lb_timeworking;
        private DevExpress.XtraEditors.SimpleButton btn_workStop;
        private DevExpress.XtraEditors.LabelControl lb1;
        private DevExpress.XtraEditors.SimpleButton btn_workStart;
        private DevExpress.XtraEditors.LabelControl lb_today;
        private DevExpress.XtraEditors.LabelControl lb_2;
    }
}

