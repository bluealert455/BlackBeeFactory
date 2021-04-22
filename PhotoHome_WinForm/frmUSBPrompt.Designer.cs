namespace PhotoHome
{
    partial class frmUSBPrompt
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
            this.lstFolderes = new SuperControls.ListPanel();
            this.btnClose = new SuperControls.GlassButton();
            this.SuspendLayout();
            // 
            // lstFolderes
            // 
            this.lstFolderes.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.lstFolderes.Location = new System.Drawing.Point(12, 12);
            this.lstFolderes.Name = "lstFolderes";
            this.lstFolderes.SelectedItem = null;
            this.lstFolderes.Size = new System.Drawing.Size(350, 375);
            this.lstFolderes.TabIndex = 0;
            this.lstFolderes.Text = "listPanel1";
            this.lstFolderes.OnItemClick += new SuperControls.ItemClickHandler(this.lstFolderes_OnItemClick);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.DimGray;
            this.btnClose.CornerRadius = 3;
            this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClose.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnClose.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(111)))), ((int)(((byte)(179)))));
            this.btnClose.InnerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.btnClose.Location = new System.Drawing.Point(287, 396);
            this.btnClose.Name = "btnClose";
            this.btnClose.Size = new System.Drawing.Size(75, 30);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "关闭";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // frmUSBPrompt
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(374, 438);
            this.Controls.Add(this.btnClose);
            this.Controls.Add(this.lstFolderes);
            this.Name = "frmUSBPrompt";
            this.Text = "frmUSBPrompt";
            this.ResumeLayout(false);

        }

        #endregion

        private SuperControls.ListPanel lstFolderes;
        private SuperControls.GlassButton btnClose;
    }
}