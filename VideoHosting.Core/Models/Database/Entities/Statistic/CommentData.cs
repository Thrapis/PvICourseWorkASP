using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoHosting.Models.Database.Entities.Statistic
{
    public class CommentData
    {
        public string VideoPageId { get; set; }
        public string Label { get; set; }
        public int CommentCount { get; set; }
    }
}