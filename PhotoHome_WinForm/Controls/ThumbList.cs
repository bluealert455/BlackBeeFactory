using CommonLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Controls
{
    public class ThumbList : Control
    {
        private int mThumbnailWidth = 200, mThumbnailHeight = 180;
        private int mThumbSelWidth = 220, mThumbSelHeight = 200;
        private int mPhotoLimit = 500;
        private int mInterval = 10;
        private CommonLib.SetIntValueCallback mThumbnailClick = null;
        private ThumbPictureBox mCurrentPicBox = null;
      
        private Panel mMainPanel = null;
        private Dictionary<int, ThumbPictureBox> mDicPicBoxCache = new Dictionary<int, ThumbPictureBox>();
        private int mMaxCacheCnt = 20;

        private Timer mWheelTimer = null;
        private int mWheelStopCnt = 0;
      
        protected int mStartValue = 0;
        private List<String> mlFileNamesInCurDir = null;
        private int mCurFilePos = -1;
    
        private string mSelectTargetFolder = null;
        private SuperDataPanel mDataPanel = null;

        private String mCurrentPath, mCurFileName, mCurName;

        public ThumbList() : base()
        {
            this.Padding = new Padding(0, 30, 0, 30);
            mMainPanel = new Panel();
        
            mMainPanel.BackColor = this.BackColor;
            mMainPanel.Padding = this.Padding;
            this.Controls.Add(mMainPanel);

            mWheelTimer = new Timer();
            mWheelTimer.Interval = 100;
            mWheelTimer.Tick += MWheelTimer_Tick;
        }

        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                mMainPanel.BackColor = value;
                base.BackColor = value;
            }
        }
        public int CurrentFileIndex
        {
            get
            {
                return mCurFilePos;
            }
        }
        public List<string> CurrentFileNames
        {
            get
            {
                return mlFileNamesInCurDir;
            }
        }
  
        public SuperDataPanel DataPanel
        {
            get
            {
                return mDataPanel;
            }
            set
            {
                mDataPanel = value;
            }
        }

        public string SelectTargetFolder
        {
            get
            {
                return mSelectTargetFolder;
            }
            set
            {
                if (mSelectTargetFolder != value)
                {
                    mSelectTargetFolder = value;
                    UpdateSelectedBox();


                }
                
            }
        }

        public void UpdateSelectedBox()
        {
            if (mDicPicBoxCache == null) return;
            if(mSelectTargetFolder!=null&&Directory.Exists(mSelectTargetFolder))
            {
                foreach (KeyValuePair<int, ThumbPictureBox> kvp in mDicPicBoxCache)
                {
                    string fileName = mlFileNamesInCurDir[kvp.Key];
                    string targetFile = Path.Combine(mSelectTargetFolder, Path.GetFileName(fileName));
                    
                    kvp.Value.IsSelected = Common.IsFileInAnotherFolder(targetFile, mSelectTargetFolder);
                }
            }
            
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

        private void GetCurrentPath(String fileName)
        {
            FileInfo info = new FileInfo(fileName);
            mCurrentPath = info.DirectoryName;
            mCurName = info.Name;
        }

        private Shell32.Folder mCurFolder = null;
        private void GetCurPathShellCls()
        {
            DirectoryInfo dirInfo = new DirectoryInfo(mCurrentPath);
            Shell32.ShellClass sh = new Shell32.ShellClass();
            mCurFolder = sh.NameSpace(mCurrentPath);
        }
        private void GetFilesInCurPath()
        {
            if (mlFileNamesInCurDir == null)
            {
                mlFileNamesInCurDir = new List<string>();
            }
            else
            {
                mlFileNamesInCurDir.Clear();
            }
            ArrayList files = new ArrayList();


            Shell32.FolderItems folderItems = mCurFolder.Items();

            foreach (Shell32.FolderItem folderItem in folderItems)
            {
                if (!folderItem.IsFolder)
                    files.Add(folderItem);

            }
            files.Sort(new FileNameComparer());
            IEnumerator enumeration = files.GetEnumerator();
            bool finded = false;
            int cnt = 0;
         
            while (enumeration.MoveNext())
            {
                Shell32.FolderItem shellFi = (Shell32.FolderItem)enumeration.Current;
                FileInfo fi = new FileInfo(shellFi.Path);
                if (CommonLib.Common.GetPosInArray(SuperDataPanel.maAllSurpportedTypes, "*" + fi.Extension) >= 0)
                {

                    mlFileNamesInCurDir.Add(fi.FullName);
                    //int r = String.Compare(info.Name, fi.Name, true);
                    if (finded == false && (mCurName.Equals(fi.Name) == true))
                    {
                        finded = true;
                        mCurFilePos = cnt;
                    }
                    cnt++;
                }
            }

            if (mDataPanel != null)
            {
                if (mlFileNamesInCurDir.Count > 1)
                    mDataPanel.ShowNavWindow();
                else
                    mDataPanel.HideNavWindow();
            }
            

        }

        private bool LoadFileByIndex()
        {

            if (mCurFilePos >= 0 && mCurFilePos < mlFileNamesInCurDir.Count)
            {
                return LoadDataFromFile(mlFileNamesInCurDir[mCurFilePos]);
            }
            else
            {
                if (mCurFilePos > (mlFileNamesInCurDir.Count - 1))
                {
                    mCurFilePos = (mlFileNamesInCurDir.Count - 1);
                }
                if (mCurFilePos < 0)
                {
                    mCurFilePos = 0;
                }
            }
            return false;
        }

        private bool LoadDataFromFile(String fileName)
        {
            if (mCurFileName == fileName) return true;
            if (mDataPanel != null)
            {
                mCurFileName = fileName;
                return mDataPanel.LoadDataFromFile(fileName);
            }
            return false;
        }

        
        private void StartFileSystemWatch()
        {
            FileSystemWatcher watcher = new FileSystemWatcher(mCurrentPath);
            watcher.EnableRaisingEvents = true;
            watcher.Created += Watcher_Created;
            watcher.Deleted += Watcher_Deleted;
            watcher.Renamed += Watcher_Renamed;
        }

        private void Watcher_Renamed(object sender, RenamedEventArgs e)
        {
            //如果删除的是当前文件，那么将切换文件
            GetFilesInCurPath();
            if (mCurFileName.Equals(e.OldFullPath) == true)
            {
               
                mCurFilePos = mlFileNamesInCurDir.IndexOf(e.FullPath);

                LoadDataFromFile(mlFileNamesInCurDir[mCurFilePos]);

            }
            RebuildList(true);
        }

        private void Watcher_Deleted(object sender, FileSystemEventArgs e)
        {
            //如果删除的是当前文件，那么将切换文件
            GetFilesInCurPath();
            if (mCurFileName.Equals(e.FullPath) == true)
            {
                if (mCurFilePos > mlFileNamesInCurDir.Count - 1)
                    mCurFilePos = mlFileNamesInCurDir.Count - 1;


                LoadFileByIndex(mCurFilePos, false, true);

            }
            RebuildList(true);
        }
        private void Watcher_Created(object sender, FileSystemEventArgs e)
        {

            

        }

        /// <summary>
        /// 加载文件的上下文，为构建缩略图列表做准备
        /// </summary>
        /// <param name="fileName"></param>
        public void LoadFileContext(String fileName)
        {
            String oldPath = mCurrentPath;
            GetCurrentPath(fileName);
            if (oldPath != mCurrentPath)
            {
                GetCurPathShellCls();
                StartFileSystemWatch();
            }
            
            GetFilesInCurPath();

            RebuildList(true);
        }

        public void NextFile()
        {
            mCurFilePos++;
            if (mCurFilePos > (mlFileNamesInCurDir.Count - 1))
            {
                mCurFilePos = mlFileNamesInCurDir.Count - 1;
                return;
            }
            LoadDataFromFile(mlFileNamesInCurDir[mCurFilePos]);

            SwitchToFile();
        }
        public void PreviousFile()
        {
            mCurFilePos--;

            if (mCurFilePos < 0)
            {
                mCurFilePos = 0;
                return;
            }

            LoadDataFromFile(mlFileNamesInCurDir[mCurFilePos]);

            SwitchToFile();
        }
        public void LoadFileByIndex(int index, bool raiseEvent = false, bool force = false)
        {
            if (index < 0 || index > mlFileNamesInCurDir.Count - 1) return;
            if (mCurFilePos == index && force == false) return;
            mCurFilePos = index;
            LoadDataFromFile(mlFileNamesInCurDir[mCurFilePos]);

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

        public void RebuildList(bool clear)
        {
            if (this.InvokeRequired == true)
            {
                BoolParamMethodCallback callback = new BoolParamMethodCallback(RebuildList);
                this.Invoke(callback,new object[] { clear });
            }
            else
            {
                if(clear)Clear();
                BuildThumbList();
            }
        }

        private void SwitchToFile()
        {
            ThumbPictureBox picBoxTemp = null;
            if (mDicPicBoxCache.ContainsKey(mCurFilePos) == true)
            {
                picBoxTemp = mDicPicBoxCache[mCurFilePos];
            }
            else
            {
                int newBoxLeft = CalcThumbBoxLeft(mCurFilePos);
                picBoxTemp = CreateThumbPicBox(newBoxLeft, mCurFilePos, CreateThumbFromFile(mlFileNamesInCurDir[mCurFilePos]));
                this.mMainPanel.Controls.Add(picBoxTemp);
                if (mDicPicBoxCache.ContainsKey(mCurFilePos) == false)
                    mDicPicBoxCache.Add(mCurFilePos, picBoxTemp);
            }
            
            if ((mBeginIndex - 1) == mCurFilePos)
                mBeginIndex = mCurFilePos;
            else if ((mEndIndex + 1) == mCurFilePos)
                mEndIndex = mCurFilePos;


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
            mHScroll.Value= mCurFilePos * (mThumbnailWidth + mInterval);
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
            return (mlFileNamesInCurDir.Count* iThumbWidth +mInterval);
        }
        private void BuildThumbList()
        {

            int iThumbnailCnt = GetThumbnailCount();
            mStartValue = mCurFilePos * (mThumbnailWidth + mInterval);
            //在缩略图列表中，当前选中的尽量居中显示
            int leftAndRightCnt = iThumbnailCnt / 2;

            int beginIndex = mCurFilePos - leftAndRightCnt;
            int endIndex = mCurFilePos + leftAndRightCnt;
            if (beginIndex < 0)
            {
                
                endIndex = endIndex - beginIndex;
                beginIndex = 0;
            }
            if (endIndex > (mlFileNamesInCurDir.Count - 1)) endIndex = mlFileNamesInCurDir.Count - 1;

            mBeginIndex = beginIndex;
            mEndIndex = endIndex;


            int iCnt = 0;
            int iThumbWidth = (mThumbnailWidth + mInterval);
            int iTotalThumbWidth = iThumbnailCnt * iThumbWidth;
            int iTotalWidth = CalcTotalWidth();

            int iBeginLeft = CalcThumbBoxLeft(beginIndex);
            int iOffset = (iTotalThumbWidth - this.Width) / 2;
            this.mMainPanel.Left =((mCurFilePos-beginIndex)<iThumbnailCnt/2)?0:-iBeginLeft - iOffset;
            this.mMainPanel.Width = iTotalWidth;
            if (beginIndex == 0)
                iBeginLeft = mInterval;
            
            for (int j = beginIndex; j <= endIndex; j++)
            {
                ThumbPictureBox thumbPicBox = null;
                bool isNew = false;
                if (mDicPicBoxCache.ContainsKey(j))
                {
                    thumbPicBox = mDicPicBoxCache[j];
                    thumbPicBox.Left = iBeginLeft + iCnt * iThumbWidth;
                }
                else
                {
                    Image thumbImg = CreateThumbFromFile(mlFileNamesInCurDir[j]);
                    thumbPicBox = CreateThumbPicBox(iBeginLeft + iCnt * iThumbWidth, j, thumbImg);
                    isNew = true;
                }
                

                if (j == mCurFilePos)
                {
                    thumbPicBox.IsCurrent = true;
                    mCurrentPicBox = thumbPicBox;
                   
                }
            
                if(isNew)mDicPicBoxCache.Add(j, thumbPicBox);
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
               
                mHScroll.OnScroll += HScroll_OnScroll;
                mHScroll.OnScrollEnd += MHScroll_OnScrollEnd;
                
                this.Controls.Add(mHScroll);
                mHScroll.BringToFront();
            }

            
            mHScroll.SmallChange = mMainPanel.Width / 100;

            mHScroll.Maximum = mMainPanel.Width;
            mHScroll.Value = mCurFilePos * (mThumbnailWidth + mInterval);

        
        }
        protected override void OnControlAdded(ControlEventArgs e)
        {
            base.OnControlAdded(e);
            e.Control.MouseLeave += Control_MouseLeave;
            e.Control.MouseEnter += Control_MouseEnter;
        }

        protected override void OnControlRemoved(ControlEventArgs e)
        {
            base.OnControlRemoved(e);
            e.Control.MouseLeave -= Control_MouseLeave;
            e.Control.MouseEnter -= Control_MouseEnter;
        }
        private void Control_MouseEnter(object sender, EventArgs e)
        {
            
            this.OnMouseEnter(e);
        }

        private void Control_MouseLeave(object sender, EventArgs e)
        {
            //this.RaiseMouseEvent("MouseLeave", e as MouseEventArgs);
            this.OnMouseLeave(e);
        }

        private string GetFormatStr(Image img)
        {

            if (img == null) return null;

            string format = Common.GetImageFormatName(img.RawFormat);

            if (format != null) format = format.ToUpper();
            return format;
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
            string formatName = Common.GetFormatNameFromFile(mlFileNamesInCurDir[fileIndex]);
            if (formatName != null) formatName = formatName.ToUpper();
            thumbPicBox.FormatName = formatName;
            if(mSelectTargetFolder!=null&&Directory.Exists(mSelectTargetFolder))
                thumbPicBox.IsSelected = Common.IsFileInAnotherFolder(mlFileNamesInCurDir[fileIndex], mSelectTargetFolder);

            thumbPicBox.MouseLeave += ThumbPicBox_MouseLeave;
            thumbPicBox.MouseEnter += ThumbPicBox_MouseEnter;
            return thumbPicBox;
        }

        public void UpdateCurThumbBoxStatus(bool selected)
        {
            if (mDicPicBoxCache.ContainsKey(mCurFilePos))
            {
                mDicPicBoxCache[mCurFilePos].IsSelected = selected;
            }
        }
        private void ThumbPicBox_MouseEnter(object sender, EventArgs e)
        {
            this.OnMouseEnter(e);
        }

        private void ThumbPicBox_MouseLeave(object sender, EventArgs e)
        {
            this.OnMouseLeave(e);
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

                imgTemp = Image.FromFile(mlFileNamesInCurDir[j]);

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
                if ((-mMainPanel.Left) < (preLeft+ mThumbnailWidth))
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
            if (mEndIndex > (mlFileNamesInCurDir.Count - 1)) mEndIndex = mlFileNamesInCurDir.Count - 1;
            mStartValue = mStartValue + delta;
        }

        private void ThumbPicBox_MouseClick(object sender, MouseEventArgs e)
        {
            ThumbPictureBox tempPic = sender as ThumbPictureBox;
            int selIndex = (int)tempPic.Tag;

            int curIndex = (int)mCurrentPicBox.Tag;
            if (selIndex == curIndex) return;

            mCurFilePos = ((int)tempPic.Tag);
            LoadFileByIndex();
       
            mCurrentPicBox.IsCurrent = false;
            tempPic.IsCurrent = true;
            mCurrentPicBox = tempPic;


        }
    }
}
