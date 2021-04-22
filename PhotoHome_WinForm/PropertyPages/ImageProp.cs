using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PropertyPages
{
    public class FileProp
    {
        public string FileType;
    }
    public class ImageProp: FileProp
    {
        public int Width;
        public int Height;
        public double Contrast;
        public double Brightness;
        
    }
}
