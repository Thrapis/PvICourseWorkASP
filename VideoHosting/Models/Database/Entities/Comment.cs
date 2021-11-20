using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoHosting.Models.Database.Entities
{
    public class Comment
    {
        public int Id { get; set; }
        public string VideoPageId { get; set; }
        /*public int? ParentCommentId { get; set; }*/
        public int AccountId { get; set; }
        public string Text { get; set; }
        public DateTime CommentDate { get; set; }

        public Comment() { CommentDate = DateTime.Now; }

        public Comment(string videoPageId, int accountId, string text) : this()
        {
            VideoPageId = videoPageId;
            AccountId = accountId;
            Text = text;
        }

    }
}