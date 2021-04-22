using CommonLib;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using ThirdPlugins;

namespace PhotoHome
{
    public partial class frmPicTest : Form
    {
        private List<ImageItem> mItems = new List<ImageItem>();
        Image[] imgs = new Image[] { PhotoHome.Properties.Resources.tb_open, PhotoHome.Properties.Resources.tb_paste };
        public frmPicTest()
        {
            InitializeComponent();
            this.Load += FrmPicTest_Load;
        }
        protected override CreateParams CreateParams
        {
            get
            {
                CreateParams param = base.CreateParams;
                if (!DesignMode)
                    param.ExStyle |= NativeMethods.WS_EX_LAYERED;
                return param;
            }
        }
        private void FrmPicTest_Load(object sender, EventArgs e)
        {
            //this.pictureBox1.Load("D:/wangshun/pictures/map/全省_打印.jpg");
            int left = 30;

            
            for(int i = 0; i < imgs.Length; i++)
            {
                ImageItem item = new ImageItem()
                {
                    index = i,
                    bounds = new Rectangle(left, 10, 48, 48)
                };
                left = item.bounds.Right + 10;
                mItems.Add(item);
            }
        }

        private void frmPicTest_KeyUp(object sender, KeyEventArgs e)
        {
        }

        private void btnGetAudioInfo_Click(object sender, EventArgs e)
        {
            
        }
        public void ShowMe(Form owner)
        {
            base.Show(owner);
            DrawBar();

        }
        private int mx, my;
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            mx = e.X;
            my = e.Y;

            DrawBar();
        }

        private void DrawBar()
        {
            Image bmp = new Bitmap(this.Width, this.Height); //PhotoHome.Properties.Resources.toolbarbg;
            Graphics g = Graphics.FromImage(bmp);
            g.Clear(Color.Transparent);
            DrawImages(g);
            UpdateLayered(bmp as Bitmap);
            bmp.Save("d:\\ll.png");
            bmp.Dispose();
            g.Dispose();

            
        }
        private void DrawImages(Graphics g)
        {
            Point p = new Point(mx, my);
            for (int i = 0; i < mItems.Count; i++)
            {
                ImageItem item = mItems[i];
                Rectangle bounds = item.bounds;
                if (bounds.Contains(p) && bounds.Width == 48)
                {
                    int d = (72 - 48) / 2;
                    bounds = new Rectangle(bounds.Left - d, bounds.Top, 72, 72);
                }
                g.DrawImage(imgs[item.index], bounds);

            }
        }
        private void frmPicTest_Load_1(object sender, EventArgs e)
        {

        }
        //protected override void OnPaint(PaintEventArgs e)
        //{
        //    Graphics g = e.Graphics;
        //    Rectangle r = this.ClientRectangle;
        //    Brush bgBrush = new SolidBrush(Color.White);
        //    g.FillRectangle(bgBrush, r);
        //    bgBrush.Dispose();
        //    DrawImages(e.Graphics);
        //}
        private void frmPicTest_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Close();
        }
        public void UpdateLayered(Bitmap face)
        {
            if (face == null)
                throw new ArgumentNullException("face");

            Graphics gClient = Graphics.FromHwnd(this.Handle);
            IntPtr dcClient = gClient.GetHdc();
            IntPtr hface = NativeMethods.CreateCompatibleBitmap(dcClient, face.Width, face.Height);
            IntPtr dcMem = NativeMethods.CreateCompatibleDC(dcClient);

            NativeMethods.SelectObject(dcMem, hface);

            Graphics gMem = Graphics.FromHdc(dcMem);
            gMem.DrawImage(face, 0, 0);
            //drawOnce = true;
            OnPaint(new PaintEventArgs(gMem, new Rectangle(Point.Empty, face.Size)));
            gMem.Flush();
            dcMem = gMem.GetHdc();
            NativeMethods.POINT ptLoc = new NativeMethods.POINT(Left, Top);
            NativeMethods.POINT pt = new NativeMethods.POINT(0, 0);
            NativeMethods.SIZE sz = new NativeMethods.SIZE(face.Width, face.Height);
            NativeMethods.BLENDFUNCTION blend = new NativeMethods.BLENDFUNCTION(NativeMethods.AC_SRC_OVER, 0, 255, NativeMethods.AC_SRC_ALPHA);
            NativeMethods.BOOL ret = NativeMethods.UpdateLayeredWindow(this.Handle, dcClient, ref ptLoc, ref sz, dcMem, ref pt, 0, ref blend, NativeMethods.ULW_ALPHA);
            gMem.ReleaseHdc();
            gClient.ReleaseHdc();
        }

    }

    public class ImageItem
    {
        public int index=-1;
        public bool hovered=false;
        public Rectangle bounds;
        
    }
}
