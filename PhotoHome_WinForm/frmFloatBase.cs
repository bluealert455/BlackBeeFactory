using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace PhotoHome
{
    public partial class frmFloatBase : Form
    {
        protected Color mBorderColor = Color.FromArgb(54, 54, 54);
        protected frmMain mMainForm = null;
        public frmFloatBase()
        {
            InitializeComponent();
            this.BackColor = Color.FromArgb(34, 34, 34);
        }
        public virtual frmMain MainForm
        {
            get
            {
                return mMainForm;
            }
            set
            {
                mMainForm = value;
            }
        }
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            this.Invalidate();
        }
        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);
            e.Control.KeyUp += Control_KeyUp;
        }

        protected void WhenKeyUp(KeyEventArgs e)
        {
            if (MainForm != null)
            {
                MainForm.WhenKeyUp(this, e);
            }
            else
            {
                frmMain main = CommonLib.Common.GetMainForm() as frmMain;
                if(main!=null)
                    main.WhenKeyUp(this, e);
            }
        }
        private void Control_KeyUp(object sender, KeyEventArgs e)
        {
            //WhenKeyUp(e);
        }

        public Color BorderColor
        {
            get
            {
                return mBorderColor;
            }
            set
            {
                if (mBorderColor != value)
                {
                    mBorderColor = value;
                    this.Invalidate();
                }
            }
        }

        protected override void OnKeyUp(KeyEventArgs e)
        {
            WhenKeyUp(e);
        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Pen borderPen = new Pen(mBorderColor,1);
            Rectangle borderRect = new Rectangle(1, 1, this.Width - 2, this.Height - 2);
            e.Graphics.DrawRectangle(borderPen, borderRect);
            borderPen.Dispose();
        }
    }
}
