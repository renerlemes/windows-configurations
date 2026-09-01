namespace Windows.Configurations
{
    partial class frmUpdater
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmUpdater));
            groupBox1 = new System.Windows.Forms.GroupBox();
            txtChangelog = new System.Windows.Forms.TextBox();
            btnInstalar = new System.Windows.Forms.Button();
            groupBox1.SuspendLayout();
            SuspendLayout();
            // 
            // groupBox1
            // 
            groupBox1.Anchor = System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left | System.Windows.Forms.AnchorStyles.Right;
            groupBox1.Controls.Add(txtChangelog);
            groupBox1.Location = new System.Drawing.Point(12, 12);
            groupBox1.Name = "groupBox1";
            groupBox1.Padding = new System.Windows.Forms.Padding(12);
            groupBox1.Size = new System.Drawing.Size(776, 397);
            groupBox1.TabIndex = 0;
            groupBox1.TabStop = false;
            groupBox1.Text = "Changelog";
            // 
            // txtChangelog
            // 
            txtChangelog.BorderStyle = System.Windows.Forms.BorderStyle.None;
            txtChangelog.Dock = System.Windows.Forms.DockStyle.Fill;
            txtChangelog.Location = new System.Drawing.Point(12, 28);
            txtChangelog.Multiline = true;
            txtChangelog.Name = "txtChangelog";
            txtChangelog.ReadOnly = true;
            txtChangelog.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            txtChangelog.Size = new System.Drawing.Size(752, 357);
            txtChangelog.TabIndex = 0;
            // 
            // btnInstalar
            // 
            btnInstalar.Location = new System.Drawing.Point(713, 415);
            btnInstalar.Name = "btnInstalar";
            btnInstalar.Size = new System.Drawing.Size(75, 23);
            btnInstalar.TabIndex = 1;
            btnInstalar.Text = "Instalar";
            btnInstalar.UseVisualStyleBackColor = true;
            btnInstalar.Click += btnInstalar_Click;
            // 
            // frmUpdater
            // 
            AutoScaleDimensions = new System.Drawing.SizeF(7F, 15F);
            AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            ClientSize = new System.Drawing.Size(800, 450);
            Controls.Add(btnInstalar);
            Controls.Add(groupBox1);
            FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            Icon = (System.Drawing.Icon)resources.GetObject("$this.Icon");
            MaximizeBox = false;
            MinimizeBox = false;
            Name = "frmUpdater";
            ShowInTaskbar = false;
            StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            Text = "Windows Configurations";
            groupBox1.ResumeLayout(false);
            groupBox1.PerformLayout();
            ResumeLayout(false);
        }

        #endregion

        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.TextBox txtChangelog;
        private System.Windows.Forms.Button btnInstalar;
    }
}