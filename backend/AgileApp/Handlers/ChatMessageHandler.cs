using System.Net.WebSockets;
using System.Text;
using AgileApp.Services;
using AgileApp.Services.Chat;
using System.Text.Json;

namespace AgileApp.Handlers
{
    public class ChatMessageHandler : WebSocketHandler
    {
    private readonly IServiceScopeFactory _serviceScopeFactory;

        public ChatMessageHandler(WebSocketConnectionManager webSocketConnectionManager, IServiceScopeFactory serviceScopeFactory)
            : base(webSocketConnectionManager)
        {
            _serviceScopeFactory = serviceScopeFactory;
        }

        public override async Task OnConnected(WebSocket socket)
        {
            await base.OnConnected(socket);

            using var scope = _serviceScopeFactory.CreateScope();
            var chatService = scope.ServiceProvider.GetRequiredService<IChatService>();

            var messages = chatService.Load();
            if (messages.Count > 0)
            {
                var loadMessage = new Models.WebsocketMessageLoad
                {
                    type = "LOAD",
                    payload = messages
                };

                await SendMessage(socket, PrepareMessageFromList(loadMessage));
            }
        }

        public override async Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var chatService = scope.ServiceProvider.GetRequiredService<IChatService>();

            if (result.MessageType == WebSocketMessageType.Text)
            {
                string resultJson = Encoding.UTF8.GetString(buffer, 0, result.Count);
                var obj = Newtonsoft.Json.JsonConvert.DeserializeObject<Models.WebsocketMessage>(resultJson);

                if (obj?.type == "MESSAGE")
                {
                    var message = "";
                    var payload = PrepareMessageFromObject(obj, out message);

                    chatService.SendMessage(message);

                    await SendMessageToAllExceptAsync(socket, payload);
                }
            }
        }

        private string PrepareMessageFromList(Models.WebsocketMessageLoad payload)
        {
            string json = JsonSerializer.Serialize(payload);
            json = json.Replace(@"\u0022", "\"").Replace("\"{", "{").Replace("}\"", "}");
            return json;
        }

        private string PrepareMessageFromObject(Models.WebsocketMessage payload, out string message)
        {
            string json = JsonSerializer.Serialize(payload);
            message = JsonSerializer.Serialize(payload.payload);
            json = json.Replace(@"\u0022", "\"");

            return json;
        }
    }
}