namespace PhotoHome
{
    partial class frmSetLayout
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
            this.btnClassifyLayout = new SuperControls.GlassButton();
            this.btnModernLayout = new SuperControls.GlassButton();
            this.btnSimpleLayout = new SuperControls.GlassButton();
            this.SuspendLayout();
            // 
            // btnClassifyLayout
            // 
            this.btnClassifyLayout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(170)))), ((int)(((byte)(170)))));
            this.btnClassifyLayout.CornerRadius = 3;
            this.btnClassifyLayout.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClassifyLayout.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(111)))), ((int)(((byte)(179)))));
            this.btnClassifyLayout.ImageKey = "classify32.png";
            this.btnClassifyLayout.InnerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.btnClassifyLayout.Location = new System.Drawing.Point(15, 15);
            this.btnClassifyLayout.Name = "btnClassifyLayout";
            this.btnClassifyLayout.OuterBorderColor = System.Drawing.Color.DimGray;
            this.btnClassifyLayout.Size = new System.Drawing.Size(42, 42);
            this.btnClassifyLayout.TabIndex = 20;
            this.btnClassifyLayout.ToolTipText = "保存";
            this.btnClassifyLayout.Click += new System.EventHandler(this.btnClassifyLayout_Click);
            // 
            // btnModernLayout
            // 
            this.btnModernLayout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(170)))), ((int)(((byte)(170)))));
            this.btnModernLayout.CornerRadius = 3;
            this.btnModernLayout.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnModernLayout.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(111)))), ((int)(((byte)(179)))));
            this.btnModernLayout.ImageKey = "modern32.png";
            this.btnModernLayout.InnerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.btnModernLayout.Location = new System.Drawing.Point(58, 15);
            this.btnModernLayout.Name = "btnModernLayout";
            this.btnModernLayout.OuterBorderColor = System.Drawing.Color.DimGray;
            this.btnModernLayout.Size = new System.Drawing.Size(42, 42);
            this.btnModernLayout.TabIndex = 21;
            this.btnModernLayout.ToolTipText = "保存";
            this.btnModernLayout.Click += new System.EventHandler(this.btnModernLayout_Click);
            // 
            // btnSimpleLayout
            // 
            this.btnSimpleLayout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(170)))), ((int)(((byte)(170)))));
            this.btnSimpleLayout.CornerRadius = 3;
            this.btnSimpleLayout.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSimpleLayout.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(111)))), ((int)(((byte)(179)))));
            this.btnSimpleLayout.ImageKey = "simple32.png";
            this.btnSimpleLayout.InnerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.btnSimpleLayout.Location = new System.Drawing.Point(101, 15);
            this.btnSimpleLayout.Name = "btnSimpleLayout";
            this.btnSimpleLayout.OuterBorderColor = System.Drawing.Color.DimGray;
            this.btnSimpleLayout.Size = new System.Drawing.Size(42, 42);
            this.btnSimpleLayout.TabIndex = 22;
            this.btnSimpleLayout.ToolTipText = "保存";
            this.btnSimpleLayout.Click += new System.EventHandler(this.btnSimpleLayout_Click);
            // 
            // frmSetLayout
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(180)))), ((int)(((byte)(180)))), ((int)(((byte)(180)))));
            this.ClientSize = new System.Drawing.Size(158, 70);
            this.Controls.Add(this.btnSimpleLayout);
            this.Controls.Add(this.btnModernLayout);
            this.Controls.Add(this.btnClassifyLayout);
            this.Name = "frmSetLayout";
            this.Text = "frmSetLayout";
            this.ResumeLayout(false);

        }

        #endregion

        private SuperControls.GlassButton btnClassifyLayout;
        private SuperControls.GlassButton btnModernLayout;
        private SuperControls.GlassButton btnSimpleLayout;
    }
}