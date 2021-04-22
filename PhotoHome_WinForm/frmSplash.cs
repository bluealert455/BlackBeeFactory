using CommonLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SuperControls;
namespace PhotoHome
{
    public partial class frmSplash : frmFloatBase
    {
        private frmMain mMainForm = null;
        public frmSplash()
        {
            InitializeComponent();
        }

        public frmMain MainForm
        {
            get
            { return mMainForm; }
            set
            {
                mMainForm = value;
            }
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            //GraphicsPath regionPath = GeometryAPI.CreateRoundRect(this.ClientRectangle, 5);
            //Region region = new Region(regionPath);
            //this.Region = region;
        }

        private void btnOpenFolder_Click(object sender, EventArgs e)
        {
            MainForm.mainDataPanel.OpenWithFolderDlg();
        }

        private void btnOpenFile_Click(object sender, EventArgs e)
        {
            MainForm.mainDataPanel.OpenWithDialog();
        }
    }
}
