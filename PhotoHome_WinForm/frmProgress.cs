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

namespace PhotoHome
{

    public partial class frmProgress : frmFloatBase
    {
        private System.Timers.Timer mStayTimer = null;
        private int mStayLength = 2000;
        public bool Canceled = false;
        public frmProgress()
        {
            InitializeComponent();
            this.SizeChanged += FrmProgress_SizeChanged;
            mStayTimer = new System.Timers.Timer(100);
            mStayTimer.Elapsed += MStayTimer_Elapsed;
        }

        private void MStayTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            mStayLength -= (int)mStayTimer.Interval;
            if(mStayLength<=0)
            {
                mStayTimer.Stop();
                this.InvokeIfRequired(l => l.Close());
            }
        }

        private void FrmProgress_SizeChanged(object sender, EventArgs e)
        {
            this.lblPrompt.Left = 10;
            this.lblPrompt.Width = this.Width - this.lblPrompt.Left * 2;
            this.progbarGoing.Left = this.lblPrompt.Left;
            this.progbarGoing.Width = this.Width - this.lblPrompt.Left * 2;
            //this.btnStopCopy.Left = this.Width - this.lblPrompt.Left - this.btnStopCopy.Width;
        }

        public void Init(ProgressBarStyle barStyle,string prompt="正在复制文件...", int min = 0,int max=100)
        {
            this.progbarGoing.Style = barStyle;
            this.progbarGoing.Minimum = min;
            this.progbarGoing.Maximum = max;
            this.lblPrompt.Text = prompt;
        }
        public void UpdateValue(int val)
        {
            this.progbarGoing.InvokeIfRequired(l => {
                l.Value = val;
                if (val == l.Maximum)
                    this.Close();
            });
        }

        public void UpdatePrompt(string prompt)
        {
            this.lblPrompt.InvokeIfRequired(l => l.Text = prompt);
        }
        private void btnStopCopy_Click(object sender, EventArgs e)
        {
            Canceled = true;
        }
        public void CloseMe()
        {
            //this.InvokeIfRequired(l =>
            //{
            //    if (l.Visible == true)
            //        l.Close();
            //});

            mStayTimer.Start();
        }
    }
}
