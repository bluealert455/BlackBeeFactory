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
    public partial class frmMsgBox : frmFloatBase
    {
        private int mPaddingVer = 10;
        private int mPaddingHor = 10;
        private MessageBoxIcon mIcon;
        private string mMsg;
        private Rectangle mContentRect;
        private int mIconSize = 48;
        private System.Timers.Timer mStayTimer = null;
        private System.Timers.Timer mHideTimer = null;
        private int mStayTimeLength = 2000;
        public frmMsgBox()
        {
            InitializeComponent();
            mStayTimer = new System.Timers.Timer(100);
            mStayTimer.Elapsed += MStayTimer_Elapsed;
            mHideTimer = new System.Timers.Timer(100);
            mHideTimer.Elapsed += MHideTimer_Elapsed;
            this.Shown += FrmMsgBox_Shown;
            this.FormClosed += FrmMsgBox_FormClosed;
        }

     
        private void FrmMsgBox_FormClosed(object sender, FormClosedEventArgs e)
        {
            this.Close();
        }

        private void MHideTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            
        }

        private void MStayTimer_Elapsed(object sender, System.Timers.ElapsedEventArgs e)
        {
            mStayTimeLength -= (int)mStayTimer.Interval;
            if (mStayTimeLength <= 0)
            {
                mStayTimer.Stop();
                this.InvokeIfRequired(l => l.Close());
            }
        }

        private void FrmMsgBox_Shown(object sender, EventArgs e)
        {
            mStayTimer.Start();
        }

        protected override void OnMouseEnter(EventArgs e)
        {
            base.OnMouseEnter(e);
            mStayTimeLength = 2000;
        }
        public void Init(MessageBoxIcon icon, string msg)
        {
            mIcon = icon;
            mMsg = msg;
        }
        public void ShowMessage(MessageBoxIcon icon,string msg,IWin32Window owner=null)
        {
            mIcon = icon;
            mMsg = msg;
            Rectangle rectScreen = Screen.PrimaryScreen.WorkingArea;
            this.Top = 10;
            this.Left = (rectScreen.Width - this.Width) / 2;
            this.Height = mIconSize + mPaddingVer * 2;
            this.TopMost = true;
            if (owner == null)
                this.Show();
            else
                this.Show(owner);

            

        }
        protected override void OnPaint(PaintEventArgs e)
        {
            base.OnPaint(e);
            Graphics g = e.Graphics;
            CommonLib.GeometryAPI.SetupGraphics(g);
            Rectangle dispRect = this.DisplayRectangle;
            mContentRect = new Rectangle(dispRect.Left + mPaddingHor, dispRect.Top + mPaddingVer, dispRect.Width - mPaddingHor * 2, dispRect.Height - mPaddingVer * 2);
            DrawIcon(g);

            DrawMsg(g);
            
        }
    
        private void DrawMsg(Graphics g)
        {
            int interval = 5;
            int msgWidth = mContentRect.Width - mIconSize- interval;
            Font msgFont = new Font("微软雅黑", 9);
            SizeF msgSize = g.MeasureString(mMsg, msgFont, msgWidth);
            if (msgSize.Height > mContentRect.Height)
            {
                this.Height = this.Height + ((int)msgSize.Height - mContentRect.Height);
                mContentRect.Height = (int)msgSize.Height;
            }

            int msgTop =mPaddingVer+ (mContentRect.Height - (int)msgSize.Height) / 2;
            Rectangle msgRect = new Rectangle(mContentRect.Left + mIconSize + interval, msgTop, msgWidth, (int)msgSize.Height);
            SolidBrush fontBrush = new SolidBrush(Color.White);
            g.DrawString(mMsg, msgFont, fontBrush, msgRect);
            msgFont.Dispose();
            fontBrush.Dispose();
            
        }
        private void DrawIcon(Graphics g)
        {
            Image iconImg = null;
            switch (mIcon)
            {
                case MessageBoxIcon.Error:
                    iconImg = PhotoHome_WinForm.Properties.Resources.msg_error;
                    break;
                case MessageBoxIcon.Question:
                    iconImg = PhotoHome_WinForm.Properties.Resources.msg_question128;
                    break;
                case MessageBoxIcon.Warning:
                    iconImg = PhotoHome_WinForm.Properties.Resources.msg_warning128;
                    break;
                case MessageBoxIcon.Information:
                    iconImg = PhotoHome_WinForm.Properties.Resources.msg_info128;
                    break;
            }

            if (iconImg != null)
            {
                Rectangle imgRect = new Rectangle(mContentRect.Left, mContentRect.Top, mIconSize, mIconSize);
                g.DrawImage(iconImg, imgRect);
            }
        }

        private void frmMsgBox_MouseDoubleClick(object sender, MouseEventArgs e)
        {
            this.Close();
        }
    }
}
