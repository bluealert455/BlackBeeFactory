namespace PhotoHome_WinForm
{
    partial class frmLeftPanel
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
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmLeftPanel));
            this.btnOpen = new System.Windows.Forms.Button();
            this.imglstLeftBar = new System.Windows.Forms.ImageList(this.components);
            this.btnPaste = new System.Windows.Forms.Button();
            this.btnCopy = new System.Windows.Forms.Button();
            this.btnSave = new System.Windows.Forms.Button();
            this.btnSettings = new System.Windows.Forms.Button();
            this.SuspendLayout();
            // 
            // btnOpen
            // 
            this.btnOpen.ImageKey = "open32";
            this.btnOpen.ImageList = this.imglstLeftBar;
            this.btnOpen.Location = new System.Drawing.Point(5, 8);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.Size = new System.Drawing.Size(42, 42);
            this.btnOpen.TabIndex = 0;
            this.btnOpen.UseVisualStyleBackColor = true;
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // imglstLeftBar
            // 
            this.imglstLeftBar.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imglstLeftBar.ImageStream")));
            this.imglstLeftBar.TransparentColor = System.Drawing.Color.Transparent;
            this.imglstLeftBar.Images.SetKeyName(0, "copy32");
            this.imglstLeftBar.Images.SetKeyName(1, "open32");
            this.imglstLeftBar.Images.SetKeyName(2, "paste32");
            this.imglstLeftBar.Images.SetKeyName(3, "save32");
            this.imglstLeftBar.Images.SetKeyName(4, "setting32");
            // 
            // btnPaste
            // 
            this.btnPaste.ImageKey = "paste32";
            this.btnPaste.ImageList = this.imglstLeftBar;
            this.btnPaste.Location = new System.Drawing.Point(53, 8);
            this.btnPaste.Name = "btnPaste";
            this.btnPaste.Size = new System.Drawing.Size(42, 42);
            this.btnPaste.TabIndex = 0;
            this.btnPaste.UseVisualStyleBackColor = true;
            this.btnPaste.Click += new System.EventHandler(this.btnPaste_Click);
            // 
            // btnCopy
            // 
            this.btnCopy.ImageKey = "copy32";
            this.btnCopy.ImageList = this.imglstLeftBar;
            this.btnCopy.Location = new System.Drawing.Point(101, 8);
            this.btnCopy.Name = "btnCopy";
            this.btnCopy.Size = new System.Drawing.Size(42, 42);
            this.btnCopy.TabIndex = 0;
            this.btnCopy.UseVisualStyleBackColor = true;
            this.btnCopy.Click += new System.EventHandler(this.btnCopy_Click);
            // 
            // btnSave
            // 
            this.btnSave.ImageKey = "save32";
            this.btnSave.ImageList = this.imglstLeftBar;
            this.btnSave.Location = new System.Drawing.Point(149, 8);
            this.btnSave.Name = "btnSave";
            this.btnSave.Size = new System.Drawing.Size(42, 42);
            this.btnSave.TabIndex = 0;
            this.btnSave.UseVisualStyleBackColor = true;
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnSettings
            // 
            this.btnSettings.ImageKey = "setting32";
            this.btnSettings.ImageList = this.imglstLeftBar;
            this.btnSettings.Location = new System.Drawing.Point(197, 8);
            this.btnSettings.Name = "btnSettings";
            this.btnSettings.Size = new System.Drawing.Size(42, 42);
            this.btnSettings.TabIndex = 0;
            this.btnSettings.UseVisualStyleBackColor = true;
            this.btnSettings.Click += new System.EventHandler(this.btnSettings_Click);
            // 
            // frmLeftPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.SystemColors.ActiveCaptionText;
            this.ClientSize = new System.Drawing.Size(244, 58);
            this.Controls.Add(this.btnSettings);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnCopy);
            this.Controls.Add(this.btnPaste);
            this.Controls.Add(this.btnOpen);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Name = "frmLeftPanel";
            this.ShowInTaskbar = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.Manual;
            this.Text = "frmLeftPanel";
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnOpen;
        private System.Windows.Forms.ImageList imglstLeftBar;
        private System.Windows.Forms.Button btnPaste;
        private System.Windows.Forms.Button btnCopy;
        private System.Windows.Forms.Button btnSave;
        private System.Windows.Forms.Button btnSettings;
    }
}