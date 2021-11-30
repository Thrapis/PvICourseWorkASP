using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoHosting.Models.Database.Entities.Sinthetic
{
    public class VideoEditInfo
    {
        public string VideoPageId { get; set; }
        public string VideoName { get; set; }
        public int Views { get; set; }
        public int PositiveRates { get; set; }
        public int NegativeRates { get; set; }
        public int MaxQuality { get; set; }
        public DateTime PageCreationDate { get; set; }
        public IEnumerable<Tag> Tags { get; set; }
    }
}