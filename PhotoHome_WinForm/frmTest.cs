using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using SuperControls;
namespace PhotoHome
{
    public partial class frmTest : Form
    {
        public frmTest()
        {
            InitializeComponent();
            this.Shown += FrmTest_Shown;
        }

        private void FrmTest_Shown(object sender, EventArgs e)
        {
            this.timer1.Start();
        }

        private void vScrollBarMobile1_Click(object sender, EventArgs e)
        {
            
        }

        private void btnGetCBData_Click(object sender, EventArgs e)
        {
            StringCollection fileCopied = Clipboard.GetFileDropList();
            this.lstCBFiles.Items.Add("---------------------------------------------");

            for(int i=0;i<fileCopied.Count;i++)
            {
                lstCBFiles.Items.Add(fileCopied[i]);
            }
            

            Type t = typeof(DataFormats);
            FieldInfo[] fis = t.GetFields();
            for(int k = 0; k < fis.Length; k++)
            {
                FieldInfo fi = fis[k];
                if (fi.IsStatic)
                {
                    object b=fi.GetValue(null);
                   
                    AddItem(b.ToString());



                }
                

            }
            AddItem("Preferred DropEffect");
            AddItem("Performed DropEffect");

            Object obj = Clipboard.GetDataObject();
            IDataObject dataObj = obj as IDataObject;

        

        }

        private void AddItem(string name)
        {
            this.lstCBFiles.Items.Add(name);
            Object objDrop = Clipboard.GetData(name);
            if (objDrop != null)
            {
                this.lstCBFiles.Items.Add("value:"+objDrop.ToString());

                if(objDrop is MemoryStream)
                {
                    MemoryStream ms = objDrop as MemoryStream;
                    byte[] vals = new byte[ms.Capacity];
                    ms.Read(vals, 0, ms.Capacity);

                    this.lstCBFiles.Items.Add(CommonLib.Common.ByteArray2Str(vals));
                }
            }
        }
        private void lstCBFiles_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            this.lstCBFiles.Items.Clear();
        }

        private void vScrollBarMobile1_OnScroll(object sender, ScrollEventArgs e)
        {
            this.Validate();
        }

        private void btnShowLayerForm_Click(object sender, EventArgs e)
        {
            SuperToolbarWindow tb = new SuperToolbarWindow();
            SuperToolBarItem newItem = new SuperToolBarItem("tbOpen", "选择文件夹或文件", PhotoHome.Properties.Resources.tb_open);
            tb.AddItem(newItem);
            newItem = new SuperToolBarItem("tbPaste", "粘贴", PhotoHome.Properties.Resources.tb_paste);
            tb.AddItem(newItem);

            newItem = new SuperToolBarItem();
            newItem.Type = EnumToolBarItemType.Seperator;
            tb.AddItem(newItem);

            newItem = new SuperToolBarItem("tbFullExtent", "全图显示", PhotoHome.Properties.Resources.tb_fullextent);
            tb.AddItem(newItem);
            newItem = new SuperToolBarItem("tbRealSize", "实际尺寸", PhotoHome.Properties.Resources.tb_realsize);
            tb.AddItem(newItem);
            newItem = new SuperToolBarItem("tbRotateLeft", "逆时针旋转90度", PhotoHome.Properties.Resources.tb_rotateleft);
            tb.AddItem(newItem);
            newItem = new SuperToolBarItem("tbRotateRight", "顺时针旋转90度", PhotoHome.Properties.Resources.tb_rotateright);
            tb.AddItem(newItem);
            newItem = new SuperToolBarItem("tbFlipHor", "水平翻转", PhotoHome.Properties.Resources.tb_fliphor);
            tb.AddItem(newItem);
            newItem = new SuperToolBarItem("tbFlipVer", "垂直翻转", PhotoHome.Properties.Resources.tb_flipver);
            tb.AddItem(newItem);

            newItem = new SuperToolBarItem();
            newItem.Type = EnumToolBarItemType.Seperator;
            tb.AddItem(newItem);

            newItem = new SuperToolBarItem("tbPlayOrPause", "播放或暂停", PhotoHome.Properties.Resources.tb_play);
            tb.AddItem(newItem);
            newItem = new SuperToolBarItem("tbStop", "停止", PhotoHome.Properties.Resources.tb_stop);
            tb.AddItem(newItem);
            newItem = new SuperToolBarItem("tbSnapshot", "视频截屏", PhotoHome.Properties.Resources.tb_snapshot);
            tb.AddItem(newItem);

            newItem = new SuperToolBarItem();
            newItem.Type = EnumToolBarItemType.Seperator;
            tb.AddItem(newItem);

            newItem = new SuperToolBarItem("tbSelect", "收藏", PhotoHome.Properties.Resources.tb_select);
            tb.AddItem(newItem);
            newItem = new SuperToolBarItem("tbSetFavFolder", "设置收藏夹", PhotoHome.Properties.Resources.tb_favfolder);
            tb.AddItem(newItem);
            newItem = new SuperToolBarItem("tbAutoSlide", "自动切换", PhotoHome.Properties.Resources.tb_slidestart);
            tb.AddItem(newItem);
            newItem = new SuperToolBarItem("tbFullScreen", "全屏", PhotoHome.Properties.Resources.tb_fullscreen);
            tb.AddItem(newItem);

            newItem = new SuperToolBarItem("tbLayout", "设置界面布局", PhotoHome.Properties.Resources.tb_layoutmodern);
            tb.AddItem(newItem);

            newItem = new SuperToolBarItem();
            newItem.Type = EnumToolBarItemType.Seperator;
            tb.AddItem(newItem);

            newItem = new SuperToolBarItem("tbSave", "保存", PhotoHome.Properties.Resources.tb_save);
            tb.AddItem(newItem);
            newItem = new SuperToolBarItem("tbQuit", "退出", PhotoHome.Properties.Resources.tb_quit);
            tb.AddItem(newItem);

            tb.Left = 100;
            tb.Top = 100;
            tb.ShowMe(this);
        }

        private void btnShowCustomWindow_Click(object sender, EventArgs e)
        {
            frmExtendForm f = new frmExtendForm();
            f.Show(this);
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            int newValue= this.superProgress1.Value + 1;
            if (newValue > 100)
                newValue = 0;
            
            this.superProgress1.Value = newValue;

        }
    }
}
