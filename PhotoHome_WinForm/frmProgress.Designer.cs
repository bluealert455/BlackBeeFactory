namespace PhotoHome
{
    partial class frmProgress
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
            this.progbarGoing = new System.Windows.Forms.ProgressBar();
            this.lblPrompt = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // progbarGoing
            // 
            this.progbarGoing.Location = new System.Drawing.Point(12, 56);
            this.progbarGoing.Name = "progbarGoing";
            this.progbarGoing.Size = new System.Drawing.Size(362, 13);
            this.progbarGoing.TabIndex = 0;
            // 
            // lblPrompt
            // 
            this.lblPrompt.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblPrompt.ForeColor = System.Drawing.Color.White;
            this.lblPrompt.Location = new System.Drawing.Point(12, 9);
            this.lblPrompt.Name = "lblPrompt";
            this.lblPrompt.Size = new System.Drawing.Size(362, 41);
            this.lblPrompt.TabIndex = 1;
            this.lblPrompt.Text = "正在复制文件...\r\n";
            // 
            // frmProgress
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(386, 95);
            this.Controls.Add(this.lblPrompt);
            this.Controls.Add(this.progbarGoing);
            this.Name = "frmProgress";
            this.Text = "frmProgress";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.ProgressBar progbarGoing;
        private System.Windows.Forms.Label lblPrompt;
    }
}