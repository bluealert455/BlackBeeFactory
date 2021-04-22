using CommonLib;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Utils
{
    public class ProgressUIQueue
    {
        public static int FormLeft = 10;
        public static int FormWidth = 280;
        public static int BaseBottom = 200;
        private static Dictionary<string, PhotoHome.frmProgress> mProgForms = null;
        static ProgressUIQueue()
        {
            mProgForms = new Dictionary<string, PhotoHome.frmProgress>();
        }

        public static void ShowProgressForm(string key,string prompt, ProgressBarStyle barStyle = ProgressBarStyle.Marquee, int min = 0,int max = 100)
        {
            lock (mProgForms)
            {
                PhotoHome.frmProgress progForm = new PhotoHome.frmProgress();
                progForm.Init(barStyle, prompt, min, max);
                Form mainForm = CommonLib.Common.GetMainForm();
                int baseTop = mainForm.Height - BaseBottom;
                int cnt = 0;
                mProgForms.Add(key, progForm);
                foreach (KeyValuePair<string, PhotoHome.frmProgress> kvp in mProgForms)
                {
                    baseTop -= kvp.Value.Height;
                    cnt++;
                }
                progForm.Location = new System.Drawing.Point(FormLeft, baseTop);
                progForm.Width = FormWidth;
                progForm.FormClosed += ProgForm_FormClosed;
                progForm.Show(mainForm);
            }
            
        }

        private static void ProgForm_FormClosed(object sender, FormClosedEventArgs e)
        {
            RemoveByValue(sender as PhotoHome.frmProgress);
        }

        private static void RemoveByValue(PhotoHome.frmProgress progBox)
        {
            string key = null;
            foreach (KeyValuePair<string, PhotoHome.frmProgress> kvp in mProgForms)
            {
                if (kvp.Value == progBox)
                    key = kvp.Key;
            }
            if (key != null)
                mProgForms.Remove(key);
        }
        public static void UpdateValue(string key, int val)
        {
            if (mProgForms.ContainsKey(key))
            {
                mProgForms[key].UpdateValue(val);
            }
        }
        public static void UpdatePrompt(string key, string prompt)
        {
            if (mProgForms.ContainsKey(key))
            {
                mProgForms[key].UpdatePrompt(prompt);
            }
        }

        public static void CloseProgressForm(string key)
        {
            lock (mProgForms)
            {
                if (mProgForms.ContainsKey(key))
                {
                    mProgForms[key].CloseMe();
                    mProgForms.Remove(key);

                }
            }
            
        }
        
    }
}
