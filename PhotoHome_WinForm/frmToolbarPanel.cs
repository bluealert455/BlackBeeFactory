using PropertyPages;
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
using CommonLib;
using SuperControls;
namespace PhotoHome
{
    public delegate void OnCommonEventHander(object sender, EventArgs e);
    public partial class frmToolbarPanel : frmFloatBase
    {
        private frmMain mMainForm;
        int mHorInterval = 0;
        int mPaddingHor = 15;
        int mPaddingVer = 5;
        System.Timers.Timer mTimerOpacity = null;
        public frmToolbarPanel()
        {
            InitializeComponent();
            this.Left = 0;
            this.Top = 10;

            this.Load += FrmToolbarPanel_Load;

            //mTimerOpacity = new System.Timers.Timer();
            //mTimerOpacity.Interval = 100;
            //mTimerOpacity.Elapsed += MTimerOpacity_Elapsed;

            
        }

        protected override void OnShown(EventArgs e)
        {
            base.OnShown(e);
            SetLayoutBtnImage();
        }
        private void MTimerOpacity_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            SetOpacityAni();
        }

        public frmMain MainForm {
            get
            {
                return mMainForm;
            }
            set
            {
                mMainForm = value;
                mMainForm.mainDataPanel.OnMediaPlaying += MainDataPanel_OnMediaPlaying;
                mMainForm.mainDataPanel.OnMediaPaused += MainDataPanel_OnMediaPaused;
                mMainForm.mainDataPanel.OnMediaStopped += MainDataPanel_OnMediaStopped;
            }
        }

        private void MainDataPanel_OnMediaStopped()
        {
            SetMediaBtnsEnabled(false);
            this.btnPlay.InvokeIfRequired(a=>a.ImageKey = "play32.png");
        }

        private void MainDataPanel_OnMediaPaused()
        {
            this.btnPlay.InvokeIfRequired(a => a.ImageKey = "play32.png");
        }

        private void MainDataPanel_OnMediaPlaying()
        {
            SetMediaBtnsEnabled(true);
            
            this.btnPlay.InvokeIfRequired(a => a.ImageKey = "pause32.png");
        }

        private void FrmToolbarPanel_Load(object sender, EventArgs e)
        {
            SetLayoutBtnImage();
            AdjustControlsAndWindow();
        }

        private void SetLayoutBtnImage()
        {
            EnumPerspective layout = (EnumPerspective)AppConfig.Instance.AppLayout;
            switch (layout)
            {
                case EnumPerspective.Classify:
                    this.btnSetLayout.ImageKey = "classify32.png";
                    break;
                case EnumPerspective.Modern:
                    this.btnSetLayout.ImageKey = "modern32.png";
                    break;
                case EnumPerspective.Simple:
                    this.btnSetLayout.ImageKey = "simple32.png";
                    break;
            }
        }
        public void AdjustControlsAndWindow()
        {
            
            int ctrlsWidth = 0;

            List<Control> ctrls = new List<Control>();
            
            for(int i = 0; i < this.Controls.Count; i++)
            {
                Control ctrl = this.Controls[i];
                if (ctrl.Visible == true)
                {
                    ctrls.Add(ctrl);
                }
                if (ctrl.Tag == null)
                {
                    ctrl.MouseEnter += Ctrl_MouseEnter;
                    ctrl.MouseLeave += Ctrl_MouseLeave;
                    ctrl.Tag = "1";
                }
            }
            ctrls.Sort(new ControlTabIndexSorter());
            for(int i = 0; i < ctrls.Count; i++)
            {
                Control ctrl = ctrls[i];
                int ctrlLeft = mPaddingHor + i * mHorInterval + ctrl.Width * i;
                int ctrlTop = mPaddingVer;
                ctrl.Location = new Point(ctrlLeft, ctrlTop);
            }
            ctrlsWidth = ctrls[ctrls.Count - 1].Right + mPaddingHor;
            this.Width = ctrlsWidth;
            this.Height = mPaddingVer * 2 + ((ctrls.Count > 0) ? ctrls[0].Height : 30);

            if(MainForm != null)
                this.Location = new Point((MainForm.Width - this.Width) / 2, this.Top);

            SetVisibleAni(false);

        }

        private void Ctrl_MouseLeave(object sender, EventArgs e)
        {
            this.OnMouseLeave(e);
        }

        private void Ctrl_MouseEnter(object sender, EventArgs e)
        {
            this.OnMouseEnter(e);
        }

        private void SetImageRotateBtnsVisible(bool visible)
        {
            this.btnFlipHor.Visible = visible;
            this.btnFlipVer.Visible = visible;
            this.btnRotateImageLeft.Visible = visible;
            this.btnRotateImageRight.Visible = visible;
        }
        private void SetImageCommonBtnsVisible(bool visible)
        {
            this.btnRealSize.Visible = visible;
            this.btnFullExtent.Visible = visible;
            this.btnSave.Visible = visible;
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
            AdjustControlsAndWindow();
        }

        
        private void SetVideoButtonsVisible(bool visible)
        {
            this.btnPlay.Visible = visible;
            this.btnStop.Visible = visible;
            this.btnTakeSnapshot.Visible = visible;
        }
        private void btnOpen_Click(object sender, EventArgs e)
        {
            MainForm.UsbMonitorEnabled = false;
            frmFileBrowser fileSelectorDlg = new frmFileBrowser();
            fileSelectorDlg.IsSplash = false;
            if (fileSelectorDlg.ShowDialog(MainForm) == DialogResult.OK)
            {
                MainForm.LoadPath(fileSelectorDlg.SelectedPath);
                fileSelectorDlg.Close();
                
            }
            MainForm.UsbMonitorEnabled = true;
        }

        private void btnPaste_Click(object sender, EventArgs e)
        {
            MainForm.mainDataPanel.PasteFile2CurPath();
        }

        private void btnCopy_Click(object sender, EventArgs e)
        {

        }

        private void btnSave_Click(object sender, EventArgs e)
        {
            MainForm.Save();
        }

        private void btnSettings_Click(object sender, EventArgs e)
        {

        }

        private void btnFullExtent_Click(object sender, EventArgs e)
        {
            MainForm.mainDataPanel.FullExtent();
        }



        private class ControlTabIndexSorter : IComparer<Control>
        {
            public int Compare(Control x, Control y)
            {
                return x.TabIndex - y.TabIndex;
            }
        }

        private void btnOpenFolder_Click(object sender, EventArgs e)
        {
            MainForm.mainDataPanel.OpenWithFolderDlg();
        }

        private void btnRealSize_Click(object sender, EventArgs e)
        {
            MainForm.mainDataPanel.ZoomToRealSize();
        }

        private void btnRotateImageLeft_Click(object sender, EventArgs e)
        {
            MainForm.mainDataPanel.RotateImageLeft();
        }

        private void btnRotateImageRight_Click(object sender, EventArgs e)
        {
            MainForm.mainDataPanel.RotateImageRight();
        }

        private void btnFlipHor_Click(object sender, EventArgs e)
        {
            MainForm.mainDataPanel.FlipHor();
        }

        private void btnFlipVer_Click(object sender, EventArgs e)
        {
            MainForm.mainDataPanel.FlipVer();
        }

        private void btnPlay_Click(object sender, EventArgs e)
        {
            MainForm.mainDataPanel.PlayOrPause();
    
        }

        private void SetMediaBtnsEnabled(bool enabled)
        {
            this.InvokeIfRequired(a =>
            {
                this.btnStop.Enabled = enabled;
                this.btnTakeSnapshot.Enabled = enabled;
            });
            
        }
        private void btnStop_Click(object sender, EventArgs e)
        {
            MainForm.mainDataPanel.Stop();
            

        }

        private void btnTakeSnapshot_Click(object sender, EventArgs e)
        {
            MainForm.mainDataPanel.TakeSnapshot(MainForm.GetSelectTargetFolder());
        }

        private void btnFullScreen_Click(object sender, EventArgs e)
        {
            MainForm.SwitchFullScreen();
            
            
        }

        private bool mShow = false;
        public void SetVisibleAni(bool visible)
        {
            if (mTimerOpacity == null) return;
            mShow = visible;
            mTimerOpacity.Start();
        }


        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            SetVisibleAni(true);
        }
        protected override void OnMouseLeave(EventArgs e)
        {
            base.OnMouseLeave(e);
            SetVisibleAni(false);

        }
        private void SetOpacityAni()
        {
            lock (this)
            {
                if (mShow == false)
                {
                    this.InvokeIfRequired(l => l.Opacity = l.Opacity - 0.1);
                    if (this.Opacity <= 0.01)
                    {
                        this.InvokeIfRequired(l => l.Opacity = 0.01);
               
                        mTimerOpacity.Stop();
                    }
                }
                else
                {
                    this.InvokeIfRequired(l => l.Opacity = l.Opacity + 0.1);
                    if (this.Opacity >= 1)
                    {
                        this.InvokeIfRequired(l => l.Opacity = 1);
                        mTimerOpacity.Stop();
                    }
                }
            }
            
        }

        public void UpdateSelectBtnImageKey(bool selected)
        {
            if (selected)
                this.btnUnselect.InvokeIfRequired(l=>l.ImageKey = "unselected32.png");
            else
                this.btnUnselect.InvokeIfRequired(l=>l.ImageKey = "selected32.png");
        }
        private void btnUnselect_Click(object sender, EventArgs e)
        {
            bool selected = mMainForm.IsCurrentFileSelected();
            if (selected)
            {
                mMainForm.UnselectCurFile();
            }
            else
            {
                mMainForm.SelectCurFile();
            }
        }

        private bool mAutoSlide = false;
        private void btnAutoSlide_Click(object sender, EventArgs e)
        {

            StartOrStopAutoSlide();
        }

        public void StartOrStopAutoSlide()
        {
            mAutoSlide = !mAutoSlide;
            mMainForm.StartAutoSlide(mAutoSlide);
            if (mAutoSlide == false)
                this.btnAutoSlide.ImageKey = "slideauto32.png";
            else
                this.btnAutoSlide.ImageKey = "slideautostop32.png";
        }

        private void btnQuit_Click(object sender, EventArgs e)
        {
            MainForm.Close();
        }

        private void btnSelectTargetFolder_Click(object sender, EventArgs e)
        {
            MainForm.UsbMonitorEnabled = false;
            frmFolderSelector folderDlg = new frmFolderSelector();
            string prompt = "当前存放选中文件的目录如下:\n" + AppConfig.Instance.SelectedTargetFolder+"\n如果想更改，请在下面选择。";
            folderDlg.SetPrompt(prompt, "点击保存按钮时，把文件保存到上述目录里。");
            folderDlg.CheckBox1Checked = AppConfig.Instance.SaveToTargetFolder;
            if (folderDlg.ShowDialog(MainForm)== DialogResult.OK)
            {
                AppConfig.Instance.SelectedTargetFolder = folderDlg.SelectedPath;
                AppConfig.Instance.SaveToTargetFolder = folderDlg.CheckBox1Checked;
                AppConfig.Instance.Save();
                
            }
            MainForm.UsbMonitorEnabled = true;
        }

        frmSetLayout mLayoutForm = null;
        private void btnSetLayout_Click(object sender, EventArgs e)
        {
            if (mLayoutForm == null || mLayoutForm.IsDisposed)
            {
                mLayoutForm = new frmSetLayout();
                mLayoutForm.MainForm = mMainForm;
                mLayoutForm.SetImageList(this.imglstLeftBar);
                mLayoutForm.AnchorControl = this.btnSetLayout;
                
            }
            if (mLayoutForm.Visible==false)
            {
                mLayoutForm.Show(mMainForm);
            }
        }
    }
}
