using Microsoft.AspNetCore.Html;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using VideoHosting.Models.Status;

namespace VideoHosting.Handlers
{
    public class VideoProgressHandler
    {
        HtmlString
        /*WebSocket socket;

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
            string pageId = await Receive();

            VideoStatusContainer statusContainer = VideoStatusContainer.GetInstance();
            VideoStatus status = statusContainer.GetStatus(pageId);

            if (status == null)
            {
                await socket.CloseAsync(WebSocketCloseStatus.Empty, "No video page by this id", CancellationToken.None);
                return;
            }
            await Send(status.ToJson());

            while (socket.State == WebSocketState.Open)
            {
                Thread.Sleep(2000);
                status = statusContainer.GetStatus(pageId);
                await Send(status.ToJson());
            }
        }

        private async Task<string> Receive()
        {
            string rc = null;
            var buffer = new ArraySegment<byte>(new byte[512]);
            var result = await socket.ReceiveAsync(buffer, CancellationToken.None);
            rc = Encoding.UTF8.GetString(buffer.Array, 0, result.Count);
            return rc;
        }

        private async Task Send(string status)
        {
            var sendbuffer = new ArraySegment<byte>(Encoding.UTF8.GetBytes(status));
            await socket.SendAsync(sendbuffer, WebSocketMessageType.Text, true, CancellationToken.None);
        }*/
    }
}