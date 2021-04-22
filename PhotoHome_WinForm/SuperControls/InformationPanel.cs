using CommonLib;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace SuperControls
{
       
    public class InformationPanel:Control
    {
        List<TextLine> mLines = new List<TextLine>();
        private float mLinesHeight = 0;
        VScrollBarMobile mVScrollBar = null;
        float mbaseTop = 5;
        private int mScrollValue = 0;

        private BufferedGraphicsContext currentContext;
        private BufferedGraphics drawingBuffer=null;
        public InformationPanel():base()
        {
            currentContext = BufferedGraphicsManager.Current;
            
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            drawingBuffer = null;
           
            this.Invalidate();
        }
        public void AddLine(TextLine line)
        {
            mLines.Add(line);
        }

        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            
        }
        public void AddLines(List<TextLine> lines)
        {
            mLines.AddRange(lines);
        }

        private void DrawItems(Graphics gfx)
        {
            if(drawingBuffer==null)
                drawingBuffer = currentContext.Allocate(gfx, this.DisplayRectangle);

            lock (mLines)
            {
                drawingBuffer.Graphics.Clear(this.BackColor);
                if (mLines != null&&mLines.Count>0)
                {
                    float baseTop = mbaseTop + mScrollValue;
                    for (int i = 0; i < mLines.Count; i++)
                    {
                        TextLine textLine = mLines[i];
                        textLine.Draw(drawingBuffer.Graphics, baseTop, this.Width);
                        baseTop += textLine.TextRectangle.Height + 5;

                        mLinesHeight += textLine.TextRectangle.Height;
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

        public void ClearLines()
        {
            mScrollValue = 0;
            mbaseTop = 5;
            mLines.Clear();
        }
    }

    public class TextField
    {
        protected String mText;
        protected Color mTextColor;
        protected Font mFont;
        protected RectangleF mTextRectangle;
        protected bool mIsImage = false;

        public TextField(String text)
        {
            Initialize(text, Color.LightGray);
        }
        public TextField(String text, Color textColor, Color lineColor)
        {
            Initialize(text, textColor);

        }
        protected virtual void Initialize(String text, Color textColor)
        {
            mText = text;
            mTextColor = textColor;
            mFont = new Font("微软雅黑", 10);
        }

        public RectangleF TextRectangle
        {
            get
            {
                return mTextRectangle;
            }
        }
        public string Content
        {
            get
            {
                return mText;
            }
        }
        public bool IsImage
        {
            get
            {
                return mIsImage;
            }
            set
            {
                mIsImage = value;
            }
        }
        public virtual void Draw(Graphics gfx, float baseTop, float width, float left)
        {
            if (mIsImage)
            {
                Uri u = new Uri(mText);

                Image img = null;
                if (File.Exists(u.LocalPath))
                    img = Image.FromFile(u.LocalPath);
                else
                    img = PhotoHome_WinForm.Properties.Resources.music_default;
                float ratio = img.Width / img.Height;
                float w = width;
                float h = w / ratio;
                if (h > img.Height)
                {
                    h = img.Height;
                    w = h * ratio;
                }
                RectangleF rect = new RectangleF((width - w) / 2, baseTop, w, h);
                gfx.DrawImage(img, rect);
            }
            else
            {
                SizeF txtFldSize = gfx.MeasureString(mText, mFont, (int)width);
                mTextRectangle = new RectangleF(left, baseTop, txtFldSize.Width, txtFldSize.Height);
                SolidBrush fontBrush = new SolidBrush(mTextColor);
                gfx.DrawString(mText, mFont, fontBrush, mTextRectangle);
                fontBrush.Dispose();
            }
            
        }
    }

    public class SectionTitle : TextField
    {

        private Color mLineColor;
        
        public SectionTitle(String text) : base(text)
        {
            mLineColor = Color.FromArgb(100, 100, 100);
        }
        
        public SectionTitle(String text, Color textColor, Color lineColor) : base(text, textColor, lineColor)
        {
            mLineColor = lineColor;

        }
        
        protected override void Initialize(string text, Color textColor)
        {
            base.Initialize(text, textColor);
            mFont.Dispose();
            mFont = new Font("微软雅黑", 13);
        }

        public override void Draw(Graphics gfx, float baseTop, float width, float left)
        {
            base.Draw(gfx, baseTop, width, left);
            Pen sepPen = new Pen(mLineColor, 1);
            mTextRectangle.Height += 2;
            gfx.DrawLine(sepPen, mTextRectangle.Left, mTextRectangle.Bottom, mTextRectangle.Left +width-4, mTextRectangle.Bottom);
            mTextRectangle.Height += 2;
            sepPen.Dispose();
        }
    }

    public class TextLine
    {
        public TextField TitleField;
        public TextField ContentField;
        public int mIndent = 10;
        private List<TextLine> mSubLines;
        public TextLine(TextField titleField)
        {
            TitleField = titleField;
            ContentField = null;
            mIndent = 0;
        }
        public TextLine(TextField titleField, TextField contentField)
        {
            TitleField = titleField;
            ContentField = contentField;
            mIndent = 15;
        }

        public void AddSubLine(TextField lblFld, TextField contentFld)
        {

            AddSubLine(new TextLine(lblFld, contentFld));
        }

        public bool HasSublines
        {
            get
            {
                return (mSubLines != null && mSubLines.Count > 0);
            }
        }
        public void AddSubLine(TextLine line)
        {
            if (mSubLines == null)
                mSubLines = new List<TextLine>();
            mSubLines.Add(line);

        }
        public void ClearItems()
        {
            if (mSubLines != null)
            {
                mSubLines.Clear();
            }
        }

        public List<TextLine> SubLines
        {
            get
            {
                return mSubLines;
            }
        }

        public RectangleF TextRectangle
        {
            get
            {
                if (ContentField == null)
                {
                    return TitleField.TextRectangle;
                }
                else
                {
                    return RectangleF.Union(ContentField.TextRectangle, TitleField.TextRectangle);
                }
            }
        }
        public void Draw(Graphics gfx, float baseTop, float width)
        {
            if (TitleField.Content.EndsWith("_img:"))
            {
                ContentField.IsImage = true;
                ContentField.Draw(gfx,baseTop, width, mIndent);
            }
            else
            {
                float actualWidth = width - mIndent - 5;
                TitleField.Draw(gfx, baseTop, actualWidth, mIndent);
                if (ContentField == null) return;

                float actualBaseTop = baseTop;
                float actualLeft = TitleField.TextRectangle.Left + TitleField.TextRectangle.Width;
                if (TitleField.TextRectangle.Width > (width * 0.7))
                {
                    actualBaseTop = actualBaseTop + TitleField.TextRectangle.Height + 5;
                    actualLeft = mIndent;
                }

                ContentField.Draw(gfx, actualBaseTop, (width - actualLeft - 5), actualLeft);
            }
            
        }
    }
}
