using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SuperControls
{
    public partial class HScrollBarMobile : VScrollBarMobile
    {
        public HScrollBarMobile():base()
        {
            this.Width = 30;
            this.Height = 13;
        }

        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            this.Width = this.Parent.Width - 2;
            this.Height = 10;
            this.Top = 2;
            this.Left = 1;

            AttachEvents();
        }
        protected override void SetCtrlSize()
        {
            mCtrlSize = this.Width;
        }

        protected override int GetPosInMouseEvent(MouseEventArgs e)
        {
            return e.X;
        }
        protected override void CalcValue(int pos)
        {
            mCtrlSize = this.Width;
            base.CalcValue(pos);
        }
        protected override int GetBarRoundedRadius()
        {
            return this.Height / 2;
        }

        protected override void CalcHoldSize()
        {
            int holdSize = GetHoldSize();
            if (holdSize < mMinHoldSize)
                holdSize = mMinHoldSize;
            if (mValue < 0)
                mValue = 0;
            if (mValue > mMaximum)
                mValue = mMaximum;
            float ratio = (float)mValue / (float)mMaximum;
            int holdLeft = (int)((float)(mContentRect.Width) * ratio);
        
            if (holdLeft < mContentRect.Left) holdLeft = mContentRect.Left;
            int maxLeft = mContentRect.Width - holdSize;
            if (holdLeft > maxLeft)
                holdLeft = maxLeft;
            
            mHoldRect = new Rectangle(holdLeft, mContentRect.Top, holdSize, mContentRect.Height);

            //if (mHoldRect.IsEmpty)
            //    mHoldRect = new Rectangle(mContentRect.Left, mContentRect.Top, mContentRect.Width, holdSize);
            //int left = mHoldRect.Left;
            //if (left < mContentRect.Left) left = mContentRect.Left;
            //mHoldRect = new Rectangle(left, mContentRect.Top, holdSize, mContentRect.Height);
        }

        protected override int GetBlankSize()
        {
            return mContentRect.Width - GetHoldSize();
        }
        protected override bool IsHoldToEnd(int dx, int dy)
        {
            if (dx >= 0 && mHoldRect.Left >= GetBlankSize()) return true;
            if (dx <= 0 && mHoldRect.Left <= mContentRect.Left) return true;
            return false;
        }
        protected override void RecalcHoldPos(int x, int y)
        {
            int w = mContentRect.Width; //GetBlankSize();
            int dx = x - mLastX;

            float ratio = (float)dx / (float)w;

            int dv = (int)((float)mMaximum * ratio);
            mValue += dv;
            //int newLeft = mHoldRect.Left + dx;
            //if (newLeft < mContentRect.Left) newLeft = mContentRect.Left;
            //int maxLeft
            //if (newLeft > GetBlankSize()) newLeft = w;
            //mHoldRect = new Rectangle(newLeft, mHoldRect.Top, mHoldRect.Width, mHoldRect.Height);
        }
    }
}
