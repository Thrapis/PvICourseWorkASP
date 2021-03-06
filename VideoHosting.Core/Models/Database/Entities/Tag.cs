using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoHosting.Models.Database.Entities
{
    public class Tag
    {
        public int Id { get; set; }
        public string VideoPageId { get; set; }
        public string Name { get; set; }

        public Tag() { }
        public Tag(string pageId, string name)
        {
            VideoPageId = pageId;
            Name = name;
        }
    }
}