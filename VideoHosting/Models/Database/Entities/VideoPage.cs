using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoHosting.Models.Database.Entities
{
    public class VideoPage
    {
        public string Id { get; set; }
        public int AccountId { get; set; }
        public string VideoName { get; set; }
        public DateTime CreationDate { get; set; }

        public VideoPage() { CreationDate = DateTime.Now; }

        public VideoPage(string id, int accountId, string videoName, DateTime creationDate)
        {
            Id = id;
            AccountId = accountId;
            VideoName = videoName;
            CreationDate = creationDate;
        }
    }
}