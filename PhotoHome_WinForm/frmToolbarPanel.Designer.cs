namespace PhotoHome
{
    partial class frmToolbarPanel
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmToolbarPanel));
            this.imglstLeftBar = new System.Windows.Forms.ImageList(this.components);
            this.btnSetLayout = new SuperControls.GlassButton();
            this.btnQuit = new SuperControls.GlassButton();
            this.btnSelectTargetFolder = new SuperControls.GlassButton();
            this.btnAutoSlide = new SuperControls.GlassButton();
            this.btnFullScreen = new SuperControls.GlassButton();
            this.btnTakeSnapshot = new SuperControls.GlassButton();
            this.btnStop = new SuperControls.GlassButton();
            this.btnPlay = new SuperControls.GlassButton();
            this.btnUnselect = new SuperControls.GlassButton();
            this.btnSave = new SuperControls.GlassButton();
            this.btnFlipVer = new SuperControls.GlassButton();
            this.btnFlipHor = new SuperControls.GlassButton();
            this.btnRotateImageRight = new SuperControls.GlassButton();
            this.btnRotateImageLeft = new SuperControls.GlassButton();
            this.btnRealSize = new SuperControls.GlassButton();
            this.btnFullExtent = new SuperControls.GlassButton();
            this.btnPaste = new SuperControls.GlassButton();
            this.btnOpen = new SuperControls.GlassButton();
            this.SuspendLayout();
            // 
            // imglstLeftBar
            // 
            this.imglstLeftBar.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imglstLeftBar.ImageStream")));
            this.imglstLeftBar.TransparentColor = System.Drawing.Color.Transparent;
            this.imglstLeftBar.Images.SetKeyName(0, "applysettings32.png");
            this.imglstLeftBar.Images.SetKeyName(1, "clipimage32.png");
            this.imglstLeftBar.Images.SetKeyName(2, "fliphor32.png");
            this.imglstLeftBar.Images.SetKeyName(3, "flipver32.png");
            this.imglstLeftBar.Images.SetKeyName(4, "folderselect32.png");
            this.imglstLeftBar.Images.SetKeyName(5, "rotateimageleft32.png");
            this.imglstLeftBar.Images.SetKeyName(6, "rotateimageright32.png");
            this.imglstLeftBar.Images.SetKeyName(7, "save32.png");
            this.imglstLeftBar.Images.SetKeyName(8, "fullscreenvideo32.png");
            this.imglstLeftBar.Images.SetKeyName(9, "pause32.png");
            this.imglstLeftBar.Images.SetKeyName(10, "play32.png");
            this.imglstLeftBar.Images.SetKeyName(11, "stop32.png");
            this.imglstLeftBar.Images.SetKeyName(12, "snapshot32.png");
            this.imglstLeftBar.Images.SetKeyName(13, "selected32.png");
            this.imglstLeftBar.Images.SetKeyName(14, "unselected32.png");
            this.imglstLeftBar.Images.SetKeyName(15, "slideauto32.png");
            this.imglstLeftBar.Images.SetKeyName(16, "slideautostop32.png");
            this.imglstLeftBar.Images.SetKeyName(17, "fullextent32.png");
            this.imglstLeftBar.Images.SetKeyName(18, "openfolder32.png");
            this.imglstLeftBar.Images.SetKeyName(19, "paste32.png");
            this.imglstLeftBar.Images.SetKeyName(20, "realsize32.png");
            this.imglstLeftBar.Images.SetKeyName(21, "close32.png");
            this.imglstLeftBar.Images.SetKeyName(22, "classify32.png");
            this.imglstLeftBar.Images.SetKeyName(23, "modern32.png");
            this.imglstLeftBar.Images.SetKeyName(24, "simple32.png");
            // 
            // btnSetLayout
            // 
            this.btnSetLayout.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(170)))), ((int)(((byte)(170)))));
            this.btnSetLayout.CornerRadius = 3;
            this.btnSetLayout.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSetLayout.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(111)))), ((int)(((byte)(179)))));
            this.btnSetLayout.ImageKey = "classify32.png";
            this.btnSetLayout.ImageList = this.imglstLeftBar;
            this.btnSetLayout.InnerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.btnSetLayout.Location = new System.Drawing.Point(716, 8);
            this.btnSetLayout.Name = "btnSetLayout";
            this.btnSetLayout.OuterBorderColor = System.Drawing.Color.DimGray;
            this.btnSetLayout.Size = new System.Drawing.Size(42, 42);
            this.btnSetLayout.TabIndex = 19;
            this.btnSetLayout.ToolTipText = "保存";
            this.btnSetLayout.Click += new System.EventHandler(this.btnSetLayout_Click);
            // 
            // btnQuit
            // 
            this.btnQuit.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(170)))), ((int)(((byte)(170)))));
            this.btnQuit.CornerRadius = 3;
            this.btnQuit.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnQuit.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(111)))), ((int)(((byte)(179)))));
            this.btnQuit.ImageKey = "close32.png";
            this.btnQuit.ImageList = this.imglstLeftBar;
            this.btnQuit.InnerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.btnQuit.Location = new System.Drawing.Point(764, 8);
            this.btnQuit.Name = "btnQuit";
            this.btnQuit.OuterBorderColor = System.Drawing.Color.DimGray;
            this.btnQuit.Size = new System.Drawing.Size(42, 42);
            this.btnQuit.TabIndex = 20;
            this.btnQuit.ToolTipText = "退出";
            this.btnQuit.Click += new System.EventHandler(this.btnQuit_Click);
            // 
            // btnSelectTargetFolder
            // 
            this.btnSelectTargetFolder.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(170)))), ((int)(((byte)(170)))));
            this.btnSelectTargetFolder.CornerRadius = 3;
            this.btnSelectTargetFolder.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSelectTargetFolder.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(111)))), ((int)(((byte)(179)))));
            this.btnSelectTargetFolder.ImageKey = "folderselect32.png";
            this.btnSelectTargetFolder.ImageList = this.imglstLeftBar;
            this.btnSelectTargetFolder.InnerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.btnSelectTargetFolder.Location = new System.Drawing.Point(668, 8);
            this.btnSelectTargetFolder.Name = "btnSelectTargetFolder";
            this.btnSelectTargetFolder.OuterBorderColor = System.Drawing.Color.DimGray;
            this.btnSelectTargetFolder.Size = new System.Drawing.Size(42, 42);
            this.btnSelectTargetFolder.TabIndex = 18;
            this.btnSelectTargetFolder.ToolTipText = "保存";
            this.btnSelectTargetFolder.Click += new System.EventHandler(this.btnSelectTargetFolder_Click);
            // 
            // btnAutoSlide
            // 
            this.btnAutoSlide.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(170)))), ((int)(((byte)(170)))));
            this.btnAutoSlide.CornerRadius = 3;
            this.btnAutoSlide.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnAutoSlide.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(111)))), ((int)(((byte)(179)))));
            this.btnAutoSlide.ImageKey = "slideauto32.png";
            this.btnAutoSlide.ImageList = this.imglstLeftBar;
            this.btnAutoSlide.InnerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.btnAutoSlide.Location = new System.Drawing.Point(572, 8);
            this.btnAutoSlide.Name = "btnAutoSlide";
            this.btnAutoSlide.OuterBorderColor = System.Drawing.Color.DimGray;
            this.btnAutoSlide.Size = new System.Drawing.Size(42, 42);
            this.btnAutoSlide.TabIndex = 16;
            this.btnAutoSlide.ToolTipText = "自动播放";
            this.btnAutoSlide.Click += new System.EventHandler(this.btnAutoSlide_Click);
            // 
            // btnFullScreen
            // 
            this.btnFullScreen.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(170)))), ((int)(((byte)(170)))));
            this.btnFullScreen.CornerRadius = 3;
            this.btnFullScreen.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFullScreen.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(111)))), ((int)(((byte)(179)))));
            this.btnFullScreen.ImageKey = "fullscreenvideo32.png";
            this.btnFullScreen.ImageList = this.imglstLeftBar;
            this.btnFullScreen.InnerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.btnFullScreen.Location = new System.Drawing.Point(476, 8);
            this.btnFullScreen.Name = "btnFullScreen";
            this.btnFullScreen.OuterBorderColor = System.Drawing.Color.DimGray;
            this.btnFullScreen.Size = new System.Drawing.Size(42, 42);
            this.btnFullScreen.TabIndex = 14;
            this.btnFullScreen.ToolTipText = "全屏";
            this.btnFullScreen.Click += new System.EventHandler(this.btnFullScreen_Click);
            // 
            // btnTakeSnapshot
            // 
            this.btnTakeSnapshot.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(170)))), ((int)(((byte)(170)))));
            this.btnTakeSnapshot.CornerRadius = 3;
            this.btnTakeSnapshot.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnTakeSnapshot.Enabled = false;
            this.btnTakeSnapshot.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(111)))), ((int)(((byte)(179)))));
            this.btnTakeSnapshot.ImageKey = "snapshot32.png";
            this.btnTakeSnapshot.ImageList = this.imglstLeftBar;
            this.btnTakeSnapshot.InnerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.btnTakeSnapshot.Location = new System.Drawing.Point(428, 8);
            this.btnTakeSnapshot.Name = "btnTakeSnapshot";
            this.btnTakeSnapshot.OuterBorderColor = System.Drawing.Color.DimGray;
            this.btnTakeSnapshot.Size = new System.Drawing.Size(42, 42);
            this.btnTakeSnapshot.TabIndex = 13;
            this.btnTakeSnapshot.ToolTipText = "抓图";
            this.btnTakeSnapshot.Click += new System.EventHandler(this.btnTakeSnapshot_Click);
            // 
            // btnStop
            // 
            this.btnStop.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(170)))), ((int)(((byte)(170)))));
            this.btnStop.CornerRadius = 3;
            this.btnStop.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnStop.Enabled = false;
            this.btnStop.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(111)))), ((int)(((byte)(179)))));
            this.btnStop.ImageKey = "stop32.png";
            this.btnStop.ImageList = this.imglstLeftBar;
            this.btnStop.InnerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.btnStop.Location = new System.Drawing.Point(380, 8);
            this.btnStop.Name = "btnStop";
            this.btnStop.OuterBorderColor = System.Drawing.Color.DimGray;
            this.btnStop.Size = new System.Drawing.Size(42, 42);
            this.btnStop.TabIndex = 12;
            this.btnStop.ToolTipText = "停止";
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnPlay
            // 
            this.btnPlay.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(170)))), ((int)(((byte)(170)))));
            this.btnPlay.CornerRadius = 3;
            this.btnPlay.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPlay.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(111)))), ((int)(((byte)(179)))));
            this.btnPlay.ImageKey = "play32.png";
            this.btnPlay.ImageList = this.imglstLeftBar;
            this.btnPlay.InnerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.btnPlay.Location = new System.Drawing.Point(332, 8);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.OuterBorderColor = System.Drawing.Color.DimGray;
            this.btnPlay.Size = new System.Drawing.Size(42, 42);
            this.btnPlay.TabIndex = 10;
            this.btnPlay.ToolTipText = "播放/暂停";
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // btnUnselect
            // 
            this.btnUnselect.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(170)))), ((int)(((byte)(170)))));
            this.btnUnselect.CornerRadius = 3;
            this.btnUnselect.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnUnselect.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(111)))), ((int)(((byte)(179)))));
            this.btnUnselect.ImageKey = "selected32.png";
            this.btnUnselect.ImageList = this.imglstLeftBar;
            this.btnUnselect.InnerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.btnUnselect.Location = new System.Drawing.Point(524, 8);
            this.btnUnselect.Name = "btnUnselect";
            this.btnUnselect.OuterBorderColor = System.Drawing.Color.DimGray;
            this.btnUnselect.Size = new System.Drawing.Size(42, 42);
            this.btnUnselect.TabIndex = 15;
            this.btnUnselect.ToolTipText = "选择/不选";
            this.btnUnselect.Click += new System.EventHandler(this.btnUnselect_Click);
            // 
            // btnSave
            // 
            this.btnSave.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(170)))), ((int)(((byte)(170)))));
            this.btnSave.CornerRadius = 3;
            this.btnSave.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnSave.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(111)))), ((int)(((byte)(179)))));
            this.btnSave.ImageKey = "save32.png";
            this.btnSave.ImageList = this.imglstLeftBar;
            this.btnSave.InnerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.btnSave.Location = new System.Drawing.Point(620, 8);
            this.btnSave.Name = "btnSave";
            this.btnSave.OuterBorderColor = System.Drawing.Color.DimGray;
            this.btnSave.Size = new System.Drawing.Size(42, 42);
            this.btnSave.TabIndex = 17;
            this.btnSave.ToolTipText = "保存";
            this.btnSave.Click += new System.EventHandler(this.btnSave_Click);
            // 
            // btnFlipVer
            // 
            this.btnFlipVer.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(170)))), ((int)(((byte)(170)))));
            this.btnFlipVer.CornerRadius = 3;
            this.btnFlipVer.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFlipVer.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(111)))), ((int)(((byte)(179)))));
            this.btnFlipVer.ImageKey = "flipver32.png";
            this.btnFlipVer.ImageList = this.imglstLeftBar;
            this.btnFlipVer.InnerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.btnFlipVer.Location = new System.Drawing.Point(285, 8);
            this.btnFlipVer.Name = "btnFlipVer";
            this.btnFlipVer.OuterBorderColor = System.Drawing.Color.DimGray;
            this.btnFlipVer.Size = new System.Drawing.Size(42, 42);
            this.btnFlipVer.TabIndex = 9;
            this.btnFlipVer.ToolTipText = "垂直翻转";
            this.btnFlipVer.Click += new System.EventHandler(this.btnFlipVer_Click);
            // 
            // btnFlipHor
            // 
            this.btnFlipHor.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(170)))), ((int)(((byte)(170)))));
            this.btnFlipHor.CornerRadius = 3;
            this.btnFlipHor.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFlipHor.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(111)))), ((int)(((byte)(179)))));
            this.btnFlipHor.ImageKey = "fliphor32.png";
            this.btnFlipHor.ImageList = this.imglstLeftBar;
            this.btnFlipHor.InnerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.btnFlipHor.Location = new System.Drawing.Point(250, 8);
            this.btnFlipHor.Name = "btnFlipHor";
            this.btnFlipHor.OuterBorderColor = System.Drawing.Color.DimGray;
            this.btnFlipHor.Size = new System.Drawing.Size(42, 42);
            this.btnFlipHor.TabIndex = 8;
            this.btnFlipHor.ToolTipText = "水平翻转";
            this.btnFlipHor.Click += new System.EventHandler(this.btnFlipHor_Click);
            // 
            // btnRotateImageRight
            // 
            this.btnRotateImageRight.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(170)))), ((int)(((byte)(170)))));
            this.btnRotateImageRight.CornerRadius = 3;
            this.btnRotateImageRight.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRotateImageRight.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(111)))), ((int)(((byte)(179)))));
            this.btnRotateImageRight.ImageKey = "rotateimageright32.png";
            this.btnRotateImageRight.ImageList = this.imglstLeftBar;
            this.btnRotateImageRight.InnerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.btnRotateImageRight.Location = new System.Drawing.Point(215, 8);
            this.btnRotateImageRight.Name = "btnRotateImageRight";
            this.btnRotateImageRight.OuterBorderColor = System.Drawing.Color.DimGray;
            this.btnRotateImageRight.Size = new System.Drawing.Size(42, 42);
            this.btnRotateImageRight.TabIndex = 7;
            this.btnRotateImageRight.ToolTipText = "顺时针旋转";
            this.btnRotateImageRight.Click += new System.EventHandler(this.btnRotateImageRight_Click);
            // 
            // btnRotateImageLeft
            // 
            this.btnRotateImageLeft.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(170)))), ((int)(((byte)(170)))));
            this.btnRotateImageLeft.CornerRadius = 3;
            this.btnRotateImageLeft.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRotateImageLeft.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(111)))), ((int)(((byte)(179)))));
            this.btnRotateImageLeft.ImageKey = "rotateimageleft32.png";
            this.btnRotateImageLeft.ImageList = this.imglstLeftBar;
            this.btnRotateImageLeft.InnerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.btnRotateImageLeft.Location = new System.Drawing.Point(180, 8);
            this.btnRotateImageLeft.Name = "btnRotateImageLeft";
            this.btnRotateImageLeft.OuterBorderColor = System.Drawing.Color.DimGray;
            this.btnRotateImageLeft.Size = new System.Drawing.Size(42, 42);
            this.btnRotateImageLeft.TabIndex = 6;
            this.btnRotateImageLeft.ToolTipText = "逆时针旋转";
            this.btnRotateImageLeft.Click += new System.EventHandler(this.btnRotateImageLeft_Click);
            // 
            // btnRealSize
            // 
            this.btnRealSize.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(170)))), ((int)(((byte)(170)))));
            this.btnRealSize.CornerRadius = 3;
            this.btnRealSize.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnRealSize.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(111)))), ((int)(((byte)(179)))));
            this.btnRealSize.ImageKey = "realsize32.png";
            this.btnRealSize.ImageList = this.imglstLeftBar;
            this.btnRealSize.InnerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.btnRealSize.Location = new System.Drawing.Point(138, 8);
            this.btnRealSize.Name = "btnRealSize";
            this.btnRealSize.OuterBorderColor = System.Drawing.Color.DimGray;
            this.btnRealSize.Size = new System.Drawing.Size(42, 42);
            this.btnRealSize.TabIndex = 5;
            this.btnRealSize.ToolTipText = "实际尺寸显示";
            this.btnRealSize.Click += new System.EventHandler(this.btnRealSize_Click);
            // 
            // btnFullExtent
            // 
            this.btnFullExtent.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(170)))), ((int)(((byte)(170)))));
            this.btnFullExtent.CornerRadius = 3;
            this.btnFullExtent.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnFullExtent.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(111)))), ((int)(((byte)(179)))));
            this.btnFullExtent.ImageKey = "fullextent32.png";
            this.btnFullExtent.ImageList = this.imglstLeftBar;
            this.btnFullExtent.InnerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.btnFullExtent.Location = new System.Drawing.Point(94, 8);
            this.btnFullExtent.Name = "btnFullExtent";
            this.btnFullExtent.OuterBorderColor = System.Drawing.Color.Gray;
            this.btnFullExtent.Size = new System.Drawing.Size(42, 42);
            this.btnFullExtent.TabIndex = 4;
            this.btnFullExtent.ToolTipText = "全图显示";
            this.btnFullExtent.Click += new System.EventHandler(this.btnFullExtent_Click);
            // 
            // btnPaste
            // 
            this.btnPaste.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(170)))), ((int)(((byte)(170)))));
            this.btnPaste.CornerRadius = 3;
            this.btnPaste.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnPaste.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(111)))), ((int)(((byte)(179)))));
            this.btnPaste.ImageKey = "paste32.png";
            this.btnPaste.ImageList = this.imglstLeftBar;
            this.btnPaste.InnerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.btnPaste.Location = new System.Drawing.Point(48, 8);
            this.btnPaste.Name = "btnPaste";
            this.btnPaste.OuterBorderColor = System.Drawing.Color.DimGray;
            this.btnPaste.Size = new System.Drawing.Size(42, 42);
            this.btnPaste.TabIndex = 3;
            this.btnPaste.ToolTipText = "粘贴";
            this.btnPaste.Click += new System.EventHandler(this.btnPaste_Click);
            // 
            // btnOpen
            // 
            this.btnOpen.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(170)))), ((int)(((byte)(170)))), ((int)(((byte)(170)))));
            this.btnOpen.CornerRadius = 3;
            this.btnOpen.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnOpen.GlowColor = System.Drawing.Color.FromArgb(((int)(((byte)(46)))), ((int)(((byte)(111)))), ((int)(((byte)(179)))));
            this.btnOpen.ImageKey = "openfolder32.png";
            this.btnOpen.ImageList = this.imglstLeftBar;
            this.btnOpen.InnerBorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(90)))), ((int)(((byte)(90)))), ((int)(((byte)(90)))));
            this.btnOpen.Location = new System.Drawing.Point(5, 8);
            this.btnOpen.Name = "btnOpen";
            this.btnOpen.OuterBorderColor = System.Drawing.Color.DimGray;
            this.btnOpen.Size = new System.Drawing.Size(42, 42);
            this.btnOpen.TabIndex = 1;
            this.btnOpen.ToolTipText = "打开文件或文件夹";
            this.btnOpen.Click += new System.EventHandler(this.btnOpen_Click);
            // 
            // frmToolbarPanel
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(80)))), ((int)(((byte)(80)))), ((int)(((byte)(80)))));
            this.ClientSize = new System.Drawing.Size(894, 48);
            this.Controls.Add(this.btnSetLayout);
            this.Controls.Add(this.btnQuit);
            this.Controls.Add(this.btnSelectTargetFolder);
            this.Controls.Add(this.btnAutoSlide);
            this.Controls.Add(this.btnFullScreen);
            this.Controls.Add(this.btnTakeSnapshot);
            this.Controls.Add(this.btnStop);
            this.Controls.Add(this.btnPlay);
            this.Controls.Add(this.btnUnselect);
            this.Controls.Add(this.btnSave);
            this.Controls.Add(this.btnFlipVer);
            this.Controls.Add(this.btnFlipHor);
            this.Controls.Add(this.btnRotateImageRight);
            this.Controls.Add(this.btnRotateImageLeft);
            this.Controls.Add(this.btnRealSize);
            this.Controls.Add(this.btnFullExtent);
            this.Controls.Add(this.btnPaste);
            this.Controls.Add(this.btnOpen);
            this.Name = "frmToolbarPanel";
            this.Text = "frmLeftPanel";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.ImageList imglstLeftBar;
        private SuperControls.GlassButton btnOpen;
        private SuperControls.GlassButton btnPaste;
        private SuperControls.GlassButton btnFullExtent;
        private SuperControls.GlassButton btnRealSize;
        private SuperControls.GlassButton btnRotateImageLeft;
        private SuperControls.GlassButton btnRotateImageRight;
        private SuperControls.GlassButton btnFlipHor;
        private SuperControls.GlassButton btnFlipVer;
        private SuperControls.GlassButton btnSave;
        private SuperControls.GlassButton btnPlay;
        private SuperControls.GlassButton btnStop;
        private SuperControls.GlassButton btnTakeSnapshot;
        private SuperControls.GlassButton btnFullScreen;
        private SuperControls.GlassButton btnUnselect;
        private SuperControls.GlassButton btnAutoSlide;
        private SuperControls.GlassButton btnSelectTargetFolder;
        private SuperControls.GlassButton btnQuit;
        private SuperControls.GlassButton btnSetLayout;
    }
}