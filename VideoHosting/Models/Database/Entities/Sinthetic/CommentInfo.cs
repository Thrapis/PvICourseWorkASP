using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoHosting.Models.Database.Entities.Sinthetic
{
	public class CommentInfo
	{
		public int CommentId { get; set; }
		public int AuthorId { get; set; }
		public string AuthorName { get; set; }
		public string Text { get; set; }
		public DateTime CommentDate { get; set; }

		public string ToJson()
		{
			return JsonConvert.SerializeObject(this, Formatting.Indented);
		}

		public static CommentInfo FromJson(string jsonCommentInfo)
		{
			return JsonConvert.DeserializeObject<CommentInfo>(jsonCommentInfo);
		}
	}
}