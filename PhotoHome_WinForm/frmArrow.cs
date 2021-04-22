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

namespace PhotoHome
{
    public enum ArrowDirection {
        Left=0,
        Right=1,
        Bottom=2,
        Top=3
    }

    public partial class frmArrow : Form
    {
        private ArrowDirection mArrowDirection = ArrowDirection.Left;
        private Control mHostCtrl = null;
        private int mArrowLineWidth = 4;
        private Color mDisabledColor = Color.LightGray;
        private int mArrowWidth = 20, mArrowHeight = 50;
        private int mPanelWidth;

        private double mMaxOpacity = 0.7;
        private Timer mShowTimer = null;
        private Timer mHideTimer = null;
        private Color mArrowMouseDownColor = Color.Gray;
        private bool mIsMouseDown = false;
        public frmArrow()
        {
            InitializeComponent();
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.Opaque, false);

            this.Opacity = 0.01;
            mPanelWidth = 60;

            mShowTimer = new Timer();
            mShowTimer.Interval = 10;
            mShowTimer.Tick += MShowTimer_Tick;

            mHideTimer = new Timer();
            mHideTimer.Interval = 10;
            mHideTimer.Tick += MHideTimer_Tick;
        }

        private void MHideTimer_Tick(object sender, EventArgs e)
        {
            this.Opacity = this.Opacity - 0.02;
            if (this.Opacity <= 0.01)
            {
                this.Opacity = 0.01;
                mHideTimer.Stop();
            }

        }

        private void MShowTimer_Tick(object sender, EventArgs e)
        {
            this.Opacity = this.Opacity + 0.02;
            if (this.Opacity >= mMaxOpacity)
            {
                this.Opacity = mMaxOpacity;
                mShowTimer.Stop();
            }
        }
        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);

        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            base.OnMouseDown(e);
            mIsMouseDown = true;
            this.Invalidate();

        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            base.OnMouseUp(e);
            mIsMouseDown = false;
            this.Invalidate();
        }
        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            this.Width = mPanelWidth;
            ResizeWindow();
        }
        public void Setup(ArrowDirection arrowDirection,Control hostCtrl)
        {
            mArrowDirection = arrowDirection;
            mHostCtrl = hostCtrl;
            hostCtrl.Resize += HostCtrl_Resize;
            ResizeWindow();
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            mHideTimer.Stop();
            mShowTimer.Start();
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            mShowTimer.Stop();
            mHideTimer.Start();
        }
        private void HostCtrl_Resize(object sender, EventArgs e)
        {
            ResizeWindow();
        }

        private void SetSize(int l,int t,int w,int h)
        {
            this.Location = new Point(l, t);
            this.Size = new Size(w, h);
        }
        private void ResizeWindow()
        {
            switch (mArrowDirection)
            {
                case ArrowDirection.Left:
                    SetSize(mHostCtrl.Left,mHostCtrl.Top, this.Width, mHostCtrl.Height);
                    break;
                case ArrowDirection.Right:
                    SetSize(mHostCtrl.Left+mHostCtrl.Width- this.Width, mHostCtrl.Top, this.Width, mHostCtrl.Height);
                    break;
                case ArrowDirection.Bottom:
                    this.Height = mPanelWidth;
                    break;
                case ArrowDirection.Top:
                    break;
            }
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics g = e.Graphics;
            CommonLib.GeometryAPI.SetupGraphics(g);

            DrawBG(g);
            DrawArrow(g);
        }

        private void DrawBG(Graphics g)
        {
            Rectangle bgRect = this.ClientRectangle;
            SolidBrush bgBrush = new SolidBrush(this.BackColor);
            g.FillRectangle(bgBrush, bgRect);

            bgBrush.Dispose();
        }

        private GraphicsPath CreateArrow()
        {
            GraphicsPath arrowPath = new GraphicsPath(); ;
           
            Rectangle arrowRect;
            if (mArrowDirection == ArrowDirection.Left || mArrowDirection == ArrowDirection.Right)
                arrowRect = GetArrowBox(mArrowWidth, mArrowHeight);
            else
                arrowRect = GetArrowBox(mArrowHeight, mArrowWidth);

            Point pt1, pt2, pt3;
            switch (mArrowDirection)
            {
                case ArrowDirection.Left:
                    pt1 = new Point(arrowRect.Right, arrowRect.Top);
                    pt2 = new Point(arrowRect.Left, arrowRect.Top + mArrowHeight / 2);
                    pt3 = new Point(arrowRect.Right, arrowRect.Bottom);
                    arrowPath.AddLine(pt1, pt2);
                    arrowPath.AddLine(pt2, pt3);
                    break;
                case ArrowDirection.Right:
                    pt1 = new Point(arrowRect.Left, arrowRect.Top);
                    pt2 = new Point(arrowRect.Right, arrowRect.Top + mArrowHeight / 2);
                    pt3 = new Point(arrowRect.Left, arrowRect.Bottom);
                    arrowPath.AddLine(pt1, pt2);
                    arrowPath.AddLine(pt2, pt3);
                    break;
                case ArrowDirection.Bottom:
                    break;
                case ArrowDirection.Top:
                    break;
            }
            return arrowPath;
        }
        private Rectangle GetArrowBox(int w,int h)
        {
            int left = (this.Width - w) / 2;
            int top = (this.Height - h) / 2;

            return new Rectangle(left, top, w, h);
        }
        private void DrawArrow(Graphics g)
        {
            Color arrowClr = this.ForeColor;
            if (this.Enabled == false)
                arrowClr = mDisabledColor;
            else
            {
                if (mIsMouseDown == true)
                    arrowClr = mArrowMouseDownColor;
            }

            GraphicsPath arrowPath = CreateArrow();
            Pen penArrow = new Pen(arrowClr, mArrowLineWidth);
            g.DrawPath(penArrow, arrowPath);
            penArrow.Dispose();
        }


    }
}
