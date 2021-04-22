using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhotoHome_WinForm
{
    public delegate void OnCommonEventHander(object sender, EventArgs e);
    public partial class frmLeftPanel : Form
    {
        public OnCommonEventHander OnOpenClick;
        public OnCommonEventHander OnPasteClick;

        public frmLeftPanel()
        {
            InitializeComponent();
            this.Left = 0;
            this.Top = 10;
        }

        private void btnOpen_Click(object sender, EventArgs e)
        {
            OnOpenClick(sender, e);
        }

        private void btnPaste_Click(object sender, EventArgs e)
        {

        }

        private void btnCopy_Click(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {

        }

        private void btnSettings_Click(object sender, EventArgs e)
        {

        }
    }
}
