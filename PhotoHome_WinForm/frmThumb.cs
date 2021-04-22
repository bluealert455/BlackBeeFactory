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
    public partial class frmThumb : frmFloatBase
    {
        private CommonLib.SetIntValueCallback mThumbnailClick = null;
        public frmThumb()
        {
            InitializeComponent();
            this.thumbnailList.ThumbSizeMode = EnumThumbSizeMode.MiddleLarge;
            this.thumbnailList.BackColor = this.BackColor;
        }

       

        protected override void OnKeyUp(KeyEventArgs e)
        {
            base.OnKeyUp(e);
            //MainForm.WhenKeyUp(this,e);
        }
        public event CommonLib.SetIntValueCallback OnThumbnailClick
        {
            add
            {
                lock (this)
                {
                    mThumbnailClick += value;
                }
            }
            remove
            {
                lock (this)
                {
                    if (mThumbnailClick != null)
                        mThumbnailClick -= value;
                }
            }
        }
        private void thumbnailList_OnThumbnailClick(int val)
        {
            if (mThumbnailClick != null)
            {
                mThumbnailClick(val);
            }
        }

        private void thumbnailList_Click(object sender, EventArgs e)
        {

        }
    }
}
