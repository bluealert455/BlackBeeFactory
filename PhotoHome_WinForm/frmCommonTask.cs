using CommonLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SuperControls;

namespace PhotoHome
{
    public partial class frmCommonTask : frmFloatBase
    {
        private SetStringValueCallback mOnSelectTargetFolderHandler = null;
        public frmCommonTask()
        {
            InitializeComponent();
        }

        public event SetStringValueCallback OnSelectTargetFolderChanged
        {
            add
            {
                lock (this)
                {
                    mOnSelectTargetFolderHandler += value;
                }
            }
            remove
            {
                lock (this)
                {
                    if (mOnSelectTargetFolderHandler != null)
                        mOnSelectTargetFolderHandler -= value;
                }
            }
        }
        public string SelectTargetFolder
        {
            get
            {
                return this.lblTargetFolder.Text;
            }
            set
            {
                this.lblTargetFolder.Text = value;
            }
        }

        public bool IsSaveToTargetFolder
        {
            get
            {
                return this.chkSaveAsCopy.Checked;
            }
        }
        private void btnTargetFolder_Click(object sender, EventArgs e)
        {
            FolderBrowserDialog folderDlg = new FolderBrowserDialog();
            folderDlg.Description = "请选择目标文件夹";
            folderDlg.ShowNewFolderButton = true;
            if (folderDlg.ShowDialog(this) == DialogResult.OK)
            {
                this.lblTargetFolder.Text = folderDlg.SelectedPath;
                AppConfig.Instance.SelectedTargetFolder = folderDlg.SelectedPath;
                AppConfig.Instance.Save();
            }
        }

     
    }
}
