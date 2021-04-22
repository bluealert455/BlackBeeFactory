namespace PropertyPages
{
    partial class ImagePropPage
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
        private new void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(ImagePropPage));
            this.chkPreview = new System.Windows.Forms.CheckBox();
            this.label6 = new System.Windows.Forms.Label();
            this.trackBrightness = new SuperControls.TrackBarMobile();
            this.trackContrast = new SuperControls.TrackBarMobile();
            this.chkResizeBatch = new System.Windows.Forms.CheckBox();
            this.chkWHRatio = new System.Windows.Forms.CheckBox();
            this.txtResizeHeight = new System.Windows.Forms.TextBox();
            this.txtResizeWidth = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.imglstBtns = new System.Windows.Forms.ImageList(this.components);
            this.lblOldWidth = new System.Windows.Forms.Label();
            this.lblOldHeight = new System.Windows.Forms.Label();
            this.lblContrastVal = new System.Windows.Forms.Label();
            this.lblBrightnessVal = new System.Windows.Forms.Label();
            this.btnApplySettings = new SuperControls.GlassButton();
            ((System.ComponentModel.ISupportInitialize)(this.trackBrightness)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackContrast)).BeginInit();
            this.SuspendLayout();
            // 
            // chkPreview
            // 
            this.chkPreview.AutoSize = true;
            this.chkPreview.Checked = true;
            this.chkPreview.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkPreview.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chkPreview.ForeColor = System.Drawing.Color.White;
            this.chkPreview.Location = new System.Drawing.Point(14, 196);
            this.chkPreview.Name = "chkPreview";
            this.chkPreview.Size = new System.Drawing.Size(51, 21);
            this.chkPreview.TabIndex = 23;
            this.chkPreview.Text = "预览";
            this.chkPreview.UseVisualStyleBackColor = true;
            // 
            // label6
            // 
            this.label6.AutoSize = true;
            this.label6.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label6.ForeColor = System.Drawing.Color.White;
            this.label6.Location = new System.Drawing.Point(5, 135);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(32, 17);
            this.label6.TabIndex = 8;
            this.label6.Text = "亮度";
            // 
            // trackBrightness
            // 
            this.trackBrightness.Location = new System.Drawing.Point(5, 150);
            this.trackBrightness.Maximum = 150;
            this.trackBrightness.Minimum = -150;
            this.trackBrightness.Name = "trackBrightness";
            this.trackBrightness.Size = new System.Drawing.Size(256, 45);
            this.trackBrightness.TabIndex = 22;
            this.trackBrightness.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackBrightness.Scroll += new System.EventHandler(this.trackBrightness_Scroll);
            this.trackBrightness.ValueChanged += new System.EventHandler(this.trackBrightness_ValueChanged);
            // 
            // trackContrast
            // 
            this.trackContrast.Location = new System.Drawing.Point(5, 94);
            this.trackContrast.Maximum = 150;
            this.trackContrast.Minimum = -150;
            this.trackContrast.Name = "trackContrast";
            this.trackContrast.Size = new System.Drawing.Size(256, 45);
            this.trackContrast.TabIndex = 21;
            this.trackContrast.TickStyle = System.Windows.Forms.TickStyle.None;
            this.trackContrast.Scroll += new System.EventHandler(this.trackContrast_Scroll);
            this.trackContrast.ValueChanged += new System.EventHandler(this.trackContrast_ValueChanged);
            // 
            // chkResizeBatch
            // 
            this.chkResizeBatch.AutoSize = true;
            this.chkResizeBatch.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chkResizeBatch.ForeColor = System.Drawing.Color.White;
            this.chkResizeBatch.Location = new System.Drawing.Point(100, 56);
            this.chkResizeBatch.Name = "chkResizeBatch";
            this.chkResizeBatch.Size = new System.Drawing.Size(135, 21);
            this.chkResizeBatch.TabIndex = 20;
            this.chkResizeBatch.Text = "批量修改选中的照片";
            this.chkResizeBatch.UseVisualStyleBackColor = true;
            this.chkResizeBatch.Visible = false;
            // 
            // chkWHRatio
            // 
            this.chkWHRatio.AutoSize = true;
            this.chkWHRatio.Checked = true;
            this.chkWHRatio.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkWHRatio.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chkWHRatio.ForeColor = System.Drawing.Color.White;
            this.chkWHRatio.Location = new System.Drawing.Point(14, 56);
            this.chkWHRatio.Name = "chkWHRatio";
            this.chkWHRatio.Size = new System.Drawing.Size(87, 21);
            this.chkWHRatio.TabIndex = 19;
            this.chkWHRatio.Text = "锁定纵横比";
            this.chkWHRatio.UseVisualStyleBackColor = true;
            // 
            // txtResizeHeight
            // 
            this.txtResizeHeight.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtResizeHeight.Location = new System.Drawing.Point(192, 29);
            this.txtResizeHeight.Name = "txtResizeHeight";
            this.txtResizeHeight.Size = new System.Drawing.Size(55, 23);
            this.txtResizeHeight.TabIndex = 17;
            this.txtResizeHeight.TextChanged += new System.EventHandler(this.txtResizeHeight_TextChanged);
            this.txtResizeHeight.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtResizeHeight_KeyUp);
            // 
            // txtResizeWidth
            // 
            this.txtResizeWidth.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.txtResizeWidth.Location = new System.Drawing.Point(52, 29);
            this.txtResizeWidth.Name = "txtResizeWidth";
            this.txtResizeWidth.Size = new System.Drawing.Size(55, 23);
            this.txtResizeWidth.TabIndex = 18;
            this.txtResizeWidth.TextChanged += new System.EventHandler(this.txtResizeWidth_TextChanged);
            this.txtResizeWidth.KeyUp += new System.Windows.Forms.KeyEventHandler(this.txtResizeWidth_KeyUp);
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label4.ForeColor = System.Drawing.Color.White;
            this.label4.Location = new System.Drawing.Point(155, 33);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(35, 17);
            this.label4.TabIndex = 11;
            this.label4.Text = "高度:";
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(12, 33);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(35, 17);
            this.label3.TabIndex = 10;
            this.label3.Text = "宽度:";
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(5, 79);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(44, 17);
            this.label5.TabIndex = 9;
            this.label5.Text = "对比度";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(3, 9);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(80, 17);
            this.label2.TabIndex = 7;
            this.label2.Text = "调整照片尺寸";
            // 
            // imglstBtns
            // 
            this.imglstBtns.ColorDepth = System.Windows.Forms.ColorDepth.Depth24Bit;
            this.imglstBtns.ImageSize = new System.Drawing.Size(32, 32);
            this.imglstBtns.TransparentColor = System.Drawing.Color.Transparent;
            // 
            // lblOldWidth
            // 
            this.lblOldWidth.AutoSize = true;
            this.lblOldWidth.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblOldWidth.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblOldWidth.ForeColor = System.Drawing.Color.Silver;
            this.lblOldWidth.Location = new System.Drawing.Point(110, 32);
            this.lblOldWidth.Name = "lblOldWidth";
            this.lblOldWidth.Size = new System.Drawing.Size(32, 17);
            this.lblOldWidth.TabIndex = 25;
            this.lblOldWidth.Tag = "1";
            this.lblOldWidth.Text = "宽度";
            this.lblOldWidth.Click += new System.EventHandler(this.lblOldWidth_Click);
            // 
            // lblOldHeight
            // 
            this.lblOldHeight.AutoSize = true;
            this.lblOldHeight.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblOldHeight.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblOldHeight.ForeColor = System.Drawing.Color.Silver;
            this.lblOldHeight.Location = new System.Drawing.Point(249, 33);
            this.lblOldHeight.Name = "lblOldHeight";
            this.lblOldHeight.Size = new System.Drawing.Size(32, 17);
            this.lblOldHeight.TabIndex = 25;
            this.lblOldHeight.Tag = "1";
            this.lblOldHeight.Text = "高度";
            this.lblOldHeight.Click += new System.EventHandler(this.lblOldHeight_Click);
            // 
            // lblContrastVal
            // 
            this.lblContrastVal.AutoSize = true;
            this.lblContrastVal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblContrastVal.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblContrastVal.ForeColor = System.Drawing.Color.Silver;
            this.lblContrastVal.Location = new System.Drawing.Point(255, 95);
            this.lblContrastVal.Name = "lblContrastVal";
            this.lblContrastVal.Size = new System.Drawing.Size(15, 17);
            this.lblContrastVal.TabIndex = 26;
            this.lblContrastVal.Tag = "1";
            this.lblContrastVal.Text = "0";
            this.lblContrastVal.Click += new System.EventHandler(this.lblContrastVal_Click);
            // 
            // lblBrightnessVal
            // 
            this.lblBrightnessVal.AutoSize = true;
            this.lblBrightnessVal.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblBrightnessVal.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblBrightnessVal.ForeColor = System.Drawing.Color.Silver;
            this.lblBrightnessVal.Location = new System.Drawing.Point(256, 151);
            this.lblBrightnessVal.Name = "lblBrightnessVal";
            this.lblBrightnessVal.Size = new System.Drawing.Size(15, 17);
            this.lblBrightnessVal.TabIndex = 26;
            this.lblBrightnessVal.Tag = "1";
            this.lblBrightnessVal.Text = "0";
            this.lblBrightnessVal.Click += new System.EventHandler(this.lblBrightnessVal_Click);
            // 
            // btnApplySettings
            // 
            this.btnApplySettings.CornerRadius = 3;
            this.btnApplySettings.Image = ((System.Drawing.Image)(resources.GetObject("btnApplySettings.Image")));
            this.btnApplySettings.Location = new System.Drawing.Point(228, 185);
            this.btnApplySettings.Name = "btnApplySettings";
            this.btnApplySettings.OuterBorderColor = System.Drawing.Color.DimGray;
            this.btnApplySettings.Size = new System.Drawing.Size(42, 42);
            this.btnApplySettings.TabIndex = 27;
            this.btnApplySettings.Click += new System.EventHandler(this.btnApplySettings_Click);
            // 
            // ImagePropPage
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.Controls.Add(this.btnApplySettings);
            this.Controls.Add(this.lblBrightnessVal);
            this.Controls.Add(this.lblContrastVal);
            this.Controls.Add(this.lblOldHeight);
            this.Controls.Add(this.lblOldWidth);
            this.Controls.Add(this.chkPreview);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.trackBrightness);
            this.Controls.Add(this.trackContrast);
            this.Controls.Add(this.chkResizeBatch);
            this.Controls.Add(this.chkWHRatio);
            this.Controls.Add(this.txtResizeHeight);
            this.Controls.Add(this.txtResizeWidth);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.label2);
            this.Name = "ImagePropPage";
            this.Size = new System.Drawing.Size(291, 286);
            ((System.ComponentModel.ISupportInitialize)(this.trackBrightness)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.trackContrast)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.CheckBox chkPreview;
        private System.Windows.Forms.Label label6;
        private SuperControls.TrackBarMobile trackBrightness;
        private SuperControls.TrackBarMobile trackContrast;
        private System.Windows.Forms.CheckBox chkResizeBatch;
        private System.Windows.Forms.CheckBox chkWHRatio;
        private System.Windows.Forms.TextBox txtResizeHeight;
        private System.Windows.Forms.TextBox txtResizeWidth;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ImageList imglstBtns;
        private System.Windows.Forms.Label lblOldWidth;
        private System.Windows.Forms.Label lblOldHeight;
        private System.Windows.Forms.Label lblContrastVal;
        private System.Windows.Forms.Label lblBrightnessVal;
        private SuperControls.GlassButton btnApplySettings;
    }
}
