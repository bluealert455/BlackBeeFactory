using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommonLib;
using PhotoHome;
using PropertyPages;
using Shell32;
using ThirdPlugins;

namespace SuperControls
{
    public delegate void FileInfoChangedHandler(List<TextLine> infoFlds,object propInfo);
    public class SuperDataPanel : SuperPictureBox
    {
        enum EnumViewModel
        {
            FitToWindow = 1,
            RealSize = 2,
            Zoomed = 3
        }

        
        private double mCanvasHeight = -1, mCanvasWidth = -1;
        private EnumViewModel mViewModel;
        
        private frmArrow mLeftArrowForm = null;
        private frmArrow mRightArrowForm = null;
        private EnumFileType mCurFileType = EnumFileType.None;

        private ThumbList mThumbnailList = null;
        private String mCurFileName,mCurrentPath;
        private VLCPlayer mMediaPlayer = null;

        private FileInfoChangedHandler mFileInfoChangedHandler = null;
        private VoidAndNonParamHandler mBeforeLoadFileHandler = null;
        private VoidAndNonParamHandler mMediaPlayingHandler = null;
        private VoidAndNonParamHandler mMediaStoppedHandler = null;
        private VoidAndNonParamHandler mMediaPausedHandler = null;
        private Utils.FileCopyCompleteHandler mFileSelectedHandler = null;

        private Dictionary<string, int> mDicCopyTaskQueues = new Dictionary<string, int>();
        private bool mAutoPlay = false;
        static SuperDataPanel()
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
                if (base.BackColor != value)
                {
                    base.BackColor = value;
                    Invalidate();
                }

            }
        }


        public SuperDataPanel()
        {
            if (DesignMode)
                return;
            mViewModel = EnumViewModel.FitToWindow;

            
            this.Resize += SuperDataPanel_Resize;
            
            mLeftArrowForm = new frmArrow();

            mLeftArrowForm.Setup(PhotoHome.ArrowDirection.Left, this);
            mLeftArrowForm.MouseClick += MLeftArrowForm_MouseClick;
            mRightArrowForm = new frmArrow();
            mRightArrowForm.Setup(PhotoHome.ArrowDirection.Right, this);
            mRightArrowForm.MouseClick += MRightArrowForm_MouseClick;

            InitializeComponent();
        }

        private void InitializeComponent()
        {
            
            this.SuspendLayout();
            mMediaPlayer = new VLCPlayer();
            mMediaPlayer.BeginInit();
            this.mMediaPlayer.BackColor = System.Drawing.Color.Black;
            this.mMediaPlayer.Dock = System.Windows.Forms.DockStyle.Fill;
            
            this.mMediaPlayer.Name = "mMediaPlayer";
            this.mMediaPlayer.OnMediaPaused += MMediaPlayer_OnMediaPaused;
            this.mMediaPlayer.OnMediaPlaying += MMediaPlayer_OnMediaPlaying;
            this.mMediaPlayer.OnMediaStopped += MMediaPlayer_OnMediaStopped;
            this.Controls.Add(this.mMediaPlayer);
            mMediaPlayer.EndInit();
            this.ResumeLayout(false);

            mMediaPlayer.Visible = false;
        }

        private void MMediaPlayer_OnMediaStopped()
        {
            if (mMediaStoppedHandler != null)
                mMediaStoppedHandler();
        }

        private void MMediaPlayer_OnMediaPlaying()
        {
            if (mMediaPlayingHandler != null)
                mMediaPlayingHandler();
        }

        private void MMediaPlayer_OnMediaPaused()
        {
            if (mMediaPausedHandler != null)
                mMediaPausedHandler();
        }
        public event Utils.FileCopyCompleteHandler OnFileSelected
        {
            add
            {
                lock (this)
                {
                    mFileSelectedHandler += value;
                }
            }
            remove
            {
                lock (this)
                {
                    if (mFileSelectedHandler != null)
                        mFileSelectedHandler -= value;
                }
            }
        }

        public event FileInfoChangedHandler OnFileInfoChanged
        {
            add
            {
                lock (this)
                {
                    mFileInfoChangedHandler += value;
                }
            }
            remove
            {
                lock (this)
                {
                    if (mFileInfoChangedHandler != null)
                        mFileInfoChangedHandler -= value;
                }
            }
        }

        public event VoidAndNonParamHandler OnBeforeLoadFile
        {
            add
            {
                lock (this)
                {
                    mBeforeLoadFileHandler += value;
                }
            }
            remove
            {
                lock (this)
                {
                    if (mBeforeLoadFileHandler != null)
                        mBeforeLoadFileHandler -= value;
                }
            }
        }
        public event VoidAndNonParamHandler OnMediaPlaying
        {
            add
            {
                lock (this)
                {
                    mMediaPlayingHandler += value;
                }
            }
            remove
            {
                lock (this)
                {
                    if (mMediaPlayingHandler != null)
                        mMediaPlayingHandler -= value;
                }
            }
        }
        public event VoidAndNonParamHandler OnMediaStopped
        {
            add
            {
                lock (this)
                {
                    mMediaStoppedHandler += value;
                }
            }
            remove
            {
                lock (this)
                {
                    if (mMediaStoppedHandler != null)
                        mMediaStoppedHandler -= value;
                }
            }
        }
        public event VoidAndNonParamHandler OnMediaPaused
        {
            add
            {
                lock (this)
                {
                    mMediaPausedHandler += value;
                }
            }
            remove
            {
                lock (this)
                {
                    if (mMediaPausedHandler != null)
                        mMediaPausedHandler -= value;
                }
            }
        }
        public EnumFileType CurFileType
        {
            get
            {
                return mCurFileType;
            }
        }

        public EnumMediaPlayStatus CurrentPlayStatus
        {
            get
            {
                return mMediaPlayer.CurrentPlayStatus;
            }
        }

        public bool AutoPlay { get => mAutoPlay; set => mAutoPlay = value; }

        public void PlayOrPause()
        {
            
            mMediaPlayer.PlayOrPause();
        }
        public void Stop()
        {
            mMediaPlayer.Stop();

        }

        public void SwitchFullScreen(bool toFull)
        {
            mMediaPlayer.SwitchFullScreen(toFull);
        }
        public void TakeSnapshot(string targetFolder)
        {
            Common.EnsureDirExisted(targetFolder);
            string shortFileName = Path.GetFileNameWithoutExtension(mCurFileName);
            string targetFileName = null;
            for(int i = 1; i < 1000; i++)
            {
                targetFileName = Path.Combine(targetFolder, shortFileName + "_" + i.ToString() + ".jpg");
                if (File.Exists(targetFileName) == false)
                {
                    
                    break;
                }
            }
            mMediaPlayer.TaskSnapshotAsync(targetFileName);
        }
        public void SetCurThumbList(ThumbList thumbList)
        {
            mThumbnailList = thumbList;
        }
        private void MLeftArrowForm_MouseClick(object sender, MouseEventArgs e)
        {
            PreviousFile();
        }

        private void MRightArrowForm_MouseClick(object sender, MouseEventArgs e)
        {
            NextFile();
        }

        
        public void SaveImage(string targetFolder=null)
        {
            if (targetFolder == null)
            {
                CloseStreamReader();
                mImage.Save(mCurFileName);
            }
            else
            {
                Common.EnsureDirExisted(targetFolder);
                string targetFile = Path.Combine(targetFolder, Path.GetFileName(mCurFileName));
                mImage.Save(targetFile);
            }
            
        }

        protected override void OnPaint(PaintEventArgs e)
        {
            switch (mCurFileType)
            {
                case EnumFileType.Image:
                    base.OnPaint(e);
                    break;
                case EnumFileType.Video:
                    break;
            }

            Pen borderPen = new Pen(mBorderColor, mBorderWidth);
            Rectangle borderRect = new Rectangle(1, 1, this.Width - 2, this.Height - 2);
            e.Graphics.DrawRectangle(borderPen, borderRect);
            borderPen.Dispose();
        }
        private void SuperDataPanel_Resize(object sender, EventArgs e)
        {
            mCanvasWidth = this.Width;
            mCanvasHeight = this.Height;
        }
        
        
        public bool LoadDataFromFile(String fileName)
        {
            if (File.Exists(fileName) == false)
            {
                Utils.MessageUIQueue.ShowMsgForm("loaddatafromfile", MessageBoxIcon.Error, "文件["+fileName+"]不存在，可能是因为文件被删除或者其所在U盘被拔出。");
                return false;
            }
            if (fileName == mCurFileName)
                return true;
            mCurFileName = fileName;
            mCurFileType = PhotoHome.Global.GetFileType(fileName);
            mCurrentPath = Path.GetFullPath(fileName);
            Clear();

            List<TextLine> tmpFlds = ReadFileInfo();
            object fileInfo = null;
            switch (mCurFileType)
            {
                case EnumFileType.Image:
                    HideMediaPlayer();
                    LoadImage(fileName); 
                    tmpFlds.AddRange(ReadImageInfo());
                    fileInfo = CreateImageProp();
                    
                    break;
                case EnumFileType.Video:
                    LoadMedia(fileName);
                    tmpFlds.AddRange(ReadMediaInfo());
                    fileInfo = CreateFileProp();
                    break;
                case EnumFileType.Audio:
                    LoadMedia(fileName);
                    tmpFlds.AddRange(ReadAudioInfo());
                    fileInfo = CreateFileProp();
                    break;
                case EnumFileType.Tif:
                    break;
                case EnumFileType.Shp:
                    break;
                default:
                    break;
            }

            if (mFileInfoChangedHandler != null)
                mFileInfoChangedHandler(tmpFlds, fileInfo);
            return true;
        }
        public override void Clear()
        {
            base.Clear();
            
        }

        public void HideMediaPlayer()
        {
            if (mMediaPlayer.Visible == true)
            {
                mMediaPlayer.Stop();
                mMediaPlayer.Visible = false;
            }
        }
        private void LoadMedia(string fileName)
        {
            if(mMediaPlayer.Visible==false)
                mMediaPlayer.Visible = true;
            mMediaPlayer.Stop();
            if (mThumbnailList != null)
            {
                string bgFile = mThumbnailList.GetBGFile(fileName);
                if (File.Exists(bgFile) == false)
                    bgFile = mThumbnailList.GetThumbFileName(fileName);
                mMediaPlayer.SetBGImage(bgFile);
                mMediaPlayer.SetMedia(fileName);

                if (mAutoPlay) mMediaPlayer.Play();
                
            }
        }
        protected ImageProp CreateImageProp()
        {
            ImageProp prop = new ImageProp();
            prop.Width = mImage.Width;
            prop.Height = mImage.Height;
            prop.Contrast = 0;
            prop.Brightness = 0;
            prop.FileType = Path.GetExtension(mCurFileName);

            return prop;
        }
        protected FileProp CreateFileProp()
        {
            FileProp prop = new FileProp();
            prop.FileType = Path.GetExtension(mCurFileName);
            return prop;
        }

        
        public bool OpenWithDialog()
        {
            OpenFileDialog fileDlg = new OpenFileDialog
            {
                Filter = ("所有支持的格式|" + PhotoHome.Global.mAllSurpportedFilter)
            };
            
            if (fileDlg.ShowDialog(this.Parent) == DialogResult.OK)
            {
                string fileName = fileDlg.FileName;
                if (mBeforeLoadFileHandler != null)
                    mBeforeLoadFileHandler();
                if (mThumbnailList != null)
                {
                    mThumbnailList.LoadFileContext(fileName);
                }
                LoadDataFromFile(fileName);

                return true;
            }
            return false;
        }

        public bool OpenWithFolderDlg()
        {
            FolderBrowserDialog dlg = new FolderBrowserDialog();
            dlg.Description = "请选择文件夹";
            if (dlg.ShowDialog(this.Parent) == DialogResult.OK)
            {
                LoadFolder(dlg.SelectedPath);
                return true;
            }
            return false;
        }

        public void LoadFolder(string folder)
        {
            if (mBeforeLoadFileHandler != null)
                mBeforeLoadFileHandler();
            if (mThumbnailList != null)
            {
                mThumbnailList.LoadFolder(folder);
                LoadDataFromFile(mThumbnailList.mCurFileName);
            }
        }
        private TextLine CreateTextLine(String lbl,String content)
        {
            return new TextLine(new TextField(lbl), new TextField(content));
        }

        public List<TextLine> ReadFileInfo()
        {
            List<TextLine> allLines = new List<TextLine>();
            TextLine commonLine = new TextLine(new SectionTitle("常规"));
            commonLine.AddSubLine(CreateTextLine("位置:", Path.GetDirectoryName(mCurFileName)));
            if (mThumbnailList != null)
            {
                commonLine.AddSubLine(CreateTextLine("数目:", "共" + mThumbnailList.CurrentFileNames.Count.ToString() + "张文件，当前为第" + (mThumbnailList.CurrentFileIndex + 1).ToString() + "个"));
            }
            commonLine.AddSubLine(CreateTextLine("名称:", Path.GetFileName(mCurFileName)));
            commonLine.AddSubLine(CreateTextLine("文件大小:", CommonLib.Common.GetFileSize2Str(mCurFileName)));
            commonLine.AddSubLine(CreateTextLine("创建时间:", File.GetCreationTime(mCurFileName).ToLongDateString()));
            commonLine.AddSubLine(CreateTextLine("修改时间:", File.GetLastWriteTime(mCurFileName).ToLongDateString()));
            
            allLines.Add(commonLine);

            return allLines;
        }
        public List<TextLine> ReadImageInfo()
        {
            if (mImage == null) return null;
            
            Dictionary<string, string> dic = CreatePhotoMetadataDic();
            Dictionary<int, string> dicIdTitle = CreateIdAndTitleDic();
            SetStringDicValue(dic, "尺寸(像素)", mImage.Width.ToString() + "*" + mImage.Height.ToString());
            SetStringDicValue(dic, "分辨率(DPI)", "水平" + ((int)mImage.HorizontalResolution).ToString() + "，垂直" + ((int)mImage.VerticalResolution).ToString());
            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < mImage.PropertyItems.Length; i++)
            {
                PropertyItem pi = mImage.PropertyItems[i];

                if (dicIdTitle.ContainsKey(pi.Id))
                {
                    SetStringDicValue(dic, dicIdTitle[pi.Id], ImagePIValue2Str(pi));
                }
            }
            
            return GenerateTextLines(dic);

        }

        private List<TextLine> ReadMediaInfo()
        {

            Dictionary<string, string> dicInfo =VLCMultimedia.GetMediaInfo(mCurFileName);
            return GenerateTextLines(dicInfo);

        }

        private List<TextLine> ReadAudioInfo()
        {

            Dictionary<string, string> dicInfo = VLCMultimedia.GetAudioInfo(mCurFileName);
            if (mThumbnailList != null)
            {
                dicInfo["cover_img"] = mThumbnailList.GetThumbFileName(mCurFileName);
            }
            
            return GenerateTextLines(dicInfo);

        }
        private List<TextLine> GenerateTextLines(Dictionary<string,string> srcDic)
        {
            List<TextLine> allLines = new List<TextLine>();
            TextLine sectionLine = null;
            foreach (KeyValuePair<String, String> kvp in srcDic)
            {
                String sKey = kvp.Key;
                if (sKey.IndexOf("section") >= 0)
                {
                    if (sectionLine != null)
                    {
                        allLines.Add(sectionLine);
                    }
                    sectionLine = new TextLine(new SectionTitle(kvp.Value));
                }
                else
                {
                    if (kvp.Value!=null&&kvp.Value.Length > 0)
                    {
                        sectionLine.AddSubLine(CreateTextLine(sKey + ":", kvp.Value));
                    }
                }
            }
            if (sectionLine != null&&sectionLine.HasSublines==true)
            {
                allLines.Add(sectionLine);
            }
            return allLines;
        }
        private void SetStringDicValue(Dictionary<String, String> dic, String key, String val)
        {
            if (dic.ContainsKey(key))
            {
                dic[key] = val;
            }
        }

        private String ImagePIValue2Str(PropertyItem pi)
        {
            String sVal = null;
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            switch (pi.Type)
            {
                case 1:
                    sVal = pi.Value[0].ToString();
                    break;
                case 2:
                    sVal = encoding.GetString(pi.Value);
                    break;
                case 3:
                    if (pi.Len < 4)
                    {
                        byte[] valTemp = new byte[4];
                        pi.Value.CopyTo(valTemp, 0);
                        sVal = BitConverter.ToInt32(valTemp, 0).ToString();
                    }
                    else
                        sVal = BitConverter.ToInt32(pi.Value, 0).ToString();
                    break;
                case 4:
                    if (pi.Len < 8)
                    {
                        byte[] valTemp = new byte[8];
                        pi.Value.CopyTo(valTemp, 0);
                        sVal = BitConverter.ToInt32(valTemp, 0).ToString();
                    }
                    else
                        sVal = BitConverter.ToInt64(pi.Value, 0).ToString();
                    break;
                case 5:
                    Byte[] tempStart = new byte[4] { pi.Value[0], pi.Value[1], pi.Value[2], pi.Value[3] };
                    Int32 num1 = BitConverter.ToInt32(tempStart, 0);
                    Int32 num2 = BitConverter.ToInt32(pi.Value, 4);

                    sVal = num1.ToString() + "/" + num2.ToString();
                    break;
                case 6:
                    break;
                case 7:
                    break;
                case 8:
                    break;
                case 9:
                    break;
                case 10:
                    break;
            }
            if (sVal != null && sVal.Length >= 1)
            {
                sVal = sVal.Substring(0, sVal.Length - 1);
            }

            return sVal;
        }

        public void ShowNavWindow()
        {
            if (mLeftArrowForm.Visible == false)
                mLeftArrowForm.Show(this);
            if (mRightArrowForm.Visible == false)
                mRightArrowForm.Show(this);
        }
        public void HideNavWindow()
        {
            if (mLeftArrowForm.Visible == true)
                mLeftArrowForm.Hide();
            if (mRightArrowForm.Visible == true)
                mRightArrowForm.Hide();
        }
        /// <summary>
        /// 把当前文件拷贝到剪贴板
        /// </summary>
        /// <param name="cut">是否剪切</param>
        public void CopyCurFileToCB(bool cut)
        {
            Clipboard.Clear();
            StringCollection fileCopied = new StringCollection();
            switch (mCurFileType)
            {
                case EnumFileType.Tif:
                    break;
                case EnumFileType.Shp:
                    break;
                case EnumFileType.Image:
                    fileCopied.Add(mCurFileName);
                    break;
                case EnumFileType.Video:
                    break;
            }

            byte[] moveEffect = new byte[] { 5, 0, 0, 0 };
            if (cut == true)
                moveEffect[0] = 2;
            MemoryStream dropEffect = new MemoryStream();
            dropEffect.Write(moveEffect, 0, moveEffect.Length);

            DataObject data = new DataObject();
            data.SetFileDropList(fileCopied);
            data.SetData("Preferred DropEffect", dropEffect);


            Clipboard.SetDataObject(data, true);
        }
        /// <summary>
        /// 把文件拷贝到目标文件夹
        /// </summary>
        /// <param name="targetFolder"></param>
        /// <param name="cut"></param>
        public void CopyCurFileToFolder(string targetFolder, bool cut = false)
        {
            Common.EnsureDirExisted(targetFolder);
            string targetFile = Path.Combine(targetFolder, Path.GetFileName(mCurFileName));
            DoFileCopyOrMove(mCurFileName, targetFile, cut,null);
        }

        public void SelectCurFileToFolder(string targetFolder,bool cut=false)
        {
            Common.EnsureDirExisted(targetFolder);
            string targetFile = Path.Combine(targetFolder, Path.GetFileName(mCurFileName));
            
            DoFileCopyOrMove(mCurFileName, targetFile, false, mFileSelectedHandler);
        }

        private bool IsFileCoping(string tgtFile)
        {
            lock (mDicCopyTaskQueues)
            {
                return mDicCopyTaskQueues.ContainsKey(tgtFile);
            }
        }
        private void PushToFileCopingQueue(string tgtFile)
        {
            lock (mDicCopyTaskQueues)
            {
                if (mDicCopyTaskQueues.ContainsKey(tgtFile) == false)
                    mDicCopyTaskQueues.Add(tgtFile, 1);
            }
        }

        private void DelFromFileCopingQueue(string tgtFile)
        {
            lock (mDicCopyTaskQueues)
            {
                if (mDicCopyTaskQueues.ContainsKey(tgtFile) == true)
                    mDicCopyTaskQueues.Remove(tgtFile);
            }
        }

        private void DoFileCopyOrMove(string srcFile, string targetFile, bool move,Utils.FileCopyCompleteHandler callback)
        {
            if (IsFileCoping(targetFile) == true) return;

                
            string tempFile = null;
            if (File.Exists(targetFile) == true)
            {
                frmOverwrite owDlg = new frmOverwrite();
                owDlg.Init(srcFile, targetFile);
                owDlg.StartPosition = FormStartPosition.CenterScreen;
                DialogResult dr = owDlg.ShowDialog(this.Parent);
                
                switch (dr)
                {
                    case DialogResult.OK:
                        tempFile = targetFile;
                        File.Delete(targetFile);
                        break;
                    case DialogResult.Cancel:
                        tempFile = null;
                        break;
                    case DialogResult.Retry:
                        tempFile = owDlg.NewName;
                        break;
                }
               
            }
            else
            {
                tempFile = targetFile;
            }
            if(tempFile!=null)
                StartCopyOrMove(srcFile, tempFile, move,callback);
        }

        private void StartCopyOrMove(string srcFile,string tgtFile,bool move,Utils.FileCopyCompleteHandler callback)
        {
            PushToFileCopingQueue(tgtFile);
            string name = Path.GetFileName(srcFile);

            Utils.ProgressUIQueue.ShowProgressForm(tgtFile, move ? "正在移动文件["+ name+"]，请稍等..." : "正在复制文件["+ name+"]，请稍等...");
            
            Task.Run(() =>
            {
                if (move)
                    File.Move(srcFile, tgtFile);
                else
                    File.Copy(srcFile, tgtFile);
                DelFromFileCopingQueue(tgtFile);
                if (mFileSelectedHandler != null)
                    mFileSelectedHandler(srcFile, tgtFile, false);
                Utils.ProgressUIQueue.UpdatePrompt(tgtFile, move ? "文件[" + name + "]移动完成" : "文件[" + name + "]复制完成");
                Utils.ProgressUIQueue.CloseProgressForm(tgtFile);
                
            });
        }
        public bool IsCurrentFileSelected(string targetFolder)
        {
            return Common.IsFileInAnotherFolder(mCurFileName, targetFolder);
        }
        public bool DeleteCurSelected(string targetFolder)
        {
            string targetFile = Path.Combine(targetFolder, Path.GetFileName(mCurFileName));
            try
            {
                if (File.Exists(targetFile))
                {
                    File.Delete(targetFile);
                }
                return true;
            }
            catch {
                return File.Exists(targetFile);
            }
            
        }
        public void CutCurrentFile()
        {

        }

        public void DeleteCurFile()
        {
            string tempFile = mCurFileName;
            //回收站机制
            string recycleDir = Path.Combine(Path.GetDirectoryName(mCurFileName), "deleted");
            Common.EnsureDirExisted(recycleDir);
            switch (mCurFileType)
            {
                case EnumFileType.Tif:
                    break;
                case EnumFileType.Shp:
                    break;
                case EnumFileType.Image:
                    CloseStreamReader();
                    File.Move(mCurFileName, Path.Combine(recycleDir, Path.GetFileName(mCurFileName)));
                    break;
                case EnumFileType.Video:
                case EnumFileType.Audio:
                    mMediaPlayer.Stop();
                    File.Move(mCurFileName, Path.Combine(recycleDir, Path.GetFileName(mCurFileName)));
                    break;
                    
            }
            if (mThumbnailList != null)
                mThumbnailList.DelThumbByName(mCurFileName);

            Utils.MessageUIQueue.ShowMsgForm("DelCurFile", MessageBoxIcon.Information, "删除[" + Path.GetFileName(tempFile) + "]成功！");
        }
        private void DoDeleteFile(string fileName)
        {
            
        }
        public void PasteFile2CurPath()
        {
            StringCollection filesPasted = Clipboard.GetFileDropList();
            bool dealed = false;
            String firstFile = null;
            EnumClipboardAction action = Common.GetClipboardAction();
            List<String> filesCreated = new List<string>();
            for (int i = 0; i < filesPasted.Count; i++)
            {
                String filePasted = filesPasted[i];
                String targetFile = Path.Combine(mCurrentPath, Path.GetFileName(filePasted));
                EnumFileType fileType = PhotoHome.Global.GetFileType(filePasted);

                switch (mCurFileType)
                {
                    case EnumFileType.Tif:

                        break;
                    case EnumFileType.Shp:
                        break;
                    case EnumFileType.Image:
                        dealed = true;
                        DoFileCopyOrMove(filePasted, targetFile, (action==EnumClipboardAction.Cut) ? true : false,null);
                        break;
                    case EnumFileType.Video:
                        break;
                }
                if (firstFile == null)
                    firstFile = targetFile;
                if (dealed)
                    filesCreated.Add(targetFile);
            }

            LoadDataFromFile(firstFile);
            if (mThumbnailList != null)
            {
                mThumbnailList.LoadFileContext(firstFile);

            }
        }

        public void NextFile()
        {
            if (mThumbnailList != null)
                mThumbnailList.NextFile();
        }
        public void PreviousFile()
        {
            if (mThumbnailList != null)
                mThumbnailList.PreviousFile();
        }
    }
    public class FileChangedEvantArgs
    {
        public String CurrentFileName;
        public List<String> CurrentFileNames;
        public int CurrtentPos;
        public Image CurrentImage;
        public List<String> FilesCreated;
        public List<String> FilesDeleted;
        public List<String> FilesRenamed;
    }

    public class FileNameComparer : System.Collections.IComparer
    {
        public int Compare(object x, object y)
        {
            Shell32.FolderItem fix = x as Shell32.FolderItem;
            Shell32.FolderItem fiy = y as Shell32.FolderItem;

            return fix.Name.CompareTo(fiy.Name);
        }
    }
}
