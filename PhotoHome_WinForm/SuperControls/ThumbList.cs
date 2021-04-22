using CommonLib;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using ThirdPlugins;

namespace SuperControls
{
    public enum EnumListItemLayout
    {
        Horizontal=1,
        Vertical=2
    }
    public class ThumbList : Control
    {
        private int mThumbnailWidth = 180, mThumbnailHeight = 135;
        private int mThumbSelWidth = 220, mThumbSelHeight = 200;
        private int mPhotoLimit = 500;
        private int mInterval = 3;
        private CommonLib.SetIntValueCallback mThumbnailClick = null;
        private ThumbBox mCurrentPicBox = null;
      
        private Dictionary<int, ThumbBox> mDicPicBoxCache = new Dictionary<int, ThumbBox>();
        private int mMaxCacheCnt = 20;

        private System.Windows.Forms.Timer mWheelTimer = null;
        private int mWheelStopCnt = 0;
   
        private List<String> mlFileNamesInCurDir = null;
        private int mCurFilePos = -1;
    
        private string mSelectTargetFolder = null;
        private SuperDataPanel mDataPanel = null;

        private String mCurrentPath, mCurName, mThumbDirName,mThumbPath;
        public string mCurFileName;

        private bool mFolderRecursion = true;
        Shell32.ShellClass mSH = null;

        private int mPaddingBottom = 20, mPaddingTop = 30,mPaddingLeft=5,mPaddingRight=18;
        private BufferedGraphicsContext currentContext;
        private BufferedGraphics ThumbListBuffer;
        private bool mIsFolderBrowser = false;
        private bool mFindNewerFile = false;

        private Dictionary<string, bool> mDicFolderCache = null;
        private EnumListItemLayout mItemLayout = EnumListItemLayout.Horizontal;
        public ThumbList() : base()
        {
            this.Padding = new Padding(0, 30, 0, 30);
    

            mWheelTimer = new System.Windows.Forms.Timer();
            mWheelTimer.Interval = 100;
            mWheelTimer.Tick += MWheelTimer_Tick;
        }
        ~ThumbList()
        {
            
        }
        private void Setup(int offsetX=0)
        {
            if (ThumbListBuffer != null)
            {
                ThumbListBuffer.Dispose();
                ThumbListBuffer = null;
            }
            currentContext = BufferedGraphicsManager.Current;
            Rectangle dispRect = this.DisplayRectangle;
            //Rectangle rectBuffer = new Rectangle(dispRect.Left - offsetX, dispRect.Top, dispRect.Width, dispRect.Height);

            ThumbListBuffer = currentContext.Allocate(this.CreateGraphics(), dispRect);
        }
        protected override void OnPaintBackground(PaintEventArgs pevent)
        {
            
        }
        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
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

        public bool FolderRecursion
        {
            get
            {
                return mFolderRecursion;
            }
            set
            {
                mFolderRecursion = value;
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

        public int ThumbnailWidth
        {
            get
            {
                return mThumbnailWidth;
            }
            set
            {
                mThumbnailWidth = value;
            }
        }
        public int ThumbnailHeight
        {
            get
            {
                return mThumbnailHeight;
            }
            set
            {
                mThumbnailHeight = value;
            }
        }
    
        public void UpdateSelectedBox()
        {
            if (mDicPicBoxCache == null) return;
            if(mSelectTargetFolder!=null&&Directory.Exists(mSelectTargetFolder))
            {
                foreach (KeyValuePair<int, ThumbBox> kvp in mDicPicBoxCache)
                {
                    string fileName = mlFileNamesInCurDir[kvp.Key];
                    string targetFile = Path.Combine(mSelectTargetFolder, Path.GetFileName(fileName));
                    
                    kvp.Value.IsSelected = Common.IsFileInAnotherFolder(targetFile, mSelectTargetFolder);
                }
            }
            this.Invalidate();
            
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
            Setup();

        }

        protected override void OnPaint(PaintEventArgs e)
        {
            Graphics gfx = e.Graphics;
            GeometryAPI.SetupGraphics(ThumbListBuffer.Graphics);
            PaintThumbBoxes(gfx);
        }
        private void PaintThumbBoxes(Graphics g)
        {
            
            lock (mDicPicBoxCache)
            {
                ThumbListBuffer.Graphics.Clear(this.BackColor);
                if (mDicPicBoxCache != null&& mDicPicBoxCache.Count>0)
                {
                    var sortedDic = from pair in mDicPicBoxCache
                                    orderby pair.Key
                                    select pair;

                    
                    foreach (KeyValuePair<int,ThumbBox> kvp in sortedDic)
                    {
                        if (mItemLayout == EnumListItemLayout.Horizontal)
                            kvp.Value.OffsetX = mScrollPos;
                        else
                            kvp.Value.OffsetY = mScrollPos;
                        kvp.Value.Draw(ThumbListBuffer.Graphics);
                    }
                    

                }
                ThumbListBuffer.Render(g);
            }
        }

  
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);
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
            if (mScrollBar != null)
            {
                mScrollBar.RaiseMouseWheel(e.Delta);
            }
        }
     
        
        private int GetThumbnailCount()
        {
            int cnt =0;
            if (mItemLayout == EnumListItemLayout.Horizontal)
            {
                Point crCnt = GetRowColCntInHor();
                cnt = this.Width / this.ActualThumbWidth;
                cnt = cnt * crCnt.Y;
            }
            else if (mItemLayout == EnumListItemLayout.Vertical)
            {
                Point crCnt = GetRowColCntInVer();
                cnt = this.Height / this.ActureThumbHeight;
                cnt = cnt * crCnt.X;
            }
            if(cnt>0)
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
            mThumbDirName = Common.MD5Encrypt(mCurrentPath);
        }

        private Shell32.Folder mCurFolder = null;
        private void GetCurPathShellCls()
        {
            if (mSH == null)
                mSH = new Shell32.ShellClass();
            mCurFolder = mSH.NameSpace(mCurrentPath);
            
        }

        private void ClearCache()
        {
            if (mlFileNamesInCurDir == null)
                mlFileNamesInCurDir = new List<string>();
            else
                mlFileNamesInCurDir.Clear();

            if(mDicFolderCache!=null) mDicFolderCache.Clear();
            if(mDicPicBoxCache!=null) mDicPicBoxCache.Clear();
        }
        private void GetFilesInCurPath()
        {

            ClearCache();
            if (mIsFolderBrowser == false)
            {
                GetFilesInPath(mCurFolder);

                if (mDataPanel != null)
                {
                    if (mlFileNamesInCurDir.Count > 1)
                        mDataPanel.ShowNavWindow();
                    else
                        mDataPanel.HideNavWindow();
                }
            }
            else
            {
                GetFolderItemsInFolder(mCurFolder);
            }
            
        }
        private void GetFilesInPath(Shell32.Folder folder)
        {
            ArrayList files = new ArrayList();
            ArrayList folders = new ArrayList();
            Shell32.FolderItems folderItems = folder.Items();

            foreach (Shell32.FolderItem folderItem in folderItems)
            {
                
                if (!folderItem.IsFolder)
                    files.Add(folderItem);
                else
                {
                    if (folderItem.Name.ToLower() != "deleted") folders.Add(folderItem);
                }
                
            }
            //默认按名称排序
            files.Sort(new FileNameComparer());
            IEnumerator enumeration = files.GetEnumerator();
            bool finded = false;
            int cnt = 0;

            DateTime dtNow = DateTime.Now;
            double dtSpan = 0;
            while (enumeration.MoveNext())
            {
                Shell32.FolderItem shellFi = (Shell32.FolderItem)enumeration.Current;
                
                FileInfo fi = new FileInfo(shellFi.Path);
                if (CommonLib.Common.GetPosInArray(PhotoHome.Global.maAllSurpportedTypes, "*" + fi.Extension) >= 0)
                {

                    mlFileNamesInCurDir.Add(fi.FullName);
                    //int r = String.Compare(info.Name, fi.Name, true);
                    if (mCurName != null)
                    {
                        if (finded == false && (mCurName.Equals(fi.Name) == true))
                        {
                            finded = true;
                            mCurFilePos = cnt;
                        }
                    }
                    else
                    {
                        if (mFindNewerFile)
                        {
                            //找最新的文件
                            DateTime ct = fi.CreationTime;
                            TimeSpan span = dtNow.Subtract(ct);
                            if (cnt == 0)
                            {
                                dtSpan = span.TotalDays;
                                mCurFilePos = cnt;
                                mCurFileName = fi.FullName;
                            }
                            else
                            {
                                //距离现在时间间隔较小的文件
                                if (dtSpan > span.TotalDays)
                                {
                                    dtSpan = span.TotalDays;
                                    mCurFilePos = cnt;
                                    mCurFileName = fi.FullName;
                                }
                            }
                        }
                        
                    }

                    cnt++;
                }
            }
            
            //递归
            if (mFolderRecursion)
            {
                folders.Sort(new FileNameComparer());
                IEnumerator folderEnum = folders.GetEnumerator();
             
               
                while (folderEnum.MoveNext())
                {
                    Shell32.FolderItem shellFi = (Shell32.FolderItem)folderEnum.Current;

                    GetFilesInPath(mSH.NameSpace(shellFi.Path));
                }
            }

            if (mCurFilePos == -1&& mlFileNamesInCurDir.Count>0)
            {
                mCurFilePos = 0;
                mCurFileName = mlFileNamesInCurDir[0];
            }
        }
        private void GetFolderItemsInFolder(Shell32.Folder folder)
        {
            if (mDicFolderCache == null) mDicFolderCache = new Dictionary<string, bool>();
          
            ArrayList files = new ArrayList();
            ArrayList folders = new ArrayList();
            Shell32.FolderItems folderItems = folder.Items();

            foreach (Shell32.FolderItem folderItem in folderItems)
            {

                if (!folderItem.IsFolder)
                    files.Add(folderItem);
                else
                    folders.Add(folderItem);

            }
            folders.Sort(new FileNameComparer());
            IEnumerator folderEnumerator = folders.GetEnumerator();

            while (folderEnumerator.MoveNext())
            {
                Shell32.FolderItem shellFi = (Shell32.FolderItem)folderEnumerator.Current;
              
                mlFileNamesInCurDir.Add(shellFi.Path);
                mDicFolderCache.Add(shellFi.Path, true);
            }

            //默认按名称排序
            files.Sort(new FileNameComparer());
            IEnumerator enumeration = files.GetEnumerator();
            
            while (enumeration.MoveNext())
            {
                Shell32.FolderItem shellFi = (Shell32.FolderItem)enumeration.Current;
                if (shellFi.Path.IndexOf("\\") < 0 && shellFi.Path.IndexOf("/") < 0) continue;
                FileInfo fi = new FileInfo(shellFi.Path);
                if (CommonLib.Common.GetPosInArray(PhotoHome.Global.maAllSurpportedTypes, "*" + fi.Extension) >= 0)
                {

                    mlFileNamesInCurDir.Add(fi.FullName);
                }
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

        public void DelThumbByIndex(int idx)
        {
            if (mDicPicBoxCache.ContainsKey(idx))
            {
                mDicPicBoxCache.Remove(idx);
            }
            mlFileNamesInCurDir.RemoveAt(idx);
            if (mCurFilePos == idx)
            {
                if (mCurFilePos < mlFileNamesInCurDir.Count-1)
                    NextFile();
                else if (mCurFilePos > 0)
                    PreviousFile();
            }

            this.Invalidate();
        }
        public void DelThumbByName(string fileName)
        {
            int idx = mlFileNamesInCurDir.IndexOf(fileName);
            DelThumbByIndex(idx);
        }
        
        private void StartFileSystemWatch()
        {
            if (mIsFolderBrowser == true) return;
            using (FileSystemWatcher watcher = new FileSystemWatcher(mCurrentPath))
            {
                watcher.EnableRaisingEvents = true;
                watcher.NotifyFilter = NotifyFilters.Size | NotifyFilters.FileName;
                watcher.Created += Watcher_Created;
                watcher.Deleted += Watcher_Deleted;
                watcher.Renamed += Watcher_Renamed;
            }
            
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

        public void LoadFolder(string pathName)
        {
            if (pathName == mCurrentPath)
            {
                //TODO:显示提示
                return;
            }
            mCurrentPath = pathName;
            mThumbDirName = Common.MD5Encrypt(mCurrentPath);
            mCurName = null;
            GetCurPathShellCls();
            StartFileSystemWatch();
            
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
            Invalidate();
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
            Invalidate();
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
                //Task.Run(() =>
                //{
                //    BuildThumbList();
                //});
                BuildThumbList(true);
            }
        }

        private async void SwitchToFile()
        {
            ThumbBox picBoxTemp = null;
            if (mDicPicBoxCache.ContainsKey(mCurFilePos) == true)
            {
                picBoxTemp = mDicPicBoxCache[mCurFilePos];
            }
            else
            {
                Point pos = GetThumbBoxPos(mCurFilePos);
                picBoxTemp = CreateThumbBox(pos, mCurFilePos,await CreateThumbFromFileAsync(mlFileNamesInCurDir[mCurFilePos]));
               
                if (mDicPicBoxCache.ContainsKey(mCurFilePos) == false)
                    mDicPicBoxCache.Add(mCurFilePos, picBoxTemp);
            }
            
            if ((mBeginIndex - 1) == mCurFilePos)
                mBeginIndex = mCurFilePos;
            else if ((mEndIndex + 1) == mCurFilePos)
                mEndIndex = mCurFilePos;

            RemoveMoreThumbBoxes();

            SwitchCurrentBox(picBoxTemp);
        }

        private void SwitchCurrentBox(ThumbBox picBox)
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
        private void EnsurePicBoxVisible(ThumbBox picBox)
        {
            int hiddenSize = GetBoxHiddenSize(picBox);

            mScrollBar.Value += hiddenSize; //mCurFilePos * (mThumbnailWidth + mInterval);
            mScrollPos -= hiddenSize;
            Invalidate();
        }

      
        private int GetBoxHiddenSize(ThumbBox picBox)
        {
            int panelLeft = mScrollPos;
            int boxLeft = picBox.Left + mScrollPos;
            
            if (boxLeft<0)
            {
                //左边被隐藏，返回负值
                return boxLeft;
            }
            else
            {
                //右边被隐藏，返回正值
                int boxRight = picBox.Right + mScrollPos;
                if (boxRight - this.Width > 0)
                    return boxRight - this.Width;
            }
            return 0;

        }
        private int CalcTotalWidth()
        {
            int iThumbWidth =this.ActualThumbWidth;
            int iThumbHeight = this.ActureThumbHeight;
            int iHeight = this.Height-mPaddingBottom-mPaddingTop-mInterval, iWidth = this.Width;
            int iRow = iHeight / iThumbHeight;
            if (iRow == 0) iRow = 1;
            int iCol = mlFileNamesInCurDir.Count / iRow;
            if (mlFileNamesInCurDir.Count % iRow>0)
                iCol++;
            return (iCol * iThumbWidth +mInterval);
        }
        private int CalcTotalHeight()
        {
            int iThumbWidth = this.ActualThumbWidth;
            int iThumbHeight = this.ActureThumbHeight;
            int iHeight = this.Height, iWidth = this.Width-mPaddingRight-mPaddingLeft-mInterval;
            int iCol = iWidth / iThumbWidth;
            if (iCol == 0) iCol = 1;
            int iRow = mlFileNamesInCurDir.Count / iCol;
            if (mlFileNamesInCurDir.Count % iCol > 0)
                iRow++;
            return (iRow * iThumbHeight + mInterval);
        }
        private int GetClientHeight()
        {
            return this.Height - mPaddingBottom - mPaddingTop;
        }
        private int GetClientWidth()
        {
            return this.Width - mPaddingRight - mPaddingLeft;
        }
        private int ActualThumbWidth
        {
            get
            {
                return mThumbnailWidth + mInterval;
            }
        }
        private int ActureThumbHeight
        {
            get
            {
                return mThumbnailHeight + mInterval;
            }
        }
        public EnumListItemLayout ItemLayout { get => mItemLayout; set => mItemLayout = value; }
      
        public bool IsFolderBrowser { get => mIsFolderBrowser; set => mIsFolderBrowser = value; }
        /// <summary>
        /// 在选择文件夹时，是否默认打开较新的文件
        /// </summary>
        public bool FindNewerFile { get => mFindNewerFile; set => mFindNewerFile = value; }

        private async void CreateThumbBoxByRange(int curPos,bool blank)
        {
    
            for (int j = mBeginIndex; j <= mEndIndex; j++)
            {
                ThumbBox thumbBox = null;
              
                if (mDicPicBoxCache.ContainsKey(j))
                {
                    thumbBox = mDicPicBoxCache[j];
                    Point pnt = GetThumbBoxPos(j);
                    thumbBox.Left = pnt.X;
                    thumbBox.Top = pnt.Y;
                }
                else
                {
                    Image thumbImg = null;
                    bool isFolder = false;
                    
                    if (blank == false)
                    {
                        isFolder = IsFolder(mlFileNamesInCurDir[j]);
                        if (isFolder == false)
                        {
                            thumbImg = await CreateThumbFromFileAsync(mlFileNamesInCurDir[j]);
                        }
                        else
                        {
                            thumbImg = CreateThumbFromFolder(mlFileNamesInCurDir[j]);
                        }
                        
                    }
                    thumbBox = CreateThumbBox(GetThumbBoxPos(j), j, thumbImg);
                    thumbBox.IsFolder = isFolder;

                }

                if (curPos>-1&&j == curPos)
                {
                    thumbBox.IsCurrent = true;
                    mCurrentPicBox = thumbBox;

                }

                if (mDicPicBoxCache.ContainsKey(j)==false) mDicPicBoxCache.Add(j, thumbBox);
            }
          
        }
        
        private bool IsFolder(string path)
        {
            if (mDicFolderCache == null) return false;
            return mDicFolderCache.ContainsKey(path);
        }
        private void CalcThumbIndexAndPos(int thumbPos)
        {
            int iThumbnailCnt = GetThumbnailCount();
            mMaxCacheCnt = iThumbnailCnt;
            mMaxCacheCnt += 10;
            //在缩略图列表中，当前选中的尽量居中显示
            int leftAndRightCnt = iThumbnailCnt / 2;

            int beginIndex = thumbPos - leftAndRightCnt;
            int endIndex = thumbPos + leftAndRightCnt;
            if (beginIndex < 0)
            {

                endIndex = endIndex - beginIndex;
                beginIndex = 0;
            }
            if (endIndex > (mlFileNamesInCurDir.Count - 1))
            {
                endIndex = mlFileNamesInCurDir.Count - 1;
                beginIndex = endIndex - iThumbnailCnt;
                if (beginIndex < 0)
                    beginIndex = 0;
            }

            Point pos = GetThumbBoxPos(beginIndex);
            mScrollPos =(mItemLayout==EnumListItemLayout.Horizontal)? -pos.X:-pos.Y;
            mBeginIndex = beginIndex;
            mEndIndex = endIndex;
        }

        private void BuildThumbList(bool toCurrent)
        {
            CalcThumbIndexAndPos(mCurFilePos);

            CreateThumbBoxByRange(mCurFilePos,false);
            
            CreateScrollBar();
            if (toCurrent == true)
            {
                SetScrollValue(Math.Abs(mScrollPos));
            }
            Invalidate();
        }

        private void SetScrollValue(int val)
        {
            if (mScrollBar != null)
            {
                if (val >= mScrollBar.Minimum && val <= mScrollBar.Maximum)
                {
                    mScrollBar.Value = val;
                }
            }
        }
        private VScrollBarMobile mScrollBar = null;
        private int mTotalWidth = 0;
        private int mTotalHeight = 0;
        private void CreateScrollBar()
        {
            int totalVal = 0;
            int max = 0;
            bool created = false;
            if (mScrollBar == null) created = true;
            bool needScroll = true;
            if (mItemLayout == EnumListItemLayout.Horizontal)
            {
                totalVal = CalcTotalWidth();
                mTotalWidth = totalVal;
                if (totalVal > this.Width)
                {
                    if (mScrollBar == null)
                    {
                        created = true;
                        mScrollBar = new HScrollBarMobile();
                    }
                    max = totalVal - this.Width;
                }
                else
                {
                    needScroll = false;
                }
                
            }
            else if (mItemLayout == EnumListItemLayout.Vertical)
            {
                totalVal = CalcTotalHeight();
                mTotalHeight = totalVal;
       
                if (totalVal > this.Height)
                {
                    if (mScrollBar == null)
                    {
                        created = true;
                        mScrollBar = new VScrollBarMobile();
                    }
                    max = totalVal - this.Height;
                }
                else
                {
                    needScroll = false;
                }
            }
            if (needScroll == false)
            {
                if (mScrollBar != null)
                {
                    this.Controls.Remove(mScrollBar);
                    mScrollBar = null;
                }
            }
            else
            {
                
                if (created)
                {
                    this.Controls.Add(mScrollBar);
                    mScrollBar.BringToFront();

                    mScrollBar.OnScroll += HScroll_OnScroll;
                    mScrollBar.OnScrollEnd += MHScroll_OnScrollEnd;
                }

                mScrollBar.SmallChange = totalVal / 100;
                mScrollBar.Maximum = max;
            }
            

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
        private void GenerateThumbs()
        {

        }
        private string GetThumbPath()
        {
            string appPath = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), "PhotoHome");
            Common.EnsureDirExisted(appPath);
            string thumbPath = Path.Combine(appPath, mThumbDirName);
            Common.EnsureDirExisted(thumbPath);
            
            return thumbPath;
        }

        public string GetThumbFileName(string fileName)
        {
            PhotoHome.EnumFileType fileType = PhotoHome.Global.GetFileType(fileName);
            string thumbPath = GetThumbPath();
            string thumbFileName = null;
            switch (fileType)
            {
                case PhotoHome.EnumFileType.Image:
                    thumbFileName = Path.Combine(thumbPath, Path.GetFileName(fileName)); 

                    break;
                case PhotoHome.EnumFileType.Video:
                case PhotoHome.EnumFileType.Audio:
                case PhotoHome.EnumFileType.Shp:
                case PhotoHome.EnumFileType.Tif:
                    thumbFileName = Path.GetFileNameWithoutExtension(fileName);
                    thumbFileName = thumbFileName +"_"+fileType.ToString()+ ".png";
                    thumbFileName = Path.Combine(thumbPath, thumbFileName);
                    break;
            }

            
            return thumbFileName;
        }
        public string GetBGFile(string fileName)
        {
            string thumbPath = GetThumbPath();
            string bgFileName = "unknown_bg.png";

            bgFileName = Path.GetFileNameWithoutExtension(fileName);
            bgFileName = bgFileName + "_bg.png";
        

            return Path.Combine(thumbPath, bgFileName);
        }
        private Image CreateImgFromFile(string fileName)
        {
            try
            {
                StreamReader reader = new StreamReader(fileName);
                Image imgTemp = (Bitmap)Bitmap.FromStream(reader.BaseStream);
                reader.Close();

                return imgTemp;
            }
            catch {
                return null;
            }
            
        }
        
        private Image CreateThumbFromFolder(string folderName)
        {
            Icon folderIcon = ShellAPI.GetPathIcon(folderName);
            
            if (folderIcon!=null)
                return folderIcon.ToBitmap();
            return null;
        }
        private async Task<Image> CreateThumbFromFileAsync(String fileName)
        {
            PhotoHome.EnumFileType fileType = PhotoHome.Global.GetFileType(fileName);

            string thumbFileName = GetThumbFileName(fileName);
            if (File.Exists(thumbFileName))
            {
                return CreateImgFromFile(thumbFileName);
            }
            else
            {
                Image thumbImg = null;
                switch (fileType)
                {
                    case PhotoHome.EnumFileType.Image:
                        thumbImg =await CreateThumbOfImageAsync(fileName, thumbFileName);
                        break;
                    case PhotoHome.EnumFileType.Video:
                        thumbImg =await CreateThumbOfVideoAsync(fileName, thumbFileName,GetBGFile(fileName));
                        break;
                    case PhotoHome.EnumFileType.Audio:
                        thumbImg = await CreateThumbOfAudioAsync(fileName, thumbFileName);
                        break;
                    case PhotoHome.EnumFileType.Shp:
                        break;
                    case PhotoHome.EnumFileType.Tif:
                        break;
                }

                return thumbImg;

            }
        }

        private Task<Image> CreateThumbOfVideoAsync(string srcFile,string thumbFile, string bgFile)
        {
            return Task.Run<Image>(() =>
            {
                return CreateThumbOfVideo(srcFile, thumbFile,bgFile);
            });
        }
        private Task<Image> CreateThumbOfAudioAsync(string srcFile, string thumbFile)
        {
            return Task.Run<Image>(() =>
            {
                return CreateThumbOfAudio(srcFile, thumbFile);
            });
        }
        private Image CreateThumbOfAudio(string srcFile, String thumbFile)
        {
            bool well=VLCMultimedia.GenerateThumbOfAudio(srcFile, thumbFile);
            if (well)
            {
                DateTime dtNow = DateTime.Now;
                Image imgTemp = null;
                while (true)
                {
                    if (File.Exists(thumbFile) && Common.IsFileOpened(thumbFile) == false)
                    {
                        imgTemp = CreateImgFromFile(thumbFile);
                        if (imgTemp != null) break;
                    }
                    TimeSpan span = dtNow.Subtract(DateTime.Now);
                    if (span.TotalSeconds > 10)
                    {
                        break;
                    }
                }

                return imgTemp;
            }
            else
            {
                return PhotoHome_WinForm.Properties.Resources.music_default;
            }
            
        }
        private Image CreateThumbOfVideo(string srcFile,String thumbFile,string bgFile)
        {
            VLCMultimedia.GenerateThumb(srcFile, thumbFile,bgFile);
            DateTime dtNow = DateTime.Now;
            Image imgTemp = null;
            while (true)
            {
                if (File.Exists(thumbFile)&&Common.IsFileOpened(thumbFile)==false)
                {
                    imgTemp= CreateImgFromFile(thumbFile);
                    if(imgTemp!=null)break;
                }
                TimeSpan span = dtNow.Subtract(DateTime.Now);
                if (span.TotalSeconds > 10)
                {
                    break;
                }
            }
            
            return imgTemp;
        }

        private Task<Image> CreateThumbOfImageAsync(string srcFile, string thumbFile)
        {
            return Task.Run<Image>(()=> {
                return CreateThumbOfImage(srcFile,thumbFile);
            });

        }

        private Image CreateThumbOfImage(string srcImgFile,string thumbFile)
        {
            Image srcImg = CreateImgFromFile(srcImgFile);
            if (srcImg == null) return null;
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
            Image thumbImage= srcImg.GetThumbnailImage((int)targetWidth, (int)targetHeight, abortCallback, IntPtr.Zero);
            try
            {
                thumbImage.Save(thumbFile);
            }
            catch { }
            
            return thumbImage;
        }
        
        private ThumbBox CreateThumbBox(Point pos,int fileIndex,Image thumbImg)
        {
            
            ThumbBox thumbBox = new ThumbBox();
            thumbBox.Image = thumbImg;
            thumbBox.BackColor = Color.DimGray;
            thumbBox.Top = pos.Y;
            thumbBox.Left = pos.X;
            thumbBox.OriginalFileIndex = fileIndex;
           
            thumbBox.Width = mThumbnailWidth;
            thumbBox.Height = mThumbnailHeight;
            thumbBox.OriginalFileName = mlFileNamesInCurDir[fileIndex];
            
            if(mSelectTargetFolder!=null&&Directory.Exists(mSelectTargetFolder))
                thumbBox.IsSelected = Common.IsFileInAnotherFolder(mlFileNamesInCurDir[fileIndex], mSelectTargetFolder);

            return thumbBox;
        }
        
        
        public void UpdateCurThumbBoxStatus(bool selected)
        {
            this.InvokeIfRequired(l =>
            {
                if (mDicPicBoxCache.ContainsKey(mCurFilePos))
                {
                    mDicPicBoxCache[mCurFilePos].IsSelected = selected;
                }
                Invalidate();
            });

            
        }
        private void ThumbPicBox_MouseEnter(object sender, EventArgs e)
        {
            this.OnMouseEnter(e);
        }

        private void ThumbPicBox_MouseLeave(object sender, EventArgs e)
        {
            this.OnMouseLeave(e);
        }

        private async void LoadScrollImages()
        {

            for (int j = mBeginIndex; j <= mEndIndex; j++)
            {
                if (mDicPicBoxCache.ContainsKey(j)&&mDicPicBoxCache[j].Image != null)
                    continue;
                Image thumbnailImg = null;
                bool isFolder = IsFolder(mlFileNamesInCurDir[j]);
                if (isFolder)
                {
                    thumbnailImg = CreateThumbFromFolder(mlFileNamesInCurDir[j]);
                }
                else
                {
                    thumbnailImg=await CreateThumbFromFileAsync(mlFileNamesInCurDir[j]);
                }
                
                ThumbBox thumbPicBox = GetThumbPicBox(j);

                if (thumbPicBox != null)
                {
                    thumbPicBox.IsFolder = isFolder;
                    thumbPicBox.Image = thumbnailImg;
                }
            }
            
            Invalidate();
        }

        protected override void OnMouseClick(MouseEventArgs e)
        {
            base.OnMouseClick(e);
            ThumbBox boxClicked = HitTest(e.X, e.Y);
            if (boxClicked == null) return;
           
            int selIndex = boxClicked.OriginalFileIndex;
            if (mCurrentPicBox != null)
            {
                int curIndex = mCurrentPicBox.OriginalFileIndex;
                if (selIndex == curIndex) return;

            }

            mCurFilePos = boxClicked.OriginalFileIndex;
            LoadFileByIndex();

            if(mCurrentPicBox!=null) mCurrentPicBox.IsCurrent = false;
            boxClicked.IsCurrent = true;
            mCurrentPicBox = boxClicked;

            Invalidate();
        }
        private ThumbBox mOldHoveredBox = null;
        protected override void OnMouseMove(MouseEventArgs e)
        {
            base.OnMouseMove(e);
            ThumbBox boxClicked = HitTest(e.X, e.Y);
            if (mOldHoveredBox == boxClicked) return;
            if (boxClicked != null)
            {
                this.Cursor = Cursors.Hand;
                boxClicked.IsHovered = true;
            }
            else
                this.Cursor = Cursors.Default;
            if (mOldHoveredBox != null)
                mOldHoveredBox.IsHovered = false;

            mOldHoveredBox = boxClicked;

            Invalidate();
        }
        private ThumbBox HitTest(int x,int y)
        {
            foreach(KeyValuePair<int,ThumbBox> kvp in mDicPicBoxCache)
            {
                if (kvp.Value.HitTest(x, y))
                    return kvp.Value;
            }
            return null;
        }
        private void MHScroll_OnScrollEnd(object sender, ScrollEventArgs e)
        {
            LoadScrollImages();
        }

        private bool IsScrollOver(int delta)
        {
            if (delta < 0 && mScrollPos >= 0) return true;
            if (mItemLayout == EnumListItemLayout.Horizontal)
            {
                if (delta > 0 && Math.Abs(mScrollPos) >= (mTotalWidth - this.Width)) return true;
            }
            else
            {
                if (delta > 0 && Math.Abs(mScrollPos) >= (mTotalHeight - this.Height)) return true;
            }

            return false;
        }
        /// <summary>
        /// 确保滚动条的位置不超限
        /// </summary>
        private void AdjustScrollPos()
        {
            if (mScrollPos >= 0) mScrollPos = 0;
            if (mItemLayout == EnumListItemLayout.Horizontal)
            {
                if (Math.Abs(mScrollPos) >= (mTotalWidth - this.Width))
                    mScrollPos = -(mTotalWidth - this.Width);
            }
            else
            {
                if (Math.Abs(mScrollPos) >= (mTotalHeight - this.Height))
                    mScrollPos = -(mTotalHeight - this.Height);
            }
        }
        private int mBeginIndex = 0;
        private int mEndIndex = 0;
        private int mScrollPos = 0;
 
        private void HScroll_OnScroll(object sender, ScrollEventArgs e)
        {
            int newPos = e.NewValue;
            int delta = newPos - e.OldValue;
            if (IsScrollOver(delta)) return;
            
            mScrollPos = mScrollPos - delta;

            AdjustScrollPos();

            int newThumbPos = GetFilePosByPos(mScrollPos); //Math.Abs(mScrollPos) / this.ActualThumbWidth;
            int moveCnt = mBeginIndex - newThumbPos;
            if (moveCnt == 0) return;
            int beginIndex, endIndex;

            beginIndex = mBeginIndex - moveCnt;
            endIndex = mEndIndex - moveCnt;
            
            if (beginIndex < 0) beginIndex = 0;
            if (endIndex > mlFileNamesInCurDir.Count - 1)
                endIndex = mlFileNamesInCurDir.Count - 1;
            if (beginIndex >= 0 && endIndex <= mlFileNamesInCurDir.Count - 1)
            {
                for (int i = beginIndex; i <= endIndex; i++)
                {
                    ThumbBox picTemp = null;
                    if (mDicPicBoxCache.ContainsKey(i) == false)
                    {
                        picTemp = CreateThumbBox(GetThumbBoxPos(i), i, null);
                        mDicPicBoxCache.Add(i, picTemp);
                    }
                }
            }

            mBeginIndex = beginIndex;
            mEndIndex = endIndex;

            RemoveMoreThumbBoxes();

            Invalidate();
            
        }

        private void RemoveMoreThumbBoxes()
        {
            if (mDicPicBoxCache.Count > mMaxCacheCnt)
            {
                int delBeginIndex = mBeginIndex - 1 - mMaxCacheCnt;
                if (delBeginIndex < 0) delBeginIndex = 0;
                int cnt = 0;
                for (int i = delBeginIndex; i < mBeginIndex - 1; i++)
                {
                    if (mDicPicBoxCache.ContainsKey(i))
                    {

                        mDicPicBoxCache.Remove(i);
                        cnt++;
                    }
                }
                if (cnt < mMaxCacheCnt)
                {
                    int remain = mMaxCacheCnt - cnt;
                    int delEndIndex = mEndIndex + 1 + remain;
                    if (delEndIndex > mlFileNamesInCurDir.Count - 1) delEndIndex = mlFileNamesInCurDir.Count - 1;
                    for (int i = (mEndIndex + 1); i <= delEndIndex; i++)
                    {
                        if (mDicPicBoxCache.ContainsKey(i))
                        {
                            mDicPicBoxCache.Remove(i);

                        }
                    }
                }
            }
        }
        private void MoveThumbBoxes(int delta)
        {
            foreach(KeyValuePair<int,ThumbBox> kvp in mDicPicBoxCache)
            {
                kvp.Value.Left -= delta;
            }
        }
        private ThumbBox GetThumbPicBox(int idx)
        {
            if (mDicPicBoxCache.ContainsKey(idx))
                return mDicPicBoxCache[idx];
            else
            {
                ThumbBox picTemp = CreateThumbBox(GetThumbBoxPos(idx), idx, null);
                mDicPicBoxCache.Add(idx, picTemp);
                return picTemp;
            }
        }

        private Point GetRowColCntInHor()
        {
            int filesCnt = this.mlFileNamesInCurDir.Count;
            int clientH = GetClientHeight();
            int rowCnt = clientH / this.ActureThumbHeight;
            if (rowCnt == 0) rowCnt = 1;
            int colCnt = filesCnt / rowCnt;
            if (filesCnt % rowCnt > 0) colCnt++;

            return new Point(colCnt, rowCnt);
        }
        private Point GetRowColCntInVer()
        {
            int filesCnt = this.mlFileNamesInCurDir.Count;
            int clientW = GetClientWidth();
            int colCnt = clientW / this.ActualThumbWidth;
            if (colCnt == 0) colCnt = 1;
            int rowCnt = filesCnt / colCnt;
            if (filesCnt % colCnt > 0) rowCnt++;

            return new Point(colCnt, rowCnt);
        }
        private int GetFilePosByPos(int pos)
        {
            int temp = Math.Abs(pos);
            if (mItemLayout == EnumListItemLayout.Horizontal)
            {
                int filesCnt = this.mlFileNamesInCurDir.Count;
                int clientH = GetClientHeight();
                int rowCnt = clientH / this.ActureThumbHeight;
                if (rowCnt == 0) rowCnt = 1;
                int col = temp / this.ActualThumbWidth;
                return col * rowCnt;
            }
            else if (mItemLayout == EnumListItemLayout.Vertical)
            {
                int filesCnt = this.mlFileNamesInCurDir.Count;
                int clientW = GetClientWidth();
                int colCnt = clientW / this.ActualThumbWidth;
                if (colCnt == 0) colCnt = 1;
                int row = temp / this.ActureThumbHeight;
                return row * colCnt;
            }
            return -1;
        }
        private Point GetThumbBoxRowCol(int idx)
        {
            int col = -1, row = -1;
           
            if (mItemLayout == EnumListItemLayout.Horizontal)
            {
                Point crCnt = GetRowColCntInHor();
                //求所在的列
                col = idx / crCnt.Y;
                row = idx % crCnt.Y;
            }
            else if (mItemLayout == EnumListItemLayout.Vertical)
            {
                Point crCnt = GetRowColCntInVer();
                //求所在的行
                row = idx / crCnt.X;
                col = idx % crCnt.X;

            }
        
            return new Point(col,row);
        }
        private Point GetThumbBoxPos(int idx)
        {
            int x = -1, y = -1;
            Point rc = GetThumbBoxRowCol(idx);
            if (rc.X==-1) return new Point(-1,-1);
            if (mItemLayout == EnumListItemLayout.Horizontal)
            {
                
                x = rc.X * this.ActualThumbWidth + mInterval;
                y = mPaddingTop + rc.Y * this.ActureThumbHeight;
               
            }else if (mItemLayout == EnumListItemLayout.Vertical)
            {

                x = mPaddingLeft + this.ActualThumbWidth *rc.X;
                y = rc.Y * this.ActureThumbHeight + mInterval;
                
            }
           
            return new Point(x, y);
        }
   
    }
}
