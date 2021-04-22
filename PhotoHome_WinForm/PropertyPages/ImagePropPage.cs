using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommonLib;

namespace PropertyPages
{

    public partial class ImagePropPage : FilePropertyPage
    {
        private SetTwoIntValueCallback mCBChangedHandler = null;
        private SetTwoIntValueCallback mWHChangedHandler = null;
        private BoolParamMethodCallback mOnSaveHandler = null;
        private ImageProp mImageProp = null;

        private double mWHRatio = 1;
        public ImagePropPage():base()
        {
            
            InitializeComponent();
        }
        /// <summary>
        /// 亮度和对比度发生变化
        /// </summary>
        public event SetTwoIntValueCallback OnCBChanged
        {
            add
            {
                lock (this)
                {
                    mCBChangedHandler += value;
                }
            }
            remove
            {
                lock (this)
                {
                    if (mCBChangedHandler != null)
                        mCBChangedHandler -= value;
                }
            }
        }

        public event SetTwoIntValueCallback OnWHChanged
        {
            add
            {
                lock (this)
                {
                    mWHChangedHandler += value;
                }
            }
            remove
            {
                lock (this)
                {
                    if (mWHChangedHandler != null)
                        mWHChangedHandler -= value;
                }
            }
        }
        public event BoolParamMethodCallback OnImageSave
        {
            add
            {
                lock (this)
                {
                    mOnSaveHandler += value;
                }
            }
            remove
            {
                lock (this)
                {
                    if (mOnSaveHandler != null)
                        mOnSaveHandler -= value;
                }
            }
        }
        public void Init(ImageProp prop)
        {
            this.txtResizeHeight.Text = prop.Height.ToString();
            this.txtResizeWidth.Text = prop.Width.ToString();

            this.lblOldWidth.Text = prop.Width.ToString();
            this.lblOldHeight.Text = prop.Height.ToString();

            mWHRatio =((double)prop.Width) / ((double)prop.Height);

            this.trackBrightness.Value =(int) prop.Brightness;
            this.trackContrast.Value =(int) prop.Contrast;

            mImageProp = prop;

            if (prop.FileType != null && prop.FileType.ToLower() == ".gif")
            {
                SetAllControlsEnabled(false);
            }
            else
            {
                SetAllControlsEnabled(true);
            }
        }
        
        private void SetAllControlsEnabled(bool bEnabled)
        {
            for(int i = 0; i<this.Controls.Count; i++)
            {
                Control ctrl = this.Controls[i];
                if(ctrl is Label||ctrl is CheckBox)
                {
                    Color enabledForeColor = (ctrl.Tag != null && ctrl.Tag.ToString() == "1") ? Color.Silver : Color.White;
                    ctrl.ForeColor = bEnabled ? enabledForeColor : Color.Gray;
                }
                else
                {
                    ctrl.Enabled = bEnabled;
                }
            }
        }
        public int ImageWidth
        {
            get
            {
                return int.Parse(this.txtResizeWidth.Text);
            }
            set
            {
                this.txtResizeWidth.Text = value.ToString();
            }
        }

        public int ImageHeight
        {
            get
            {
                return int.Parse(txtResizeHeight.Text);
            }
            set
            {
                this.txtResizeHeight.Text = value.ToString();
            }
        }

        private void trackContrast_Scroll(object sender, EventArgs e)
        {
            
        }

        private void trackBrightness_Scroll(object sender, EventArgs e)
        {
            
        }

        private void btnSave_Click(object sender, EventArgs e)
        {
           
        }

        private bool IsIntTextInputValid(TextBox txtBox)
        {
            string val = txtBox.Text;
            bool isOK = false;
            if (Common.IsInt(val))
            {
                if (int.Parse(val) > 0)
                {
                    isOK = true;
                }
            }

            txtBox.ForeColor = isOK ? Color.Black : Color.Red;
            return isOK;

        }
        private void txtResizeWidth_TextChanged(object sender, EventArgs e)
        {
            IsIntTextInputValid(this.txtResizeWidth);
        }

        private void txtResizeHeight_TextChanged(object sender, EventArgs e)
        {
            IsIntTextInputValid(this.txtResizeHeight);
        }

        private void txtResizeWidth_KeyUp(object sender, KeyEventArgs e)
        {
            if (IsIntTextInputValid(this.txtResizeWidth))
            {
                int w = int.Parse(txtResizeWidth.Text);
                int h = (int)((double)w / mWHRatio);
                this.txtResizeHeight.Text = h.ToString();
                if (mWHChangedHandler != null && this.chkPreview.Checked)
                    mWHChangedHandler(w, h);
            }
        }

        private void txtResizeHeight_KeyUp(object sender, KeyEventArgs e)
        {
            if (IsIntTextInputValid(this.txtResizeHeight))
            {
                int h = int.Parse(txtResizeHeight.Text);
                int w = (int)((double)h * mWHRatio);
                this.txtResizeWidth.Text = w.ToString();
                if (mWHChangedHandler != null&&this.chkPreview.Checked)
                    mWHChangedHandler(w, h);
            }
        }

        private void lblOldWidth_Click(object sender, EventArgs e)
        {
            ResetWH();
        }

        private void lblOldHeight_Click(object sender, EventArgs e)
        {
            ResetWH();
        }

        private void ResetWH()
        {
            this.txtResizeHeight.Text = lblOldHeight.Text;
            this.txtResizeWidth.Text = lblOldWidth.Text;
            if (mWHChangedHandler != null && this.chkPreview.Checked)
                mWHChangedHandler(mImageProp.Width,mImageProp.Height);
        }

        private void trackContrast_ValueChanged(object sender, EventArgs e)
        {
            if (this.chkPreview.Checked)
            {
                if (mCBChangedHandler != null)
                    mCBChangedHandler(trackContrast.Value, trackBrightness.Value);
            }

            this.lblContrastVal.Text = trackContrast.Value.ToString();
        }

        private void trackBrightness_ValueChanged(object sender, EventArgs e)
        {
            if (this.chkPreview.Checked)
            {
                if (mCBChangedHandler != null)
                    mCBChangedHandler(trackContrast.Value, trackBrightness.Value);
            }
            this.lblBrightnessVal.Text = trackBrightness.Value.ToString();
        }

        private void lblContrastVal_Click(object sender, EventArgs e)
        {
            this.trackContrast.Value = 0;
            this.lblContrastVal.Text = "0";
        }

        private void lblBrightnessVal_Click(object sender, EventArgs e)
        {
            this.trackBrightness.Value = 0;
            this.lblBrightnessVal.Text = "0";
        }

        private void btnApplySettings_Click(object sender, EventArgs e)
        {
            if (IsIntTextInputValid(this.txtResizeWidth) && IsIntTextInputValid(this.txtResizeHeight))
            {
                int w = int.Parse(this.txtResizeWidth.Text);
                int h = int.Parse(this.txtResizeHeight.Text);
                if (mWHChangedHandler != null && this.chkPreview.Checked)
                    mWHChangedHandler(mImageProp.Width, mImageProp.Height);
            }

            if (trackBrightness.Value != mImageProp.Brightness || trackContrast.Value != mImageProp.Contrast)
            {
                if (mCBChangedHandler != null)
                    mCBChangedHandler(trackContrast.Value, trackBrightness.Value);
            }
        }
    }
}
