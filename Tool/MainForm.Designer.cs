namespace com.mkysoft.gib.tool
{
    partial class MainForm
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
            this.btnGonder = new System.Windows.Forms.Button();
            this.lbLog = new System.Windows.Forms.ListBox();
            this.rbHataliZarfID = new System.Windows.Forms.RadioButton();
            this.txtGIBServisAdresi = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.fdOpen = new System.Windows.Forms.OpenFileDialog();
            this.rbZarf = new System.Windows.Forms.RadioButton();
            this.btnGozat = new System.Windows.Forms.Button();
            this.rbHataliOzetDeger = new System.Windows.Forms.RadioButton();
            this.SuspendLayout();
            // 
            // btnGonder
            // 
            this.btnGonder.Location = new System.Drawing.Point(307, 89);
            this.btnGonder.Name = "btnGonder";
            this.btnGonder.Size = new System.Drawing.Size(75, 23);
            this.btnGonder.TabIndex = 0;
            this.btnGonder.Text = "Gönder";
            this.btnGonder.UseVisualStyleBackColor = true;
            this.btnGonder.Click += new System.EventHandler(this.BtnGonder_Click);
            // 
            // lbLog
            // 
            this.lbLog.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.lbLog.FormattingEnabled = true;
            this.lbLog.Location = new System.Drawing.Point(0, 160);
            this.lbLog.Name = "lbLog";
            this.lbLog.Size = new System.Drawing.Size(800, 290);
            this.lbLog.TabIndex = 1;
            // 
            // rbHataliZarfID
            // 
            this.rbHataliZarfID.AutoSize = true;
            this.rbHataliZarfID.Checked = true;
            this.rbHataliZarfID.Location = new System.Drawing.Point(12, 61);
            this.rbHataliZarfID.Name = "rbHataliZarfID";
            this.rbHataliZarfID.Size = new System.Drawing.Size(117, 17);
            this.rbHataliZarfID.TabIndex = 0;
            this.rbHataliZarfID.Text = "Hatalı zarf numarası";
            this.rbHataliZarfID.UseVisualStyleBackColor = true;
            // 
            // txtGIBServisAdresi
            // 
            this.txtGIBServisAdresi.Location = new System.Drawing.Point(107, 12);
            this.txtGIBServisAdresi.Name = "txtGIBServisAdresi";
            this.txtGIBServisAdresi.Size = new System.Drawing.Size(336, 20);
            this.txtGIBServisAdresi.TabIndex = 2;
            this.txtGIBServisAdresi.Text = "http://localhost:50993/ReceiverGIB.svc";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(12, 15);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(89, 13);
            this.label1.TabIndex = 3;
            this.label1.Text = "GİB Servis Adresi";
            // 
            // fdOpen
            // 
            this.fdOpen.FileName = "openFileDialog1";
            // 
            // rbZarf
            // 
            this.rbZarf.AutoSize = true;
            this.rbZarf.Location = new System.Drawing.Point(12, 38);
            this.rbZarf.Name = "rbZarf";
            this.rbZarf.Size = new System.Drawing.Size(44, 17);
            this.rbZarf.TabIndex = 4;
            this.rbZarf.Text = "Zarf";
            this.rbZarf.UseVisualStyleBackColor = true;
            this.rbZarf.CheckedChanged += new System.EventHandler(this.RbZarf_CheckedChanged);
            // 
            // btnGozat
            // 
            this.btnGozat.Location = new System.Drawing.Point(138, 35);
            this.btnGozat.Name = "btnGozat";
            this.btnGozat.Size = new System.Drawing.Size(75, 23);
            this.btnGozat.TabIndex = 5;
            this.btnGozat.Text = "Gözat";
            this.btnGozat.UseVisualStyleBackColor = true;
            this.btnGozat.Visible = false;
            this.btnGozat.Click += new System.EventHandler(this.BtnGozat_Click);
            // 
            // rbHataliOzetDeger
            // 
            this.rbHataliOzetDeger.AutoSize = true;
            this.rbHataliOzetDeger.Location = new System.Drawing.Point(12, 84);
            this.rbHataliOzetDeger.Name = "rbHataliOzetDeger";
            this.rbHataliOzetDeger.Size = new System.Drawing.Size(105, 17);
            this.rbHataliOzetDeger.TabIndex = 6;
            this.rbHataliOzetDeger.Text = "Hatalı özet değer";
            this.rbHataliOzetDeger.UseVisualStyleBackColor = true;
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(800, 450);
            this.Controls.Add(this.rbHataliOzetDeger);
            this.Controls.Add(this.btnGozat);
            this.Controls.Add(this.rbZarf);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.txtGIBServisAdresi);
            this.Controls.Add(this.rbHataliZarfID);
            this.Controls.Add(this.lbLog);
            this.Controls.Add(this.btnGonder);
            this.Name = "Form1";
            this.Text = "GIB Tester";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnGonder;
        private System.Windows.Forms.ListBox lbLog;
        private System.Windows.Forms.RadioButton rbHataliZarfID;
        private System.Windows.Forms.TextBox txtGIBServisAdresi;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.OpenFileDialog fdOpen;
        private System.Windows.Forms.RadioButton rbZarf;
        private System.Windows.Forms.Button btnGozat;
        private System.Windows.Forms.RadioButton rbHataliOzetDeger;
    }
}

