using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoHosting.Models.Database.Entities.Sinthetic
{
    public class VideoPreviewInfo
    {
        public string VideoPageId { get; set; }
        public string VideoName { get; set; }
        public string Author { get; set; }
        public int Views { get; set; }
        public byte[] ThumbnailData { get; set; }

        public string GetThumbnailForPage()
        {
            string base64String = Convert.ToBase64String(ThumbnailData, 0, ThumbnailData.Length);
            return "data:image/jpg;base64," + base64String;
        }
    }
}