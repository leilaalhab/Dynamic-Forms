using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Threading.Tasks;
using Google.Protobuf;

namespace DynamicForms.Controllers
{
    public class WebSocketController : ControllerBase
    {
        [Route("/ws")]
        public async Task Get()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                WebSocket webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                Console.WriteLine("WebSocket Connected");
                await Echo(webSocket);
            }
            else
            {
                Console.WriteLine("Unexpected error.");
            }
        }

        private static async Task Echo(WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];
            var receiveResult = await webSocket.ReceiveAsync(new ArraySegment<byte>(buffer), CancellationToken.None);
            ReadOnlyMemory<byte> temp = buffer;
            
            Request request;
            request = DynamicForms.Request.Parser.ParseFrom(buffer.Take(receiveResult.Count).ToArray());

            Console.WriteLine(request.Value);

        }
    }
}