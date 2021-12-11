using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoHosting.Models.Status
{
    public class VideoStatus
    {
        public string PageId { get; set; }
        public string LastDoneAction { get; set; }
        public string HandlingAction { get; set; }
        public int Progress { get; set; }
        public string Body { get; set; }

        public VideoStatus(string pageId, string lastDoneAction, string handlingAction, int progress)
        {
            PageId = pageId;
            LastDoneAction = lastDoneAction;
            HandlingAction = handlingAction;
            Progress = progress;
        }

        public VideoStatus(string pageId, string lastDoneAction, string handlingAction,
            int progress, string body) : this(pageId, lastDoneAction, handlingAction, progress)
        {
            Body = body;
        }

        public string ToJson()
        {
            return JsonConvert.SerializeObject(this, Formatting.Indented);
        }
    }
}