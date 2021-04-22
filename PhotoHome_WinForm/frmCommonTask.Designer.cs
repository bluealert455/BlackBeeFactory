namespace PhotoHome
{
    partial class frmCommonTask
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(frmCommonTask));
            this.lblTargetFolder = new System.Windows.Forms.Label();
            this.lblSelectPathPrompt = new System.Windows.Forms.Label();
            this.btnTargetFolder = new SuperControls.GlassButton();
            this.chkSaveAsCopy = new System.Windows.Forms.CheckBox();
            this.SuspendLayout();
            // 
            // lblTargetFolder
            // 
            this.lblTargetFolder.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTargetFolder.ForeColor = System.Drawing.Color.White;
            this.lblTargetFolder.Location = new System.Drawing.Point(12, 28);
            this.lblTargetFolder.Name = "lblTargetFolder";
            this.lblTargetFolder.Size = new System.Drawing.Size(240, 38);
            this.lblTargetFolder.TabIndex = 20;
            this.lblTargetFolder.Text = "d:\\Photo";
            // 
            // lblSelectPathPrompt
            // 
            this.lblSelectPathPrompt.AutoSize = true;
            this.lblSelectPathPrompt.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblSelectPathPrompt.ForeColor = System.Drawing.Color.White;
            this.lblSelectPathPrompt.Location = new System.Drawing.Point(12, 9);
            this.lblSelectPathPrompt.Name = "lblSelectPathPrompt";
            this.lblSelectPathPrompt.Size = new System.Drawing.Size(215, 17);
            this.lblSelectPathPrompt.TabIndex = 21;
            this.lblSelectPathPrompt.Text = "把选中的文件自动复制到如下文件夹里:";
            // 
            // btnTargetFolder
            // 
            this.btnTargetFolder.CornerRadius = 3;
            this.btnTargetFolder.Image = ((System.Drawing.Image)(resources.GetObject("btnTargetFolder.Image")));
            this.btnTargetFolder.Location = new System.Drawing.Point(250, 19);
            this.btnTargetFolder.Name = "btnTargetFolder";
            this.btnTargetFolder.OuterBorderColor = System.Drawing.Color.DimGray;
            this.btnTargetFolder.Size = new System.Drawing.Size(42, 42);
            this.btnTargetFolder.TabIndex = 22;
            this.btnTargetFolder.Click += new System.EventHandler(this.btnTargetFolder_Click);
            // 
            // chkSaveAsCopy
            // 
            this.chkSaveAsCopy.AutoSize = true;
            this.chkSaveAsCopy.Checked = true;
            this.chkSaveAsCopy.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chkSaveAsCopy.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.chkSaveAsCopy.ForeColor = System.Drawing.Color.White;
            this.chkSaveAsCopy.Location = new System.Drawing.Point(15, 64);
            this.chkSaveAsCopy.Name = "chkSaveAsCopy";
            this.chkSaveAsCopy.Size = new System.Drawing.Size(219, 21);
            this.chkSaveAsCopy.TabIndex = 25;
            this.chkSaveAsCopy.Text = "点击保存按钮时，把文件保存到这里";
            this.chkSaveAsCopy.UseVisualStyleBackColor = true;
            // 
            // frmCommonTask
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(304, 88);
            this.Controls.Add(this.chkSaveAsCopy);
            this.Controls.Add(this.btnTargetFolder);
            this.Controls.Add(this.lblTargetFolder);
            this.Controls.Add(this.lblSelectPathPrompt);
            this.Name = "frmCommonTask";
            this.Text = "frmCommonTask";
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        protected System.Windows.Forms.Label lblTargetFolder;
        protected System.Windows.Forms.Label lblSelectPathPrompt;
        private SuperControls.GlassButton btnTargetFolder;
        private System.Windows.Forms.CheckBox chkSaveAsCopy;
    }
}