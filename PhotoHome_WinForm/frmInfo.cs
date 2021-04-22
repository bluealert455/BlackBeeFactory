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
    public partial class frmInfo : frmFloatBase
    {
        public frmInfo()
        {
            InitializeComponent();
        }

        public RichTextBox InfoBox
        {
            get
            {
                return this.richtxtInfo;
            }
        }
        public void AppendText(string text)
        {
            if (this.richtxtInfo.InvokeRequired == true)
            {
                CommonLib.SetStringValueCallback callback = new CommonLib.SetStringValueCallback(AppendText);
                this.Invoke(callback, new object[] { text });
            }
            else
            {
                if (text.EndsWith("\n") == false || text.EndsWith("\r") == false)
                    text = text + "\n";
                this.richtxtInfo.AppendText(text);
                this.richtxtInfo.Update();

            }
        }
        public void ClearContent()
        {
            if (this.richtxtInfo.InvokeRequired == true)
            {
                CommonLib.VoidAndNonParamHandler callback = new CommonLib.VoidAndNonParamHandler(ClearContent);
                this.Invoke(callback);
            }
            else
            {
                richtxtInfo.Clear();

            }
        }

        
    }
}
