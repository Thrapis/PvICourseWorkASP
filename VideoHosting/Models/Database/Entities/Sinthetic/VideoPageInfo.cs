using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using static VideoHosting.Models.Database.Entities.AuthVideoView;

namespace VideoHosting.Models.Database.Entities.Sinthetic
{
    public class VideoPageInfo
    {
        public string VideoPageId { get; set; }
        public string VideoName { get; set; }
        public string Author { get; set; }
        public int Views { get; set; }
        public RateType Rate { get; set; }
        public int PositiveRates { get; set; }
        public int NegativeRates { get; set; }
        public int MaxQuality { get; set; }
        public IEnumerable<CommentInfo> Comments { get; set; }
        public IEnumerable<Tag> Tags { get; set; }
    }
}