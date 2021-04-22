using CommonLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SuperControls
{
    public delegate void ItemClickHandler(ListItem item);
    public class ListPanel:Control
    {
        List<ListItem> mItems = new List<ListItem>();
        private float mLinesHeight = 0;
        VScrollBarMobile mVScrollBar = null;
        float mbaseTop = 5;
        private int mScrollValue = 0;

        private BufferedGraphicsContext currentContext;
        private BufferedGraphics drawingBuffer = null;
        private ListItem mSelectedItem = null;
        private ListItem mHoveredItem = null;
        private ItemClickHandler mItemClickHandler = null;
        private int mItemMarginHor=5;
        public ListPanel() : base()
        {
            currentContext = BufferedGraphicsManager.Current;

        }
        public event ItemClickHandler OnItemClick
        {
            add
            {
                lock (this)
                {
                    mItemClickHandler += value;
                }
            }
            remove
            {
                lock (this)
                {
                    if (mItemClickHandler != null)
                        mItemClickHandler -= value;
                }
            }
        }

        public int ItemCount
        {
            get
            {
                if (mItems == null)
                    return mItems.Count;
                else
                    return 0;
            }
        }
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            drawingBuffer = null;

            this.Invalidate();
        }
        public void RemoveByGroup(string group)
        {
            List<ListItem> itemsRemoved = new List<ListItem>();
            for(int i = 0; i < mItems.Count; i++)
            {
                if (mItems[i].Group == group)
                    itemsRemoved.Add(mItems[i]);
            }
            for (int i = 0; i < itemsRemoved.Count; i++)
            {
                mItems.Remove(itemsRemoved[i]);
            }

            this.Invalidate();

        }
        public void AddItem(string text,string group=null,Image img=null)
        {
            AddItem(new ListItem(text,group,img));
        }
        public void AddItem(ListItem item)
        {
            mItems.Add(item);
            item.Index = mItems.Count - 1;
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {

        }
        public void AddItems(List<ListItem> items)
        {
            for(int i = 0; i < items.Count; i++)
            {
                AddItem(items[i]);
            }
        }

        private void DrawItems(Graphics gfx)
        {
            if (drawingBuffer == null)
                drawingBuffer = currentContext.Allocate(gfx, this.DisplayRectangle);

            lock (mItems)
            {
                drawingBuffer.Graphics.Clear(this.BackColor);
                if (mItems != null && mItems.Count > 0)
                {
                    float baseTop = mbaseTop + mScrollValue;
                    for (int i = 0; i < mItems.Count; i++)
                    {
                        ListItem item = mItems[i];
                        item.Draw(drawingBuffer.Graphics, mItemMarginHor, (int)baseTop, this.Width- mItemMarginHor*2);
                        baseTop += item.Bounds.Height + 5;

                        mLinesHeight += item.Bounds.Height;
                    }


                }
                drawingBuffer.Render(gfx);
            }

        }
        protected override void OnPaint(PaintEventArgs e)
        {
            //base.OnPaint(e);

            Graphics gfx = e.Graphics;
            GeometryAPI.SetupGraphics(gfx);
            mLinesHeight = 10;
            DrawItems(gfx);
            if (mLinesHeight > this.Height)
            {
                if (mVScrollBar == null)
                {
                    mVScrollBar = new VScrollBarMobile();

                    mVScrollBar.OnScroll += MVScrollBar_Scroll;
                    this.Controls.Add(mVScrollBar);
                }

                mVScrollBar.Maximum = (int)mLinesHeight;
            }
            else
            {
                if (mVScrollBar != null)
                {
                    this.Controls.Remove(mVScrollBar);
                    mVScrollBar = null;
                }
            }

        }
        private ListItem HitTest(int x,int y)
        {
            for(int i = 0; i < mItems.Count; i++)
            {
                if (mItems[i].HitTest(x, y))
                    return mItems[i];
            }

            return null;
        }
        

        public ListItem SelectedItem { get => mSelectedItem; set => mSelectedItem = value; }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            ListItem tempItem = HitTest(e.X, e.Y);

            if (tempItem == null)
            {
                if (mHoveredItem != null)
                    mHoveredItem.IsHovered = false;

                this.Cursor = Cursors.Default;
            }
            else
            {
                if (mHoveredItem != tempItem)
                {
                    if (mHoveredItem != null) mHoveredItem.IsHovered = false;
                    tempItem.IsHovered = true;
                    this.Cursor = Cursors.Hand;
                }
            }
            mHoveredItem = tempItem;
            Invalidate();
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            if (mHoveredItem != null)
            {
                mHoveredItem.IsHovered = false;
                mHoveredItem = null;
            }
            this.Invalidate();
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            ListItem tempItem = HitTest(e.X, e.Y);
            if (tempItem == null)
            {
                if (mSelectedItem != null)
                    mSelectedItem.IsSelected = false;
            }
            else
            {
                if (mSelectedItem != tempItem)
                {
                    if (mSelectedItem != null) mSelectedItem.IsSelected = false;
                    tempItem.IsSelected = true;
                }
            }
            mSelectedItem = tempItem;
            Invalidate();
        }
        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (mSelectedItem != null)
            {
                if (mItemClickHandler != null)
                    mItemClickHandler(mSelectedItem);
            }
        }
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            base.OnMouseWheel(e);
            if (mVScrollBar != null)
            {
                mVScrollBar.RaiseMouseWheel(e.Delta);
            }
        }
        private void MVScrollBar_Scroll(object sender, ScrollEventArgs e)
        {
            mScrollValue = -e.NewValue;
            Refresh();
        }

        public void ClearItems()
        {
            mSelectedItem = null;
            mHoveredItem = null;
            mScrollValue = 0;
            mbaseTop = 5;
            mItems.Clear();
        }
    }
    public class ListItem
    {
        private string mText;
        private Rectangle mBounds;
        private Image mImage;
        private Color mForeColor=Color.White;
        private Color mBackColor=Color.FromArgb(100,100,100);
        private Color mSelectedForeColor=Color.Black;
        private Color mSelectedBackColor = Color.FromArgb(200,200,200);
        private Color mHoveredColor = Color.DarkGray;
        private bool mIsSelected=false;
        private bool mIsHovered=false;
        private int mImgSize = 32;
        private int mPaddingVer = 10, mPaddingHor = 10;
        private int mFontSize = 10;
        private int mIndex = -1;
        private string mGroup = null;
        private object mTag = null;
        public ListItem(string text)
        {
            mText = text;
        }
        public ListItem(string text, string group)
        {
            mText = text;
            mGroup = group;
        }
        public ListItem(string text,string group,Image img)
        {
            mText = text;
            mImage = img;
            mGroup = group;
        }
        public string Text { get => mText; set => mText = value; }
        public Rectangle Bounds { get => mBounds; set => mBounds = value; }
        public Image Image { get => mImage; set => mImage = value; }
        public bool IsSelected { get => mIsSelected; set => mIsSelected = value; }
        public bool IsHovered { get => mIsHovered; set => mIsHovered = value; }
        public int Index { get => mIndex; set => mIndex = value; }
        public string Group { get => mGroup; set => mGroup = value; }
        public object Tag { get => mTag; set => mTag = value; }

        public bool HitTest(int x,int y)
        {
            return this.Bounds.Contains(x, y);
        }
        public void Draw(Graphics g,int left,int top,int width)
        {
            Font txtFont = new Font("微软雅黑", mFontSize);
            Color bkColor = mBackColor;
            Color foreColor = mForeColor;
            if (mIsSelected)
            {
                bkColor = mSelectedBackColor;
                foreColor = mSelectedForeColor;
            }
            else if (mIsHovered)
                bkColor = mHoveredColor;

            
            SolidBrush foreBrush = new SolidBrush(foreColor);
            SolidBrush bgBrush = new SolidBrush(bkColor);
            
            if (mImage == null)
            {
                SizeF textSize = g.MeasureString(mText, txtFont, width - mPaddingHor * 2);
                mBounds = new Rectangle(left, top, width, (int)textSize.Height+mPaddingVer*2);
                Rectangle textRect = new Rectangle(mBounds.Left + mPaddingHor, mBounds.Top+mPaddingVer, mBounds.Width - mPaddingHor * 2, (int)textSize.Height);
                g.FillRectangle(bgBrush, mBounds);
                g.DrawString(mText, txtFont, foreBrush, textRect);
            }

            txtFont.Dispose();
            bgBrush.Dispose();
            foreBrush.Dispose();
        }

    }
}
