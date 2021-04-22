using CommonLib;
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
    public partial class frmExtendForm : Form
    {
        protected Color mBorderColor = Color.FromArgb(54, 54, 54);
        protected int mAnchorHeight = 0;
        public frmExtendForm()
        {
            DoubleBuffered = true;

            InitializeComponent();

            //BackColor = Color.Transparent;

            SetRegion();

            SetStyle(ControlStyles.AllPaintingInWmPaint | ControlStyles.OptimizedDoubleBuffer | ControlStyles.ResizeRedraw | ControlStyles.SupportsTransparentBackColor | ControlStyles.UserPaint, true);
            SetStyle(ControlStyles.Opaque, false);
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            DrawBorder(e.Graphics);
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            SetRegion();
        }
        protected virtual void DrawBorder(Graphics g)
        {
            SmoothingMode sm = g.SmoothingMode;
            g.CompositingQuality = CompositingQuality.HighQuality;
            g.SmoothingMode = SmoothingMode.HighQuality;
            g.InterpolationMode = InterpolationMode.HighQualityBilinear;
            Rectangle clientRect = this.ClientRectangle;
            GraphicsPath regionPath = GeometryAPI.CreateRoundRect(clientRect, GeometryAPI.RoundBorderRadius);
            //this.Region = new Region(regionPath);
            SolidBrush bgBrush = new SolidBrush(Color.Black);
            g.FillPath(bgBrush, regionPath);
            Rectangle borderRect = new Rectangle(clientRect.Left + 1, clientRect.Top, clientRect.Width - 3, clientRect.Height);
            GraphicsPath borderPath = GeometryAPI.CreateRoundRect(borderRect, GeometryAPI.RoundBorderRadius);
            Pen borderPen = new Pen(Color.Red, 1);

            g.DrawPath(borderPen, borderPath);
            borderPen.Dispose();
            regionPath.Dispose();
            borderPath.Dispose();
            bgBrush.Dispose();

            g.SmoothingMode = sm;
        }

        protected virtual Region CreateRegion()
        {
            Region region = null;
            // nW and nH are just shorthand for the width and height of the form
            int nW = this.Width;
            int nH = this.Height;

            int nOuter = mAnchorHeight;
            int nInner = mAnchorHeight + GeometryAPI.RoundBorderRadius;


            #region generate the outer frame (m_rOuterFrame) which becomes the shape of the balloon form
            Size pSize = new Size(nW - (nInner + nInner), nH - (nOuter + nOuter));
            Point pLoc = new Point(nInner, nOuter);
            Rectangle aRect = new Rectangle(pLoc, pSize);
        
            region = new Region(aRect);

            // generate 2nd pass region for the outer frame, starting to form the rounded edges
            pSize.Width += 4;
            pSize.Height -= 2;
            pLoc.X -= 2;
            pLoc.Y += 1;
            region.Union(new Rectangle(pLoc, pSize));

            // generate 3rd pass for the outer frame, developing rounded edges
            pSize.Width += 2;
            pSize.Height -= 2;
            pLoc.X -= 1;
            pLoc.Y += 1;
            region.Union(new Rectangle(pLoc, pSize));

            // generate 4th pass for the outer frame, developing rounded edges
            pSize.Width += 2;
            pSize.Height -= 2;
            pLoc.X -= 1;
            pLoc.Y += 1;
            region.Union(new Rectangle(pLoc, pSize));

            // generate 5th pass for the outer frame, completing the rounded edges
            pSize.Width += 2;
            pSize.Height -= 4;
            pLoc.X -= 1;
            pLoc.Y += 2;
            region.Union(new Rectangle(pLoc, pSize));
            #endregion

            return region;
        }
        protected virtual void SetRegion()
        {

            if (this.Region != null)
            {
                this.Region.Dispose();
                this.Region = null;
            }

            this.Region = CreateRegion();
            // force a complete redraw
            this.Invalidate();
        }

        private void frmExtendForm_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Close();
        }
    }
}
