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

namespace SuperControls
{
    public class ThumbBox
    {
     
        private int mInterval = 12;
        private int mSelInterval = 14;

        private bool mIsCurrent = false;

        private int mOriginalWidth = -1, mOriginalHeight = -1;
        private Color mBackColor;
        private Color mBorderColor;
        private bool mHovered = false;
        private int mBorderWidth = 0;
        private int mSizeChangeStep = 2;
        private bool mWithShadow = false;

        private bool mIsSelected = false;
        private string mFormatName = null;
        private bool mShowSummary = true;
        private string mOriginalFileName = null;
        private string mThumbFileName = null;
        private string mOriginalName = null;
        private string mShortName = null;
        private int mNameMaxLength = 24;

        private RectangleF mContentRectangle;
        private RectangleF mBorderRect;
        private int mRoundedRadius = 0;
        private int mShadowWidth = 10;
        public Image Image = null;
        private Rectangle mClientRectangle;
        private Rectangle mActualRectangle;
        public int OriginalFileIndex;

        private MouseEventHandler mOnMouseClickHandler = null;

        private Color mCurrentColor;
        private Color mHoverColor;

        private int mOffsetX = 0;
        private int mOffsetY = 0;
        private bool mIsFolder = false;


        public ThumbBox()
        {
            mBackColor = Color.DimGray;
            mBorderColor = Color.LightGray;

            mCurrentColor = Color.FromArgb(130, 231, 14, 34);
            mHoverColor = Color.FromArgb(100, 252, 234, 132);
        }
        public event MouseEventHandler MouseClick
        {
            add
            {
                lock (this)
                {
                    mOnMouseClickHandler += value;
                }
            }
            remove
            {
                lock (this)
                {
                    if (mOnMouseClickHandler != null)
                        mOnMouseClickHandler -= value;
                }
            }
        }
        public Rectangle ClientRectangle
        {
            get
            {
                return mClientRectangle;
            }
            set
            {
                mClientRectangle = value;
            }
        }
        
        public int Left
        {
            get
            {
                if (mActualRectangle.IsEmpty == false)
                    return mActualRectangle.Left;
                else
                    return 0;
            }
            set
            {
                mActualRectangle = new Rectangle(value, mActualRectangle.Top, mActualRectangle.Width, mActualRectangle.Height);
                mClientRectangle = new Rectangle(mActualRectangle.Left + OffsetX, mActualRectangle.Top + OffsetY, mActualRectangle.Width, mActualRectangle.Height);
            }
        }
        public int Right
        {
            get
            {
                if (mActualRectangle.IsEmpty == false)
                    return mActualRectangle.Right;
                else
                    return 0;
            }
            
        }
        public int Bottom
        {
            get
            {
                if (mActualRectangle.IsEmpty == false)
                    return mActualRectangle.Bottom;
                else
                    return 0;
            }

        }
        public int Top
        {
            get
            {
                if (mActualRectangle.IsEmpty == false)
                    return mActualRectangle.Top;
                else
                    return 0;
            }
            set
            {
                mActualRectangle = new Rectangle(mActualRectangle.Left,value, mActualRectangle.Width, mActualRectangle.Height);
                mClientRectangle = new Rectangle(mActualRectangle.Left + OffsetX, mActualRectangle.Top + OffsetY, mActualRectangle.Width, mActualRectangle.Height);
            }
        }

        public int Width
        {
            get
            {
                if (mClientRectangle.IsEmpty == false)
                    return mClientRectangle.Width;
                else
                    return 0;
            }
            set
            {
               
                mActualRectangle = new Rectangle(mActualRectangle.Left, mActualRectangle.Top, value, mActualRectangle.Height);
                mClientRectangle = new Rectangle(mActualRectangle.Left + OffsetX, mActualRectangle.Top + OffsetY, mActualRectangle.Width, mActualRectangle.Height);
            }
        }

        public int Height
        {
            get
            {
                if (mActualRectangle.IsEmpty == false)
                    return mActualRectangle.Height;
                else
                    return 0;
            }
            set
            {
                mActualRectangle = new Rectangle(mActualRectangle.Left, mActualRectangle.Top, mActualRectangle.Width,value);
                mClientRectangle = new Rectangle(mActualRectangle.Left + OffsetX, mActualRectangle.Top + OffsetY, mActualRectangle.Width, mActualRectangle.Height);
            }
        }
        public int OffsetX
        {
            get
            {
                return mOffsetX;
            }
            set
            {
                mOffsetX = value;
                mClientRectangle = new Rectangle(mActualRectangle.Left + OffsetX, mActualRectangle.Top + OffsetY, mActualRectangle.Width, mActualRectangle.Height);
            }
        }

        public int OffsetY
        {
            get{ return mOffsetY; }
            set {
                mOffsetY = value;
                mClientRectangle = new Rectangle(mActualRectangle.Left + OffsetX, mActualRectangle.Top + OffsetY, mActualRectangle.Width, mActualRectangle.Height);
            }
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
                        SelectMe();
                    }
                    else
                    {
                        UnselectMe();
                    }
                        
                }
            }
        }
        public void RaiseMouseClick(MouseEventArgs me)
        {
            MouseEventArgs e = new MouseEventArgs(me.Button,me.Clicks,me.X-this.Left,me.Y-this.Top,me.Delta);
            if (mOnMouseClickHandler != null)
            {
                mOnMouseClickHandler(this, e);
            }
        }
        public string OriginalFileName
        {
            get
            {
                return mOriginalFileName;
            }
            set
            {
                mOriginalFileName = value;
                if (mOriginalFileName != null)
                {
                    mOriginalName = System.IO.Path.GetFileName(mOriginalFileName);
                    string formatName = Common.GetFormatNameFromFile(mOriginalFileName);
                    if (formatName != null) formatName = formatName.ToUpper();
                    mFormatName = formatName;

                    mShortName = Common.FileName2ShortName(mOriginalName, mNameMaxLength);
                }
                else
                {
                    mOriginalName = null;
                    mFormatName = null;
                    mShortName = null;
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
                   
                }
            }
        }
   

        public Color BackColor
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
                }

            }
        }

        private bool mIsHovered = false;
        public bool IsHovered
        {
            get
            {
                return mIsHovered;
            }
            set
            {
                mIsHovered = value;
            }
        }

        public bool IsFolder { get => mIsFolder; set => mIsFolder = value; }

        private void UnselectMe()
        {
            mIsCurrent = false;
            
        }
        private void SelectMe()
        {
            mIsCurrent = true;
        }
        
        
        public bool HitTest(int x,int y)
        {
            return this.mClientRectangle.Contains(x, y);
        }

        public void Draw(Graphics g)
        {
            DrawBorderAndBG(g);

            //base.OnPaint(pe);
            DrawImage(g);
            Color maskColor = Color.Empty;
            if (IsCurrent)
                maskColor = mCurrentColor;
            else if (IsHovered)
                maskColor = mHoverColor;
            if (maskColor != Color.Empty)
                DrawMask(g,maskColor);

            DrawSummary(g);
            DrawStatus(g);

            
        }
        private void DrawMask(Graphics g,Color maskColor)
        {
            SolidBrush curBrush = new SolidBrush(maskColor);
            GraphicsPath borderPath = GeometryAPI.CreateRoundRect(mBorderRect, mRoundedRadius);
            g.FillPath(curBrush,borderPath);
            curBrush.Dispose();
        }
        private void DrawSummary(Graphics g)
        {
            if (mShowSummary == false) return;
            if (string.IsNullOrEmpty(mOriginalName) == true) return;

            int padding = 2;
            Font summaryFont = new Font("微软雅黑", 8);
            SizeF summarySize = g.MeasureString(mShortName, summaryFont, (int)(this.mBorderRect.Width- padding*2));
            RectangleF summaryRect = new RectangleF(this.mBorderRect.X, this.mBorderRect.Bottom-summarySize.Height,this.mBorderRect.Width,summarySize.Height+2*padding);
            GraphicsPath summaryPath = GeometryAPI.CreateBottomRoundRect(summaryRect, mRoundedRadius);

            SolidBrush summaryBGBrush = new SolidBrush(Color.FromArgb(200, Color.Black));
            SolidBrush fontBrush = new SolidBrush(Color.LightGray);
            g.FillPath(summaryBGBrush, summaryPath);
            summaryRect = new RectangleF(summaryRect.Left + padding, summaryRect.Top, summaryRect.Width - 2 * padding, summaryRect.Height - 2 * padding);
            StringFormat sf = new StringFormat();
            sf.Alignment = StringAlignment.Center;
            sf.LineAlignment = StringAlignment.Center;
            g.DrawString(mShortName, summaryFont, fontBrush, summaryRect,sf);

            summaryFont.Dispose();
            summaryBGBrush.Dispose();
            fontBrush.Dispose();

        }
        private void DrawStatus(Graphics g)
        {
            Font statusFont = new Font("微软雅黑", 6);

            List<StatusBox> boxies = new List<StatusBox>();
     
            if (mFormatName != null&&mShowSummary==false)
            {
                boxies.Add(StatusBox.Create(mFormatName, statusFont, g));
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
                
                
            }
        }

        


        private void DrawBorderAndBG(Graphics g)
        {
            int shadowWidth = (mWithShadow) ? mShadowWidth : 0;
            Rectangle clientRect = this.ClientRectangle;
            Rectangle borderRect = new Rectangle(clientRect.Left, clientRect.Top, clientRect.Width - mBorderWidth, clientRect.Height - mBorderWidth);
            borderRect = new Rectangle(borderRect.Left + shadowWidth, borderRect.Top + shadowWidth, borderRect.Width - shadowWidth * 2, borderRect.Height - shadowWidth * 2);

            //边框
            if (mBorderWidth > 0)
            {
                GraphicsPath borderPath = GeometryAPI.CreateRoundRect(borderRect, mRoundedRadius);
                Pen borderPen = new Pen(mBorderColor, mBorderWidth);
                g.DrawPath(borderPen, borderPath);
                borderPen.Dispose();
            }
            
            //背景
            float fWidthDelta = (float)mBorderWidth / 2;
            RectangleF ContentRect = new RectangleF(borderRect.Left + fWidthDelta, borderRect.Top + fWidthDelta, borderRect.Width - mBorderWidth, borderRect.Height - mBorderWidth);
            GraphicsPath contentPath = GeometryAPI.CreateRoundRect(ContentRect, mRoundedRadius);
            SolidBrush bgBrush = new SolidBrush(mBackColor);
            g.FillPath(bgBrush, contentPath);
            bgBrush.Dispose();

            mContentRectangle = ContentRect;
            mBorderRect = borderRect;

            //this.Region = CreateRegion(borderRect);
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
