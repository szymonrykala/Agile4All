using System.Net.WebSockets;
using System.Text;
using AgileApp.Services;
using AgileApp.Services.Chat;
using System.Text.Json;
using AgileApp.Models;

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
                var decodedMessages = messages.Select(m => JsonSerializer.Deserialize<Payload>(m));
                var loadMessage = new Models.WebsocketMessageLoad
                {
                    type = "LOAD",
                    payload = decodedMessages.ToList()
                };

                await SendMessage(socket, JsonSerializer.Serialize(loadMessage));
            }
        }

        public override async Task ReceiveAsync(WebSocket socket, WebSocketReceiveResult result, byte[] buffer)
        {
            using var scope = _serviceScopeFactory.CreateScope();
            var chatService = scope.ServiceProvider.GetRequiredService<IChatService>();

            if (result.MessageType == WebSocketMessageType.Text)
            {
                string resultJson = Encoding.UTF8.GetString(buffer, 0, result.Count);
                var obj = JsonSerializer.Deserialize<Models.WebsocketMessage>(resultJson);

                if (obj?.type == "MESSAGE")
                {
                    var options = new JsonSerializerOptions
                    {
                        Encoder = System.Text.Encodings.Web.JavaScriptEncoder.UnsafeRelaxedJsonEscaping,
                    };
                    var message = JsonSerializer.Serialize(obj.payload, options);
                    var payload = JsonSerializer.Serialize(obj);

                    chatService.SendMessage(message);

                    await SendMessageToAllExceptAsync(socket, payload);
                }
            }
        }
    }
}