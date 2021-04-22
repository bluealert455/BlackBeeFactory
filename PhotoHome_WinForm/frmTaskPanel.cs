using PropertyPages;
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
    public partial class frmTaskPanel : frmFloatBase
    {
        public Control CurPropPage = null;
        private frmMain mMainForm = null;
        public frmTaskPanel()
        {
            InitializeComponent();
        }
        
        public void Init(object propInfo)
        {
            bool needCreate = false;
            if(propInfo is ImageProp)
            {
                if (CurPropPage != null && !(CurPropPage is ImagePropPage))
                {
                    this.Controls.Remove(CurPropPage);
                    needCreate = true;
                }
                else if (CurPropPage == null)
                    needCreate = true;


                if (needCreate)
                {
                    CurPropPage = new ImagePropPage();
                    CurPropPage.BackColor = this.BackColor;
                    ((ImagePropPage)CurPropPage).OnCBChanged += FrmTaskPanel_OnCBValueChanged;
                    ((ImagePropPage)CurPropPage).OnImageSave += FrmTaskPanel_OnImageSave;
                    ((ImagePropPage)CurPropPage).OnWHChanged += FrmTaskPanel_OnWHChanged;
                }
                ((ImagePropPage)CurPropPage).Init(propInfo as ImageProp);
                CurPropPage.Location = new Point(2, 2);
                this.Controls.Add(CurPropPage);

            }
        }
        public frmMain MainForm
        {
            get
            {
                return mMainForm;
            }
            set
            {
                mMainForm = value;
            }
        }
        private void FrmTaskPanel_OnWHChanged(int w, int h)
        {
            MainForm.mainDataPanel.ResizeImage(w, h);
        }

        private void FrmTaskPanel_OnSelectTargetFolderChanged(string str)
        {
            MainForm.mThumbLeft.thumbnailList.SelectTargetFolder = str;
        }

        private void FrmTaskPanel_OnImageSave(bool saveToSelectFolder)
        {
            MainForm.mainDataPanel.SaveImage(saveToSelectFolder?MainForm.GetSelectTargetFolder():null);
            MainForm.mThumbLeft.thumbnailList.UpdateCurThumbBoxStatus(true);
            
        }

        private void FrmTaskPanel_OnCBValueChanged(int val1, int val2)
        {
            MainForm.mainDataPanel.AdjustContrastAndBright(val1, val2);
        }

        private void FrmTaskPanel_OnContrastChanged(int val)
        {
            MainForm.mainDataPanel.AdjustContrast(val);
        }

        private void FrmTaskPanel_OnBrightnessChanged(int val)
        {
            MainForm.mainDataPanel.AdjustBrightness(val);
        }
    }
}
