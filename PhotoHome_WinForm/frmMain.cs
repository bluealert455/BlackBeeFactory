using CommonLib;
using PropertyPages;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Management;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SuperControls;

namespace PhotoHome
{
    public enum EnumPerspective
    {
        Classify=1,
        Modern =2,
        Simple=3
    }
    public partial class frmMain : Form
    {
        //private frmToolbarPanel mToolbarPanel = null;
        private SuperToolbarWindow mToolbarWindow = null;
        private SuperToolbarWindow mLayoutToolbarWindow = null;
        public frmThumb mThumbLeft = null;
        private frmThumb mThumbRight = null;
        private frmInfoPanel mInfoWindow = null;
        private frmTaskPanel mTaskPanel = null;
        private int mVerInterval = 5;
        private int mHorInterval = 5;
        private int mLeftPanelLeft = 0;
        private int mLeftPanelWidth = 300;
        private frmFileBrowser mFileBrowser = null;
        private EnumPerspective mPerspective = EnumPerspective.Simple;
        private bool mIsFullScreen = false;
        
        private System.Timers.Timer mSystemDirWatcherTimer = null;
        private Dictionary<string, string> mDicRemovableDrives = new Dictionary<string, string>();

        private frmUSBPrompt mUSBPromptWindow = null;
        public string PathFromArg = null;
        private bool mUsbMonitorEnabled = true;
        
        public frmMain()
        {
            InitializeComponent();
            this.Padding = new Padding(0);
            this.BackColor = Color.Black;
            CreateToolbarWindow();
            this.mainDataPanel.OnFileInfoChanged += MainDataPanel_OnFileInfoChanged;
            this.mainDataPanel.OnBeforeLoadFile += MainDataPanel_OnBeforeLoadFile;
            this.mainDataPanel.OnFileSelected += MainDataPanel_OnFileSelected;
            this.mainDataPanel.OnMediaPlaying += MainDataPanel_OnMediaPlaying;
            this.mainDataPanel.OnMediaPaused += MainDataPanel_OnMediaPaused;
            this.mainDataPanel.OnMediaStopped += MainDataPanel_OnMediaStopped;
            mThumbLeft = new frmThumb();
            if (mInfoWindow != null)
                return;
            mInfoWindow = new frmInfoPanel();
       
            this.Shown += FrmMain_Shown;
         
            mTaskPanel = new frmTaskPanel();
            mTaskPanel.MainForm =this;
            
           
            this.FormClosed += FrmMain_FormClosed;
            this.Load += FrmMain_Load;
            
            mThumbLeft.thumbnailList.DataPanel = this.mainDataPanel;
            mThumbLeft.thumbnailList.OnKeyUpEx += ThumbnailList_OnKeyUpEx;
            mainDataPanel.SetCurThumbList(mThumbLeft.thumbnailList);

            //mSplashWindow = new frmSplash();
            //mSplashWindow.MainForm = this;

            mFileBrowser = new frmFileBrowser();
            ScanUsbDrives(true);

            mSystemDirWatcherTimer = new System.Timers.Timer(1000);
            mSystemDirWatcherTimer.Elapsed += MSystemDirWatcherTimer_Elapsed;

            mUSBPromptWindow = new frmUSBPrompt();
            mUSBPromptWindow.MainForm = this;
        }

        private void ThumbnailList_OnKeyUpEx(object sender, KeyEventArgs e)
        {
            WhenKeyUp(sender, e);
        }

        private SuperToolBarItem mtbPasteBtn = null;
        private SuperToolBarItem mtbSelectBtn = null;
        private SuperToolBarItem mtbFlipHorBtn = null;
        private SuperToolBarItem mtbFlipVerBtn = null;
        private SuperToolBarItem mtbRotateImageLeftBtn = null;
        private SuperToolBarItem mtbRotateImageRightBtn = null;
        private SuperToolBarItem mtbRealSizeBtn = null;
        private SuperToolBarItem mtbFullExtentBtn = null;
        private SuperToolBarItem mtbSaveBtn = null;
        private SuperToolBarItem mtbPlayBtn = null;
        private SuperToolBarItem mtbStopBtn = null;
        private SuperToolBarItem mtbSnapshotBtn = null;
        private SuperToolBarItem mtbSlideAutoBtn = null;
        private SuperToolBarItem mtbLayoutBtn = null;
        private void CreateToolbarWindow()
        {
            mToolbarWindow = new SuperToolbarWindow();
            mToolbarWindow.OnItemClick += MToolbarWindow_OnItemClick;
            mToolbarWindow.KeyUp += MToolbarWindow_KeyUp;
            SuperToolBarItem newItem = new SuperToolBarItem("tbOpen", "选择文件夹或文件", PhotoHome.Properties.Resources.tb_open);
            mToolbarWindow.AddItem(newItem);
            mtbPasteBtn = new SuperToolBarItem("tbPaste", "粘贴", PhotoHome.Properties.Resources.tb_paste);
            mToolbarWindow.AddItem(mtbPasteBtn);
            mtbPasteBtn.Enabled = this.mainDataPanel.CanPaste();
            newItem = new SuperToolBarItem();
            newItem.Type = EnumToolBarItemType.Seperator;
            mToolbarWindow.AddItem(newItem);

            mtbFullExtentBtn = new SuperToolBarItem("tbFullExtent", "全图显示", PhotoHome.Properties.Resources.tb_fullextent);
            mToolbarWindow.AddItem(mtbFullExtentBtn);
            mtbRealSizeBtn = new SuperToolBarItem("tbRealSize", "实际尺寸", PhotoHome.Properties.Resources.tb_realsize);
            mToolbarWindow.AddItem(mtbRealSizeBtn);
            mtbRotateImageLeftBtn = new SuperToolBarItem("tbRotateLeft", "逆时针旋转90度", PhotoHome.Properties.Resources.tb_rotateleft);
            mToolbarWindow.AddItem(mtbRotateImageLeftBtn);
            mtbRotateImageRightBtn = new SuperToolBarItem("tbRotateRight", "顺时针旋转90度", PhotoHome.Properties.Resources.tb_rotateright);
            mToolbarWindow.AddItem(mtbRotateImageRightBtn);
            mtbFlipHorBtn = new SuperToolBarItem("tbFlipHor", "水平翻转", PhotoHome.Properties.Resources.tb_fliphor);
            mToolbarWindow.AddItem(mtbFlipHorBtn);
            mtbFlipVerBtn = new SuperToolBarItem("tbFlipVer", "垂直翻转", PhotoHome.Properties.Resources.tb_flipver);
            mToolbarWindow.AddItem(mtbFlipVerBtn);

            newItem = new SuperToolBarItem();
            newItem.Type = EnumToolBarItemType.Seperator;
            mToolbarWindow.AddItem(newItem);

            mtbPlayBtn = new SuperToolBarItem("tbPlayOrPause", "播放或暂停", PhotoHome.Properties.Resources.tb_play);
            mToolbarWindow.AddItem(mtbPlayBtn);
            mtbStopBtn = new SuperToolBarItem("tbStop", "停止", PhotoHome.Properties.Resources.tb_stop);
            mToolbarWindow.AddItem(mtbStopBtn);
            mtbSnapshotBtn = new SuperToolBarItem("tbSnapshot", "视频截屏", PhotoHome.Properties.Resources.tb_snapshot);
            mToolbarWindow.AddItem(mtbSnapshotBtn);

            newItem = new SuperToolBarItem();
            newItem.Type = EnumToolBarItemType.Seperator;
            mToolbarWindow.AddItem(newItem);

            newItem = new SuperToolBarItem("tbSelect", "收藏", PhotoHome.Properties.Resources.tb_select);
            mToolbarWindow.AddItem(newItem);
            mtbSelectBtn = newItem;
            newItem = new SuperToolBarItem("tbSetFavFolder", "设置收藏夹", PhotoHome.Properties.Resources.tb_favfolder);
            mToolbarWindow.AddItem(newItem);
            mtbSlideAutoBtn = new SuperToolBarItem("tbAutoSlide", "自动切换文件", PhotoHome.Properties.Resources.tb_slidestart);
            mToolbarWindow.AddItem(mtbSlideAutoBtn);
            newItem = new SuperToolBarItem("tbFullScreen", "全屏", PhotoHome.Properties.Resources.tb_fullscreen);
            mToolbarWindow.AddItem(newItem);

            mtbLayoutBtn = new SuperToolBarItem("tbLayout", "设置界面布局", PhotoHome.Properties.Resources.tb_layoutmodern);
            mToolbarWindow.AddItem(mtbLayoutBtn);

            newItem = new SuperToolBarItem();
            newItem.Type = EnumToolBarItemType.Seperator;
            mToolbarWindow.AddItem(newItem);

            mtbSaveBtn = new SuperToolBarItem("tbSave", "保存", PhotoHome.Properties.Resources.tb_save);
            mToolbarWindow.AddItem(mtbSaveBtn);
            newItem = new SuperToolBarItem("tbQuit", "退出", PhotoHome.Properties.Resources.tb_quit);
            mToolbarWindow.AddItem(newItem);
        }

        private void MToolbarWindow_KeyUp(object sender, KeyEventArgs e)
        {
            WhenKeyUp(sender, e);
        }

        private void MToolbarWindow_OnItemClick(UIItem node)
        {
            if (node.Name != "tbLayout")
            {
                if (mLayoutToolbarWindow != null && mLayoutToolbarWindow.Visible)
                {
                    mLayoutToolbarWindow.Hide();
                }
            }
            switch (node.Name)
            {
                case "tbOpen":
                    OpenFolderOrFileDlg();
                    break;
                case "tbPaste":
                    mainDataPanel.PasteFile2CurPath();
                    break;
                case "tbFullExtent":
                    mainDataPanel.FullExtent();
                    break;
                case "tbRealSize":
                    mainDataPanel.ZoomToRealSize();
                    break;
                case "tbRotateLeft":
                    mainDataPanel.RotateImageLeft();
                    break;
                case "tbRotateRight":
                    mainDataPanel.RotateImageRight();
                    break;
                case "tbFlipHor":
                    mainDataPanel.FlipHor();
                    break;
                case "tbFlipVer":
                    mainDataPanel.FlipVer();
                    break;
                case "tbPlayOrPause":
                    mainDataPanel.PlayOrPause();
                    break;
                case "tbStop":
                    mainDataPanel.Stop();
                    break;
                case "tbSnapshot":
                    mainDataPanel.TakeSnapshot(GetSelectTargetFolder());
                    break;
                case "tbSelect":
                    SelectFileToFavorites();
                    break;
                case "tbSetFavFolder":
                    SelectFavFolder();
                    break;
                case "tbAutoSlide":
                    StartOrStopAutoSlide();
                    break;
                case "tbFullScreen":
                    SwitchFullScreen();
                    break;
                case "tbLayout":
                    SetLayout();
                    break;
                case "tbSave":
                    Save();
                    break;
                case "tbQuit":
                    this.Close();
                    break;

            }
        }
        private void MainDataPanel_OnMediaStopped()
        {
            SetMediaBtnsEnabled(false);
            this.InvokeIfRequired(a => {
                mtbPlayBtn.Image = PhotoHome.Properties.Resources.tb_play;
                mToolbarWindow.Redraw();
            });

            
        }

        private void MainDataPanel_OnMediaPaused()
        {
            this.InvokeIfRequired(a => {
                mtbPlayBtn.Image = PhotoHome.Properties.Resources.tb_play;
                mToolbarWindow.Redraw();
            });
        }

        private void MainDataPanel_OnMediaPlaying()
        {
            SetMediaBtnsEnabled(true);
            this.InvokeIfRequired(a => {
                mtbPlayBtn.Image = PhotoHome.Properties.Resources.tb_pause;
                mToolbarWindow.Redraw();
            });
          
        }
        private void SetMediaBtnsEnabled(bool enabled)
        {
            this.InvokeIfRequired(a =>
            {
                mtbPlayBtn.Enabled = enabled;
                mtbSnapshotBtn.Enabled = enabled;
            });

        }
        private void SelectFileToFavorites()
        {
            bool selected = IsCurrentFileSelected();
            if (selected)
            {
                UnselectCurFile();
            }
            else
            {
                SelectCurFile();
            }
        }

        private void SetLayout()
        {
            
            if(mLayoutToolbarWindow==null|| mLayoutToolbarWindow.IsDisposed)
            {
                mLayoutToolbarWindow = new SuperToolbarWindow();
                mLayoutToolbarWindow.OpenFishEye = false;
                mLayoutToolbarWindow.OnItemClick += MLayoutToolbarWindow_OnItemClick;
                mLayoutToolbarWindow.KeyUp += MLayoutToolbarWindow_KeyUp;
                SuperToolBarItem newItem = new SuperToolBarItem("tbLayoutClassify", "传统布局", PhotoHome.Properties.Resources.tb_layoutclassify);
                mLayoutToolbarWindow.AddItem(newItem);
                newItem = new SuperToolBarItem("tbLayoutModern", "时尚布局", PhotoHome.Properties.Resources.tb_layoutmodern);
                mLayoutToolbarWindow.AddItem(newItem);
                newItem = new SuperToolBarItem("tbLayoutSimple", "简约布局", PhotoHome.Properties.Resources.tb_layoutsimple);
                mLayoutToolbarWindow.AddItem(newItem);

                //mToolbarWindow.SizeChanged += MToolbarWindow_SizeChanged;
            }
            if (mLayoutToolbarWindow.Visible == false)
            {
                mLayoutToolbarWindow.BringToFront();
                mLayoutToolbarWindow.Location = new Point(mToolbarWindow.Right - 40, mToolbarWindow.Top);
                mLayoutToolbarWindow.Show(this);
            }
            
        }

        private void MLayoutToolbarWindow_KeyUp(object sender, KeyEventArgs e)
        {
            WhenKeyUp(sender, e);
        }

        private void MToolbarWindow_SizeChanged(object sender, EventArgs e)
        {
            if (mLayoutToolbarWindow != null && mLayoutToolbarWindow.Visible)
            {
                mLayoutToolbarWindow.Location = new Point(mToolbarWindow.Right - 40, mToolbarWindow.Top);
            }
        }

        private void MLayoutToolbarWindow_OnItemClick(UIItem node)
        {
            SuperToolBarItem tbItem = node as SuperToolBarItem;
            mLayoutToolbarWindow.Hide();

            EnumPerspective selectedPers= EnumPerspective.Simple;
            switch (node.Name)
            {
                case "tbLayoutClassify":
                    selectedPers = EnumPerspective.Classify;
                    break;
                case "tbLayoutModern":
                    selectedPers = EnumPerspective.Modern;
                    break;
                case "tbLayoutSimple":
                    selectedPers = EnumPerspective.Simple;
                    break;
            }

            if (selectedPers == Perspective) return;

            mtbLayoutBtn.Image = tbItem.Image;
            Perspective = selectedPers;
            SaveLayoutConfig(Perspective);
            mToolbarWindow.Redraw();
        }
        private void SaveLayoutConfig(EnumPerspective layout)
        {
            CommonLib.AppConfig.Instance.AppLayout = (int)layout;
            CommonLib.AppConfig.Instance.Save();
        }
        private void OpenFolderOrFileDlg()
        {
            UsbMonitorEnabled = false;
            frmFileBrowser fileSelectorDlg = new frmFileBrowser();
            fileSelectorDlg.IsSplash = false;
            this.KeyPreview = false;
            if (fileSelectorDlg.ShowDialog(this) == DialogResult.OK)
            {
                LoadPath(fileSelectorDlg.SelectedPath);

            }
            this.KeyPreview = true;
            UsbMonitorEnabled = true;
        }
        private void SelectFavFolder()
        {
            UsbMonitorEnabled = false;
            frmFolderSelector folderDlg = new frmFolderSelector();
            string prompt = "当前收藏夹:\n" + AppConfig.Instance.SelectedTargetFolder + "\n如果想更改，请在下面选择。";
            folderDlg.SetPrompt(prompt, "点击保存按钮时，把文件存放到收藏夹里。");
            folderDlg.CheckBox1Checked = AppConfig.Instance.SaveToTargetFolder;
            if (folderDlg.ShowDialog(this) == DialogResult.OK)
            {
                AppConfig.Instance.SelectedTargetFolder = folderDlg.SelectedPath;
                AppConfig.Instance.SaveToTargetFolder = folderDlg.CheckBox1Checked;
                AppConfig.Instance.Save();

            }
            UsbMonitorEnabled = true;
        }
        private void SetImageRotateBtnsVisible(bool visible)
        {
            this.mtbFlipHorBtn.Visible = visible;
            this.mtbFlipVerBtn.Visible = visible;
            this.mtbRotateImageLeftBtn.Visible = visible;
            this.mtbRotateImageRightBtn.Visible = visible;
        }
        private void SetImageCommonBtnsVisible(bool visible)
        {
            this.mtbRealSizeBtn.Visible = visible;
            this.mtbFullExtentBtn.Visible = visible;
            this.mtbSaveBtn.Visible = visible;
        }
        public void SetButtonVisible(FileProp prop)
        {
            string fileType = prop.FileType.ToLower();
            switch (fileType)
            {
                case ".gif":
                    SetImageRotateBtnsVisible(false);
                    SetVideoButtonsVisible(false);
                    SetImageCommonBtnsVisible(true);
                    break;
                case ".bmp":
                case ".png":
                case ".jpg":
                case ".jpeg":
                    SetImageRotateBtnsVisible(true);
                    SetVideoButtonsVisible(false);
                    SetImageCommonBtnsVisible(true);
                    break;
                case ".avi":
                case ".mp4":
                case ".rmvb":
                case ".mkv":
                case ".rm":
                case ".mp3":
                case ".flac":
                case ".ape":
                case ".m4a":
                    SetImageRotateBtnsVisible(false);
                    SetVideoButtonsVisible(true);
                    SetImageCommonBtnsVisible(false);
                    break;
                default:
                    SetImageRotateBtnsVisible(false);
                    SetVideoButtonsVisible(false);
                    SetImageCommonBtnsVisible(false);
                    break;

            }
            mToolbarWindow.Redraw();
        }


        private void SetVideoButtonsVisible(bool visible)
        {
            this.mtbPlayBtn.Visible = visible;
            this.mtbStopBtn.Visible = visible;
            this.mtbSnapshotBtn.Visible = visible;
        }
        
        private void MSystemDirWatcherTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            if (mUsbMonitorEnabled == false)
            {
                mSystemDirWatcherTimer.Stop();
                return;
            }
                
            ScanUsbDrives(false);
            CanPaste();
            if (mUsbMonitorEnabled == false)
                mSystemDirWatcherTimer.Stop();
        }
        private void CanPaste()
        {
            if (mtbPasteBtn != null)
            {
                mToolbarWindow.InvokeIfRequired(l => {
                    mtbPasteBtn.Enabled = this.mainDataPanel.CanPaste();
                    mToolbarWindow.Redraw();
                });

            }
        }
        private void ShowUSBPromptWindow(List<string> usbFolders,string group)
        {
            if (mUSBPromptWindow.Visible)
                mUSBPromptWindow.InvokeIfRequired(l => mUSBPromptWindow.AddToList(usbFolders,group, false));
            else
            {

                mUSBPromptWindow.InvokeIfRequired(l => {
                    mUSBPromptWindow.Visible = true;
                    //mUSBPromptWindow.TopMost = true;
                });
                mUSBPromptWindow.InvokeIfRequired(l => mUSBPromptWindow.FillList(usbFolders, group));
            }
        }
        private void RemoveFromUSBPrompWindow(string group)
        {
            if (mUSBPromptWindow.Visible)
            {
                mUSBPromptWindow.InvokeIfRequired(l => mUSBPromptWindow.RemoveByGroup(group));
            }
        }
        private void ScanUsbDrives(bool first)
        {
            DriveInfo[] allDrives = DriveInfo.GetDrives();
            List<string> newUSBs = new List<string>();
            Dictionary<string, string> dicUSBsInserted = new Dictionary<string, string>();
            foreach (DriveInfo d in allDrives)
            {
                if (d.IsReady == true)
                {
                    if (d.DriveType == DriveType.Removable)
                    {
                        if(first)
                            mDicRemovableDrives.Add(d.Name, d.VolumeLabel);
                        else
                        {
                            if (mDicRemovableDrives.ContainsKey(d.Name) == false || mDicRemovableDrives[d.Name] != d.VolumeLabel)
                            {
                                mDicRemovableDrives[d.Name] = d.VolumeLabel;
                                newUSBs.Add(d.Name);
                            }
                        }

                        dicUSBsInserted.Add(d.Name, d.VolumeLabel);
                    }
                }
            }
            List<string> usbsRemoved = new List<string>();
            foreach(KeyValuePair<string,string> kvp in mDicRemovableDrives)
            {
                if (dicUSBsInserted.ContainsKey(kvp.Key) == false)
                    usbsRemoved.Add(kvp.Key);
                else
                {
                    if (mDicRemovableDrives[kvp.Key] != dicUSBsInserted[kvp.Key])
                        usbsRemoved.Add(kvp.Key);
                }
            }

            for(int j = 0; j < usbsRemoved.Count; j++)
            {
                mDicRemovableDrives.Remove(usbsRemoved[j]);
                RemoveFromUSBPrompWindow(usbsRemoved[j]);
            }
            if (newUSBs.Count > 0)
            {
                this.mSystemDirWatcherTimer.Stop();
                List<string> folderHasFile = new List<string>();
               
                for(int i = 0; i < newUSBs.Count; i++)
                {
                    string folderName = newUSBs[i];
                    string[] dirs = Directory.GetDirectories(folderName);
                    for(int k = 0; k < dirs.Length; k++)
                    {
                        DirectoryInfo di = new DirectoryInfo(dirs[k]);
                        List<string> temp = SearchSubFolders(di, "dcim");
                        if(temp!=null)
                            folderHasFile.AddRange(temp);
                    }
                    if (folderHasFile.Count > 0)
                    {
                        ShowUSBPromptWindow(folderHasFile,folderName);
                    }

                }
                if(mUsbMonitorEnabled)
                    this.mSystemDirWatcherTimer.Start();
            }
        }
        private List<string> SearchSubFolders(DirectoryInfo item, string pattern)
        {
            if ((item.Attributes & FileAttributes.System) == FileAttributes.System)
                return null;
            List<string> temp = new List<string>();
            if (item.Name.ToLower().IndexOf(pattern) >= 0)
                temp.Add(item.FullName);

            foreach (DirectoryInfo subItem in item.EnumerateDirectories()) 
            {
                temp.AddRange(SearchSubFolders(subItem, pattern));
            }

            return temp;
        }
        private List<string> SearchSubFolders(Shell32.FolderItem item,string pattern)
        {
            List<string> temp = new List<string>();
            if (item.Name.ToLower().IndexOf(pattern) >= 0)
                temp.Add(item.Path);
            Shell32.Folder folder = item.GetFolder as Shell32.Folder;
            foreach(Shell32.FolderItem subItem in folder.Items())
            {
                if (subItem.IsFolder && subItem.IsBrowsable)
                    temp.AddRange(SearchSubFolders(subItem, pattern));
            }

            return temp;
        }
        private void MainDataPanel_OnFileSelected(string srcFile, string targetFile, bool canceled)
        {
            if (canceled == false && File.Exists(targetFile))
            {
                mThumbLeft.thumbnailList.UpdateCurThumbBoxStatus(true);
                UpdateSelectBtnImageKey(true);
            }
        }
        public void UpdateSelectBtnImageKey(bool selected)
        {
            this.InvokeIfRequired(l => {
                if (selected)
                {
                    mtbSelectBtn.Image = PhotoHome.Properties.Resources.tb_select;
                }
                else
                {
                    mtbSelectBtn.Image = PhotoHome.Properties.Resources.tb_unselect;
                }
                mToolbarWindow.Redraw();
            });
        }
        public bool UsbMonitorEnabled
        {
            get => mUsbMonitorEnabled;
            set{
                mUsbMonitorEnabled = value;
                if (value)
                    mSystemDirWatcherTimer.Start();
                else
                    mSystemDirWatcherTimer.Stop();
            }
        }
        private void MainDataPanel_OnBeforeLoadFile()
        {
            if(mThumbLeft.Visible==false)
                ShowWindows();
        }

        private void MainDataPanel_OnFileInfoChanged(List<SuperControls.TextLine> infoFlds,object propInfo)
        {
            mInfoWindow.Fill(infoFlds);
            if (propInfo != null)
            {
                if (mainDataPanel.CurFileType == EnumFileType.Image)
                {
                    if(mPerspective==EnumPerspective.Classify)
                        SetTaskPanelVisible(true);
                    mTaskPanel.Init(propInfo);
                }
                else
                {
                    SetTaskPanelVisible(false);
                }
                

                SetButtonVisible(propInfo as FileProp);
            }
            else
            {
                SetTaskPanelVisible(false);
            }
            UpdateSelectBtnImageKey(IsCurrentFileSelected());
           
        }
        
        

        private void SetTaskPanelVisible(bool visible)
        {
            if (visible)
            {
                if (mTaskPanel.Visible == false)
                {
                    mInfoWindow.Height = mInfoWindow.Height - mTaskPanel.Height - mVerInterval;
                    mTaskPanel.Visible = true;
                }
                
            }
            else
            {
                if (mTaskPanel.Visible)
                {
                    mTaskPanel.Visible = false;
                    mInfoWindow.Height = mInfoWindow.Height + mTaskPanel.Height + mVerInterval;
                }
                
            }
        }
        private void FrmMain_Load(object sender, EventArgs e)
        {
            this.mThumbLeft.thumbnailList.SelectTargetFolder = GetSelectTargetFolder();
        }

        private void FrmMain_FormClosed(object sender, FormClosedEventArgs e)
        {
            mainDataPanel.Clear();
        }
        

        public void SwitchFullScreen()
        {
            mIsFullScreen = !mIsFullScreen;
            switch (mPerspective)
            {
                case EnumPerspective.Classify:
                    FullScreenInClassify(mIsFullScreen);
                    break;
                case EnumPerspective.Modern:
                    FullScreenInModern(mIsFullScreen);
                    break;
                case EnumPerspective.Simple:
                    FullScreenInSimple(mIsFullScreen);
                    break;
            }
            //if(mToolbarWindow.Visible)
            //    mToolbarPanel.AdjustControlsAndWindow();
            mainDataPanel.SwitchFullScreen(mIsFullScreen);
        }
        private void FullScreenInClassify(bool toFull)
        {
            if (toFull)
            {
                mInfoWindow.Left = -mLeftPanelWidth - 20;
                mTaskPanel.Left = mInfoWindow.Left;
                mThumbLeft.Top = mThumbLeft.Top+mThumbLeft.Height + 20;
                this.Padding = new Padding(0);
                mToolbarWindow.Visible = false;
            }
            else
            {
                mToolbarWindow.Visible = true;
                mInfoWindow.Left = mLeftPanelLeft;
                mTaskPanel.Left = mLeftPanelLeft;
                mThumbLeft.Top= mTaskPanel.Bottom + mVerInterval;
                this.Padding = new Padding(mLeftPanelWidth + mLeftPanelLeft + mHorInterval, mVerInterval, 0, mThumbLeft.Height + mVerInterval);
            }
        }

        private void FullScreenInModern(bool toFull)
        {
            if (toFull)
            {
                mThumbLeft.Left = -mThumbLeft.Width - 2 * mHorInterval;
                this.Padding = new Padding(0);
                mToolbarWindow.Visible = false;
            }
            else
            {
                mToolbarWindow.Visible = true;
                mThumbLeft.Left = mHorInterval;
                this.Padding = new Padding(mThumbLeft.Width +2*mHorInterval, mVerInterval, 0, mVerInterval);
            }
            

        }
        private void FullScreenInSimple(bool toFull)
        {
            int totalHeight = this.Height;
            if (toFull)
            {
                mToolbarWindow.Visible = false;
                mThumbLeft.Location = new Point(0, totalHeight + 10);
                this.Padding = new Padding(0);
            }
            else
            {

                mToolbarWindow.Visible = true;

                mThumbLeft.Location = new Point(0, totalHeight - mThumbListHeight - mVerInterval);
                this.Padding = new Padding(0, 0, 0, mThumbLeft.Height + mVerInterval);

                Utils.ProgressUIQueue.BaseBottom = mThumbLeft.Height + mVerInterval + 10;
            }
            

        }


        public void LoadPath(string path)
        {
            
            if (File.Exists(path))
            {
                //文件
                mThumbLeft.thumbnailList.LoadFileContext(path);
                mainDataPanel.LoadDataFromFile(path);
            }
            else
            {
                mThumbLeft.thumbnailList.LoadFolder(path);
                mainDataPanel.LoadDataFromFile(mThumbLeft.thumbnailList.mCurFileName);
            }
        }
        private void FrmMain_Shown(object sender, EventArgs e)
        {
            if (PathFromArg != null)
            {
                mainDataPanel.PlayMediaForcely = true;
                LoadData(PathFromArg);
            }
            else
            {
                mFileBrowser.Location = new Point((this.Width - mFileBrowser.Width) / 2, (this.Height - mFileBrowser.Height) / 2);
                mFileBrowser.ShowDialog(this);
                if (mFileBrowser.DialogResult == DialogResult.OK)
                {
                    LoadData(mFileBrowser.SelectedPath);
                }
                else
                {
                    this.Close();
                }
            }
            
            
            //mUSBPromptWindow.StartPosition = FormStartPosition.CenterScreen;
        }

        private void LoadData(string folder)
        {
            mPerspective = (EnumPerspective)AppConfig.Instance.AppLayout;
            ShowWindows();
            
            LoadPath(folder);
            mFileBrowser.Close();

            mSystemDirWatcherTimer.Start();

            mUSBPromptWindow.Left = -1000;
            mUSBPromptWindow.Show(this);
            mUSBPromptWindow.Visible = false;
            mUSBPromptWindow.Left = this.Left + (this.Width - mUSBPromptWindow.Width) / 2;
            mUSBPromptWindow.Top = this.Top + (this.Height - mUSBPromptWindow.Height) / 2;
        }
        private int mThumbListHeight = 140;
        private void ShowWindowsClassify()
        {
            int totalHeight = this.Height;
            mInfoWindow.Top = mVerInterval;
            mInfoWindow.Left = mLeftPanelLeft;
            mInfoWindow.Width = mLeftPanelWidth;

            mInfoWindow.Height = totalHeight - mVerInterval - mVerInterval - mTaskPanel.Height - mThumbListHeight - mVerInterval; ;
            if(mInfoWindow.Visible==false)  mInfoWindow.Show(this);


            mTaskPanel.Top = mInfoWindow.Bottom + mVerInterval;
            mTaskPanel.Left = mLeftPanelLeft;
            mTaskPanel.Width = mLeftPanelWidth;
            //mTaskPanel.Height = totalHeight - mInfoWindow.Bottom - mThumbLeft.Height - mVerInterval;
            if (mTaskPanel.Visible == false) mTaskPanel.Show(this);
            mThumbLeft.thumbnailList.ItemLayout = EnumListItemLayout.Horizontal;
            mThumbLeft.Left = mLeftPanelLeft;
            mThumbLeft.Width = this.Width;
            mThumbLeft.Top = mTaskPanel.Bottom + mVerInterval;
            mThumbLeft.Height = totalHeight-mThumbLeft.Top;
            if (mThumbLeft.Visible == false) mThumbLeft.Show(this);

            this.Padding = new Padding(mLeftPanelWidth + mLeftPanelLeft + mHorInterval, mVerInterval, 0, mThumbLeft.Height + mVerInterval);

            Utils.ProgressUIQueue.BaseBottom = mThumbLeft.Height + mVerInterval + 10;
        }

        private void ShowWindowsModern()
        {
            SetToolWindowVisible(false);
            mThumbLeft.Location = new Point(mHorInterval, mVerInterval);
            mThumbLeft.thumbnailList.ItemLayout = EnumListItemLayout.Vertical;
            int thumbWidth = this.Width / 4;
            mThumbLeft.Size = new Size(thumbWidth, this.Height - 2 * mVerInterval);

            this.Padding = new Padding(thumbWidth +2*mHorInterval, mVerInterval, 0, mVerInterval);
            if(mThumbLeft.Visible==false)
                mThumbLeft.Show(this);

            Utils.ProgressUIQueue.BaseBottom = mVerInterval + 10;
        }

        private void SetToolWindowVisible(bool visible)
        {
            if (mTaskPanel.Visible != visible)
                mTaskPanel.Visible = visible;
            if (mInfoWindow.Visible != visible)
                mInfoWindow.Visible = visible;
    

        }
        private void ShowWindowsSimple()
        {
            SetToolWindowVisible(false);
            int totalHeight = this.Height;

            mThumbLeft.thumbnailList.ItemLayout = EnumListItemLayout.Horizontal;
            mThumbLeft.Location = new Point(0, totalHeight - mThumbListHeight - mVerInterval);
            mThumbLeft.Size = new Size(this.Width, mThumbListHeight);
            
            if(mThumbLeft.Visible==false) mThumbLeft.Show(this);

            this.Padding = new Padding(0, 0, 0, mThumbLeft.Height + mVerInterval);

            Utils.ProgressUIQueue.BaseBottom = mThumbLeft.Height + mVerInterval + 10;
        }

        public EnumPerspective Perspective
        {
            get
            {
                return mPerspective;
            }
            set
            {
                if (mPerspective != value)
                {
                    mPerspective = value;
                    ShowWindows();
                }
                
            }
        }
        private void ShowWindows()
        {
            int toolbarLeft = 0;

            switch (mPerspective)
            {
                case EnumPerspective.Classify:
                    ShowWindowsClassify();
                    break;
                case EnumPerspective.Modern:
                    ShowWindowsModern();
                    break;
                case EnumPerspective.Simple:
                    ShowWindowsSimple();
                    break;
            }
            mainDataPanel.BackColor = mTaskPanel.BackColor;
            toolbarLeft = (this.Width - mToolbarWindow.Width) / 2; //this.Padding.Left + (mainDataPanel.Width - mToolbarPanel.Width) / 2;

            mToolbarWindow.Location = new Point(toolbarLeft,0);
            if(mToolbarWindow.Visible==false)
                mToolbarWindow.Show(this);

        }

        private void AddScrollBars()
        {

        }
        
        public void OnOpenClick()
        {
            mainDataPanel.OpenWithDialog();
        }
        
        private void OnCopyClick(object sender, EventArgs e)
        {

        }
        private void OnPasteClick(object sender, EventArgs e)
        {

        }
        private void OnSettingClick(object sender, EventArgs e)
        {

        }
      
        private void frmMain_Load(object sender, EventArgs e)
        {
            
        }

        private void ShowThumbRight()
        {
            if (mThumbRight == null)
            {
                mThumbRight = new frmThumb();
                mThumbRight.thumbnailList.DataPanel = mainDataPanel;
            }
            if (mThumbRight.Visible == false)
            {
                mThumbLeft.Width = this.Width / 2 - 2;
                mThumbRight.Left = mThumbLeft.Width + 4;
                mThumbRight.Top = mThumbLeft.Top;
                mThumbRight.Width = mThumbLeft.Width;

                mThumbRight.Show(this);

                mThumbLeft.thumbnailList.RebuildList(false);
                
            }
        }

        public void Save()
        {
            mainDataPanel.SaveImage(AppConfig.Instance.SaveToTargetFolder? GetSelectTargetFolder() : null);
            mThumbLeft.thumbnailList.UpdateCurThumbBoxStatus(true);
        }
        public string GetSelectTargetFolder()
        {
            return AppConfig.Instance.SelectedTargetFolder;
        }

        public void WhenKeyUp(object sender, KeyEventArgs e)
        {
            switch (e.KeyCode)
            {
                case Keys.Escape:
                    if (mIsFullScreen)
                    {
                        SwitchFullScreen();
                    }
                    else
                    {
                        Close();
                    }
                    
                    break;
                case Keys.Right:
                case Keys.Left:
                case Keys.Up:
                case Keys.Down:
                    if (mThumbLeft.thumbnailList.Focused == false)
                    {
                        mThumbLeft.thumbnailList.WhenKeyUp(e);
                    }
                    
                    break;
                case Keys.C:
                    if (e.Control == true)
                        mainDataPanel.CopyCurFileToCB(false);
                    break;
                case Keys.X:
                    if (e.Control == true)
                        mainDataPanel.CopyCurFileToCB(true);
                    else
                        mainDataPanel.SelectCurFileToFolder(GetSelectTargetFolder(), true);
                    break;
                case Keys.V:
                    if (e.Control == true&& mtbPasteBtn.Enabled)
                        mainDataPanel.PasteFile2CurPath();
                    break;
                case Keys.Delete:
                    mainDataPanel.DeleteCurFile();
                    break;
                case Keys.S:
                    if (e.Control == true)
                        mainDataPanel.SaveImage();
                    else
                        //选择图片
                        SelectCurFile();
                    break;
                case Keys.D:
                    UnselectCurFile();
                    break;
                case Keys.Space:
                    if(mainDataPanel.CurFileType==EnumFileType.Video||mainDataPanel.CurFileType==EnumFileType.Audio)
                        mainDataPanel.PlayOrPause();
                    break;
                case Keys.F5:
                case Keys.R:
                    StartOrStopAutoSlide();
                    break;
                case Keys.F11:
                    SwitchFullScreen();
                    break;

            }
        }
        private bool mAutoSlide = false;
        public void StartOrStopAutoSlide()
        {
            mAutoSlide = !mAutoSlide;
            StartAutoSlide(mAutoSlide);
            if (mAutoSlide == false)
                mtbSlideAutoBtn.Image = PhotoHome.Properties.Resources.tb_slidestart;
            else
                mtbSlideAutoBtn.Image = PhotoHome.Properties.Resources.tb_slidestop;

            mToolbarWindow.Redraw();
        }
        public void SelectCurFile()
        {
            mainDataPanel.SelectCurFileToFolder(GetSelectTargetFolder());
        }
        public bool UnselectCurFile()
        {
            bool well = mainDataPanel.DeleteCurSelected(GetSelectTargetFolder());
            if (well)
            {
                mThumbLeft.thumbnailList.UpdateCurThumbBoxStatus(false);
                UpdateSelectBtnImageKey(false);
            }
            return well;
        }
        public bool IsCurrentFileSelected()
        {
            return mainDataPanel.IsCurrentFileSelected(GetSelectTargetFolder());
        }
        
        private void frmMain_KeyUp(object sender, KeyEventArgs e)
        {
            if (sender != this) return;
            WhenKeyUp(sender,e);
        }

        public void StartAutoSlide(bool start)
        {
            //if (start)
            //    this.timerAutoSlide.Start();
            //else
            //    this.timerAutoSlide.Stop();

            mThumbLeft.thumbnailList.AutoSlide = start;
        }
        private void timerAutoSlide_Tick(object sender, EventArgs e)
        {
            mainDataPanel.NextFile();
        }
    }
}
