using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SuperControls;
namespace PhotoHome
{
    public partial class frmInfoPanel : frmFloatBase
    {
        public frmInfoPanel()
        {
            InitializeComponent();
            this.KeyUp += InfoPanel_KeyUp;
        }

        private void InfoPanel_KeyUp(object sender, KeyEventArgs e)
        {
            frmMain mainForm = CommonLib.Common.GetMainForm() as frmMain;
            mainForm.WhenKeyUp(sender, e);
        }

        public void AppendText(string title,string content)
        {
            if (this.infoPanel.InvokeRequired == true)
            {
                CommonLib.SetTwoStringValueCallback callback = new CommonLib.SetTwoStringValueCallback(AppendText);
                this.Invoke(callback, new object[] { title,content });
            }
            else
            {

                if (String.IsNullOrEmpty(content)==false)
                {
                    infoPanel.AddLine(new SuperControls.TextLine(new SuperControls.TextField(title), new SuperControls.TextField(content)));
                }
                else{
                    infoPanel.AddLine(new SuperControls.TextLine(new SuperControls.SectionTitle(title)));
                }
                
            }
        }

        public void Fill(List<TextLine> txtLines)
        {
            if (txtLines == null) return;
            ClearContent();
            for(int i = 0; i < txtLines.Count; i++)
            {
                TextLine txtLine = txtLines[i];
                if (txtLine.ContentField == null)
                {
                    if (txtLine.HasSublines)
                    {
                        infoPanel.AddLine(txtLine);
                        infoPanel.AddLines(txtLine.SubLines);
                    }
                }
                else
                {
                    infoPanel.AddLine(txtLine);
                }
                
            }
            infoPanel.IsDirty = true;
            Refresh();
        }
        public override void Refresh()
        {
            //base.Refresh();
            infoPanel.Invalidate();
        }
        public void ClearContent()
        {
            
            infoPanel.ClearLines();
            infoPanel.Refresh();
        }

        
    }
}
