using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoHosting.Models.Database.Entities
{
    public class NonAuthVideoView
    {
        public int Id { get; set; }
        public string VideoPageId { get; set; }
        public string IPAddress { get; set; }
        public DateTime ViewDate { get; set; }
        public NonAuthVideoView() { ViewDate = DateTime.Now; }
    }
}