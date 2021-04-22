using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Drawing.Drawing2D;
using CommonLib;


namespace Controls
{
    public partial class VScrollBarMobile : Control
    {
        protected Color mBackColor = Color.FromArgb(39, 39, 39);
        protected Color mHoldColor = Color.FromArgb(100, 100, 100);
        protected Color mHoldMouseDownColor = Color.FromArgb(200, 200, 200);
        protected Color mHoldMouseOverColor = Color.FromArgb(150, 150, 150);
        protected Cursor mOldCursor;
        protected int mRoundRadius;
        protected int mMinHoldSize = 20;
        protected Rectangle mHoldRect;
        protected Rectangle mBorderRect;
        protected Rectangle mContentRect;
        protected int mMaximum = 100;
        protected int mMinimum = 0;
        protected int mLargeChange = 10;
        protected int mSmallChange = 10;
        protected int mValue = 0;
        protected bool mIsInHold = false;

        protected int mOldValue = 0;
        protected ScrollEventHandler mScrollEventHandler = null;
        protected ScrollEventHandler mScrollEndEventHandler = null;
        protected int mCtrlSize = 0;
        protected Control mOldParent = null;

        public VScrollBarMobile()
        {
            InitializeComponent();

            SetStyle(
                 ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw
                 | ControlStyles.Selectable | ControlStyles.AllPaintingInWmPaint
                 | ControlStyles.UserPaint, true);

            mOldCursor = this.Cursor;

            this.Height = 30;
            this.Width = 13;

            
        }
        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            if (this.Parent != null)
            {
                this.Left = this.Parent.Width - 11;
                this.Top = 2;
                this.Width = 10;
                this.Height = this.Parent.Height - 4;
            }
            
            
            AttachEvents();

          
        }
        protected virtual void AttachEvents()
        {
            if (mOldParent != null && this.Parent != mOldParent)
            {
                mOldParent.MouseEnter -= Parent_MouseEnter;
                mOldParent.MouseLeave -= Parent_MouseLeave;
            }
            if (this.Parent != null)
            {
               
                this.Parent.MouseEnter += Parent_MouseEnter;
                this.Parent.MouseLeave += Parent_MouseLeave;
                HideByMousePos();
            }
            
            mOldParent = this.Parent;
        }


        private void Parent_MouseLeave(object sender, EventArgs e)
        {
            HideByMousePos();
        }

        private void HideByMousePos()
        {
            if (this.Parent == null)
            {
                this.Visible = false;
                return;
                    
            }
            POINT pnt = new POINT();
            WindowAPI.GetCursorPos(ref pnt);

            Rectangle rect = this.Parent.RectangleToScreen(this.Parent.ClientRectangle);
            if (!rect.Contains(new Point((int)pnt.X, (int)pnt.Y)))
            {
                this.Visible = false;
            }
        }
        private void Parent_MouseEnter(object sender, EventArgs e)
        {
            if (this.Visible == false) this.Visible = true;
        }

        public int Maximum
        {
            get
            {
                return mMaximum;
            }

            set
            {
                mMaximum = value;

                if (IsHandleCreated)
                {
                    Invalidate();
                }
            }
        }

        public int SmallChange
        {
            get
            {
                return mSmallChange;
            }

            set
            {
                mSmallChange = value;

               
            }
        }
        public int LargeChange
        {
            get
            {
                return mLargeChange;
            }

            set
            {
                mLargeChange = value;


            }
        }
        public int Minimum
        {
            get
            {
                return mMinimum;
            }

            set
            {
                mMinimum = value;

                if (IsHandleCreated)
                {
                    Invalidate();
                }
            }
        }
        public int Value
        {
            get
            {
                return mValue;
            }

            set
            {
                mValue = value;

                if (IsHandleCreated)
                {
                    Invalidate();
                }
            }
        }
        public event ScrollEventHandler OnScroll
        {
            add
            {
                lock (this)
                {
                    mScrollEventHandler += value;
                }
            }
            remove
            {
                lock (this)
                {
                    if (mScrollEventHandler != null)
                        mScrollEventHandler -= value;
                }
            }
        }
        public event ScrollEventHandler OnScrollEnd
        {
            add
            {
                lock (this)
                {
                    mScrollEndEventHandler += value;
                }
            }
            remove
            {
                lock (this)
                {
                    if (mScrollEndEventHandler != null)
                        mScrollEndEventHandler -= value;
                }
            }
        }
        private void RaiseScrollEvent(ScrollEventType scrollType)
        {
            object sender = this;
            ScrollEventArgs args = new ScrollEventArgs(scrollType,this.mOldValue,this.mValue);
            if (mScrollEventHandler != null)
            {
                mScrollEventHandler(sender, args);
            }
        }
        private void RaiseScrollEndEvent(ScrollEventType scrollType)
        {
            object sender = this;
            ScrollEventArgs args = new ScrollEventArgs(scrollType, this.mValue);
            if (mScrollEndEventHandler != null)
            {
                mScrollEndEventHandler(sender, args);
            }
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBilinear;

            DrawBar(g);

            DrawHold(g);
        }

        protected virtual int GetBarRoundedRadius()
        {
            return this.ClientRectangle.Width/2;
        }
        private void DrawBar(Graphics g)
        {
            SolidBrush bgBrush = new SolidBrush(mBackColor);
            Rectangle borderRect = this.ClientRectangle;
            Rectangle contentRect = new Rectangle(borderRect.Left + 1, borderRect.Top + 1, borderRect.Width - 2, borderRect.Height - 2);
            int radius = GetBarRoundedRadius();

            GraphicsPath borderRound = GeometryAPI.CreateRoundRect(borderRect, radius);
            GraphicsPath contentRound = GeometryAPI.CreateRoundRect(contentRect, radius);

            Pen borderPen = new Pen(Color.Gray);

            g.DrawPath(borderPen, borderRound);
            g.FillPath(bgBrush, contentRound);

            borderPen.Dispose();
            bgBrush.Dispose();

            mBorderRect = borderRect;
            mContentRect = contentRect;

            mRoundRadius = radius;
        }

        protected virtual void SetCtrlSize()
        {
            mCtrlSize = this.Height;
        }
        protected virtual int GetThumbSize()
        {
            SetCtrlSize();

            if (this.mMaximum == 0 || this.mLargeChange == 0)
            {
                return mCtrlSize;
            }

            float newThumbSize = ((float)this.mLargeChange * (float)mCtrlSize) /
              (float)this.mMaximum;

            return Convert.ToInt32(Math.Min((float)mCtrlSize, Math.Max(newThumbSize, 10f)));
        }

        protected virtual int GetPosInMouseEvent(MouseEventArgs e)
        {
            return e.Y;
        }
        protected virtual void CalcValue(int pos)
        {
            float ratio = (float)pos / (float)mCtrlSize;
            mValue = (int)((float)mMaximum * ratio);
        }
        private bool IsInHold(int x, int y)
        {
            return mHoldRect.Contains(x, y);
        }

        private bool mIsMouseDown = false;
        private int mMouseDownX, mMouseDownY;
        private bool mIsScrolled = false;
        protected override void OnMouseMove(MouseEventArgs e)
        {
  
            base.OnMouseMove(e);
            mIsInHold = IsInHold(e.X, e.Y);
            if (mIsInHold == true)
            {
                this.Cursor = Cursors.Hand;
            }
            else
            {
                this.Cursor = mOldCursor;
            }

            bool isMoveHold = false;
            if (mIsMouseDown == true)
            {
                //int offsetY = e.Y - mMouseDownY;
                mOldValue = mValue;
                CalcValue(GetPosInMouseEvent(e));
                isMoveHold = true;

            }
            this.Invalidate();

            if (isMoveHold)
            {
                RaiseScrollEvent(ScrollEventType.ThumbTrack);
                mIsScrolled = true;
            }
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            mIsMouseDown = true;
            mMouseDownX = e.X;
            mMouseDownY = e.Y;
            this.Invalidate();
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            //base.OnMouseUp(e);
            mIsMouseDown = false;
            this.Invalidate();

            if (mIsScrolled == true)
            {
                mIsScrolled = false;
                RaiseScrollEndEvent(ScrollEventType.ThumbTrack);
            }

        }
    
        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
           
           
        }

        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            HideByMousePos();
        }
        public void RaiseMouseWheel(int delta)
        {
            ScrollEventType scrollType = ScrollEventType.SmallIncrement;
            this.mOldValue = this.mValue;
            if (delta < 0)
            {
                if (this.mValue > this.mMaximum)
                    return;
                this.mValue += mSmallChange;
                if (this.mValue > this.mMaximum)
                {
                    this.mValue = this.mMaximum;
                }

            }
            else
            {
                if (this.mValue < mMinimum)
                    return;
                this.mValue -= mSmallChange;
                if (this.mValue < mMinimum)
                {
                    this.mValue = mMinimum;
                }
                scrollType = ScrollEventType.SmallDecrement;

            }
            this.Invalidate();
            RaiseScrollEvent(scrollType);
        }
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            RaiseMouseWheel(e.Delta);


        }
        protected void CalcHoldSize()
        {
            int holdSize = GetThumbSize();
            if (holdSize < mMinHoldSize)
                holdSize = mMinHoldSize;
            if (mValue < 0)
                mValue = 0;
            if (mValue > mMaximum)
                mValue = mMaximum;
            float ratio = (float)mValue / (float)mMaximum;
            int holdTop = (int)((float)mCtrlSize * ratio);
            holdTop = holdTop - holdSize / 2;
            if (holdTop < 0) holdTop = 0;
            int maxTop = mCtrlSize - holdSize - 2;
            if (holdTop > maxTop)
                holdTop = maxTop;
            CreateHoldRect(mContentRect.Left, holdTop + 1, mContentRect.Width, holdSize);
        }

        protected virtual void CreateHoldRect(int left,int top,int width,int height)
        {
            mHoldRect = new Rectangle(left, top, width, height);
        }
        private void DrawHold(Graphics g)
        {
            CalcHoldSize();

            Color holdColor = mHoldColor;
            if (mIsInHold)
            {
                holdColor = mHoldMouseOverColor;
                if (mIsMouseDown == true)
                {
                    holdColor = mHoldMouseDownColor;
                }
            }
            GraphicsPath holdPath = GeometryAPI.CreateRoundRect(mHoldRect, mRoundRadius - 1);
            SolidBrush holdBrush = new SolidBrush(holdColor);
            g.FillPath(holdBrush, holdPath);

            holdBrush.Dispose();

        }

    }
}
