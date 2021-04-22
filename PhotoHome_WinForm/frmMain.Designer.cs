namespace PhotoHome
{
    partial class frmMain
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmMain));
            this.mainDataPanel = new SuperControls.SuperDataPanel();
            this.SuspendLayout();
            // 
            // mainDataPanel
            // 
            this.mainDataPanel.BackColor = System.Drawing.Color.Black;
            this.mainDataPanel.Cursor = System.Windows.Forms.Cursors.Hand;
            this.mainDataPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mainDataPanel.Image = null;
            this.mainDataPanel.Location = new System.Drawing.Point(300, 0);
            this.mainDataPanel.MediaAutoPlay = false;
            this.mainDataPanel.Name = "mainDataPanel";
            this.mainDataPanel.Size = new System.Drawing.Size(688, 365);
            this.mainDataPanel.TabIndex = 0;
            // 
            // frmMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DimGray;
            this.ClientSize = new System.Drawing.Size(988, 565);
            this.Controls.Add(this.mainDataPanel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.KeyPreview = true;
            this.Name = "frmMain";
            this.Padding = new System.Windows.Forms.Padding(300, 0, 0, 200);
            this.Tag = "MainForm";
            this.Text = "黑蜜蜂";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.frmMain_Load);
            this.KeyUp += new System.Windows.Forms.KeyEventHandler(this.frmMain_KeyUp);
            this.ResumeLayout(false);

        }

        #endregion

        public SuperControls.SuperDataPanel mainDataPanel;
    }
}

