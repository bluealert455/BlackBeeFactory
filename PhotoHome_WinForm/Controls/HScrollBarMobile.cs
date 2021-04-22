using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Data;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Controls
{
    public partial class HScrollBarMobile : VScrollBarMobile
    {
        public HScrollBarMobile():base()
        {
            this.Width = 30;
            this.Height = 13;
        }

        protected override void OnParentChanged(EventArgs e)
        {
            base.OnParentChanged(e);
            this.Width = this.Parent.Width - 2;
            this.Height = 10;
            this.Top = 2;
            this.Left = 1;

            AttachEvents();
        }
        protected override void SetCtrlSize()
        {
            mCtrlSize = this.Width;
        }

        protected override int GetPosInMouseEvent(MouseEventArgs e)
        {
            return e.X;
        }
        protected override void CalcValue(int pos)
        {
            mCtrlSize = this.Width;
            base.CalcValue(pos);
        }
        protected override int GetBarRoundedRadius()
        {
            return this.Height / 2;
        }

        protected override void CreateHoldRect(int left, int top, int width, int height)
        {
            
            base.CreateHoldRect(top,left, height, this.Height-2);
        }
    }
}
