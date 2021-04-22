using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace Utils
{
    public class ConfigMgr
    {
        public static string ConfigXMLFile = Path.Combine(Application.StartupPath, "config.xml");

    }
}
