namespace PhotoHome
{
    partial class frmFileBrowser
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmFileBrowser));
            this.panelTop = new System.Windows.Forms.Panel();
            this.navCurFolder = new SuperControls.NavInputBox();
            this.label1 = new System.Windows.Forms.Label();
            this.txtSearch = new System.Windows.Forms.TextBox();
            this.panelBottom = new System.Windows.Forms.Panel();
            this.btnOK = new SuperControls.GlassButton();
            this.btnClose = new SuperControls.GlassButton();
            this.chkAudio = new System.Windows.Forms.CheckBox();
            this.chkVideo = new System.Windows.Forms.CheckBox();
            this.chkFindInSubFolders = new System.Windows.Forms.CheckBox();
            this.chkAllTypes = new System.Windows.Forms.CheckBox();
            this.chkPhoto = new System.Windows.Forms.CheckBox();
            this.splitContainer1 = new System.Windows.Forms.SplitContainer();
            this.treeFolder = new SuperControls.TreePanel();
            this.imgFolderTree = new System.Windows.Forms.ImageList(this.components);
            this.filesList = new SuperControls.ThumbList();
            this.panelTop.SuspendLayout();
            this.panelBottom.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).BeginInit();
            this.splitContainer1.Panel1.SuspendLayout();
            this.splitContainer1.Panel2.SuspendLayout();
            this.splitContainer1.SuspendLayout();
            this.SuspendLayout();
            // 
            // panelTop
            // 
            this.panelTop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.panelTop.Controls.Add(this.navCurFolder);
            this.panelTop.Controls.Add(this.label1);
            this.panelTop.Controls.Add(this.txtSearch);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(2, 2);
            this.panelTop.Margin = new System.Windows.Forms.Padding(3, 3, 3, 10);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(765, 45);
            this.panelTop.TabIndex = 0;
            this.panelTop.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnMouseUpCommon);
            // 
            // navCurFolder
            // 
            this.navCurFolder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this.navCurFolder.Font = new System.Drawing.Font("Microsoft YaHei", 9F);
            this.navCurFolder.ForeColor = System.Drawing.Color.White;
            this.navCurFolder.IsDirty = true;
            this.navCurFolder.IsEditing = false;
            this.navCurFolder.Location = new System.Drawing.Point(59, 12);
            this.navCurFolder.Name = "navCurFolder";
            this.navCurFolder.Path = null;
            this.navCurFolder.SelectedNode = null;
            this.navCurFolder.Size = new System.Drawing.Size(485, 23);
            this.navCurFolder.TabIndex = 2;
            this.navCurFolder.Text = "navInputBox1";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(6, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(47, 17);
            this.label1.TabIndex = 1;
            this.label1.Text = "请选择:";
            this.label1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnMouseUpCommon);
            // 
            // txtSearch
            // 
            this.txtSearch.AutoCompleteMode = System.Windows.Forms.AutoCompleteMode.Suggest;
            this.txtSearch.AutoCompleteSource = System.Windows.Forms.AutoCompleteSource.HistoryList;
            this.txtSearch.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(34)))), ((int)(((byte)(34)))), ((int)(((byte)(34)))));
            this.txtSearch.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.txtSearch.ForeColor = System.Drawing.Color.White;
            this.txtSearch.Location = new System.Drawing.Point(550, 13);
            this.txtSearch.Name = "txtSearch";
            this.txtSearch.Size = new System.Drawing.Size(212, 21);
            this.txtSearch.TabIndex = 0;
            this.txtSearch.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtSearch_KeyUp);
            this.txtSearch.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnMouseUpCommon);
            // 
            // panelBottom
            // 
            this.panelBottom.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.panelBottom.Controls.Add(this.btnOK);
            this.panelBottom.Controls.Add(this.btnClose);
            this.panelBottom.Controls.Add(this.chkAudio);
            this.panelBottom.Controls.Add(this.chkVideo);
            this.panelBottom.Controls.Add(this.chkFindInSubFolders);
            this.panelBottom.Controls.Add(this.chkAllTypes);
            this.panelBottom.Controls.Add(this.chkPhoto);
            this.panelBottom.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelBottom.Location = new System.Drawing.Point(2, 436);
            this.panelBottom.Name = "panelBottom";
            this.panelBottom.Size = new System.Drawing.Size(765, 45);
            this.panelBottom.TabIndex = 1;
            this.panelBottom.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnMouseUpCommon);
            // 
            // btnOK
            // 
            this.btnOK.BackColor = System.Drawing.Color.DimGray;
            this.btnOK.CornerRadius = 3;
            this.btnOK.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOK.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOK.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(111)))), ((int)(((byte)(179)))));
            this.btnOK.InnerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.btnOK.Location = new System.Drawing.Point(598, 8);
            this.btnOK.Name = "btnOK";
            this.btnOK.OuterBorderColor = System.Drawing.Color.Black;
            this.btnOK.Size = new System.Drawing.Size(75, 30);
            this.btnOK.TabIndex = 1;
            this.btnOK.Text = "确定";
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnClose
            // 
            this.btnClose.BackColor = System.Drawing.Color.DimGray;
            this.btnClose.CornerRadius = 3;
            this.btnClose.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnClose.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnClose.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnClose.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(111)))), ((int)(((byte)(179)))));
            this.btnClose.InnerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.btnClose.Location = new System.Drawing.Point(679, 8);
            this.btnClose.Name = "btnClose";
            this.btnClose.OuterBorderColor = System.Drawing.Color.Black;
            this.btnClose.Size = new System.Drawing.Size(75, 30);
            this.btnClose.TabIndex = 1;
            this.btnClose.Text = "关闭";
            this.btnClose.Click += new System.EventHandler(this.btnClose_Click);
            // 
            // chkAudio
            // 
            this.chkAudio.AutoSize = true;
            this.chkAudio.Checked = true;
            this.chkAudio.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAudio.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chkAudio.ForeColor = System.Drawing.Color.White;
            this.chkAudio.Location = new System.Drawing.Point(221, 13);
            this.chkAudio.Name = "chkAudio";
            this.chkAudio.Size = new System.Drawing.Size(80, 21);
            this.chkAudio.TabIndex = 0;
            this.chkAudio.Tag = "4";
            this.chkAudio.Text = "音频/音乐";
            this.chkAudio.UseVisualStyleBackColor = true;
            this.chkAudio.CheckedChanged += new System.EventHandler(this.chkAudio_CheckedChanged);
            this.chkAudio.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnMouseUpCommon);
            // 
            // chkVideo
            // 
            this.chkVideo.AutoSize = true;
            this.chkVideo.Checked = true;
            this.chkVideo.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkVideo.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chkVideo.ForeColor = System.Drawing.Color.White;
            this.chkVideo.Location = new System.Drawing.Point(168, 13);
            this.chkVideo.Name = "chkVideo";
            this.chkVideo.Size = new System.Drawing.Size(51, 21);
            this.chkVideo.TabIndex = 0;
            this.chkVideo.Tag = "2";
            this.chkVideo.Text = "视频";
            this.chkVideo.UseVisualStyleBackColor = true;
            this.chkVideo.CheckedChanged += new System.EventHandler(this.chkVideo_CheckedChanged);
            this.chkVideo.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnMouseUpCommon);
            // 
            // chkFindInSubFolders
            // 
            this.chkFindInSubFolders.AutoSize = true;
            this.chkFindInSubFolders.Checked = true;
            this.chkFindInSubFolders.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkFindInSubFolders.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chkFindInSubFolders.ForeColor = System.Drawing.Color.White;
            this.chkFindInSubFolders.Location = new System.Drawing.Point(331, 13);
            this.chkFindInSubFolders.Name = "chkFindInSubFolders";
            this.chkFindInSubFolders.Size = new System.Drawing.Size(147, 21);
            this.chkFindInSubFolders.TabIndex = 0;
            this.chkFindInSubFolders.Text = "获取子文件夹中的文件";
            this.chkFindInSubFolders.UseVisualStyleBackColor = true;
            this.chkFindInSubFolders.CheckedChanged += new System.EventHandler(this.chkFindInSubFolders_CheckedChanged);
            this.chkFindInSubFolders.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnMouseUpCommon);
            // 
            // chkAllTypes
            // 
            this.chkAllTypes.AutoSize = true;
            this.chkAllTypes.Checked = true;
            this.chkAllTypes.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkAllTypes.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chkAllTypes.ForeColor = System.Drawing.Color.White;
            this.chkAllTypes.Location = new System.Drawing.Point(6, 13);
            this.chkAllTypes.Name = "chkAllTypes";
            this.chkAllTypes.Size = new System.Drawing.Size(111, 21);
            this.chkAllTypes.TabIndex = 0;
            this.chkAllTypes.Text = "所有支持的格式";
            this.chkAllTypes.UseVisualStyleBackColor = true;
            this.chkAllTypes.CheckedChanged += new System.EventHandler(this.chkAllTypes_CheckedChanged);
            this.chkAllTypes.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnMouseUpCommon);
            // 
            // chkPhoto
            // 
            this.chkPhoto.AutoSize = true;
            this.chkPhoto.Checked = true;
            this.chkPhoto.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPhoto.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chkPhoto.ForeColor = System.Drawing.Color.White;
            this.chkPhoto.Location = new System.Drawing.Point(118, 13);
            this.chkPhoto.Name = "chkPhoto";
            this.chkPhoto.Size = new System.Drawing.Size(51, 21);
            this.chkPhoto.TabIndex = 0;
            this.chkPhoto.Tag = "1";
            this.chkPhoto.Text = "照片";
            this.chkPhoto.UseVisualStyleBackColor = true;
            this.chkPhoto.CheckedChanged += new System.EventHandler(this.chkPhoto_CheckedChanged);
            this.chkPhoto.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnMouseUpCommon);
            // 
            // splitContainer1
            // 
            this.splitContainer1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.splitContainer1.Location = new System.Drawing.Point(2, 47);
            this.splitContainer1.Name = "splitContainer1";
            // 
            // splitContainer1.Panel1
            // 
            this.splitContainer1.Panel1.Controls.Add(this.treeFolder);
            this.splitContainer1.Panel1.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
            // 
            // splitContainer1.Panel2
            // 
            this.splitContainer1.Panel2.Controls.Add(this.filesList);
            this.splitContainer1.Panel2.Padding = new System.Windows.Forms.Padding(0, 3, 0, 3);
            this.splitContainer1.Size = new System.Drawing.Size(765, 389);
            this.splitContainer1.SplitterDistance = 254;
            this.splitContainer1.TabIndex = 2;
            this.splitContainer1.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnMouseUpCommon);
            // 
            // treeFolder
            // 
            this.treeFolder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.treeFolder.Dock = System.Windows.Forms.DockStyle.Fill;
            this.treeFolder.Font = new System.Drawing.Font("Microsoft YaHei", 9F);
            this.treeFolder.ForeColor = System.Drawing.Color.White;
            this.treeFolder.ImageList = this.imgFolderTree;
            this.treeFolder.IsDirty = false;
            this.treeFolder.Location = new System.Drawing.Point(0, 3);
            this.treeFolder.Name = "treeFolder";
            this.treeFolder.SelectedNode = null;
            this.treeFolder.Size = new System.Drawing.Size(254, 383);
            this.treeFolder.TabIndex = 0;
            this.treeFolder.Text = "treePanel1";
            this.treeFolder.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnMouseUpCommon);
            // 
            // imgFolderTree
            // 
            this.imgFolderTree.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgFolderTree.ImageStream")));
            this.imgFolderTree.TransparentColor = System.Drawing.Color.Transparent;
            this.imgFolderTree.Images.SetKeyName(0, "quickaccess16.png");
            // 
            // filesList
            // 
            this.filesList.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(50)))), ((int)(((byte)(50)))), ((int)(((byte)(50)))));
            this.filesList.DataPanel = null;
            this.filesList.Dock = System.Windows.Forms.DockStyle.Fill;
            this.filesList.Filter = null;
            this.filesList.FindNewerFile = false;
            this.filesList.Font = new System.Drawing.Font("Microsoft YaHei", 9F);
            this.filesList.ForeColor = System.Drawing.Color.White;
            this.filesList.IsDirty = false;
            this.filesList.IsFolderBrowser = false;
            this.filesList.ItemLayout = SuperControls.EnumListItemLayout.Vertical;
            this.filesList.Location = new System.Drawing.Point(0, 3);
            this.filesList.Name = "filesList";
            this.filesList.Padding = new System.Windows.Forms.Padding(0, 30, 0, 30);
            this.filesList.SelectedNode = null;
            this.filesList.SelectTargetFolder = null;
            this.filesList.Size = new System.Drawing.Size(507, 383);
            this.filesList.TabIndex = 0;
            this.filesList.Text = "thumbList1";
            this.filesList.ThumbSizeMode = SuperControls.EnumThumbSizeMode.Middle;
            this.filesList.TitleMaxLength = 20;
            this.filesList.OnFolderChanged += new CommonLib.SetStringValueCallback(this.filesList_OnFolderChanged);
            this.filesList.MouseUp += new System.Windows.Forms.MouseEventHandler(this.OnMouseUpCommon);
            // 
            // frmFileBrowser
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(70)))), ((int)(((byte)(70)))), ((int)(((byte)(70)))));
            this.CancelButton = this.btnClose;
            this.ClientSize = new System.Drawing.Size(768, 482);
            this.Controls.Add(this.splitContainer1);
            this.Controls.Add(this.panelBottom);
            this.Controls.Add(this.panelTop);
            this.Name = "frmFileBrowser";
            this.Padding = new System.Windows.Forms.Padding(2, 2, 1, 1);
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "frmFileBrowser";
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.panelBottom.ResumeLayout(false);
            this.panelBottom.PerformLayout();
            this.splitContainer1.Panel1.ResumeLayout(false);
            this.splitContainer1.Panel2.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.splitContainer1)).EndInit();
            this.splitContainer1.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Panel panelBottom;
        private System.Windows.Forms.SplitContainer splitContainer1;
        private System.Windows.Forms.CheckBox chkAllTypes;
        private System.Windows.Forms.CheckBox chkPhoto;
        private System.Windows.Forms.CheckBox chkVideo;
        private System.Windows.Forms.CheckBox chkAudio;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.TextBox txtSearch;
        private System.Windows.Forms.CheckBox chkFindInSubFolders;
        private SuperControls.GlassButton btnClose;
        private SuperControls.GlassButton btnOK;
        private SuperControls.ThumbList filesList;
        private System.Windows.Forms.ImageList imgFolderTree;
        private SuperControls.TreePanel treeFolder;
        private SuperControls.NavInputBox navCurFolder;
    }
}