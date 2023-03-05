using AgileApp.Services.Chat;
using Microsoft.AspNetCore.Mvc;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Text.Encodings.Web;
using System.Text.Unicode;

namespace AgileApp.Controllers
{
    // <snippet>
    public class WebSocketController : ControllerBase
    {
        private readonly IChatService _chatService;

        public WebSocketController(IChatService chatService)
        {
            _chatService = chatService;
        }

        [HttpGet("/chat")]
        public async Task Get()
        {
            if (HttpContext.WebSockets.IsWebSocketRequest)
            {
                using var webSocket = await HttpContext.WebSockets.AcceptWebSocketAsync();
                await Load(webSocket);
                await Echo(webSocket);
            }
            else
            {
                HttpContext.Response.StatusCode = StatusCodes.Status400BadRequest;
            }
        }
        // </snippet>

        private async Task Echo(WebSocket webSocket)
        {
            var buffer = new byte[1024 * 4];
            var receiveResult = await webSocket.ReceiveAsync(
                new ArraySegment<byte>(buffer), CancellationToken.None);

            while (!receiveResult.CloseStatus.HasValue)
            {
                receiveResult = await webSocket.ReceiveAsync(
                    new ArraySegment<byte>(buffer), CancellationToken.None);

                await Send(buffer);
            }

            await webSocket.CloseAsync(
                receiveResult.CloseStatus.Value,
                receiveResult.CloseStatusDescription,
                CancellationToken.None);
        }

        private static T Deserialize<T>(byte[] data) where T : class
        {
            using (var stream = new MemoryStream(data))
            using (var reader = new StreamReader(stream, Encoding.UTF8))
                return Newtonsoft.Json.JsonSerializer.Create().Deserialize(reader, typeof(T)) as T;
        }

        private async Task Send(byte[] buffer)
        {
            string s = Encoding.ASCII.GetString(buffer);
            s = s.Trim();
            var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.WebsocketMessage>(s);

            if (obj.type == "MESSAGE")
            {
                var message = PrepareMessageFromObject(obj);

                _chatService.SendMessage(message);
            }
        }

        private async Task Load(WebSocket webSocket)
        {
            var response = new Models.WebsocketMessageLoad();
            response.type = "LOAD";

            var messages = _chatService.Load();
            if (messages.Count > 0)
            {
                response.payload = messages;

                int count = 0;
                var msgRes = PrepareMessageFromList(response, out count);

                await webSocket.SendAsync(
                        new ArraySegment<byte>(msgRes, 0, count),
                        WebSocketMessageType.Text,
                        true,
                        CancellationToken.None);
            }
        }

        private static string PrepareMessageFromObject(Models.WebsocketMessage payload)
        {
            string json = JsonSerializer.Serialize<Models.Payload>(payload.payload);
            json = json.Replace(@"\u0022", "\"");

            return json;
        }

        private byte[] PrepareMessageFromList(Models.WebsocketMessageLoad payload, out int count)
        {
            string json = JsonSerializer.Serialize<Models.WebsocketMessageLoad>(payload);
            json = json.Replace(@"\u0022", "\"").Replace("\"{", "{").Replace("}\"", "}");
            byte[] jsonBytesTable = Encoding.ASCII.GetBytes(json);
            var countRequest = new ArraySegment<byte>(jsonBytesTable);

            count = countRequest.Count; 
            return jsonBytesTable;
        }
    }
}
