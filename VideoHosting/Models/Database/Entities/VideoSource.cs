using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Web;
using VideoHosting.Models.Utils;

namespace VideoHosting.Models.Database.Entities
{
    public class VideoSource
    {
        public long Id { get; set; }
        public string VideoPageId { get; set; }
        public byte[] Data { get; set; }
        public long Size { get; set; }
        public int Quality { get; set; }
        public string Format { get; set; }
        public VideoSource() { }
        public VideoSource(string videoPath, string videoPageId, int quality)
        {
            using (FileStream fs = new FileStream(videoPath, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader reader = new BinaryReader(fs))
                {
                    if (fs.Length > int.MaxValue)
                        throw new Exception($"Max size of video is {int.MaxValue}. Your file size {fs.Length}");
                    byte[] BlobValue = reader.ReadBytes((int)fs.Length);
                    VideoPageId = videoPageId;
                    Data = BlobValue;
                    Size = fs.Length;
                    Format = FilePathHelper.GetExtension(videoPath);
                    Quality = quality;
                }
            }
        }
    }
}