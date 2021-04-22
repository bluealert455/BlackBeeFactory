using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using System.Xml;
namespace PhotoHome
{
    public class Config
    {
        private static string ConfigFileName = null;
        static Config()
        {
            ConfigFileName = Application.StartupPath + "\\config.xml";
        }

        private static XmlDocument GetConfigDoc(bool create)
        {
            if (File.Exists(ConfigFileName))
            {
                XmlDocument doc = new XmlDocument();
                doc.Load(ConfigFileName);
                return doc;
            }
            else
            {
                if (create)
                {
                    XmlDocument doc = new XmlDocument();
                    doc.LoadXml("<?xml version=\"1.0\" encoding=\"utf-8\" ?><config></config>");
                    return doc;
                }
                return null;
            }
        }
        private static XmlElement GetRecentsElement(XmlDocument doc)
        {
            XmlNode node = doc.SelectSingleNode("./config/recents");
            if (node != null)
                return doc.SelectSingleNode("./config/recents") as XmlElement;
            else
                return null;
        }
        private static XmlElement GetSettingsElement(XmlDocument doc)
        {
            XmlNode node = doc.SelectSingleNode("./config/settings");
            if (node != null)
                return doc.SelectSingleNode("./config/settings") as XmlElement;
            else
                return null;
        }
        public static List<String> GetRecentNames()
        {
            
            XmlDocument doc = GetConfigDoc(false);
            if (doc == null) return null;
            
            XmlElement recentsEle = GetRecentsElement(doc);
            if (recentsEle == null || recentsEle.ChildNodes.Count == 0) return null;
            List<String> lstTemp = new List<string>();

            for(int i = 0; i < recentsEle.ChildNodes.Count; i++)
            {
                XmlNode node = recentsEle.ChildNodes[i];
                lstTemp.Add(node.Attributes["path"].InnerText);
            }
            return lstTemp;
        }

        public static void SaveRecent(string recent)
        {
            XmlDocument doc = GetConfigDoc(true);
            XmlElement recentsEle = GetRecentsElement(doc);
            if (recentsEle == null)
            {
                recentsEle = doc.CreateElement("recents");
                doc.DocumentElement.AppendChild(recentsEle);
            }

            recent = recent.ToLower();
            string path = "./recent[@path='" + recent + "']";
            XmlNode nodeExisted = recentsEle.SelectSingleNode(path);
            if (nodeExisted != null)
                recentsEle.RemoveChild(nodeExisted);

            XmlElement recentEle = doc.CreateElement("recent");
            recentEle.SetAttribute("dt", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"));
            recentEle.InnerText = recent;
            if (recentsEle.ChildNodes.Count > 0)
                recentsEle.InsertBefore(recentEle, recentsEle.ChildNodes[0]);
            else
                recentsEle.AppendChild(recentEle);

            doc.Save(ConfigFileName);
        }

        public static string GetSelectTargetFolder()
        {
            XmlDocument doc = GetConfigDoc(false);
            if (doc == null) return null;
            XmlElement settingsEle = GetSettingsElement(doc);
            if(settingsEle!=null)
            {
                XmlNode nodeTemp = settingsEle.SelectSingleNode("./targetFolder");
                if (nodeTemp != null)
                {
                    return nodeTemp.InnerText;
                }
            }
            return null;
        }

        public static void  SaveSelectTargetFolder(string targetFolder)
        {
            XmlDocument doc = GetConfigDoc(true);
            XmlElement settingsEle = GetSettingsElement(doc);
            if (settingsEle == null)
            {
                settingsEle = doc.CreateElement("recents");
                doc.DocumentElement.AppendChild(settingsEle);
            }
        }
    }
}
