using CommonLib;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace PhotoHome
{
    [Serializable]
    public class AppConfig
    {
        private static volatile AppConfig mInstance = null;
        private static Object syncObj = new Object();

        private static string mConfigFileName = null;

        private List<RecentItem> mRecents = new List<RecentItem>();
        
        static AppConfig() {
            mConfigFileName = Application.StartupPath + "\\config.xml";
        }
        private AppConfig() { }
        public static AppConfig Instance
        {
            get
            {
                if (mInstance == null)
                {
                    lock (syncObj)
                    {
                        if (mInstance == null)
                        {
                            if (File.Exists(mConfigFileName))
                            {
                                XmlSerializer serializer = new XmlSerializer(typeof(AppConfig));
                                FileStream stream = new FileStream(mConfigFileName, FileMode.Open);
                                mInstance = serializer.Deserialize(stream) as AppConfig;
                                stream.Close();
                                //把不存在的文件或文件夹删除
                                mInstance.TrimRecents();
                            }
                            else
                            {
                                mInstance = new AppConfig();
                            }
                        }
                    }
                }
                return mInstance;
            }
        }

        public List<RecentItem> Recents
        {
            get
            {
                return mRecents;
            }
        }

        public void AddRecent(string path)
        {
            path = path.ToLower();

            List<RecentItem> itemsRemoved = new List<RecentItem>();
            for(int i = 0; i < mRecents.Count; i++)
            {
                if (mRecents[i].Path == path)
                {
                    itemsRemoved.Add(mRecents[i]);
                }
            }

            for(int j = 0; j < itemsRemoved.Count; j++)
            {
                mRecents.Remove(itemsRemoved[j]);
            }
            RecentItem item = new RecentItem()
            {
                Path = path,
                AccessTime = DateTime.Now
            };
            if (mRecents.Count > 0)
                mRecents.Insert(0, item);
            else
                mRecents.Add(item);

            if (mRecents.Count > 20)
            {
                mRecents.RemoveRange(20, mRecents.Count - 20);
            }

           
        }

        private void TrimRecents()
        {
            List<RecentItem> itemsRemoved = new List<RecentItem>();
            for (int i = 0; i < mRecents.Count; i++)
            {
                if (File.Exists(mRecents[i].Path)==false&&Directory.Exists(mRecents[i].Path)==false)
                {
                    itemsRemoved.Add(mRecents[i]);
                }
            }

            for (int j = 0; j < itemsRemoved.Count; j++)
            {
                mRecents.Remove(itemsRemoved[j]);
            }
        }
        private string mSelectedTargetFolder = null;
        public string SelectedTargetFolder
        {
            get
            {
                return mSelectedTargetFolder;
            }
            set
            {
                mSelectedTargetFolder = value;
              
            }
        }
        private int mAppLayout = 1;
        public int AppLayout {
            set {
                mAppLayout = value;
              
            }
            get {
                return mAppLayout;
            }
        }

        public int SurppotedFileTypes { get => (int)Global.CurrentSurpportedType; set => Global.CurrentSurpportedType =(EnumFileType)value; }
        public bool FindInSubFolders { get => Global.FolderRecursion; set => Global.FolderRecursion = value; }

        public void Save()
        {
            XmlSerializer serializer = new XmlSerializer(typeof(AppConfig));
            StreamWriter writer = new StreamWriter(mConfigFileName);
            serializer.Serialize(writer, this);
            writer.Close();
        }

    }

    [Serializable]
    public class RecentItem
    {
        public string Path { get; set; }
        public DateTime AccessTime { get; set; }
    }
}
