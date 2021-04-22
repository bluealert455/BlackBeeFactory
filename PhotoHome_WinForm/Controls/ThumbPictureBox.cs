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
using System.Drawing.Imaging;
using System.Reflection;

namespace Controls
{
    public partial class ThumbPictureBox : PictureBox
    {
        private System.Windows.Forms.Timer mHoverTimer = null;
        private System.Windows.Forms.Timer mLeaveTimer = null;
        private System.Windows.Forms.Timer mClickTimer = null;
        private System.Windows.Forms.Timer mToNormalTimer = null;
        private int mInterval = 12;
        private int mSelInterval = 14;

        private bool mIsCurrent = false;

        private int mOriginalWidth = -1, mOriginalHeight = -1;
        private Color mBackColor;
        private Color mBorderColor;
        private bool mHovered = false;
        private int mBorderWidth = 1;
        private int mSizeChangeStep = 2;
        private bool mWithShadow = false;

        private bool mIsSelected = false;
        public string FormatName = null;
        public ThumbPictureBox()
        {
            InitializeComponent();

            mHoverTimer = new Timer();
            mHoverTimer.Interval = 5;
            mHoverTimer.Tick += MHoverTimer_Tick;

            mLeaveTimer = new Timer();
            mLeaveTimer.Interval = 5;
            mLeaveTimer.Tick += MLeaveTimer_Tick;

            this.Cursor = Cursors.Hand;

            mBackColor = this.BackColor;
            mBorderColor = Color.LightGray;
            DoubleBuffered = true;

            base.BackColor = Color.Transparent;
            BackColor = SystemColors.Control;
            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.Opaque, false);
            
        }
        public bool IsCurrent
        {
            get
            {
                return mIsCurrent;
            }
            set
            {
                if (mIsCurrent != value)
                {
                    mIsCurrent = value;
                    if (mIsCurrent)
                    {
                        this.BringToFront();
                        SelectMe();
                    }
                    else
                    {
                        UnselectMe();
                    }
                        
                }
            }
        }

        public bool IsSelected
        {
            get
            {
                return mIsSelected;
            }
            set
            {
                if (mIsSelected != value)
                {
                    mIsSelected = value;
                    Invalidate();
                }
            }
        }
        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            if (mOriginalWidth == -1)
            {
                mOriginalWidth = this.Width;
                mOriginalHeight = this.Height;

            }
        }
      

        public new Color BackColor
        {
            get
            {
                return mBackColor;
            }
            set
            {
                if (!mBackColor.Equals(value))
                {
                    mBackColor = value;

                    //RecalcRects();
                    Refresh();

                    OnBackColorChanged(EventArgs.Empty);
                }

            }
        }

        private void UnselectMe()
        {
            if (mToNormalTimer == null)
            {
                mToNormalTimer = new Timer();
                mToNormalTimer.Interval = 10;
                mToNormalTimer.Tick += MToNormalTimer_Tick;
            }
            StartTimer(mToNormalTimer);
            
        }
        private void SelectMe()
        {
            if (mClickTimer == null)
            {
                mClickTimer = new Timer();
                mClickTimer.Interval = 10;
                mClickTimer.Tick += MClickTimer_Tick;
            }
            StartTimer(mClickTimer);
        }

        private void MToNormalTimer_Tick(object sender, EventArgs e)
        {
           
            ReduceSizeByTimer(mToNormalTimer);
        }

        protected override void OnPaint(PaintEventArgs pe)
        {
            Graphics g = pe.Graphics;
     
            CommonLib.GeometryAPI.SetupGraphics(g);
            DrawBorderAndBG(g);

            //base.OnPaint(pe);
            DrawImage(g);

            DrawStatus(g);
        }

        private void DrawStatus(Graphics g)
        {
            Font statusFont = new Font("微软雅黑", 6);

            List<StatusBox> boxies = new List<StatusBox>();
     
            if (FormatName != null)
            {
                boxies.Add(StatusBox.Create(FormatName, statusFont, g));
            }
           
            if (mIsSelected == true)
            {
                boxies.Add(StatusBox.Create("选", statusFont, g));
            }

            if (boxies.Count > 0)
            {
                Color statusBoxColor = Color.FromArgb(23, 203, 61);
                Color statusFontColor = Color.FromArgb(50,50,50);

                SolidBrush statusBrush = new SolidBrush(statusBoxColor);
                SolidBrush textBrush = new SolidBrush(statusFontColor);
                float baseLeft = this.mBorderRect.Right;
                float baseTop = this.mBorderRect.Bottom - boxies[0].BoxSize.Height - 3;
                for (int i = 0; i < boxies.Count; i++)
                {
                    baseLeft = baseLeft - boxies[i].BoxSize.Width - 3;
                    StatusBox box = boxies[i];
                    box.Bounds = new RectangleF(baseLeft, baseTop, box.BoxSize.Width, box.BoxSize.Height);

                    GraphicsPath statusRounded = GeometryAPI.CreateRoundRect(box.Bounds, 3);
                    g.FillPath(statusBrush, statusRounded);
                    g.DrawString(box.Content, statusFont, textBrush, box.Bounds.Left + box.PaddingHor, box.Bounds.Top + box.PaddingVer);
                }

                
                statusBrush.Dispose();
                textBrush.Dispose();
            }
            statusFont.Dispose();
        }


   
        private void DrawImage(Graphics g)
        {
            if (this.Image != null)
            {
                
                int padding = 8;
                Rectangle imageRect = new Rectangle((int)mContentRectangle.Left+1, (int)mContentRectangle.Top + padding, (int)mContentRectangle.Width-1, (int)mContentRectangle.Height - padding * 2);
                int imgWidth = this.Image.Width;
                int imgHeight = this.Image.Height;
                float imgRatio = (float)imgWidth / (float)imgHeight;
                switch (this.SizeMode)
                {
                    case PictureBoxSizeMode.AutoSize:
                        break;
                    case PictureBoxSizeMode.CenterImage:
                        break;
                    case PictureBoxSizeMode.Normal:
                        g.DrawImageUnscaled(this.Image, imageRect);
                        break;
                    case PictureBoxSizeMode.StretchImage:
                        g.DrawImage(this.Image, imageRect);
                        break;
                    case PictureBoxSizeMode.Zoom:
                        if (imgWidth > imageRect.Width)
                        {
                            imgWidth = imageRect.Width;
                            imgHeight = (int)((float)imgWidth / imgRatio);
                        }

                        if (imgHeight > imageRect.Height)
                        {
                            imgHeight = imageRect.Height;
                            imgWidth= (int)((float)imgHeight * imgRatio);
                        }
                        int offsetX = (imageRect.Width-imgWidth) / 2;
                        int offsetY = (imageRect.Height-imgHeight) / 2;

                        imageRect = new Rectangle(imageRect.Left + offsetX, imageRect.Top + offsetY, imgWidth, imgHeight);
                        g.DrawImage(this.Image, imageRect);
                        break;

                }
                
            }
        }

        private RectangleF mContentRectangle;
        private RectangleF mBorderRect;
        private int mRoundedRadius = 8;
        private int mShadowWidth = 10;


        private void DrawBorderAndBG(Graphics g)
        {
            int shadowWidth = (mWithShadow) ? mShadowWidth : 0;
            Rectangle clientRect = this.ClientRectangle;
            Rectangle borderRect = new Rectangle(clientRect.Left, clientRect.Top, clientRect.Width - 1, clientRect.Height - 1);
            borderRect = new Rectangle(borderRect.Left + shadowWidth, borderRect.Top + shadowWidth, borderRect.Width - shadowWidth * 2, borderRect.Height - shadowWidth * 2);

            //边框
            GraphicsPath borderPath = GeometryAPI.CreateRoundRect(borderRect, mRoundedRadius);
            Pen borderPen = new Pen(mBorderColor, mBorderWidth);
            g.DrawPath(borderPen, borderPath);
            borderPen.Dispose();
            //背景
            float fWidthDelta = (float)mBorderWidth / 2;
            RectangleF ContentRect = new RectangleF(borderRect.Left + fWidthDelta, borderRect.Top + fWidthDelta, borderRect.Width - mBorderWidth, borderRect.Height - mBorderWidth);
            GraphicsPath contentPath = GeometryAPI.CreateRoundRect(ContentRect, mRoundedRadius);
            SolidBrush bgBrush = new SolidBrush(mBackColor);
            g.FillPath(bgBrush, contentPath);
            bgBrush.Dispose();

            mContentRectangle = ContentRect;
            mBorderRect = borderRect;

            this.Region = CreateRegion(borderRect);
            if (mWithShadow)
            {
                GeometryAPI.DrawRectShadow(GeometryAPI.RectF2Rect(mBorderRect), mRoundedRadius, g, Color.LightGray, 200, 10, 2, ShadowPosition.Outter);
            }
        }

        private Region CreateRegion(Rectangle borderRect)
        {
            Rectangle regionRect = new Rectangle(borderRect.Left - 1, borderRect.Top - 1, borderRect.Width + 2, borderRect.Height + 2);
            GraphicsPath regionPath = GeometryAPI.CreateRoundRect(regionRect, mRoundedRadius);
            return new Region(regionPath);
        }
        private void DrawShadow(Graphics g)
        {
            //阴影
            GeometryAPI.DrawRectShadow(GeometryAPI.RectF2Rect(mBorderRect), mRoundedRadius, g, Color.LightGray, 200, 10, 2, ShadowPosition.Outter);
        }
        private void MLeaveTimer_Tick(object sender, EventArgs e)
        {
            ReduceSizeByTimer(mLeaveTimer);

        }

        private void ReduceSizeByTimer(Timer timer)
        {
            if ((this.Width - mOriginalWidth) <= 0)
            {
                timer.Stop();
                return;
            }
            
            this.Left++;
            this.Top++;
            this.Width -= mSizeChangeStep;
            this.Height -= mSizeChangeStep;
        }

        private void AddSizeByTimer(Timer timer,int interval)
        {
            if ((this.Width - mOriginalWidth) >= interval * 2)
            {
                timer.Stop();
                return;
            }

            this.Left--;
            this.Top--;
            this.Width += mSizeChangeStep;
            this.Height += mSizeChangeStep;
        }
        private void MHoverTimer_Tick(object sender, EventArgs e)
        {
            
            AddSizeByTimer(mHoverTimer, mInterval);

        }

        private void StopOtherTimer(Timer timer)
        {
            Timer[] timers = new Timer[] { mToNormalTimer, mClickTimer, mHoverTimer, mLeaveTimer };
            for(int i = 0; i < timers.Length; i++)
            {
                if (timer!=null&&timer != timers[i])
                {
                    timer.Stop();
                }
            }
        }

        private void StartTimer(Timer timer)
        {
            Timer[] timers = new Timer[] { mToNormalTimer, mClickTimer, mHoverTimer, mLeaveTimer };
            //先把其他timer停了
            for (int i = 0; i < timers.Length; i++)
            {
                if (timers[i] != null && timer != timers[i])
                {
                    timers[i].Stop();
                }
            }
            timer.Start();
        }
        protected override void OnMouseClick(MouseEventArgs e)
        {
            this.IsCurrent = true;
           
            
            
            base.OnMouseClick(e);

        }


        private void MClickTimer_Tick(object sender, EventArgs e)
        {
            
            AddSizeByTimer(mClickTimer,mSelInterval);
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            this.BringToFront();
            if (IsCurrent == true)
                return;

            base.OnMouseHover(e);

            mHovered = true;
            StartTimer(mHoverTimer);
        }
   
        protected override void OnMouseLeave(EventArgs e)
        {
            if (IsCurrent == true)
                return;

            base.OnMouseLeave(e);

            if (mHovered)
            {
                StartTimer(mLeaveTimer);
            }
            this.SendToBack();
            mHovered = false;
        }
    }

    class StatusBox
    {
        public String Content;
        public SizeF BoxSize;
        public int PaddingHor = 2;
        public int PaddingVer = 1;
        public RectangleF Bounds;
        public static StatusBox Create(string s,Font font,Graphics g)
        {
            StatusBox box = new StatusBox();
            box.Content = s;
            box.BoxSize = g.MeasureString(s, font);
            box.BoxSize.Height = box.BoxSize.Height + box.PaddingVer * 2;
            box.BoxSize.Width = box.BoxSize.Width + box.PaddingHor * 2;
            return box;
        }
    }
}
