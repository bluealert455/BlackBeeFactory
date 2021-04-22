using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows.Forms;
using Vlc.DotNet.Core.Interops;
using Vlc.DotNet.Core.Interops.Signatures;

namespace ThirdPlugins
{
    public class VLCMultimedia
    {
        public static DirectoryInfo VLCLibDir = null;
        static VLCMultimedia()
        {
            string appPath = Application.StartupPath;
            VLCLibDir =
                new DirectoryInfo(Path.Combine(appPath, "libvlc", IntPtr.Size == 4 ? "win-x86" : "win-x64"));
        }


        //public static Task<Image> GenerateThumbAsync(string videoFile,string thumbFile)
        //{
        //    return Task.Run<Image>(() => 
        //    {
        //        return GenerateThumb(videoFile, thumbFile);
        //    });
        //}
        public static Dictionary<string, string> GetAudioInfo(string videoFile)
        {
            var options = new[]
            {
                "--intf", "dummy", /* no interface                   */
                "--vout", "dummy", /* we don't want video output     */
                "--no-audio", /* we don't want audio decoding   */
                "--no-video-title-show", /* nor the filename displayed     */
                "--no-stats", /* no stats */
                "--no-sub-autodetect-file", /* we don't want subtitles        */
                "--no-snapshot-preview", /* no blending in dummy vout      */
            };
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("section1", "音频");
            using (var mediaPlayer = new Vlc.DotNet.Core.VlcMediaPlayer(VLCLibDir, options))
            {
                mediaPlayer.SetMedia(new Uri(videoFile));

                Vlc.DotNet.Core.VlcMedia media = mediaPlayer.GetMedia();
                media.Parse();

                dic.Add("时长", media.Duration.ToString(@"hh\:mm\:ss"));
                dic.Add("艺术家", media.Artist);
                dic.Add("专辑", media.Album);
                dic.Add("版权所有", media.Copyright);
                dic.Add("年份", media.Date);
                dic.Add("类型", media.Genre);
                dic.Add("发行", media.Publisher);
                dic.Add("cover_img", media.ArtworkURL);
                var mediaInformations = media.Tracks;

                foreach (var mediaInformation in mediaInformations)
                {
                    if (mediaInformation.Type == MediaTrackTypes.Audio)
                    {

                    }
                    else if (mediaInformation.Type == MediaTrackTypes.Video)
                    {

                       
                    }
                    else if (mediaInformation.Type == MediaTrackTypes.Text)
                    {

                    }
                }
            }
            return dic;

        }

        public static bool GenerateThumbOfAudio(string videoFile,string thumbFile)
        {
            var options = new[]
            {
                "--intf", "dummy", /* no interface                   */
                "--vout", "dummy", /* we don't want video output     */
                "--no-audio", /* we don't want audio decoding   */
                "--no-video-title-show", /* nor the filename displayed     */
                "--no-stats", /* no stats */
                "--no-sub-autodetect-file", /* we don't want subtitles        */
                "--no-snapshot-preview", /* no blending in dummy vout      */
            };
       
            using (var mediaPlayer = new Vlc.DotNet.Core.VlcMediaPlayer(VLCLibDir, options))
            {
                mediaPlayer.SetMedia(new Uri(videoFile));

                Vlc.DotNet.Core.VlcMedia media = mediaPlayer.GetMedia();
                media.Parse();
                
                if (string.IsNullOrEmpty(media.ArtworkURL) == false)
                {
                    
                    string artworkFile = media.ArtworkURL;
                    Uri u = new Uri(artworkFile);
                    try
                    {
                        if (File.Exists(u.LocalPath) && File.Exists(thumbFile) == false)
                            File.Copy(u.LocalPath, thumbFile);
                    }
                    catch { }
                    
                    return true;
                }
                return false;
                
            }
        }
        public static Dictionary<string,string> GetMediaInfo(string videoFile)
        {
            var options = new[]
            {
                "--intf", "dummy", /* no interface                   */
                "--vout", "dummy", /* we don't want video output     */
                "--no-audio", /* we don't want audio decoding   */
                "--no-video-title-show", /* nor the filename displayed     */
                "--no-stats", /* no stats */
                "--no-sub-autodetect-file", /* we don't want subtitles        */
                "--no-snapshot-preview", /* no blending in dummy vout      */
            };
            Dictionary<string, string> dic = new Dictionary<string, string>();
            dic.Add("section1", "视频");
            using (var mediaPlayer = new Vlc.DotNet.Core.VlcMediaPlayer(VLCLibDir, options))
            {
                mediaPlayer.SetMedia(new Uri(videoFile));
                
                Vlc.DotNet.Core.VlcMedia media = mediaPlayer.GetMedia();
                media.Parse();
                
                dic.Add("时长", media.Duration.ToString(@"hh\:mm\:ss"));
               
                var mediaInformations =media.Tracks;

                foreach (var mediaInformation in mediaInformations)
                {
                    if (mediaInformation.Type == MediaTrackTypes.Audio)
                    {
                           
                    }
                    else if (mediaInformation.Type == MediaTrackTypes.Video)
                    {
                           
                        var videoTrack = mediaInformation.TrackInfo as VideoTrack;
                        dic.Add("宽度", videoTrack?.Width.ToString() ?? "");
                        dic.Add("高度", videoTrack?.Height.ToString() ?? "");
                    }
                    else if (mediaInformation.Type == MediaTrackTypes.Text)
                    {
                           
                    }
                }
            }
            return dic;

        }

        public async static void GenerateThumb(string videoFile, string thumbFile, string bgFile = null)
        {
            var options = new[]
            {
                "--intf", "dummy", /* no interface                   */
                "--vout", "dummy", /* we don't want video output     */
                "--no-audio", /* we don't want audio decoding   */
                "--no-video-title-show", /* nor the filename displayed     */
                "--no-stats", /* no stats */
                "--no-sub-autodetect-file", /* we don't want subtitles        */
                "--no-snapshot-preview", /* no blending in dummy vout      */
            };

            using (var mediaPlayer = new Vlc.DotNet.Core.VlcMediaPlayer(VLCLibDir, options))
            {
                mediaPlayer.SetMedia(new Uri(videoFile));
                
                TaskCompletionSource<bool> tcs = new TaskCompletionSource<bool>();
                var lastSnapshot = 0L;
                mediaPlayer.TimeChanged += (sender, e) =>
                {
                    //一秒，如果视频较长，则两秒，避免抓黑屏
                    int interval = 1000;
                    if (mediaPlayer.Length > 10000)
                        interval = 2000;
                    var snapshotInterval = e.NewTime / interval;
                    
                    if (snapshotInterval > lastSnapshot)
                    {
                        lastSnapshot = snapshotInterval;
                     
                        ThreadPool.QueueUserWorkItem(_ =>
                        {
                            mediaPlayer.TakeSnapshot(0, thumbFile, 400, 0);
                            if (bgFile != null)
                            {
                                mediaPlayer.TakeSnapshot(new FileInfo(bgFile));
                            }
                            mediaPlayer.Stop();
                        });
                    }
                };

                mediaPlayer.EncounteredError += (sender, e) =>
                {
                    Console.Error.Write("An error occurred");
                    tcs.TrySetCanceled();
                };

                mediaPlayer.EndReached += (sender, e) => { ThreadPool.QueueUserWorkItem(_ => tcs.TrySetResult(true)); };

                mediaPlayer.Play();

                await tcs.Task;
            }
           
        }
    }
}
