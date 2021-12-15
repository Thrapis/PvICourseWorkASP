using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoHosting.Models.Database.Entities
{
    public class AuthVideoView
    {
        public int Id { get; set; }
        public string VideoPageId { get; set; }
        public int AccountId { get; set; }
        public DateTime ViewDate { get; set; }
        public RateType Rate { get; set; }
        public AuthVideoView() 
        {
            ViewDate = DateTime.Now;
            Rate = RateType.None;
        }
        public enum RateType 
        {
            Dislike = -1,
            None = 0,
            Like = 1
        }
    }
}