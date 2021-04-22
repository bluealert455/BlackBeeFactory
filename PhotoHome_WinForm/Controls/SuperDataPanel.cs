using System;
using System.Collections;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Drawing;
using System.Drawing.Drawing2D;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;
using CommonLib;
using PhotoHome_WinForm;
using PropertyPages;
using Shell32;

namespace Controls
{
    public delegate void FileInfoChangedHandler(List<TextLine> infoFlds,object propInfo);
    public class SuperDataPanel : SuperPictureBox
    {
        enum EnumViewModel
        {
            FitToWindow = 1,
            RealSize = 2,
            Zoomed = 3
        }

        enum EnumFileType
        {
            None = 0,
            Image = 1,
            Video = 2,
            Tif = 3,
            Shp = 4,
            Unsurpported = 5
        }
        private double mCanvasHeight = -1, mCanvasWidth = -1;
        private EnumViewModel mViewModel;
        

        public static String[] maPhotoTypes = null;
        public static String[] maVideoTypes = null;
        public static String[] maTiffTypes = null;
        public static String[] maGISTypes = null;
        public static String[] maAllSurpportedTypes = null;

        public static String mPhotoFilter = null;
        public static String mVideoFilter = null;
        public static String mTiffFilter = null;
        public static String mGISFilter = null;

        
        private frmArrow mLeftArrowForm = null;
        private frmArrow mRightArrowForm = null;
        private EnumFileType mCurFileType = EnumFileType.None;

        private ThumbList mThumbnailList = null;
        private String mCurFileName,mCurrentPath;

        private FileInfoChangedHandler mFileInfoChangedHandler = null;
        private VoidAndNonParamHandler mBeforeLoadFileHandler = null;
        static SuperDataPanel()
        {
            maPhotoTypes = new String[] { "*.bmp", "*.png", "*.gif", "*.jpeg", "*.jpg" };
            maVideoTypes = new String[] { "*.avi", "*.mp4", "*.wmv", "*.mkv", "*.rmvb" };
            maTiffTypes = new String[] { "*.tif", "*.tiff" };
            maGISTypes = new String[] { "*.shp" };

            mPhotoFilter = String.Join(";", maPhotoTypes);
            mVideoFilter = String.Join(";", maVideoTypes);
            mTiffFilter = String.Join(";", maTiffTypes);
            mGISFilter = String.Join(";", maGISTypes);

            //maAllSurpportedTypes = new string[maPhotoTypes.Length + maVideoTypes.Length + maTiffTypes.Length + maGISTypes.Length];
            //maPhotoTypes.CopyTo(maAllSurpportedTypes, 0);
            //maVideoTypes.CopyTo(maAllSurpportedTypes, maPhotoTypes.Length);
            //maTiffTypes.CopyTo(maAllSurpportedTypes, maPhotoTypes.Length + maVideoTypes.Length);
            //maGISTypes.CopyTo(maAllSurpportedTypes, maPhotoTypes.Length + maVideoTypes.Length + maTiffTypes.Length);

            maAllSurpportedTypes = new string[maPhotoTypes.Length];
            maPhotoTypes.CopyTo(maAllSurpportedTypes, 0);
        }

        public override Color BackColor
        {
            get
            {
                return base.BackColor;
            }
            set
            {
                if (base.BackColor != value)
                {
                    base.BackColor = value;
                    Invalidate();
                }

            }
        }
        public SuperDataPanel()
        {
            if (DesignMode)
                return;
            mViewModel = EnumViewModel.FitToWindow;

            
            this.Resize += SuperDataPanel_Resize;
            
            mLeftArrowForm = new frmArrow();

            mLeftArrowForm.Setup(PhotoHome_WinForm.ArrowDirection.Left, this);
            mLeftArrowForm.MouseClick += MLeftArrowForm_MouseClick;
            mRightArrowForm = new frmArrow();
            mRightArrowForm.Setup(PhotoHome_WinForm.ArrowDirection.Right, this);
            mRightArrowForm.MouseClick += MRightArrowForm_MouseClick;

        }
        public event FileInfoChangedHandler OnFileInfoChanged
        {
            add
            {
                lock (this)
                {
                    mFileInfoChangedHandler += value;
                }
            }
            remove
            {
                lock (this)
                {
                    if (mFileInfoChangedHandler != null)
                        mFileInfoChangedHandler -= value;
                }
            }
        }

        public event VoidAndNonParamHandler OnBeforeLoadFile
        {
            add
            {
                lock (this)
                {
                    mBeforeLoadFileHandler += value;
                }
            }
            remove
            {
                lock (this)
                {
                    if (mBeforeLoadFileHandler != null)
                        mBeforeLoadFileHandler -= value;
                }
            }
        }
        private Dictionary<String, String> CreatePhotoMetadataDic()
        {
            Dictionary<String, String> dic = new Dictionary<string, string>();
            dic.Add("section1", "来源");
            dic.Add("作者", "");
            dic.Add("标题", "");
            dic.Add("拍摄时间", "");
            dic.Add("修改时间", "");

            dic.Add("section2", "相机参数");
            dic.Add("制造商", "");
            dic.Add("型号", "");
            dic.Add("相机软件", "");
            dic.Add("光圈值", "");
            dic.Add("曝光时间(秒)", "");
            dic.Add("ISO", "");
            dic.Add("曝光补偿", "");
            dic.Add("焦距", "");
            dic.Add("最大光圈", "");
            dic.Add("测光模式", "");
            dic.Add("目标距离", "");
            dic.Add("闪光灯模式", "");
            dic.Add("闪光灯能量", "");
            dic.Add("35mm焦距", "");

            dic.Add("section3", "位置");
            dic.Add("经度", "");
            dic.Add("纬度", "");
            dic.Add("高度", "");

            return dic;
        }

        private Dictionary<int, String> CreateIdAndTitleDic()
        {
            Dictionary<int, String> dic = new Dictionary<int, string>();
            dic.Add(315, "作者");
            dic.Add(0x0320, "标题");
            dic.Add(306, "拍摄时间");

            dic.Add(0x010F, "制造商");
            dic.Add(0x0110, "型号");
            dic.Add(305, "相机软件");
            dic.Add(37378, "光圈值");
            dic.Add(0x829A, "曝光时间(秒)");
            dic.Add(34867, "ISO");
            dic.Add(41986, "曝光补偿");
            dic.Add(37386, "焦距");
            dic.Add(37381, "最大光圈");
            dic.Add(37383, "测光模式");
            dic.Add(37382, "目标距离");
            dic.Add(37385, "闪光灯模式");
            dic.Add(37387, "闪光灯能量");
            dic.Add(41989, "35mm焦距");

            dic.Add(4, "经度");
            dic.Add(2, "纬度");
            dic.Add(6, "高度");


            return dic;

        }
        public void AdjustContrast(int threshold)
        {
            Bitmap sourceBitmap = mImage as Bitmap;
            BitmapData sourceData = sourceBitmap.LockBits(new Rectangle(0, 0,
                                        sourceBitmap.Width, sourceBitmap.Height),
                                        ImageLockMode.ReadOnly, PixelFormat.Format32bppArgb);


            byte[] pixelBuffer = new byte[sourceData.Stride * sourceData.Height];
            Marshal.Copy(sourceData.Scan0, pixelBuffer, 0, pixelBuffer.Length);
            sourceBitmap.UnlockBits(sourceData);
            
            double contrastLevel = Math.Pow((100.0 + threshold) / 100.0, 2);
            
            double blue = 0;
            double green = 0;
            double red = 0;


            for (int k = 0; k + 4 < pixelBuffer.Length; k += 4)
            {
                blue = ((((pixelBuffer[k] / 255.0) - 0.5) *
                            contrastLevel) + 0.5) * 255.0;


                green = ((((pixelBuffer[k + 1] / 255.0) - 0.5) *
                            contrastLevel) + 0.5) * 255.0;


                red = ((((pixelBuffer[k + 2] / 255.0) - 0.5) *
                            contrastLevel) + 0.5) * 255.0;


                if (blue > 255)
                { blue = 255; }
                else if (blue < 0)
                { blue = 0; }


                if (green > 255)
                { green = 255; }
                else if (green < 0)
                { green = 0; }


                if (red > 255)
                { red = 255; }
                else if (red < 0)
                { red = 0; }


                pixelBuffer[k] = (byte)blue;
                pixelBuffer[k + 1] = (byte)green;
                pixelBuffer[k + 2] = (byte)red;
            }


            Bitmap resultBitmap = new Bitmap(sourceBitmap.Width, sourceBitmap.Height);
            
            BitmapData resultData = resultBitmap.LockBits(new Rectangle(0, 0,
                                        resultBitmap.Width, resultBitmap.Height),
                                        ImageLockMode.WriteOnly, PixelFormat.Format32bppArgb);


            Marshal.Copy(pixelBuffer, 0, resultData.Scan0, pixelBuffer.Length);
            resultBitmap.UnlockBits(resultData);
            mImage = resultBitmap;

            Invalidate();
            
        }
        public void AdjustBrightness(int Value)
        {

            Bitmap TempBitmap = mImage as Bitmap;

            Bitmap NewBitmap = new Bitmap(TempBitmap.Width, TempBitmap.Height);

            Graphics NewGraphics = Graphics.FromImage(NewBitmap);

            float FinalValue = (float)Value / 100.0f; //255.0f;

            float[][] FloatColorMatrix ={

                    new float[] {1, 0, 0, 0, 0},

                    new float[] {0, 1, 0, 0, 0},

                    new float[] {0, 0, 1, 0, 0},

                    new float[] {0, 0, 0, 1, 0},

                    new float[] {FinalValue, FinalValue, FinalValue, 1, 1}
                };

            ColorMatrix NewColorMatrix = new ColorMatrix(FloatColorMatrix);

            ImageAttributes Attributes = new ImageAttributes();

            Attributes.SetColorMatrix(NewColorMatrix);

            NewGraphics.DrawImage(TempBitmap, new Rectangle(0, 0, TempBitmap.Width, TempBitmap.Height), 0, 0, TempBitmap.Width, TempBitmap.Height, GraphicsUnit.Pixel, Attributes);

            Attributes.Dispose();

            NewGraphics.Dispose();

            mImage = NewBitmap;
            Invalidate();
        }
        private Image mOldImage = null;
        public void AdjustContrastAndBright(int contrastVal,int brightnessVal)
        {
            if (mOldImage == null) mOldImage = mImage.Clone() as Image;
            mImage = ApplyContrastAndBright(mOldImage as Bitmap, (float)contrastVal / 100.0f, (float)brightnessVal/100.0f);
            Invalidate();
        }

        public void ResizeImage(int w,int h)
        {
            if (mOldImage == null) mOldImage = mImage.Clone() as Image;
            var destRect = new Rectangle(0, 0, w, h);
            var destImage = new Bitmap(w, h);
            /*
             * maintains DPI regardless of physical size -- may increase quality when reducing image dimensions or when printing
             * 
             * */
            destImage.SetResolution(mOldImage.HorizontalResolution, mOldImage.VerticalResolution);

            using (var graphics = Graphics.FromImage(destImage))
            {
                graphics.CompositingMode = CompositingMode.SourceCopy;
                graphics.CompositingQuality = CompositingQuality.HighQuality;
                graphics.InterpolationMode = InterpolationMode.HighQualityBicubic;
                graphics.SmoothingMode = SmoothingMode.HighQuality;
                graphics.PixelOffsetMode = PixelOffsetMode.HighQuality;

                using (var wrapMode = new ImageAttributes())
                {
                    /*
                     * prevents ghosting around the image borders -- 
                     * naïve resizing will sample transparent pixels 
                     * beyond the image boundaries, but by mirroring 
                     * the image we can get a better sample (this setting is very noticeable)
                     * 
                     */
                    wrapMode.SetWrapMode(WrapMode.TileFlipXY);
                    graphics.DrawImage(mOldImage, destRect, 0, 0, mOldImage.Width, mOldImage.Height, GraphicsUnit.Pixel, wrapMode);
                }
            }

            mImage = destImage;
            Invalidate();
        }
        public static bool IsIndexedPixelFormat(PixelFormat fmt)
        {
            return fmt.ToString().EndsWith("Indexed");
        }
        public static Bitmap ApplyContrastAndBright(Bitmap srcBmp, float contrast, float bightness)
        {

            byte[] maptable = new byte[256];//查找表
            for (int i = 0; i < 256; i++)
            {
                float k = (float)Math.Tan((45 + 44.9999999 * contrast) / 180f * Math.PI);
                float y = (i - 127.5f * (1 - bightness)) * k + 127.5f * (1 + bightness);
                maptable[i] = (byte)Math.Min(255, Math.Max(0, y));
            }
            Bitmap tgtbmp;//
            if (IsIndexedPixelFormat(srcBmp.PixelFormat))//处理索引颜色的像素格式
            {
                tgtbmp = (Bitmap)srcBmp.Clone();
                ColorPalette cp = tgtbmp.Palette;
                for (int i = 0; i < tgtbmp.Palette.Entries.Length; i++)
                {
                    cp.Entries[i] = Color.FromArgb(maptable[cp.Entries[i].R], maptable[cp.Entries[i].G], maptable[cp.Entries[i].B]);
                }
                tgtbmp.Palette = cp;
            }
            else
            {
                tgtbmp = new Bitmap(srcBmp.Width, srcBmp.Height, srcBmp.PixelFormat);
                BitmapData srcdata = srcBmp.LockBits(new Rectangle(0, 0, srcBmp.Width, srcBmp.Height), ImageLockMode.ReadOnly, srcBmp.PixelFormat);
                BitmapData tgtdata = tgtbmp.LockBits(new Rectangle(0, 0, tgtbmp.Width, tgtbmp.Height), ImageLockMode.WriteOnly, srcBmp.PixelFormat);
                int pxsize = System.Drawing.Image.GetPixelFormatSize(srcBmp.PixelFormat) / 8;//像素大小(字节)
                bool isAlpha = System.Drawing.Image.IsAlphaPixelFormat(srcBmp.PixelFormat);
                int offset = srcdata.Stride - srcdata.Width * pxsize;
                unsafe
                {
                    byte* srcptr = (byte*)srcdata.Scan0;
                    byte* tgtptr = (byte*)tgtdata.Scan0;
                    for (int i = 0; i < srcdata.Height; i++)
                    {
                        for (int j = 0; j < srcdata.Width; j++)
                        {
                            for (int w = 0; w < pxsize; w++)
                            {
                                if (isAlpha && w == pxsize - 1)//Alpha通道直接复制，不处理（通常Alpha通道位于像素最后部件）
                                    *tgtptr = *srcptr;
                                else *tgtptr = maptable[*srcptr];
                                srcptr++; tgtptr++;
                            }
                        }
                        srcptr += offset;
                        tgtptr += offset;
                    }
                }
                srcBmp.UnlockBits(srcdata);
                tgtbmp.UnlockBits(tgtdata);
            }
            return tgtbmp;
        }


        public void SetCurThumbList(ThumbList thumbList)
        {
            mThumbnailList = thumbList;
        }
        private void MLeftArrowForm_MouseClick(object sender, MouseEventArgs e)
        {
            PreviousFile();
        }

        private void MRightArrowForm_MouseClick(object sender, MouseEventArgs e)
        {
            NextFile();
        }

        
        public void SaveImage(string targetFolder=null)
        {
            if (targetFolder == null)
            {
                CloseStreamReader();
                mImage.Save(mCurFileName);
            }
            else
            {
                Common.EnsureDirExisted(targetFolder);
                string targetFile = Path.Combine(targetFolder, Path.GetFileName(mCurFileName));
                mImage.Save(targetFile);
            }
            
        }
        private void SuperDataPanel_Resize(object sender, EventArgs e)
        {
            mCanvasWidth = this.Width;
            mCanvasHeight = this.Height;
        }
        
        private EnumFileType GetFileType(String FileName)
        {
            FileInfo info = new FileInfo(FileName);
            String ext = info.Extension;
            String pattern = "*" + ext;

            if (CommonLib.Common.GetPosInArray(maPhotoTypes, pattern) > -1)
            {
                return EnumFileType.Image;
            }
            if (CommonLib.Common.GetPosInArray(maVideoTypes, pattern) > -1)
            {
                return EnumFileType.Video;
            }
            if (CommonLib.Common.GetPosInArray(maTiffTypes, pattern) > -1)
            {
                return EnumFileType.Tif;
            }
            if (CommonLib.Common.GetPosInArray(maGISTypes, pattern) > -1)
            {
                return EnumFileType.Shp;
            }

            return EnumFileType.Unsurpported;
        }
        
        public bool LoadDataFromFile(String fileName)
        {
            if (fileName == mCurFileName)
                return true;
            mCurFileName = fileName;
            mCurFileType = GetFileType(fileName);
            mCurrentPath = Path.GetFullPath(fileName);

            List<TextLine> tmpFlds = null;
            switch (mCurFileType)
            {
                case EnumFileType.Image:
                    LoadImage(fileName);
                    tmpFlds = ReadImageInfo();

                    mOldImage = null;
                    break;
                case EnumFileType.Video:
                    break;
                case EnumFileType.Tif:
                    break;
                case EnumFileType.Shp:
                    break;
                default:
                    break;
            }

            if (mFileInfoChangedHandler != null)
                mFileInfoChangedHandler(tmpFlds, CreateImageProp());
            return true;
        }

        private ImageProp CreateImageProp()
        {
            ImageProp prop = new ImageProp();
            prop.Width = mImage.Width;
            prop.Height = mImage.Height;
            prop.Contrast = 0;
            prop.Brightness = 0;
            prop.FileType = Path.GetExtension(mCurFileName);

            return prop;
        }
        public bool OpenWithDialog()
        {
            OpenFileDialog fileDlg = new OpenFileDialog
            {
                Filter = ("图片|" + mPhotoFilter)
            };
            
            if (fileDlg.ShowDialog(this.Parent) == DialogResult.OK)
            {
                string fileName = fileDlg.FileName;
                if (mBeforeLoadFileHandler != null)
                    mBeforeLoadFileHandler();
                if (mThumbnailList != null)
                {
                    mThumbnailList.LoadFileContext(fileName);
                }
                LoadDataFromFile(fileName);

                return true;
            }
            return false;
        }

        private TextLine CreateTextLine(String lbl,String content)
        {
            return new TextLine(new TextField(lbl), new TextField(content));
        }

        public List<TextLine> ReadImageInfo()
        {
            if (mImage == null) return null;
            List<TextLine> allLines = new List<TextLine>();
            TextLine commonLine = new TextLine(new SectionTitle("常规"));
            commonLine.AddSubLine(CreateTextLine("位置:",Path.GetDirectoryName(mCurFileName)));
            if (mThumbnailList != null)
            {
                commonLine.AddSubLine(CreateTextLine("数目:", "共" + mThumbnailList.CurrentFileNames.Count.ToString() + "张图片，当前为第" + (mThumbnailList.CurrentFileIndex + 1).ToString() + "张"));
            }
            commonLine.AddSubLine(CreateTextLine("名称:", Path.GetFileName(mCurFileName)));
            commonLine.AddSubLine(CreateTextLine("尺寸(像素):", mImage.Width.ToString() + "*" + mImage.Height.ToString()));
            commonLine.AddSubLine(CreateTextLine("分辨率(DPI):", "水平" + ((int)mImage.HorizontalResolution).ToString() + "，垂直" + ((int)mImage.VerticalResolution).ToString()));
            commonLine.AddSubLine(CreateTextLine("文件大小:", CommonLib.Common.GetFileSize2Str(mCurFileName)));

            allLines.Add(commonLine);

            Dictionary<string, string> dic = CreatePhotoMetadataDic();
            Dictionary<int, string> dicIdTitle = CreateIdAndTitleDic();
            SetStringDicValue(dic, "修改时间", File.GetLastWriteTime(mCurFileName).ToLongDateString());

            StringBuilder sb = new StringBuilder();
            for (int i = 0; i < mImage.PropertyItems.Length; i++)
            {
                PropertyItem pi = mImage.PropertyItems[i];

                if (dicIdTitle.ContainsKey(pi.Id))
                {
                    SetStringDicValue(dic, dicIdTitle[pi.Id], ImagePIValue2Str(pi));
                }
            }
            TextLine sectionLine = null;
            foreach (KeyValuePair<String, String> kvp in dic)
            {
                String sKey = kvp.Key;
                if (sKey.IndexOf("section") >= 0)
                {
                    if (sectionLine != null)
                    {
                        allLines.Add(sectionLine);
                    }
                    sectionLine =new TextLine(new SectionTitle(kvp.Value));
                }
                else
                {
                    if (kvp.Value.Length > 0)
                    {
                        sectionLine.AddSubLine(CreateTextLine(sKey + ":", kvp.Value));
                    }
                }
            }

            return allLines;
            
        }

        private void SetStringDicValue(Dictionary<String, String> dic, String key, String val)
        {
            if (dic.ContainsKey(key))
            {
                dic[key] = val;
            }
        }

        private String ImagePIValue2Str(PropertyItem pi)
        {
            String sVal = null;
            System.Text.ASCIIEncoding encoding = new System.Text.ASCIIEncoding();
            switch (pi.Type)
            {
                case 1:
                    sVal = pi.Value[0].ToString();
                    break;
                case 2:
                    sVal = encoding.GetString(pi.Value);
                    break;
                case 3:
                    if (pi.Len < 4)
                    {
                        byte[] valTemp = new byte[4];
                        pi.Value.CopyTo(valTemp, 0);
                        sVal = BitConverter.ToInt32(valTemp, 0).ToString();
                    }
                    else
                        sVal = BitConverter.ToInt32(pi.Value, 0).ToString();
                    break;
                case 4:
                    if (pi.Len < 8)
                    {
                        byte[] valTemp = new byte[8];
                        pi.Value.CopyTo(valTemp, 0);
                        sVal = BitConverter.ToInt32(valTemp, 0).ToString();
                    }
                    else
                        sVal = BitConverter.ToInt64(pi.Value, 0).ToString();
                    break;
                case 5:
                    Byte[] tempStart = new byte[4] { pi.Value[0], pi.Value[1], pi.Value[2], pi.Value[3] };
                    Int32 num1 = BitConverter.ToInt32(tempStart, 0);
                    Int32 num2 = BitConverter.ToInt32(pi.Value, 4);

                    sVal = num1.ToString() + "/" + num2.ToString();
                    break;
                case 6:
                    break;
                case 7:
                    break;
                case 8:
                    break;
                case 9:
                    break;
                case 10:
                    break;
            }
            if (sVal != null && sVal.Length >= 1)
            {
                sVal = sVal.Substring(0, sVal.Length - 1);
            }

            return sVal;
        }

        public void ShowNavWindow()
        {
            if (mLeftArrowForm.Visible == false)
                mLeftArrowForm.Show(this);
            if (mRightArrowForm.Visible == false)
                mRightArrowForm.Show(this);
        }
        public void HideNavWindow()
        {
            if (mLeftArrowForm.Visible == true)
                mLeftArrowForm.Hide();
            if (mRightArrowForm.Visible == true)
                mRightArrowForm.Hide();
        }
        /// <summary>
        /// 把当前文件拷贝到剪贴板
        /// </summary>
        /// <param name="cut">是否剪切</param>
        public void CopyCurFileToCB(bool cut)
        {
            Clipboard.Clear();
            StringCollection fileCopied = new StringCollection();
            switch (mCurFileType)
            {
                case EnumFileType.Tif:
                    break;
                case EnumFileType.Shp:
                    break;
                case EnumFileType.Image:
                    fileCopied.Add(mCurFileName);
                    break;
                case EnumFileType.Video:
                    break;
            }

            byte[] moveEffect = new byte[] { 5, 0, 0, 0 };
            if (cut == true)
                moveEffect[0] = 2;
            MemoryStream dropEffect = new MemoryStream();
            dropEffect.Write(moveEffect, 0, moveEffect.Length);

            DataObject data = new DataObject();
            data.SetFileDropList(fileCopied);
            data.SetData("Preferred DropEffect", dropEffect);


            Clipboard.SetDataObject(data, true);
        }
        /// <summary>
        /// 把文件拷贝到目标文件夹
        /// </summary>
        /// <param name="targetFolder"></param>
        /// <param name="cut"></param>
        public void CopyCurFileToFolder(string targetFolder, bool cut = false)
        {
            Common.EnsureDirExisted(targetFolder);
            string targetFile = Path.Combine(targetFolder, Path.GetFileName(mCurFileName));
            //TODO:需要做覆盖判断
            if (cut == false)
            {
                File.Copy(mCurFileName, targetFile);
            }
            else
            {
                File.Move(mCurFileName, targetFile);
            }


        }

        public bool DeleteCurSelected(string targetFolder)
        {
            string targetFile = Path.Combine(targetFolder, Path.GetFileName(mCurFileName));
            try
            {
                if (File.Exists(targetFile))
                {
                    File.Delete(targetFile);
                }
                return true;
            }
            catch {
                return File.Exists(targetFile);
            }
            
        }
        public void CutCurrentFile()
        {

        }

        public void DeleteCurFile()
        {
            switch (mCurFileType)
            {
                case EnumFileType.Tif:
                    break;
                case EnumFileType.Shp:
                    break;
                case EnumFileType.Image:
                    CloseStreamReader();
                    File.Delete(mCurFileName);
                    break;
                case EnumFileType.Video:
                    break;
            }
        }
        public void PasteFile2CurPath()
        {
            StringCollection filesPasted = Clipboard.GetFileDropList();
            bool dealed = false;
            String firstFile = null;
            EnumClipboardAction action = Common.GetClipboardAction();
            List<String> filesCreated = new List<string>();
            for (int i = 0; i < filesPasted.Count; i++)
            {
                String filePasted = filesPasted[i];
                String targetFile = Path.Combine(mCurrentPath, Path.GetFileName(filePasted));
                EnumFileType fileType = GetFileType(filePasted);

                switch (mCurFileType)
                {
                    case EnumFileType.Tif:

                        break;
                    case EnumFileType.Shp:
                        break;
                    case EnumFileType.Image:
                        dealed = true;
                        if (action == EnumClipboardAction.Copy)
                            File.Copy(filePasted, targetFile, true);
                        else if (action == EnumClipboardAction.Cut)
                            File.Move(filePasted, targetFile);
                        break;
                    case EnumFileType.Video:
                        break;
                }
                if (firstFile == null)
                    firstFile = targetFile;
                if (dealed)
                    filesCreated.Add(targetFile);
            }

            LoadDataFromFile(firstFile);
            if (mThumbnailList != null)
            {
                mThumbnailList.LoadFileContext(firstFile);

            }
        }

        public void NextFile()
        {
            if (mThumbnailList != null)
                mThumbnailList.NextFile();
        }
        public void PreviousFile()
        {
            if (mThumbnailList != null)
                mThumbnailList.PreviousFile();
        }
    }
    public class FileChangedEvantArgs
    {
        public String CurrentFileName;
        public List<String> CurrentFileNames;
        public int CurrtentPos;
        public Image CurrentImage;
        public List<String> FilesCreated;
        public List<String> FilesDeleted;
        public List<String> FilesRenamed;
    }

    public class FileNameComparer : System.Collections.IComparer
    {
        public int Compare(object x, object y)
        {
            Shell32.FolderItem fix = x as Shell32.FolderItem;
            Shell32.FolderItem fiy = y as Shell32.FolderItem;

            return fix.Name.CompareTo(fiy.Name);
        }
    }
}
