using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Controls
{
    public class ThumbnailList : Control
    {
        private int mThumbnailWidth = 200, mThumbnailHeight = 180;
        private int mThumbSelWidth = 220, mThumbSelHeight = 200;
        private int mPhotoLimit = 500;
        private int mInterval = 10;
        private CommonLib.SetIntValueCallback mThumbnailClick = null;
        private ThumbPictureBox mCurrentPicBox = null;
        private List<String> mFileNames = null;
        private Dictionary<string, string> mDicFileNames = null;

        private Panel mMainPanel = null;
        private Dictionary<int, ThumbPictureBox> mDicPicBoxCache = new Dictionary<int, ThumbPictureBox>();
        private int mMaxCacheCnt = 20;

        private Timer mWheelTimer = null;
        private int mWheelStopCnt = 0;
        protected int mCurrentIndex = -1;
        protected int mStartValue = 0;
        public ThumbnailList() : base()
        {
            this.Padding = new Padding(0, 30, 0, 30);
            mMainPanel = new Panel();
            this.BackColor = Color.Black;
            mMainPanel.BackColor = this.BackColor;
            mMainPanel.Padding = this.Padding;
            this.Controls.Add(mMainPanel);

            mWheelTimer = new Timer();
            mWheelTimer.Interval = 100;
            mWheelTimer.Tick += MWheelTimer_Tick;
        }

        private void MWheelTimer_Tick(object sender, EventArgs e)
        {
            mWheelStopCnt++;
            if (mWheelStopCnt == 5)
            {
                mWheelTimer.Stop();
                MHScroll_OnScrollEnd(null, null);
            }
        }

        protected override void OnSizeChanged(EventArgs e)
        {
            base.OnSizeChanged(e);
            mMainPanel.Top = 0;
            mMainPanel.Height = this.Height;
            mMainPanel.Left = 0;
            mMainPanel.Width = this.Width;

        }
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
            mThumbnailHeight = this.Height - mMainPanel.Padding.Top - mMainPanel.Padding.Bottom;
            if (mThumbnailHeight < 30)
            {
                mThumbnailHeight = 30;
            }

            double ratio = 4.0/3.0;

            mThumbnailWidth =(int)((double)mThumbnailHeight* ratio);
            mThumbSelWidth = mThumbnailWidth + mInterval * 2;
            mThumbSelHeight = mThumbnailHeight + mInterval * 2;
        }

        public event CommonLib.SetIntValueCallback OnThumbnailClick
        {
            add
            {
                lock (this)
                {
                    mThumbnailClick += value;
                }
            }
            remove
            {
                lock (this)
                {
                    if (mThumbnailClick != null)
                        mThumbnailClick -= value;
                }
            }
        }

        protected override void OnMouseWheel(MouseEventArgs e)
        {
            mWheelStopCnt = 0;
            if(mWheelTimer.Enabled==false)
                mWheelTimer.Start();
            if (mHScroll != null)
            {
                mHScroll.RaiseMouseWheel(e.Delta);
            }
        }
        
        public Image GenerateThumbnail(Image sourceImage)
        {
            if (mPhotoLimit > sourceImage.Height && mPhotoLimit > sourceImage.Width)
            {
                return sourceImage.Clone() as Image;
            }

            int sourceWidth = sourceImage.Width, sourceHeight = sourceImage.Height;
            int targetWidth =mThumbnailWidth, targetHeight=mThumbnailHeight;
            int sourceRatio = sourceWidth / sourceHeight;
            targetWidth = mThumbnailHeight * sourceRatio;

            if (targetWidth > mThumbnailWidth)
            {
                targetWidth = mThumbnailWidth;
                targetHeight = targetWidth / sourceRatio;

            }
            Image targetImage = new Bitmap(targetWidth, targetHeight);
            Graphics sourceGraphics = Graphics.FromImage(sourceImage);
            
            Graphics destGraphics = Graphics.FromImage(targetImage);


            IntPtr destDC = destGraphics.GetHdc();
            IntPtr destCDC =CommonLib.WindowAPI.CreateCompatibleDC(destDC);
            IntPtr oldDest = CommonLib.WindowAPI.SelectObject(destCDC, IntPtr.Zero);

            IntPtr sourceDC = sourceGraphics.GetHdc();
            IntPtr sourceCDC = CommonLib.WindowAPI.CreateCompatibleDC(sourceDC);
            IntPtr sourceHB = ((Bitmap)sourceImage).GetHbitmap();
            IntPtr oldSource = CommonLib.WindowAPI.SelectObject(sourceCDC, sourceHB);

            int success = CommonLib.WindowAPI.StretchBlt(destDC, 0, 0, Width, Height, sourceCDC, 0, 0, sourceWidth, sourceHeight, (int)CommonLib.TernaryRasterOperations.SRCCOPY);

            CommonLib.WindowAPI.SelectObject(destCDC, oldDest);
            CommonLib.WindowAPI.SelectObject(sourceCDC, oldSource);

            CommonLib.WindowAPI.DeleteObject(destCDC);
            CommonLib.WindowAPI.DeleteObject(sourceCDC);
            CommonLib.WindowAPI.DeleteObject(sourceHB);

            destGraphics.ReleaseHdc();
            sourceGraphics.ReleaseHdc();
            return targetImage;
        }

        private int GetThumbnailCount()
        {
            int cnt = this.Width / (mThumbnailWidth + mInterval);
            cnt += 2;
            return cnt;
        }
        public bool ThumbnailCallback()
        {
            return false;
        }
        public void WhenFileChanged(FileChangedEvantArgs args)
        {
            
            
            mCurrentIndex = args.CurrtentPos;
            SwitchToFile();
        }


        public void Clear()
        {
            if (this.InvokeRequired == true)
            {
                CommonLib.VoidAndNonParamHandler callback = new CommonLib.VoidAndNonParamHandler(Clear);
                this.Invoke(callback);
            }
            else
            {
                mMainPanel.Controls.Clear();
                mDicPicBoxCache.Clear();
            }
            
        }
        public void WhenFilesRenamed(FileChangedEvantArgs args)
        {
            WhenFileSelected(args);
        }
        public void WhenFilesDeleted(FileChangedEvantArgs args)
        {
            WhenFileSelected(args);
        }
        public void WhenFilesCreated(FileChangedEvantArgs args)
        {
            WhenFileSelected(args);
        }
        public void WhenFileSelected(FileChangedEvantArgs args)
        {
            if (this.InvokeRequired == true)
            {
                FileSelectedHandler callback = new FileSelectedHandler(WhenFileSelected);
                this.Invoke(callback,args);
            }
            else
            {
                Clear();
                mFileNames = args.CurrentFileNames;
                mCurrentIndex = args.CurrtentPos;

                BuildThumbList();
            }
        }

        private void SwitchToFile()
        {
            ThumbPictureBox picBoxTemp = null;
            if (mDicPicBoxCache.ContainsKey(mCurrentIndex) == true)
            {
                picBoxTemp = mDicPicBoxCache[mCurrentIndex];
            }
            else
            {
                int newBoxLeft = CalcThumbBoxLeft(mCurrentIndex);
                picBoxTemp = CreateThumbPicBox(newBoxLeft, mCurrentIndex, CreateThumbFromFile(mFileNames[mCurrentIndex]));
                this.mMainPanel.Controls.Add(picBoxTemp);
                if (mDicPicBoxCache.ContainsKey(mCurrentIndex) == false)
                    mDicPicBoxCache.Add(mCurrentIndex, picBoxTemp);
            }
            
            if ((mBeginIndex - 1) == mCurrentIndex)
                mBeginIndex = mCurrentIndex;
            else if ((mEndIndex + 1) == mCurrentIndex)
                mEndIndex = mCurrentIndex;


            SwitchCurrentBox(picBoxTemp);
        }

        private void SwitchCurrentBox(ThumbPictureBox picBox)
        {
            if (mCurrentPicBox != null)
                mCurrentPicBox.IsCurrent = false;
            picBox.IsCurrent = true;
            mCurrentPicBox = picBox;

            EnsurePicBoxVisible(mCurrentPicBox);
        }
        private int CalcThumbBoxLeft(int index)
        {
            int iThumbWidth = (mThumbnailWidth + mInterval);
            return mInterval + index * iThumbWidth;
        }
        private void EnsurePicBoxVisible(ThumbPictureBox picBox)
        {
            int hiddenSize = GetBoxHiddenSize(picBox);
            mMainPanel.Left += hiddenSize;
            mHScroll.Value= mCurrentIndex * (mThumbnailWidth + mInterval);
        }

        private int GetBoxHiddenSize(ThumbPictureBox picBox)
        {
            int panelLeft =Math.Abs(mMainPanel.Left);
            int boxLeft = picBox.Left - mInterval;
            
            if (panelLeft > boxLeft)
            {
                //左边被隐藏，返回正值
                return panelLeft - boxLeft;
            }
            else
            {
                int boxRight = picBox.Right + mInterval;
                int panelRight = panelLeft + this.Width;
                if (boxRight > panelRight)
                {
                    //右边被隐藏返回负值
                    return panelRight - boxRight;
                }
            }
            return 0;

        }
        private int CalcTotalWidth()
        {
            int iThumbWidth = (mThumbnailWidth + mInterval);
            return (mFileNames.Count* iThumbWidth +mInterval);
        }
        private void BuildThumbList()
        {

            int iThumbnailCnt = GetThumbnailCount();
            mStartValue = mCurrentIndex * (mThumbnailWidth + mInterval);
            //在缩略图列表中，当前选中的尽量居中显示
            int leftAndRightCnt = iThumbnailCnt / 2;

            int beginIndex = mCurrentIndex - leftAndRightCnt;
            int endIndex = mCurrentIndex + leftAndRightCnt;
            if (beginIndex < 0)
            {
                beginIndex = 0;
                endIndex = endIndex - beginIndex;
            }
            if (endIndex > (mFileNames.Count - 1)) endIndex = mFileNames.Count - 1;


            mBeginIndex = beginIndex;
            mEndIndex = endIndex;


            int iCnt = 0;
            int iThumbWidth = (mThumbnailWidth + mInterval);
            int iTotalThumbWidth = iThumbnailCnt * iThumbWidth;
            int iTotalWidth = CalcTotalWidth();

            int iBeginLeft = CalcThumbBoxLeft(beginIndex);
            int iOffset = (iTotalThumbWidth - this.Width) / 2;
            this.mMainPanel.Left = -iBeginLeft - iOffset;
            this.mMainPanel.Width = iTotalWidth;
            if (beginIndex == 0)
                iBeginLeft = mInterval;
            
            for (int j = beginIndex; j <= endIndex; j++)
            {
                Image thumbImg = CreateThumbFromFile(mFileNames[j]);
                ThumbPictureBox thumbPicBox = CreateThumbPicBox(iBeginLeft + iCnt * iThumbWidth, j, thumbImg);

                if (j == mCurrentIndex)
                {
                    thumbPicBox.IsCurrent = true;
                    mCurrentPicBox = thumbPicBox;
                   
                }
        
                mDicPicBoxCache.Add(j, thumbPicBox);
                mMainPanel.Controls.Add(thumbPicBox);
                iCnt++;
            }
            mCurrentPicBox.BringToFront();
            CreateScrollBar();
        }
        HScrollBarMobile mHScroll = null;
        private void CreateScrollBar()
        {
            int totalWidth = CalcTotalWidth();
            if (totalWidth > this.Width&&mHScroll==null)
            {
                mHScroll = new HScrollBarMobile();
                mHScroll.Width = this.Width - 2;
                mHScroll.Height = 10;
                mHScroll.Top = 2;
                mHScroll.Left = 1;
                mHScroll.SmallChange = mMainPanel.Width / 100;
                mHScroll.OnScroll += HScroll_OnScroll;
                mHScroll.OnScrollEnd += MHScroll_OnScrollEnd;
                
                this.Controls.Add(mHScroll);
                mHScroll.BringToFront();
            }
            mHScroll.Maximum = mMainPanel.Width;
            mHScroll.Value = mCurrentIndex * (mThumbnailWidth + mInterval);

        
        }

        private Image CreateThumbFromFile(String fileName)
        {

            StreamReader reader = new StreamReader(fileName);
            Image imgTemp = (Bitmap)Bitmap.FromStream(reader.BaseStream);
            reader.Close();
            return CreateThumbImage(imgTemp);
        }
        private Image CreateThumbImage(Image srcImg)
        {
            Image.GetThumbnailImageAbort abortCallback = new Image.GetThumbnailImageAbort(ThumbnailCallback);
            float sourceWidth = srcImg.Width, sourceHeight = srcImg.Height;
            float targetWidth = mThumbSelWidth, targetHeight = mThumbSelHeight;
            float sourceRatio = sourceWidth / sourceHeight;
            targetWidth = mThumbSelHeight * sourceRatio;
            if (targetWidth > mThumbSelWidth)
            {
                targetWidth = mThumbSelWidth;
                targetHeight = targetWidth / sourceRatio;

            }
            return  srcImg.GetThumbnailImage((int)targetWidth, (int)targetHeight, abortCallback, IntPtr.Zero);
        }
        private ThumbPictureBox CreateThumbPicBox(int left,int fileIndex,Image thumbImg)
        {
            
            ThumbPictureBox thumbPicBox = new ThumbPictureBox();
            thumbPicBox.Image = thumbImg;
            thumbPicBox.BackColor = Color.DimGray;
            thumbPicBox.SizeMode = PictureBoxSizeMode.Zoom;
            thumbPicBox.Top = mMainPanel.Padding.Top;
            thumbPicBox.Left = left;
            thumbPicBox.Tag = fileIndex;
            thumbPicBox.MouseClick += ThumbPicBox_MouseClick;

            thumbPicBox.Width = mThumbnailWidth;
            thumbPicBox.Height = mThumbnailHeight;

            return thumbPicBox;
        }
        private void LoadScrollImages()
        {
            Image.GetThumbnailImageAbort abortCallback = new Image.GetThumbnailImageAbort(ThumbnailCallback);
            for (int j = mBeginIndex; j <= mEndIndex; j++)
            {
                if (mDicPicBoxCache[j].Image != null)
                    continue;
                Image thumbnailImg = null;
                Image imgTemp = null;

                imgTemp = Image.FromFile(mFileNames[j]);

                float sourceWidth = imgTemp.Width, sourceHeight = imgTemp.Height;
                float targetWidth = mThumbSelWidth, targetHeight = mThumbSelHeight;
                float sourceRatio = sourceWidth / sourceHeight;
                targetWidth = mThumbSelHeight * sourceRatio;
                if (targetWidth > mThumbSelWidth)
                {
                    targetWidth = mThumbSelWidth;
                    targetHeight = targetWidth / sourceRatio;

                }
                thumbnailImg = imgTemp.GetThumbnailImage((int)targetWidth, (int)targetHeight, abortCallback, IntPtr.Zero);
                ThumbPictureBox thumbPicBox = mDicPicBoxCache[j];

                thumbPicBox.Image = thumbnailImg;
            }
        }
        private void MHScroll_OnScrollEnd(object sender, ScrollEventArgs e)
        {
            LoadScrollImages();
        }
        private int mBeginIndex = 0;
        private int mEndIndex = 0;
        private void HScroll_OnScroll(object sender, ScrollEventArgs e)
        {
            int newPos = e.NewValue;
            int delta = newPos - e.OldValue;
           
            float scrollRatio = (float)mHScroll.Value / (float)mHScroll.Maximum;
            int deltaLeft = (int)((float)this.Width * scrollRatio);
          
            if (delta<0)
            {
                if (mMainPanel.Left >= 0) return;
                mMainPanel.Left = (-e.NewValue) + deltaLeft;
                int temp = mBeginIndex - 1;
                int preLeft = (mInterval + temp * (mInterval + mThumbnailWidth));
                if ((-mMainPanel.Left) < preLeft)
                {
                    if (mDicPicBoxCache.ContainsKey(temp)==false)
                    {
                        ThumbPictureBox picTemp = CreateThumbPicBox(preLeft, temp, null);
                       
                        mMainPanel.Controls.Add(picTemp);
                        mDicPicBoxCache.Add(temp, picTemp);
                        
                    }
                    else
                    {
                        //mMainPanel.Controls.Add(mDicPicBoxCache[temp]);
                    }
                    mBeginIndex--;
                    mEndIndex--;
                }
            }
            else
            {
                mMainPanel.Left = (-e.NewValue) + deltaLeft - delta;
                int nextRight = (-mMainPanel.Left) + this.Width;
                PictureBox endPicBox = mDicPicBoxCache[mEndIndex];
                int temp = mEndIndex+1;
                
                if (nextRight > endPicBox.Right)
                {
                    if (mDicPicBoxCache.ContainsKey(temp) == false)
                    {
                        ThumbPictureBox picTemp = CreateThumbPicBox(endPicBox.Right+mInterval, temp, null);
                        mMainPanel.Controls.Add(picTemp);
                        mDicPicBoxCache.Add(temp, picTemp);
                    }
                    else
                    {
                        //mMainPanel.Controls.Add(mDicPicBoxCache[temp]);
                    }
                    mEndIndex++;
                    mBeginIndex++;
                }
                
            }

            if (mBeginIndex < 0) mBeginIndex = 0;
            if (mEndIndex > (mFileNames.Count - 1)) mEndIndex = mFileNames.Count - 1;
            mStartValue = mStartValue + delta;
        }

        private void ThumbPicBox_MouseClick(object sender, MouseEventArgs e)
        {
            ThumbPictureBox tempPic = sender as ThumbPictureBox;
            int selIndex = (int)tempPic.Tag;

            int curIndex = (int)mCurrentPicBox.Tag;
            if (selIndex == curIndex) return;

           
            if (mThumbnailClick != null)
            {
                mThumbnailClick((int)tempPic.Tag);
            }
            mCurrentPicBox.IsCurrent = false;
            tempPic.IsCurrent = true;
            mCurrentPicBox = tempPic;


        }
    }
}
