using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Text;
using System.Windows.Forms;

namespace Controls
{
    public class SuperPictureBox:Control
    {
        protected Image mImage;
        private BufferedGraphicsContext currentContext;
        private BufferedGraphics myBuffer;
        private PointF viewPortCenter;
        private float Zoom = 1.0f;

        private bool draging = false;
        private Point lastMouse;

        private EventHandler FrameChangedHandler=null;
        private Rectangle mCurExtent;
  
        protected StreamReader mCurStreamReader = null;

        public SuperPictureBox() : base()
        {
            this.Cursor = Cursors.Hand;
            setup(false);
        }
        protected override void OnPaintBackground(PaintEventArgs e)
        {
            
        }

        public Image Image
        {
            get
            {
                return mImage;
            }
            set
            {
                StopAnimate();
                mImage = value;
                if (mImage != null)
                    PrepareAnimate();
            }
        }


        private void setup(bool resetViewport)
        {
            if (myBuffer != null)
                myBuffer.Dispose();
            currentContext = BufferedGraphicsManager.Current;
            myBuffer = currentContext.Allocate(this.CreateGraphics(), this.DisplayRectangle);
            if (mImage != null)
            {
                if (resetViewport)
                {
                    SetViewPort(GetFullScreenRect());
                }

            }
            this.Focus();
            this.Invalidate();
        }

       
        private Rectangle GetFullScreenRect()
        {
           
            Rectangle imageRect = this.DisplayRectangle;

            int imgWidth = this.mImage.Width;
            int imgHeight = this.mImage.Height;
            float imgRatio = (float)imgWidth / (float)imgHeight;

            if (imgWidth > imageRect.Width)
            {
                imgWidth = imageRect.Width;
                imgHeight = (int)((float)imgWidth / imgRatio);
            }

            if (imgHeight > imageRect.Height)
            {
                imgHeight = imageRect.Height;
                imgWidth = (int)((float)imgHeight * imgRatio);
            }
            int offsetX = (mImage.Width - imgWidth) / 2;
            int offsetY = (mImage.Height - imgHeight) / 2;

            imageRect = new Rectangle(offsetX, offsetY, imgWidth, imgHeight);

            return imageRect;
        }

        private void PrepareAnimate()
        {
            if (mImage!=null&&ImageAnimator.CanAnimate(mImage)&&Parent!=null)
            {
                if (FrameChangedHandler == null)
                {
                    FrameChangedHandler = new EventHandler(OnFrameChanged);
                }
                ImageAnimator.Animate(mImage, FrameChangedHandler);

            }
        }

        private void OnFrameChanged(object sender, EventArgs e)
        {
        
            lock (mImage)
            {
                ImageAnimator.UpdateFrames();
               
            }
      
            Invalidate();
        }
        public void FullScreen()
        {
            SetViewPort(GetFullScreenRect());
            this.Invalidate();
        }

        private void StopAnimate()
        {
            if (mImage!=null&&ImageAnimator.CanAnimate(mImage)){
                ImageAnimator.StopAnimate(mImage, FrameChangedHandler);
            }
               
        }
        public void LoadImage(String url)
        {
            if (this.InvokeRequired)
            {
                CommonLib.SetStringValueCallback callback = new CommonLib.SetStringValueCallback(LoadImage);
                this.Invoke(callback, new object[] { url });
            }
            else
            {
                StopAnimate();
                CloseStreamReader();
                StreamReader reader = new StreamReader(url);

                mImage = Image.FromStream(reader.BaseStream);
                //reader.Close();

                setup(true);
                PrepareAnimate();
                mCurStreamReader = reader;
            }
            
        }
        private void SetViewPort(RectangleF worldCords)
        {
            if (worldCords.Height > worldCords.Width)
            {
                this.Zoom = worldCords.Width / mImage.Width;
            }
            else
                this.Zoom = worldCords.Height / mImage.Height;

            float zoomWidth = this.Width / this.Zoom;
            float zoomHeight = this.Height / this.Zoom;

            
            viewPortCenter = new PointF(worldCords.X + ((worldCords.Width ) / 2.0f), worldCords.Y + ((worldCords.Height) / 2.0f));

        }
        private void DrawImage(Graphics g)
        {
            if (this.mImage != null)
            {
                int padding = 0;
                Rectangle imageRect = new Rectangle((int)DisplayRectangle.Left + 1, (int)DisplayRectangle.Top + padding, (int)DisplayRectangle.Width - 1, (int)DisplayRectangle.Height - padding * 2);
                int imgWidth = this.mImage.Width;
                int imgHeight = this.mImage.Height;
                float imgRatio = (float)imgWidth / (float)imgHeight;
        
                if (imgWidth > imageRect.Width)
                {
                    imgWidth = imageRect.Width;
                    imgHeight = (int)((float)imgWidth / imgRatio);
                }

                if (imgHeight > imageRect.Height)
                {
                    imgHeight = imageRect.Height;
                    imgWidth = (int)((float)imgHeight * imgRatio);
                }
                int offsetX = (imageRect.Width - imgWidth) / 2;
                int offsetY = (imageRect.Height - imgHeight) / 2;

                imageRect = new Rectangle(imageRect.Left + offsetX, imageRect.Top + offsetY, imgWidth, imgHeight);
                g.DrawImage(mImage, imageRect);
                
                

            }
        }
        private void SetViewPort(Rectangle screenCords)
        {
            SetViewPort(new RectangleF(screenCords.Left,screenCords.Top,screenCords.Width,screenCords.Height));
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            if (mImage == null)
            {
               
                SolidBrush bgBrush = new SolidBrush(this.BackColor);
                e.Graphics.FillRectangle(bgBrush, this.DisplayRectangle);
               
                bgBrush.Dispose();
            }
            else
            {
                PaintImage();
            }
            
        }
        
        protected override void OnMouseWheel(MouseEventArgs e)
        {
            Zoom += Zoom * (e.Delta / 1200.0f); 
            if (e.Delta > 0) 
                viewPortCenter = new PointF(viewPortCenter.X + ((e.X - (Width / 2)) / (2 * Zoom)), viewPortCenter.Y + ((e.Y - (Height / 2)) / (2 * Zoom)));
            this.Invalidate();
        }
        protected override void OnMouseDown(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                draging = true;

            
        }
        
        protected override void OnMouseUp(MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left)
                draging = false;
        }

        protected override void OnMouseMove(MouseEventArgs e)
        {
            if (draging)
            {
                viewPortCenter = new PointF(viewPortCenter.X + ((lastMouse.X - e.X) / Zoom), viewPortCenter.Y + ((lastMouse.Y - e.Y) / Zoom));
                this.Invalidate();
            }
            //viewPointTemp= new PointF(viewPortCenter.X + ((e.X - (Width / 2)) / (2 * Zoom)), viewPortCenter.Y + ((e.Y - (Height / 2)) / (2 * Zoom)));
            lastMouse = e.Location;

        }

        protected void CloseStreamReader()
        {
            if (mCurStreamReader != null)
            {
                mCurStreamReader.Close();
                mCurStreamReader = null;
            }
        }
        public virtual void Clear()
        {
            CloseStreamReader();
        }
        private void PaintImage()
        {
        
            lock (mImage)
            {
                if (mImage != null)
                {
                    float widthZoomed = this.Width / Zoom;
                    float heigthZoomed = this.Height / Zoom;

                    //如果大于30000，DrawImage会崩溃
                    if (widthZoomed > 30000.0f)
                    {
                        Zoom = this.Width / 30000.0f;
                        widthZoomed = 30000.0f;
                    }
                    if (heigthZoomed > 30000.0f)
                    {
                        Zoom = this.Height / 30000.0f;
                        heigthZoomed = 30000.0f;
                    }

                    //不能再放大了，两个像素
                    if (widthZoomed < 2.0f)
                    {
                        Zoom = this.Width / 2.0f;
                        widthZoomed = 2.0f;
                    }
                    if (heigthZoomed < 2.0f)
                    {
                        Zoom = this.Height / 2.0f;
                        heigthZoomed = 2.0f;
                    }

                    float wz2 = widthZoomed / 2.0f;
                    float hz2 = heigthZoomed / 2.0f;
                    Rectangle drawRect = new Rectangle(
                        (int)(viewPortCenter.X - wz2),
                        (int)(viewPortCenter.Y - hz2),
                        (int)(widthZoomed),
                        (int)(heigthZoomed));

                    myBuffer.Graphics.Clear(this.BackColor); 
                    
                    myBuffer.Graphics.DrawImage(mImage, this.DisplayRectangle, drawRect, GraphicsUnit.Pixel);
                    myBuffer.Render(this.CreateGraphics());

                }
            }
           
        }
    }
}
