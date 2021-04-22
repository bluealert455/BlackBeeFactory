using SuperControls;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhotoHome
{
    public partial class frmSetLayout : frmPopupWindow
    {
        private frmMain mMainForm;
        public frmSetLayout()
        {
            InitializeComponent();
        }

        public void SetImageList(ImageList imgList)
        {
            this.btnClassifyLayout.ImageList = imgList;
            this.btnModernLayout.ImageList = imgList;
            this.btnSimpleLayout.ImageList = imgList;

            this.btnClassifyLayout.ImageKey = "classify32.png";
            this.btnSimpleLayout.ImageKey = "simple32.png";
            this.btnModernLayout.ImageKey = "modern32.png";

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

        private void SetAnchorButtonImg(string imgKey)
        {
            GlassButton btn = mAnchorControl as GlassButton;
            btn.ImageKey = imgKey;
        }
        private void btnClassifyLayout_Click(object sender, EventArgs e)
        {
            this.Hide();
            MainForm.Perspective = EnumPerspective.Classify;
            SetAnchorButtonImg("classify32.png");

            SaveLayoutConfig(EnumPerspective.Classify);

            this.Close();
        }

        private void SaveLayoutConfig(EnumPerspective layout)
        {
            CommonLib.AppConfig.Instance.AppLayout = (int)layout;
            CommonLib.AppConfig.Instance.Save();
        }
        private void btnModernLayout_Click(object sender, EventArgs e)
        {
            this.Hide();
            MainForm.Perspective = EnumPerspective.Modern;
            SetAnchorButtonImg("modern32.png");
            SaveLayoutConfig(EnumPerspective.Modern);
            this.Close();

        }

        private void btnSimpleLayout_Click(object sender, EventArgs e)
        {
            this.Hide();
            MainForm.Perspective = EnumPerspective.Simple;
            SetAnchorButtonImg("simple32.png");
            SaveLayoutConfig(EnumPerspective.Simple);
            this.Close();
        }
    }
}
