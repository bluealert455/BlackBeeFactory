using PropertyPages;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Runtime.InteropServices;
using System.Text;
using System.Windows.Forms;

namespace SuperControls
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
        protected Image mOldImage = null;
        protected StreamReader mCurStreamReader = null;

        protected Color mBorderColor = Color.FromArgb(54, 54, 54);
        protected int mBorderWidth = 1;
        public SuperPictureBox() : base()
        {
            this.Cursor = Cursors.Hand;
            setup(false);
        }
        ~SuperPictureBox()
        {
            
            currentContext.Dispose();
            myBuffer.Dispose();
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
            myBuffer = currentContext.Allocate(this.CreateGraphics(), GetDispRectangle());
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
        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            if(mImage!=null)
                setup(false);
        }
        private Rectangle GetDispRectangle()
        {
            Rectangle rect = this.DisplayRectangle;
            return new Rectangle(1, 1, rect.Width - 2, rect.Height - 2);
        }
        public void RotateImageRight()
        {
            mImage.RotateFlip(RotateFlipType.Rotate270FlipXY);
            Invalidate();
        }
        public void RotateImageLeft()
        {
            mImage.RotateFlip(RotateFlipType.Rotate90FlipXY);
            Invalidate();
        }

        public void FlipHor()
        {
            mImage.RotateFlip(RotateFlipType.Rotate180FlipY);
            Invalidate();
        }

        public void FlipVer()
        {
            mImage.RotateFlip(RotateFlipType.Rotate180FlipX);
            Invalidate();
        }

       
        private Rectangle GetFullScreenRect()
        {
           
            Rectangle imageRect = GetDispRectangle();

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
        public void FullExtent()
        {
            SetViewPort(GetFullScreenRect());
            this.Invalidate();
        }

        public void ZoomToRealSize()
        {
            this.Zoom = 1;
            
            Invalidate();
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

            
            viewPortCenter = new PointF(worldCords.X + ((worldCords.Width ) / 2.0f), worldCords.Y + ((worldCords.Height) / 2.0f));

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
                e.Graphics.FillRectangle(bgBrush, GetDispRectangle());
               
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
            this.Image = null;
            this.mOldImage = null;
        }
        private void PaintImage()
        {
        
            lock (mImage)
            {
                if (mImage != null)
                {
                    int dispWidth = (this.Width - 2 * mBorderWidth);
                    int dispHeight = (this.Height - 2 * mBorderWidth);
                    float widthZoomed = dispWidth / Zoom;
                    float heigthZoomed = dispHeight / Zoom;

                    //如果大于30000，DrawImage会崩溃
                    if (widthZoomed > 30000.0f)
                    {
                        Zoom = dispWidth / 30000.0f;
                        widthZoomed = 30000.0f;
                    }
                    if (heigthZoomed > 30000.0f)
                    {
                        Zoom = dispHeight / 30000.0f;
                        heigthZoomed = 30000.0f;
                    }

                    //不能再放大了，两个像素
                    if (widthZoomed < 2.0f)
                    {
                        Zoom = dispWidth / 2.0f;
                        widthZoomed = 2.0f;
                    }
                    if (heigthZoomed < 2.0f)
                    {
                        Zoom =dispWidth / 2.0f;
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
                    
                    myBuffer.Graphics.DrawImage(mImage, GetDispRectangle(), drawRect, GraphicsUnit.Pixel);
                    myBuffer.Render(this.CreateGraphics());

                }
            }
           
        }

        protected Dictionary<String, String> CreatePhotoMetadataDic()
        {
            Dictionary<String, String> dic = new Dictionary<string, string>();
            dic.Add("section1", "照片");
            dic.Add("作者", "");
            dic.Add("标题", "");
            dic.Add("拍摄时间", "");
            dic.Add("尺寸(像素)", "");
            dic.Add("分辨率(DPI)", "");

            dic.Add("section2", "相机参数");
            dic.Add("制造商", "");
            dic.Add("型号", "");
            dic.Add("相机软件", "");
            dic.Add("光圈值", "");
            dic.Add("曝光时间(秒)", "");
            dic.Add("ISO", "");
            dic.Add("曝光补偿", "");
            dic.Add("焦距", "");
            dic.Add("最大光圈", "");
            dic.Add("测光模式", "");
            dic.Add("目标距离", "");
            dic.Add("闪光灯模式", "");
            dic.Add("闪光灯能量", "");
            dic.Add("35mm焦距", "");

            dic.Add("section3", "位置");
            dic.Add("经度", "");
            dic.Add("纬度", "");
            dic.Add("高度", "");

            return dic;
        }

        protected Dictionary<int, String> CreateIdAndTitleDic()
        {
            Dictionary<int, String> dic = new Dictionary<int, string>();
            dic.Add(315, "作者");
            dic.Add(0x0320, "标题");
            dic.Add(306, "拍摄时间");

            dic.Add(0x010F, "制造商");
            dic.Add(0x0110, "型号");
            dic.Add(305, "相机软件");
            dic.Add(37378, "光圈值");
            dic.Add(0x829A, "曝光时间(秒)");
            dic.Add(34867, "ISO");
            dic.Add(41986, "曝光补偿");
            dic.Add(37386, "焦距");
            dic.Add(37381, "最大光圈");
            dic.Add(37383, "测光模式");
            dic.Add(37382, "目标距离");
            dic.Add(37385, "闪光灯模式");
            dic.Add(37387, "闪光灯能量");
            dic.Add(41989, "35mm焦距");

            dic.Add(4, "经度");
            dic.Add(2, "纬度");
            dic.Add(6, "高度");


            return dic;

        }


        public void AdjustContrast(int threshold)
        {
            Bitmap sourceBitmap = mImage as Bitmap;
            BitmapData sourceData = sourceBitmap.LockBits(new Rectangle(0, 0,
                                        sourceBitmap.Width, sourceBitmap.Height),
                                        ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);


            byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];
            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);
            sourceBitmap.UnlockBits(sourceData);

            double contrastLevel = Math.Pow((100.0 + threshold) / 100.0, 2);

            double blue = 0;
            double green = 0;
            double red = 0;


            for (int k = 0; k + 4 < pixelBuffer.Length; k += 4)
            {
                blue = ((((pixelBuffer[k] / 255.0) - 0.5) *
                            contrastLevel) + 0.5) * 255.0;


                green = ((((pixelBuffer[k + 1] / 255.0) - 0.5) *
                            contrastLevel) + 0.5) * 255.0;


                red = ((((pixelBuffer[k + 2] / 255.0) - 0.5) *
                            contrastLevel) + 0.5) * 255.0;


                if (blue > 255)
                { blue = 255; }
                else if (blue < 0)
                { blue = 0; }


                if (green > 255)
                { green = 255; }
                else if (green < 0)
                { green = 0; }


                if (red > 255)
                { red = 255; }
                else if (red < 0)
                { red = 0; }


                pixelBuffer[k] = (byte)blue;
                pixelBuffer[k + 1] = (byte)green;
                pixelBuffer[k + 2] = (byte)red;
            }


            Bitmap resultBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height);

            BitmapData resultData = resultBitmap.LockBits(new Rectangle(0, 0,
                                        resultBitmap.Width, resultBitmap.Height),
                                        ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);


            Marshal.Copy(pixelBuffer, 0, resultData.Scan0, pixelBuffer.Length);
            resultBitmap.UnlockBits(resultData);
            mImage = resultBitmap;

            Invalidate();

        }
        public void AdjustBrightness(int Value)
        {

            Bitmap TempBitmap = mImage as Bitmap;

            Bitmap NewBitmap = new Bitmap(TempBitmap.Width, TempBitmap.Height);

            Graphics NewGraphics = Graphics.FromImage(NewBitmap);

            float FinalValue = (float)Value / 100.0f; //255.0f;

            float[][] FloatColorMatrix ={

                    new float[] {1, 0, 0, 0, 0},

                    new float[] {0, 1, 0, 0, 0},

                    new float[] {0, 0, 1, 0, 0},

                    new float[] {0, 0, 0, 1, 0},

                    new float[] {FinalValue, FinalValue, FinalValue, 1, 1}
                };

            ColorMatrix NewColorMatrix = new ColorMatrix(FloatColorMatrix);

            ImageAttributes Attributes = new ImageAttributes();

            Attributes.SetColorMatrix(NewColorMatrix);

            NewGraphics.DrawImage(TempBitmap, new Rectangle(0, 0, TempBitmap.Width, TempBitmap.Height), 0, 0, TempBitmap.Width, TempBitmap.Height, GraphicsUnit.Pixel, Attributes);

            Attributes.Dispose();

            NewGraphics.Dispose();

            mImage = NewBitmap;
            Invalidate();
        }
        
        public void AdjustContrastAndBright(int contrastVal, int brightnessVal)
        {
            if (mOldImage == null) mOldImage = mImage.Clone() as Image;
            mImage = ApplyContrastAndBright(mOldImage as Bitmap, (float)contrastVal / 100.0f, (float)brightnessVal / 100.0f);
            Invalidate();
        }

        public void ResizeImage(int w, int h)
        {
            if (mImage == null) return;
            if (mOldImage == null) mOldImage = mImage.Clone() as Image;
            var destRect = new Rectangle(0, 0, w, h);
            var destImage = new Bitmap(w, h);
            /*
             * maintains DPI regardless of physical size -- may increase quality when reducing image dimensions or when printing
             * 
             * */
            destImage.SetResolution(mOldImage.HorizontalResolution, mOldImage.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    /*
                     * prevents ghosting around the image borders -- 
                     * naïve resizing will sample transparent pixels 
                     * beyond the image boundaries, but by mirroring 
                     * the image we can get a better sample (this setting is very noticeable)
                     * 
                     */
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(mOldImage, destRect, 0, 0, mOldImage.Width, mOldImage.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            mImage = destImage;
            Invalidate();
        }
        public static bool IsIndexedPixelFormat(PixelFormat fmt)
        {
            return fmt.ToString().EndsWith("Indexed");
        }
        public static Bitmap ApplyContrastAndBright(Bitmap srcBmp, float contrast, float bightness)
        {

            byte[] maptable = new byte[256];//查找表
            for (int i = 0; i < 256; i++)
            {
                float k = (float)Math.Tan((45 + 44.9999999 * contrast) / 180f * Math.PI);
                float y = (i - 127.5f * (1 - bightness)) * k + 127.5f * (1 + bightness);
                maptable[i] = (byte)Math.Min(255, Math.Max(0, y));
            }
            Bitmap tgtbmp;//
            if (IsIndexedPixelFormat(srcBmp.PixelFormat))//处理索引颜色的像素格式
            {
                tgtbmp = (Bitmap)srcBmp.Clone();
                ColorPalette cp = tgtbmp.Palette;
                for (int i = 0; i < tgtbmp.Palette.Entries.Length; i++)
                {
                    cp.Entries[i] = Color.FromArgb(maptable[cp.Entries[i].R], maptable[cp.Entries[i].G], maptable[cp.Entries[i].B]);
                }
                tgtbmp.Palette = cp;
            }
            else
            {
                tgtbmp = new Bitmap(srcBmp.Width, srcBmp.Height, srcBmp.PixelFormat);
                BitmapData srcdata = srcBmp.LockBits(new Rectangle(0, 0, srcBmp.Width, srcBmp.Height), ImageLockMode.ReadOnly, srcBmp.PixelFormat);
                BitmapData tgtdata = tgtbmp.LockBits(new Rectangle(0, 0, tgtbmp.Width, tgtbmp.Height), ImageLockMode.WriteOnly, srcBmp.PixelFormat);
                int pxsize = System.Drawing.Image.GetPixelFormatSize(srcBmp.PixelFormat) / 8;//像素大小(字节)
                bool isAlpha = System.Drawing.Image.IsAlphaPixelFormat(srcBmp.PixelFormat);
                int offset = srcdata.Stride - srcdata.Width * pxsize;
                unsafe
                {
                    byte* srcptr = (byte*)srcdata.Scan0;
                    byte* tgtptr = (byte*)tgtdata.Scan0;
                    for (int i = 0; i < srcdata.Height; i++)
                    {
                        for (int j = 0; j < srcdata.Width; j++)
                        {
                            for (int w = 0; w < pxsize; w++)
                            {
                                if (isAlpha && w == pxsize - 1)//Alpha通道直接复制，不处理（通常Alpha通道位于像素最后部件）
                                    *tgtptr = *srcptr;
                                else *tgtptr = maptable[*srcptr];
                                srcptr++; tgtptr++;
                            }
                        }
                        srcptr += offset;
                        tgtptr += offset;
                    }
                }
                srcBmp.UnlockBits(srcdata);
                tgtbmp.UnlockBits(tgtdata);
            }
            return tgtbmp;
        }

    }
}
