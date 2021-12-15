using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using VideoHosting.Models.Utils;

namespace VideoHosting.Models.Database.Entities
{
    public class VideoThumbnail
    {
        public long Id { get; set; }
        public string VideoPageId { get; set; }
        public byte[] Data { get; set; }
        public long Size { get; set; }
        public string Format { get; set; }
        public VideoThumbnail() { }
        public VideoThumbnail(string imagePath, string videoPageId)
        {
            using (FileStream fs = new FileStream(imagePath, FileMode.Open, FileAccess.Read))
            {
                using (BinaryReader reader = new BinaryReader(fs))
                {
                    if (fs.Length > int.MaxValue)
                        throw new Exception($"Max size of image is {int.MaxValue}. Your file size {fs.Length}");
                    byte[] BlobValue = reader.ReadBytes((int)fs.Length);
                    VideoPageId = videoPageId;
                    Data = BlobValue;
                    Size = fs.Length;
                    Format = FilePathHelper.GetExtension(imagePath);
                }
            }
        }
        public string GetForPage()
        {
            string base64String = Convert.ToBase64String(Data, 0, Data.Length);
            return "data:image/jpg;base64," + base64String;
        }
    }
}