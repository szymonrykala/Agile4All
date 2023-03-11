using System.Net.WebSockets;
using System.Collections.Concurrent;

namespace AgileApp.Services
{
    public class WebSocketConnectionManager
    {
        private ConcurrentDictionary<string, WebSocket> _sockets = new();

        public WebSocket GetSocketById(string id) => _sockets[id];

        public ConcurrentDictionary<string, WebSocket> GetAll() => _sockets;

        public string GetId(WebSocket socket) => _sockets.FirstOrDefault(p => p.Value == socket).Key;

        public void AddSocket(WebSocket socket)
        {
            _sockets.TryAdd(CreateConnectionId(), socket);
        }

        public async Task RemoveSocket(string id)
        {
            _sockets.TryRemove(id, out WebSocket socket);

            await socket.CloseAsync(closeStatus: WebSocketCloseStatus.NormalClosure,
                                    statusDescription: "Closed by the WebSocketConnectionManager",
                                    cancellationToken: CancellationToken.None);
        }

        private string CreateConnectionId() => Guid.NewGuid().ToString();
    }
}