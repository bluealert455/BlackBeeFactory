namespace PhotoHome
{
    partial class frmTest
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
            this.btnGetCBData = new System.Windows.Forms.Button();
            this.lstCBFiles = new System.Windows.Forms.ListBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnShowLayerForm = new System.Windows.Forms.Button();
            this.btnShowCustomWindow = new System.Windows.Forms.Button();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.superProgress1 = new SuperControls.SuperProgress();
            this.vScrollBarMobile1 = new SuperControls.VScrollBarMobile();
            this.superProgress2 = new SuperControls.SuperProgress();
            this.SuspendLayout();
            // 
            // btnGetCBData
            // 
            this.btnGetCBData.Location = new System.Drawing.Point(12, 12);
            this.btnGetCBData.Name = "btnGetCBData";
            this.btnGetCBData.Size = new System.Drawing.Size(133, 23);
            this.btnGetCBData.TabIndex = 5;
            this.btnGetCBData.Text = "Clipboard";
            this.btnGetCBData.UseVisualStyleBackColor = true;
            this.btnGetCBData.Click += new System.EventHandler(this.btnGetCBData_Click);
            // 
            // lstCBFiles
            // 
            this.lstCBFiles.BackColor = System.Drawing.Color.Black;
            this.lstCBFiles.ForeColor = System.Drawing.Color.White;
            this.lstCBFiles.FormattingEnabled = true;
            this.lstCBFiles.ItemHeight = 12;
            this.lstCBFiles.Location = new System.Drawing.Point(12, 41);
            this.lstCBFiles.Name = "lstCBFiles";
            this.lstCBFiles.Size = new System.Drawing.Size(393, 424);
            this.lstCBFiles.TabIndex = 6;
            this.lstCBFiles.SelectedIndexChanged += new System.EventHandler(this.lstCBFiles_SelectedIndexChanged);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(151, 12);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(133, 23);
            this.btnClear.TabIndex = 5;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnShowLayerForm
            // 
            this.btnShowLayerForm.Location = new System.Drawing.Point(304, 12);
            this.btnShowLayerForm.Name = "btnShowLayerForm";
            this.btnShowLayerForm.Size = new System.Drawing.Size(85, 23);
            this.btnShowLayerForm.TabIndex = 5;
            this.btnShowLayerForm.Text = "Fish Eye";
            this.btnShowLayerForm.UseVisualStyleBackColor = true;
            this.btnShowLayerForm.Click += new System.EventHandler(this.btnShowLayerForm_Click);
            // 
            // btnShowCustomWindow
            // 
            this.btnShowCustomWindow.Location = new System.Drawing.Point(402, 12);
            this.btnShowCustomWindow.Name = "btnShowCustomWindow";
            this.btnShowCustomWindow.Size = new System.Drawing.Size(91, 23);
            this.btnShowCustomWindow.TabIndex = 5;
            this.btnShowCustomWindow.Text = "extend form";
            this.btnShowCustomWindow.UseVisualStyleBackColor = true;
            this.btnShowCustomWindow.Click += new System.EventHandler(this.btnShowCustomWindow_Click);
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // superProgress1
            // 
            this.superProgress1.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.superProgress1.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.superProgress1.Location = new System.Drawing.Point(480, 249);
            this.superProgress1.Max = 100;
            this.superProgress1.Min = 0;
            this.superProgress1.Name = "superProgress1";
            this.superProgress1.ProgressStyle = SuperControls.EnumProgressStyle.Circle;
            this.superProgress1.ProgressType = SuperControls.EnumProgressType.Continuous;
            this.superProgress1.Size = new System.Drawing.Size(35, 35);
            this.superProgress1.TabIndex = 8;
            this.superProgress1.Text = "superProgress1";
            this.superProgress1.Value = 10;
            // 
            // vScrollBarMobile1
            // 
            this.vScrollBarMobile1.LargeChange = 10;
            this.vScrollBarMobile1.Location = new System.Drawing.Point(690, 2);
            this.vScrollBarMobile1.Maximum = 10000;
            this.vScrollBarMobile1.Minimum = 0;
            this.vScrollBarMobile1.Name = "vScrollBarMobile1";
            this.vScrollBarMobile1.Size = new System.Drawing.Size(12, 519);
            this.vScrollBarMobile1.SmallChange = 10;
            this.vScrollBarMobile1.TabIndex = 7;
            this.vScrollBarMobile1.Text = "vScrollBarMobile1";
            this.vScrollBarMobile1.Value = 0;
            this.vScrollBarMobile1.OnScroll += new System.Windows.Forms.ScrollEventHandler(this.vScrollBarMobile1_OnScroll);
            // 
            // superProgress2
            // 
            this.superProgress2.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.superProgress2.BorderColor = System.Drawing.Color.FromArgb(((int)(((byte)(60)))), ((int)(((byte)(60)))), ((int)(((byte)(60)))));
            this.superProgress2.Location = new System.Drawing.Point(546, 249);
            this.superProgress2.Max = 100;
            this.superProgress2.Min = 0;
            this.superProgress2.Name = "superProgress2";
            this.superProgress2.ProgressStyle = SuperControls.EnumProgressStyle.Circle;
            this.superProgress2.ProgressType = SuperControls.EnumProgressType.Marquee;
            this.superProgress2.Size = new System.Drawing.Size(35, 35);
            this.superProgress2.TabIndex = 8;
            this.superProgress2.Text = "superProgress1";
            this.superProgress2.Value = 10;
            // 
            // frmTest
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(54)))), ((int)(((byte)(54)))), ((int)(((byte)(54)))));
            this.ClientSize = new System.Drawing.Size(686, 484);
            this.Controls.Add(this.superProgress2);
            this.Controls.Add(this.superProgress1);
            this.Controls.Add(this.vScrollBarMobile1);
            this.Controls.Add(this.lstCBFiles);
            this.Controls.Add(this.btnShowCustomWindow);
            this.Controls.Add(this.btnShowLayerForm);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnGetCBData);
            this.Name = "frmTest";
            this.Text = "frmTest";
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnGetCBData;
        private System.Windows.Forms.ListBox lstCBFiles;
        private System.Windows.Forms.Button btnClear;
        private SuperControls.VScrollBarMobile vScrollBarMobile1;
        private System.Windows.Forms.Button btnShowLayerForm;
        private System.Windows.Forms.Button btnShowCustomWindow;
        private SuperControls.SuperProgress superProgress1;
        private System.Windows.Forms.Timer timer1;
        private SuperControls.SuperProgress superProgress2;
    }
}