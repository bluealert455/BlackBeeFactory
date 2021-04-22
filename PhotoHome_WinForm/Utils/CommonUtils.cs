using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils
{
    public delegate void FileCopyCompleteHandler(string srcFile, string targetFile, bool canceled);
    public class CommonUtils
    {
        public static void Copy(string srcFile, string tgtFile, bool move,PhotoHome.frmProgress progWindow, FileCopyCompleteHandler callback)
        {
            byte[] buffer = new byte[1024 * 1024]; // 1MB buffer
            bool canceled = false;
            using (FileStream source = new FileStream(srcFile, FileMode.Open, FileAccess.Read))
            {
                long fileLength = source.Length;
                using (FileStream dest = new FileStream(tgtFile, FileMode.CreateNew, FileAccess.Write))
                {
                    long totalBytes = 0;
                    int currentBlockSize = 0;

                    while ((currentBlockSize = source.Read(buffer, 0, buffer.Length)) > 0)
                    {
                        totalBytes += currentBlockSize;
                        double persentage = (double)totalBytes * 100.0 / fileLength;

                        dest.Write(buffer, 0, currentBlockSize);
                        if(progWindow!=null)
                        {
                            progWindow.UpdateValue((int)persentage);
                            if (progWindow.Canceled)
                            {
                                canceled = true;
                                break;
                            }
                                
                        }
                        
                    }
                }
            }

            if (canceled == true)
            {
                File.Delete(tgtFile);
            }
            else
            {
                if (move == true)
                    File.Delete(srcFile);
            }

            if (callback != null)
                callback(srcFile, tgtFile, canceled);
            if (progWindow != null)
                progWindow.CloseMe();

        }
    }
}
