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
    public partial class frmUSBPrompt : frmFloatBase
    {
        private frmMain mMainForm = null;
        public frmUSBPrompt()
        {
            InitializeComponent();
            this.Shown += FrmUSBPrompt_Shown;
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
        private void FrmUSBPrompt_Shown(object sender, EventArgs e)
        {
           // MessageBox.Show("GSDG");
        }

        public void ShowMe()
        {

        }
        public void RemoveByGroup(string group)
        {
            this.lstFolderes.RemoveByGroup(group);
            if (this.lstFolderes.ItemCount == 0)
                this.Visible = false;
        }
        public void FillList(List<string> items,string group)
        {
            AddToList(items,group,true);
        }
        public void AddToList(List<string> items,string group,bool clear)
        {
            if(clear)lstFolderes.ClearItems();
            for (int i = 0; i < items.Count; i++)
            {
                lstFolderes.AddItem(items[i],group);
            }
            lstFolderes.Invalidate();
        }
        private void btnClose_Click(object sender, EventArgs e)
        {
            this.Hide();
        }

        private void lstFolderes_OnItemClick(SuperControls.ListItem item)
        {
            MainForm.mainDataPanel.LoadFolder(item.Text);
            this.Hide();
        }
    }
}
