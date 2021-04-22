using Vlc.DotNet.Forms;

namespace ThirdPlugins
{
    partial class VLCPlayer
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(VLCPlayer));
            this.panelControlBar = new System.Windows.Forms.Panel();
            this.lblCurrentTime = new System.Windows.Forms.Label();
            this.lblTotalTime = new System.Windows.Forms.Label();
            this.imgForControlBar = new System.Windows.Forms.ImageList(this.components);
            this.btnPlay = new SuperControls.GlassButton();
            this.trackVideoProgess = new SuperControls.TrackBarMobile();
            this.panelControlBar.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackVideoProgess)).BeginInit();
            this.SuspendLayout();
            // 
            // panelControlBar
            // 
            this.panelControlBar.Controls.Add(this.lblCurrentTime);
            this.panelControlBar.Controls.Add(this.lblTotalTime);
            this.panelControlBar.Controls.Add(this.trackVideoProgess);
            this.panelControlBar.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panelControlBar.Location = new System.Drawing.Point(0, 498);
            this.panelControlBar.Name = "panelControlBar";
            this.panelControlBar.Size = new System.Drawing.Size(766, 41);
            this.panelControlBar.TabIndex = 1;
            // 
            // lblCurrentTime
            // 
            this.lblCurrentTime.AutoSize = true;
            this.lblCurrentTime.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblCurrentTime.ForeColor = System.Drawing.Color.White;
            this.lblCurrentTime.Location = new System.Drawing.Point(1, 13);
            this.lblCurrentTime.Name = "lblCurrentTime";
            this.lblCurrentTime.Size = new System.Drawing.Size(56, 17);
            this.lblCurrentTime.TabIndex = 2;
            this.lblCurrentTime.Text = "00:00:00";
            // 
            // lblTotalTime
            // 
            this.lblTotalTime.AutoSize = true;
            this.lblTotalTime.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTotalTime.ForeColor = System.Drawing.Color.White;
            this.lblTotalTime.Location = new System.Drawing.Point(705, 13);
            this.lblTotalTime.Name = "lblTotalTime";
            this.lblTotalTime.Size = new System.Drawing.Size(56, 17);
            this.lblTotalTime.TabIndex = 2;
            this.lblTotalTime.Text = "00:00:00";
            // 
            // imgForControlBar
            // 
            this.imgForControlBar.ImageStream = ((System.Windows.Forms.ImageListStreamer)(resources.GetObject("imgForControlBar.ImageStream")));
            this.imgForControlBar.TransparentColor = System.Drawing.Color.Transparent;
            this.imgForControlBar.Images.SetKeyName(0, "pause32.png");
            this.imgForControlBar.Images.SetKeyName(1, "play32.png");
            this.imgForControlBar.Images.SetKeyName(2, "stop32.png");
            this.imgForControlBar.Images.SetKeyName(3, "fullscreenvideo32.png");
            // 
            // btnPlay
            // 
            this.btnPlay.CornerRadius = 49;
            this.btnPlay.Image = ((System.Drawing.Image)(resources.GetObject("btnPlay.Image")));
            this.btnPlay.Location = new System.Drawing.Point(330, 185);
            this.btnPlay.Name = "btnPlay";
            this.btnPlay.OuterBorderColor = System.Drawing.Color.DimGray;
            this.btnPlay.Size = new System.Drawing.Size(98, 98);
            this.btnPlay.TabIndex = 2;
            this.btnPlay.Click += new System.EventHandler(this.btnPlay_Click);
            // 
            // trackVideoProgess
            // 
            this.trackVideoProgess.Enabled = false;
            this.trackVideoProgess.Location = new System.Drawing.Point(52, 11);
            this.trackVideoProgess.Name = "trackVideoProgess";
            this.trackVideoProgess.Size = new System.Drawing.Size(647, 45);
            this.trackVideoProgess.TabIndex = 0;
            this.trackVideoProgess.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackVideoProgess.ValueChanged += new System.EventHandler(this.trackVideoProgess_ValueChanged);
            // 
            // VLCPlayer
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.btnPlay);
            this.Controls.Add(this.panelControlBar);
            this.Name = "VLCPlayer";
            this.Size = new System.Drawing.Size(766, 539);
            this.Click += new System.EventHandler(this.VLCPlayer_Click);
            this.MouseClick += new System.Windows.Forms.MouseEventHandler(this.VLCPlayer_MouseClick);
            this.panelControlBar.ResumeLayout(false);
            this.panelControlBar.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.trackVideoProgess)).EndInit();
            this.ResumeLayout(false);

        }


        
        #endregion

        private System.Windows.Forms.Panel panelControlBar;
        private SuperControls.TrackBarMobile trackVideoProgess;
        private System.Windows.Forms.Label lblTotalTime;
        private System.Windows.Forms.ImageList imgForControlBar;
        private SuperControls.GlassButton btnPlay;
        private System.Windows.Forms.Label lblCurrentTime;
    }
}
