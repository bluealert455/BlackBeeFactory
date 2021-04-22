using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SuperControls
{
    public class TreePanel:Control
    {
       
        private List<TreePanelNode> mNodes = null;
        
        private Color mSelectedForeColor = Color.Black;
        private Color mSelectedBackColor = Color.FromArgb(200, 200, 200);
        private Color mHoveredColor = Color.DarkGray;

        private VScrollBarMobile mVScrollBar = null;
        private HScrollBarMobile mHScrollBar = null;

        private BufferedGraphicsContext currentContext;
        private BufferedGraphics drawingBuffer = null;

        private int mIndent = 10;
        private int mNodePartInterval = 5;
        private ImageList mImgList = null;
        private int mBaseTop = 5;
        private int mTotalWidth=0, mTotalHeight=0;
        private int mPlusImageSize = 16;
        private int mNodeInterval = 3;
        private int mScrollingTop = 3;
        private int mScrollingLeft = 0;
        public TreePanel() : base()
        {
            mNodes = new List<TreePanelNode>();
            currentContext = BufferedGraphicsManager.Current;
            this.Font = new Font("微软雅黑", 9);
            this.ForeColor = Color.White;
            this.BackColor= Color.FromArgb(100, 100, 100);
            mScrollingTop = mNodeInterval;
        }
        ~TreePanel()
        {
            if (currentContext != null)
            {
                currentContext.Dispose();
                currentContext = null;
            }
            if (drawingBuffer != null)
            {
                drawingBuffer.Dispose();
                drawingBuffer = null;
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if (drawingBuffer != null)
            {
                drawingBuffer.Dispose();
                drawingBuffer = null;
            }
            
        }

        [Browsable(false)]
        [DesignerSerializationVisibility(DesignerSerializationVisibility.Hidden)]
        public List<TreePanelNode> Nodes
        {
            get
            {
                return mNodes;
            }
            set
            {
                mNodes = value;
            }
        }

        public ImageList ImageList
        {
            get
            {
                return mImgList;

            }
            set
            {
                mImgList = value;
            }
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (drawingBuffer == null)
                drawingBuffer = currentContext.Allocate(e.Graphics, this.DisplayRectangle);

            lock (this.mNodes)
            {
                drawingBuffer.Graphics.Clear(this.BackColor);
            
                if (this.Nodes.Count > 0)
                {
                    DrawNodes(drawingBuffer.Graphics);
                }

                drawingBuffer.Render(e.Graphics);
            }

            if (this.mTotalHeight > this.Height)
            {
                if (mVScrollBar == null) mVScrollBar = new VScrollBarMobile();
                mVScrollBar.Maximum = this.mTotalHeight;

                this.Controls.Add(mVScrollBar);
            }
            else
            {
                if(mVScrollBar != null)
                {
                    this.Controls.Remove(mVScrollBar);
                    mVScrollBar = null;
                }
            }

            if (this.mTotalWidth > this.Width)
            {
                if (mHScrollBar == null) mHScrollBar = new HScrollBarMobile();
                mHScrollBar.Maximum = this.mTotalWidth;
                this.Controls.Add(mHScrollBar);
            }
            else
            {
                if (mHScrollBar != null)
                {
                    this.Controls.Remove(mHScrollBar);
                    mHScrollBar = null;
                }
            }

        }
        
        private void DrawNodes(Graphics g)
        {
            SolidBrush textBrush = new SolidBrush(this.ForeColor);
            for(int i = 0; i < this.Nodes.Count; i++)
            {
                DrawNodesByLevel(0, g, this.Nodes[i], mScrollingTop, textBrush);
            }
            textBrush.Dispose();
        }
        private void DrawNodesByLevel(int level,Graphics g,TreePanelNode node,int curTop,SolidBrush txtBrush)
        {
            int left =mScrollingLeft+level * mIndent;
            SizeF nodeSize = g.MeasureString(node.Text,this.Font);

            int w = left + mPlusImageSize + mNodePartInterval + mPlusImageSize + mNodePartInterval + (int)nodeSize.Width;
            
            int curLeft = left;
            if (node.Nodes.Count > 0)
            {
                if (node.IsExpanded)
                    g.DrawImage(PhotoHome_WinForm.Properties.Resources.arrow_down_16, curLeft, curTop);
                else
                    g.DrawImage(PhotoHome_WinForm.Properties.Resources.arrow_right_16, curLeft, curTop);
                curLeft = curLeft + mPlusImageSize + mNodePartInterval;
                w = left + mPlusImageSize + mNodePartInterval + mPlusImageSize + mNodePartInterval + (int)nodeSize.Width;
            }
            else
            {
                w = left + mPlusImageSize + mNodePartInterval + (int)nodeSize.Width;
            }

            if (mTotalWidth < w) mTotalWidth = w;

            if (this.ImageList != null && this.ImageList.Images[node.ImageKey] != null)
            {
                g.DrawImage(this.ImageList.Images[node.ImageKey], curLeft, curTop);
            }
            curLeft = curLeft + 16 + mNodePartInterval;
            g.DrawString(node.Text,this.Font, txtBrush, curLeft, curTop);
            int txtHeight = (int)nodeSize.Height;
            
            node.Bounds = new Rectangle(left, curTop, w, mPlusImageSize > txtHeight ? mPlusImageSize : txtHeight);

            int newTop = curTop +node.Bounds.Height+mNodeInterval;
            mTotalHeight = newTop;
            if (node.IsExpanded&&node.Nodes.Count>0)
            {
                for(int i = 0; i < node.Nodes.Count; i++)
                {
                    int newLevel = level + 1;
                    DrawNodesByLevel(newLevel, g, node.Nodes[i], newTop, txtBrush);
                }
            }

        }
        
    }
    [Serializable]
    public class TreePanelNode
    {
      
        private List<TreePanelNode> mNodes = null;
        private String mText = null;
        private Object mTag = null;

        private string mImageKey = null;
        private bool mIsExpanded = false;
        private Rectangle mBounds = Rectangle.Empty;
        public TreePanelNode()
        {
            mNodes = new List<TreePanelNode>();
        }
        public TreePanelNode(String text)
        {
            mNodes = new List<TreePanelNode>();
            mText = text;
        }

        public List<TreePanelNode> Nodes
        {
            get
            {
                return mNodes;
            }
            set
            {
                mNodes = value;
            }
        }

        public string Text
        {
            get
            {
                return mText;
            }
            set
            {
                mText = value;
            }
        }
        public object Tag
        {
            get
            {
                return mTag;
            }
            set
            {
                mTag = value;
            }
        }
        public string ImageKey
        {
            get
            {
                return mImageKey;
            }
            set { mImageKey = value; }
        }
        public bool IsExpanded
        {
            get
            {
                return mIsExpanded;
            }
            set
            {
                mIsExpanded = value;
            }
        }
        public Rectangle Bounds
        {
            get
            {
                return mBounds;
            }
            set { mBounds = value; }
        }
    }
}
