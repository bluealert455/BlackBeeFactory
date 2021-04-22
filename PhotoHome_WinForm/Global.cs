using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PhotoHome
{
    public enum EnumFileType
    {
        None = 0,
        Image = 1,
        Video = 2,
        Audio=3,
        Tif = 4,
        Shp = 5,
        
        Unsurpported = 100
    }
    public class Global
    {
        public static String[] maPhotoTypes = null;
        public static String[] maVideoTypes = null;
        public static String[] maAudioTypes = null;
        public static String[] maTiffTypes = null;
        public static String[] maGISTypes = null;
        public static String[] maAllSurpportedTypes = null;

        public static String mPhotoFilter = null;
        public static String mVideoFilter = null;
        public static String mAudioFilter = null;
        public static String mTiffFilter = null;
        public static String mGISFilter = null;
        public static String mAllSurpportedFilter = null;
        static Global()
        {
            maPhotoTypes = new String[] { "*.bmp", "*.png", "*.gif", "*.jpeg", "*.jpg" };
            maVideoTypes = new String[] { "*.avi", "*.mp4", "*.wmv", "*.mkv", "*.rmvb"};
            maAudioTypes = new String[] { "*.mp3", "*.ape", "*.flac","*.m4a"};
            maTiffTypes = new String[] { "*.tif", "*.tiff" };
            maGISTypes = new String[] { "*.shp" };

            mPhotoFilter = String.Join(";", maPhotoTypes);
            mVideoFilter = String.Join(";", maVideoTypes);
            mAudioFilter = String.Join(";", maAudioTypes);
            mTiffFilter = String.Join(";", maTiffTypes);
            mGISFilter = String.Join(";", maGISTypes);

            mAllSurpportedFilter = mPhotoFilter + ";" + mVideoFilter+";"+ mAudioFilter;
            //maAllSurpportedTypes = new string[maPhotoTypes.Length + maVideoTypes.Length + maTiffTypes.Length + maGISTypes.Length];
            //maPhotoTypes.CopyTo(maAllSurpportedTypes, 0);
            //maVideoTypes.CopyTo(maAllSurpportedTypes, maPhotoTypes.Length);
            //maTiffTypes.CopyTo(maAllSurpportedTypes, maPhotoTypes.Length + maVideoTypes.Length);
            //maGISTypes.CopyTo(maAllSurpportedTypes, maPhotoTypes.Length + maVideoTypes.Length + maTiffTypes.Length);

            maAllSurpportedTypes = new string[maPhotoTypes.Length + maVideoTypes.Length+maAudioTypes.Length];
            maPhotoTypes.CopyTo(maAllSurpportedTypes, 0);
            maVideoTypes.CopyTo(maAllSurpportedTypes, maPhotoTypes.Length);
            maAudioTypes.CopyTo(maAllSurpportedTypes, maPhotoTypes.Length+maVideoTypes.Length);
        }

        public static EnumFileType GetFileType(String FileName)
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
            if (CommonLib.Common.GetPosInArray(maAudioTypes, pattern) > -1)
                return EnumFileType.Audio;

            return EnumFileType.Unsurpported;
        }

    }
}
