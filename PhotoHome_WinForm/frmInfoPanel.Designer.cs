namespace PhotoHome
{
    partial class frmInfoPanel
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
            this.infoPanel = new SuperControls.InformationPanel();
            this.SuspendLayout();
            // 
            // infoPanel
            // 
            this.infoPanel.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(35)))), ((int)(((byte)(35)))), ((int)(((byte)(35)))));
            this.infoPanel.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.infoPanel.BorderWidth = 1;
            this.infoPanel.Dock = System.Windows.Forms.DockStyle.Fill;
            this.infoPanel.Font = new System.Drawing.Font("Microsoft YaHei", 9F);
            this.infoPanel.ForeColor = System.Drawing.Color.White;
            this.infoPanel.IsDirty = false;
            this.infoPanel.IsEditable = false;
            this.infoPanel.Location = new System.Drawing.Point(3, 3);
            this.infoPanel.Name = "infoPanel";
            this.infoPanel.Padding = new System.Windows.Forms.Padding(3);
            this.infoPanel.SelectedNode = null;
            this.infoPanel.Size = new System.Drawing.Size(284, 414);
            this.infoPanel.TabIndex = 0;
            // 
            // frmInfoPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(290, 420);
            this.Controls.Add(this.infoPanel);
            this.KeyPreview = false;
            this.Name = "frmInfoPanel";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.ResumeLayout(false);

        }

        #endregion

        private SuperControls.InformationPanel infoPanel;
    }
}