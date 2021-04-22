using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vlc.DotNet.Forms;
using CommonLib;
using System.Threading;
using System.IO;
using PhotoHome;

namespace ThirdPlugins
{
    public enum EnumMediaPlayStatus
    {
        None=0,
        Playing=1,
        Stopped=2,
        Paused=3
    }
    public partial class VLCPlayer : UserControl
    {
        public VlcControl mVlcControl = null;
        private bool mIsFullScreen = false;
        private Form mFullScreenWindow = null;
        private bool mIsMouseDown = false;
        private EnumMediaPlayStatus mCurrentStatus = EnumMediaPlayStatus.None;
        private string mFileName = null;
        private bool mEndReached = false;
        private VoidAndNonParamHandler mMediaPlayingHandler = null;
        private VoidAndNonParamHandler mMediaStoppedHandler = null;
        private VoidAndNonParamHandler mMediaPausedHandler = null;
        public VLCPlayer()
        {

            InitializeComponent();

            this.SuspendLayout();
            this.mVlcControl = new Vlc.DotNet.Forms.VlcControl();
            ((System.ComponentModel.ISupportInitialize)(this.mVlcControl)).BeginInit();
            this.mVlcControl.BackColor = System.Drawing.Color.Black;
            this.mVlcControl.Dock = System.Windows.Forms.DockStyle.Fill;
            this.mVlcControl.Location = new System.Drawing.Point(0, 0);
            this.mVlcControl.Name = "mVlcControl";
            this.mVlcControl.Size = new System.Drawing.Size(766, 498);
            this.mVlcControl.Spu = -1;
            this.mVlcControl.TabIndex = 0;
            this.mVlcControl.VlcLibDirectory = VLCMultimedia.VLCLibDir;
            this.Controls.Add(this.mVlcControl);
            ((System.ComponentModel.ISupportInitialize)(this.mVlcControl)).EndInit();
            this.ResumeLayout(false);

            this.SizeChanged += VLCPlayer_SizeChanged;
 
            this.mVlcControl.LengthChanged += MVlcControl_LengthChanged;
            this.mVlcControl.EndReached += MVlcControl_EndReached;
            this.mVlcControl.TimeChanged += MVlcControl_TimeChanged;
            this.mVlcControl.Stopped += MVlcControl_Stopped;
            this.mVlcControl.Paused += MVlcControl_Paused;
            this.mVlcControl.Playing += MVlcControl_Playing;
            this.mVlcControl.MouseClick += MVlcControl_MouseClick;
            
            trackVideoProgess.Scroll += TrackVideoProgess_Scroll;
            trackVideoProgess.MouseDown += TrackVideoProgess_MouseDown;
            trackVideoProgess.MouseUp += TrackVideoProgess_MouseUp;
            
            //trackVideoProgess.ValueChanged += TrackVideoProgess_ValueChanged;

        }

        private void MVlcControl_MouseClick(object sender, MouseEventArgs e)
        {
            PlayOrPause();
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
        public EnumMediaPlayStatus CurrentPlayStatus
        {
            get
            {
                return mCurrentStatus;
            }
        }
        private void MVlcControl_TimeChanged(object sender, Vlc.DotNet.Core.VlcMediaPlayerTimeChangedEventArgs e)
        {
            lblCurrentTime.InvokeIfRequired(l => l.Text = TimeSpan.FromMilliseconds(mVlcControl.Time).ToString(@"hh\:mm\:ss"));
            trackVideoProgess.InvokeIfRequired(t =>
            {
                t.Value = (int)(TimeSpan.FromMilliseconds(mVlcControl.Time).TotalMilliseconds);

            });
        }

        private void MVlcControl_Playing(object sender, Vlc.DotNet.Core.VlcMediaPlayerPlayingEventArgs e)
        {
            mEndReached = false;
            trackVideoProgess.InvokeIfRequired(t => t.Enabled = true);

            if (mMediaPlayingHandler != null)
            {
                mMediaPlayingHandler();
            }
        }

        private void MVlcControl_Paused(object sender, Vlc.DotNet.Core.VlcMediaPlayerPausedEventArgs e)
        {
            if (mMediaPausedHandler != null)
                mMediaPausedHandler();
        }

        private void MVlcControl_Stopped(object sender, Vlc.DotNet.Core.VlcMediaPlayerStoppedEventArgs e)
        {
            trackVideoProgess.InvokeIfRequired(t => t.Enabled = false);
            if (mMediaStoppedHandler!=null)
                mMediaStoppedHandler();
        }

        private void MVlcControl_EndReached(object sender, Vlc.DotNet.Core.VlcMediaPlayerEndReachedEventArgs e)
        {
            mEndReached = true;
      
        }
        
 
        private void TrackVideoProgess_MouseUp(object sender, MouseEventArgs e)
        {
            if(mCurrentStatus == EnumMediaPlayStatus.Playing)
            {
                //mIsMouseDown = false;
                //this.timerPlay.Start();
                //this.mVlcControl.SetPause(false);
            }
                
            
        }

        private void TrackVideoProgess_MouseDown(object sender, MouseEventArgs e)
        {
            if(mCurrentStatus== EnumMediaPlayStatus.Playing)
            {
                //mIsMouseDown = true;
                //this.timerPlay.Stop();
                //this.mVlcControl.SetPause(true);
                //mIsMouseDown = true;
            }
               
        }

        private void TrackVideoProgess_Scroll(object sender, EventArgs e)
        {
            if (mVlcControl.IsPlaying == false && mEndReached)
            {
                int seconds = (int)TimeSpan.FromMilliseconds(this.trackVideoProgess.Value).TotalSeconds;

                if (mCurrentStatus == EnumMediaPlayStatus.Playing)
                {
                    mVlcControl.Play(new Uri(mFileName), new string[] { "--start-time", seconds.ToString() });
                }
            }

            this.mVlcControl.Time = this.trackVideoProgess.Value;
        }

        private void MVlcControl_LengthChanged(object sender, Vlc.DotNet.Core.VlcMediaPlayerLengthChangedEventArgs e)
        {
            lblTotalTime.InvokeIfRequired(l => l.Text = TimeSpan.FromMilliseconds(e.NewLength).ToString(@"hh\:mm\:ss"));
            trackVideoProgess.InvokeIfRequired(t =>
            {
                t.Minimum = 0;
                t.Maximum = (int)(TimeSpan.FromMilliseconds(e.NewLength)).TotalMilliseconds;
            });

        }
        private void VLCPlayer_SizeChanged(object sender, EventArgs e)
        {
            this.lblCurrentTime.Left = 2;
            this.trackVideoProgess.Left = this.lblCurrentTime.Right+2;
            this.trackVideoProgess.Width = this.panelControlBar.Width - this.trackVideoProgess.Left-this.lblTotalTime.Width - 4;
            this.lblTotalTime.Left = this.trackVideoProgess.Right + 2;

            this.btnPlay.Left = (this.mVlcControl.Width - this.btnPlay.Width) / 2;
            this.btnPlay.Top = (this.mVlcControl.Bottom- this.btnPlay.Height-this.panelControlBar.Height-20);

           
        }

        public bool ControlBarVisible
        {
            get
            {
                return this.panelControlBar.Visible;
            }
            set
            {
                this.panelControlBar.Visible = value;
            }

        }

        public bool IsPlaying
        {
            get
            {
                return this.mVlcControl.IsPlaying;
            }
        }
  
        public void BeginInit()
        {
            this.mVlcControl.BeginInit();
        }
        public void EndInit()
        {
            this.mVlcControl.EndInit();
        }
        public void PlayFirst(string fileName)
        {
            this.mVlcControl.Play(new Uri(fileName));
            
        }

        public void SwitchFullScreen(bool toFull)
        {
            this.panelControlBar.Visible = !toFull;
        }

        public async void TaskSnapshotAsync(string fileName)
        {
            TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
            ThreadPool.QueueUserWorkItem(_ =>
            {
                this.mVlcControl.TakeSnapshot(fileName);
            });
            await tcs.Task;
            
        }

        public void SetBGImage(string fileName)
        {
            if (File.Exists(fileName) == false) return;
            this.mVlcControl.BeginInit();
            this.mVlcControl.BackgroundImage = Image.FromFile(fileName);
            this.mVlcControl.BackgroundImageLayout = ImageLayout.Center;
            this.mVlcControl.EndInit();
        }

        public void SetMedia(string fileName)
        {
            ResetUI();
            mFileName = fileName;
            this.mVlcControl.VlcMediaPlayer.SetMedia(new Uri(fileName));
            this.mVlcControl.GetCurrentMedia().Parse();
            lblTotalTime.InvokeIfRequired(l => l.Text = this.mVlcControl.GetCurrentMedia().Duration.ToString(@"hh\:mm\:ss"));
        }

        private void ResetUI()
        {
            this.trackVideoProgess.Value = this.trackVideoProgess.Minimum;
            this.lblCurrentTime.Text = "00:00:00";
            this.lblTotalTime.Text = "00:00:00";
            this.btnPlay.Visible = true;
        }
        public void Play()
        {
            
            if(Global.GetFileType(mFileName)==EnumFileType.Audio)
                this.mVlcControl.Play(new Uri(mFileName),new string[] { "--audio-visual", "visual","--role", "music" });
            else
                this.mVlcControl.Play(new Uri(mFileName));
            
            this.btnPlay.Visible = false;
            mCurrentStatus = EnumMediaPlayStatus.Playing;
        }
        public void PlayOrPause()
        {
            if (mCurrentStatus == EnumMediaPlayStatus.Stopped||mCurrentStatus==EnumMediaPlayStatus.None)
            {
                Play();
            }
            else
            {
                bool doPause = mCurrentStatus == EnumMediaPlayStatus.Playing ? true : false;
                this.mVlcControl.SetPause(doPause);
                mCurrentStatus = doPause == true ? EnumMediaPlayStatus.Paused : EnumMediaPlayStatus.Playing;
            }
            
         
        }
        public void Stop()
        {
            if (mCurrentStatus != EnumMediaPlayStatus.Stopped && mCurrentStatus != EnumMediaPlayStatus.None)
            {
                this.mVlcControl.Stop();
                
            }
            mCurrentStatus = EnumMediaPlayStatus.Stopped;
            ResetUI();
        }
        public long Time
        {
            get
            {
                return this.mVlcControl.Time;
            }
            set
            {
                this.mVlcControl.Time = value;
            }
        }
        private void btnPlay_Click(object sender, EventArgs e)
        {
            Play();
        }

        private void btnPlayOrPause_Click(object sender, EventArgs e)
        {
            //if(mVlcControl.IsPlaying)
            //{
            //    mVlcControl.Pause();
            //    this.btnPlayOrPause.ImageKey = "play32.png";

            //}
            //else
            //{
            //    mVlcControl.Play();
            //    this.btnPlayOrPause.ImageKey = "pause32.png";
            //}

            //this.btnStop.Enabled = true;
            
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            mVlcControl.Stop();
            //this.btnStop.Enabled = false;
            //this.btnPlayOrPause.Enabled = true;
            //this.btnPlayOrPause.ImageKey = "play32.png";
        }

        private void btnVideoFullScreen_Click(object sender, EventArgs e)
        {
            
        }

        private void timerPlay_Tick(object sender, EventArgs e)
        {
            
        }

        private void trackVideoProgess_ValueChanged(object sender, EventArgs e)
        {

        }

        private void VLCPlayer_MouseClick(object sender, MouseEventArgs e)
        {
            PlayOrPause();
        }

        private void VLCPlayer_Click(object sender, EventArgs e)
        {
            PlayOrPause();
        }
    }
}
