namespace PhotoHome
{
    partial class frmOverwrite
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
            this.btnOverwrite = new SuperControls.GlassButton();
            this.btnCancel = new SuperControls.GlassButton();
            this.lblPrompt = new System.Windows.Forms.Label();
            this.lblSourceFileInfo = new System.Windows.Forms.Label();
            this.lblTargetFileInfo = new System.Windows.Forms.Label();
            this.btnRename = new SuperControls.GlassButton();
            this.SuspendLayout();
            // 
            // btnOverwrite
            // 
            this.btnOverwrite.CornerRadius = 3;
            this.btnOverwrite.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnOverwrite.Location = new System.Drawing.Point(237, 258);
            this.btnOverwrite.Name = "btnOverwrite";
            this.btnOverwrite.Size = new System.Drawing.Size(75, 23);
            this.btnOverwrite.TabIndex = 0;
            this.btnOverwrite.Text = "覆盖";
            this.btnOverwrite.Click += new System.EventHandler(this.btnOverwrite_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.CornerRadius = 3;
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnCancel.Location = new System.Drawing.Point(318, 258);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 23);
            this.btnCancel.TabIndex = 0;
            this.btnCancel.Text = "取消";
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // lblPrompt
            // 
            this.lblPrompt.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblPrompt.ForeColor = System.Drawing.Color.White;
            this.lblPrompt.Location = new System.Drawing.Point(21, 18);
            this.lblPrompt.Name = "lblPrompt";
            this.lblPrompt.Size = new System.Drawing.Size(443, 43);
            this.lblPrompt.TabIndex = 1;
            this.lblPrompt.Text = "文件已经存在，是否覆盖？";
            // 
            // lblSourceFileInfo
            // 
            this.lblSourceFileInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblSourceFileInfo.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblSourceFileInfo.ForeColor = System.Drawing.Color.White;
            this.lblSourceFileInfo.Location = new System.Drawing.Point(21, 70);
            this.lblSourceFileInfo.Name = "lblSourceFileInfo";
            this.lblSourceFileInfo.Size = new System.Drawing.Size(210, 173);
            this.lblSourceFileInfo.TabIndex = 1;
            this.lblSourceFileInfo.Text = "label1";
            // 
            // lblTargetFileInfo
            // 
            this.lblTargetFileInfo.BackColor = System.Drawing.Color.FromArgb(((int)(((byte)(64)))), ((int)(((byte)(64)))), ((int)(((byte)(64)))));
            this.lblTargetFileInfo.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.lblTargetFileInfo.ForeColor = System.Drawing.Color.White;
            this.lblTargetFileInfo.Location = new System.Drawing.Point(254, 71);
            this.lblTargetFileInfo.Name = "lblTargetFileInfo";
            this.lblTargetFileInfo.Size = new System.Drawing.Size(210, 173);
            this.lblTargetFileInfo.TabIndex = 1;
            this.lblTargetFileInfo.Text = "label1";
            // 
            // btnRename
            // 
            this.btnRename.CornerRadius = 3;
            this.btnRename.Font = new System.Drawing.Font("Microsoft YaHei", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(134)));
            this.btnRename.Location = new System.Drawing.Point(86, 258);
            this.btnRename.Name = "btnRename";
            this.btnRename.Size = new System.Drawing.Size(144, 23);
            this.btnRename.TabIndex = 0;
            this.btnRename.Text = "重命名为：";
            this.btnRename.Click += new System.EventHandler(this.btnRename_Click);
            // 
            // frmOverwrite
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 12F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(487, 294);
            this.Controls.Add(this.lblTargetFileInfo);
            this.Controls.Add(this.lblSourceFileInfo);
            this.Controls.Add(this.lblPrompt);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnRename);
            this.Controls.Add(this.btnOverwrite);
            this.Name = "frmOverwrite";
            this.Text = "frmOverwrite";
            this.ResumeLayout(false);

        }

        #endregion

        private SuperControls.GlassButton btnOverwrite;
        private SuperControls.GlassButton btnCancel;
        private System.Windows.Forms.Label lblPrompt;
        private System.Windows.Forms.Label lblSourceFileInfo;
        private System.Windows.Forms.Label lblTargetFileInfo;
        private SuperControls.GlassButton btnRename;
    }
}