using CommonLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Utils
{
    public class MessageUIQueue
    {
        public static int FormLeft = 0;
        public static int FormWidth = 300;
        public static int BaseBottom = 200;
        private static Dictionary<string, PhotoHome.frmMsgBox> mMsgForms = null;
        static MessageUIQueue()
        {
            mMsgForms = new Dictionary<string, PhotoHome.frmMsgBox>();
        }

        public static void ShowMsgForm(string key, MessageBoxIcon icon, string msg)
        {
            lock (mMsgForms)
            {
                PhotoHome.frmMsgBox msgForm = new PhotoHome.frmMsgBox();
                msgForm.Init(icon,msg);
                Form mainForm = CommonLib.Common.GetMainForm();
                FormLeft =mainForm.Left+(mainForm.Width - FormWidth) / 2;
                int baseTop = 40;
                int cnt = 0;
                foreach (KeyValuePair<string, PhotoHome.frmMsgBox> kvp in mMsgForms)
                {
                    baseTop += kvp.Value.Height;
                    cnt++;
                }
                msgForm.Location = new System.Drawing.Point(FormLeft, baseTop);
                msgForm.Width = FormWidth;
                mMsgForms.Add(key, msgForm);

                msgForm.FormClosed += MsgForm_FormClosed;
                msgForm.Show(mainForm);

            }

        }

        private static void MsgForm_FormClosed(object sender, FormClosedEventArgs e)
        {

            RemoveByValue(sender as PhotoHome.frmMsgBox);
        }
        
        private static void RemoveByValue(PhotoHome.frmMsgBox msgBox)
        {
            string key = null;
            foreach (KeyValuePair<string, PhotoHome.frmMsgBox> kvp in mMsgForms)
            {
                if (kvp.Value == msgBox)
                    key = kvp.Key;
            }
            if (key != null)
                mMsgForms.Remove(key);
        }
        public static void CloseMsgForm(string key)
        {
            lock (mMsgForms)
            {
                if (mMsgForms.ContainsKey(key))
                {
                    mMsgForms[key].InvokeIfRequired(l =>
                    {
                        l.Close();
                    });

                    mMsgForms.Remove(key);

                }
            }

        }
    }
}
