namespace User
{
    partial class Login
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Login));
            this.labelControl1 = new DevExpress.XtraEditors.LabelControl();
            this.labelControl2 = new DevExpress.XtraEditors.LabelControl();
            this.txt_id = new DevExpress.XtraEditors.TextEdit();
            this.txt_pass = new DevExpress.XtraEditors.TextEdit();
            this.simpleButton1 = new DevExpress.XtraEditors.SimpleButton();
            this.pnl_loadding = new DevExpress.XtraWaitForm.ProgressPanel();
            ((System.ComponentModel.ISupportInitialize)(this.txt_id.Properties)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_pass.Properties)).BeginInit();
            this.SuspendLayout();
            // 
            // labelControl1
            // 
            this.labelControl1.Location = new System.Drawing.Point(26, 227);
            this.labelControl1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.labelControl1.Name = "labelControl1";
            this.labelControl1.Size = new System.Drawing.Size(150, 25);
            this.labelControl1.TabIndex = 0;
            this.labelControl1.Text = "Tên đăng nhập:";
            // 
            // labelControl2
            // 
            this.labelControl2.Location = new System.Drawing.Point(82, 281);
            this.labelControl2.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.labelControl2.Name = "labelControl2";
            this.labelControl2.Size = new System.Drawing.Size(93, 25);
            this.labelControl2.TabIndex = 1;
            this.labelControl2.Text = "Mật khẩu:";
            // 
            // txt_id
            // 
            this.txt_id.EditValue = "nv1@gmail.com";
            this.txt_id.Location = new System.Drawing.Point(192, 219);
            this.txt_id.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txt_id.Name = "txt_id";
            this.txt_id.Size = new System.Drawing.Size(322, 32);
            this.txt_id.TabIndex = 2;
            // 
            // txt_pass
            // 
            this.txt_pass.EditValue = "Aa@123";
            this.txt_pass.Location = new System.Drawing.Point(192, 275);
            this.txt_pass.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.txt_pass.Name = "txt_pass";
            this.txt_pass.Properties.PasswordChar = '*';
            this.txt_pass.Size = new System.Drawing.Size(322, 32);
            this.txt_pass.TabIndex = 3;
            // 
            // simpleButton1
            // 
            this.simpleButton1.Image = ((System.Drawing.Image)(resources.GetObject("simpleButton1.Image")));
            this.simpleButton1.Location = new System.Drawing.Point(132, 348);
            this.simpleButton1.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.simpleButton1.Name = "simpleButton1";
            this.simpleButton1.Size = new System.Drawing.Size(236, 81);
            this.simpleButton1.TabIndex = 4;
            this.simpleButton1.Text = "Đăng Nhập";
            this.simpleButton1.Click += new System.EventHandler(this.simpleButton1_Click);
            // 
            // pnl_loadding
            // 
            this.pnl_loadding.Appearance.BackColor = System.Drawing.Color.Transparent;
            this.pnl_loadding.Appearance.Options.UseBackColor = true;
            this.pnl_loadding.AppearanceCaption.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F);
            this.pnl_loadding.AppearanceCaption.Options.UseFont = true;
            this.pnl_loadding.AppearanceDescription.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F);
            this.pnl_loadding.AppearanceDescription.Options.UseFont = true;
            this.pnl_loadding.Caption = "Kết Nối Server";
            this.pnl_loadding.Location = new System.Drawing.Point(100, 62);
            this.pnl_loadding.LookAndFeel.SkinName = "The Asphalt World";
            this.pnl_loadding.LookAndFeel.UseDefaultLookAndFeel = false;
            this.pnl_loadding.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.pnl_loadding.Name = "pnl_loadding";
            this.pnl_loadding.Size = new System.Drawing.Size(356, 123);
            this.pnl_loadding.TabIndex = 0;
            this.pnl_loadding.Text = "progressPanel1";
            // 
            // Login
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(12F, 25F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(538, 648);
            this.Controls.Add(this.pnl_loadding);
            this.Controls.Add(this.simpleButton1);
            this.Controls.Add(this.txt_pass);
            this.Controls.Add(this.txt_id);
            this.Controls.Add(this.labelControl2);
            this.Controls.Add(this.labelControl1);
            this.Margin = new System.Windows.Forms.Padding(6, 6, 6, 6);
            this.Name = "Login";
            this.Text = "Login";
            this.Load += new System.EventHandler(this.Login_Load);
            ((System.ComponentModel.ISupportInitialize)(this.txt_id.Properties)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.txt_pass.Properties)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private DevExpress.XtraEditors.LabelControl labelControl1;
        private DevExpress.XtraEditors.LabelControl labelControl2;
        private DevExpress.XtraEditors.TextEdit txt_id;
        private DevExpress.XtraEditors.TextEdit txt_pass;
        private DevExpress.XtraEditors.SimpleButton simpleButton1;
        private DevExpress.XtraWaitForm.ProgressPanel pnl_loadding;
    }
}