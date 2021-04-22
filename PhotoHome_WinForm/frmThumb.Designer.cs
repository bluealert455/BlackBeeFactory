namespace PhotoHome
{
    partial class frmThumb
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
            this.thumbnailList = new SuperControls.ThumbList();
            this.SuspendLayout();
            // 
            // thumbnailList
            // 
            this.thumbnailList.AutoSlide = false;
            this.thumbnailList.AutoSlideInterval = 2;
            this.thumbnailList.BackColor = System.Drawing.Color.Black;
            this.thumbnailList.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.thumbnailList.BorderWidth = 1;
            this.thumbnailList.DataPanel = null;
            this.thumbnailList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.thumbnailList.Filter = null;
            this.thumbnailList.FindNewerFile = false;
            this.thumbnailList.Font = new System.Drawing.Font("Microsoft YaHei", 9F);
            this.thumbnailList.ForeColor = System.Drawing.Color.Gray;
            this.thumbnailList.IsDirty = false;
            this.thumbnailList.IsEditable = false;
            this.thumbnailList.IsFolderBrowser = false;
            this.thumbnailList.ItemLayout = SuperControls.EnumListItemLayout.Horizontal;
            this.thumbnailList.Location = new System.Drawing.Point(3, 3);
            this.thumbnailList.Name = "thumbnailList";
            this.thumbnailList.Padding = new System.Windows.Forms.Padding(0, 30, 0, 30);
            this.thumbnailList.SelectedNode = null;
            this.thumbnailList.SelectTargetFolder = null;
            this.thumbnailList.Size = new System.Drawing.Size(794, 134);
            this.thumbnailList.TabIndex = 0;
            this.thumbnailList.ThumbSizeMode = SuperControls.EnumThumbSizeMode.Middle;
            this.thumbnailList.TitleMaxLength = 20;
            this.thumbnailList.OnThumbnailClick += new CommonLib.SetIntValueCallback(this.thumbnailList_OnThumbnailClick);
            this.thumbnailList.Click += new System.EventHandler(this.thumbnailList_Click);
            // 
            // frmThumb
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this.ClientSize = new System.Drawing.Size(800, 140);
            this.Controls.Add(this.thumbnailList);
            this.KeyPreview = false;
            this.Name = "frmThumb";
            this.Padding = new System.Windows.Forms.Padding(3);
            this.Text = "";
            this.ResumeLayout(false);

        }

        #endregion

        public SuperControls.ThumbList thumbnailList;
    }
}