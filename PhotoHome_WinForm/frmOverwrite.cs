using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhotoHome
{
    public partial class frmOverwrite : frmFloatBase
    {
        private string mShortName;
        private string mNewName;
        public frmOverwrite()
        {
            InitializeComponent();
        }

        public string NewName
        {
            get
            {
                return mNewName;
            }
        }
        protected override void OnKeyUp(KeyEventArgs e)
        {
            
        }
        private void btnOverwrite_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.OK;
            this.Close();
        }

        public void Init(string srcFile,string tgtFile)
        {
            lblPrompt.Text = "文件[" + Path.GetFileName(srcFile) + "]已经存在，是否覆盖？";
            lblSourceFileInfo.Text = GetFileSummary(srcFile);
            lblTargetFileInfo.Text = GetFileSummary(tgtFile);

            GetNewName(tgtFile);

            mNewName = Path.Combine(Path.GetDirectoryName(tgtFile), mNewName);

            btnRename.Text = "重命名为:" + mShortName;

        }

        public string ShowModal(string srcFile,string tgtFile,IWin32Window owner)
        {
            frmOverwrite dlg = new frmOverwrite();
            dlg.Init(srcFile, tgtFile);
            DialogResult dr = dlg.ShowDialog(owner);
            string s = "OK";
            switch (dr)
            {
                case DialogResult.OK:
                    s = "OK";
                    break;
                case DialogResult.Cancel:
                    s = "CANCEL";
                    break;
                case DialogResult.Retry:
                    s = dlg.NewName;
                    break;
            }
            return s;
        }
        private void GetNewName(string tgtFile)
        {
            int maxLen = 8;
            string name = Path.GetFileNameWithoutExtension(tgtFile);
            string ext = Path.GetExtension(tgtFile);
            string dirName = Path.GetDirectoryName(tgtFile);
            int n = 1;
            string newName = null;
            while (true)
            {
                newName = name + "_" + n.ToString() + ext;
                string tmp = Path.Combine(dirName, newName);
                if (File.Exists(tmp) == false)
                    break;
                n++;
            }
            if (name.Length > maxLen)
                mShortName = name.Substring(0, maxLen) + "~" + "_" + n.ToString() + ext;
            else
                mShortName = newName;

            mNewName=newName;

        }
        private string GetFileSummary(string file)
        {
            StringBuilder sb = new StringBuilder();
            string s = "大小:" + CommonLib.Common.GetFileSize2Str(file);
            sb.AppendLine(s);
            s = "创建时间:" + File.GetCreationTime(file).ToString("yyyy-MM-dd HH:mm:ss");
            sb.AppendLine(s);
            s = "修改时间:" + File.GetLastWriteTime(file).ToString("yyyy-MM-dd HH:mm:ss");
            sb.AppendLine(s);

            return sb.ToString();
            
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Cancel;
            this.Close();
        }

        private void btnRename_Click(object sender, EventArgs e)
        {
            this.DialogResult = DialogResult.Retry;
            this.Close();
        }
    }
}
