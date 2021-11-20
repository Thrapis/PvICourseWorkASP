using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace VideoHosting.Models.Status
{
    public class VideoStatusContainer
    {
        private static VideoStatusContainer _instance;
        private Dictionary<string, VideoStatus> _statuses;

        private VideoStatusContainer() { _statuses = new Dictionary<string, VideoStatus>(); }

        public static VideoStatusContainer GetInstance()
        {
            if (_instance == null)
                _instance = new VideoStatusContainer();
            return _instance;
        }

        public void AddStatus(VideoStatus status)
        {
            if (_statuses.ContainsKey(status.PageId))
            {
                _statuses[status.PageId] = status;
            }
            else
                _statuses.Add(status.PageId, status);
        }

        public VideoStatus GetStatus(string pageId)
        {
            if (_statuses.ContainsKey(pageId))
            {
                return _statuses[pageId];
            }
            else return null;
        }
    }
}