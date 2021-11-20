using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Web;
using System.Web.WebSockets;
using VideoHosting.Models.Database.Connections;
using VideoHosting.Models.Database.Contexts;
using VideoHosting.Models.Database.Entities;
using VideoHosting.Models.Database.Entities.Sinthetic;
using VideoHosting.Models.Status;

namespace VideoHosting.Handlers
{
    public class NewCommentHandler : IHttpHandler
    {
        WebSocket socket;

        public bool IsReusable
        {
            get { return false; }
        }

        public void ProcessRequest(HttpContext context)
        {
            if (context.IsWebSocketRequest)
                context.AcceptWebSocketRequest(WebSocketRequest);
        }

        private async Task WebSocketRequest(AspNetWebSocketContext context)
        {
            socket = context.WebSocket;
            string recive = await Receive();
            string[] splitters = recive.Split(';');
            string pageId = splitters[0];
            int lastCommentId = int.Parse(splitters[1]);

            var connection = MainOracleConnection.GetConnection();
            InfoPackageContext infoPackageContext = new InfoPackageContext(connection);
            IEnumerable<CommentInfo> comments = infoPackageContext.GetCommentsOfVideoPageAfter(pageId, lastCommentId);
            
            while (socket.State == WebSocketState.Open)
            {
                foreach (var comment in comments)
                {
                    await Send(comment.ToJson());
                }
                if (comments.Count() > 0)
                    lastCommentId = comments.Max(c => c.CommentId);
                Thread.Sleep(3000);
                comments = infoPackageContext.GetCommentsOfVideoPageAfter(pageId, lastCommentId);
            }
        }

        private async Task<string> Receive()
        {
            var buffer = new ArraySegment<byte>(new byte[1024]);
            var result = await socket.ReceiveAsync(buffer, CancellationToken.None);
            return Encoding.UTF8.GetString(buffer.Array, 0, result.Count);
        }

        private async Task Send(string status)
        {
            var sendbuffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(status));
            await socket.SendAsync(sendbuffer, WebSocketMessageType.Text, true, CancellationToken.None);
        }
    }
}