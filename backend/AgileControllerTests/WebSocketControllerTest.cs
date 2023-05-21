using AgileApp.Services.Chat;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Moq;
using System;
using System.IO;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Xunit;

namespace AgileControllerTests
{
    public class WebSocketControllerTests
    {
        [Fact]
        public async Task Get_WithWebSocketRequest_ReturnsOkResult()
        {
            // Arrange
            var chatServiceMock = new Mock<IChatService>();

            var controller = new WebSocketController(chatServiceMock.Object);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.WebSockets.IsWebSocketRequest = true;

            var webSocketMock = new Mock<WebSocket>();
            webSocketMock.SetupSequence(ws => ws.ReceiveAsync(
                It.IsAny<ArraySegment<byte>>(),
                It.IsAny<CancellationToken>()))
                .ReturnsAsync(new WebSocketReceiveResult(1, WebSocketMessageType.Text, true))
                .ReturnsAsync(new WebSocketReceiveResult(0, WebSocketMessageType.Close, true));

            controller.ControllerContext.HttpContext.WebSockets.AcceptWebSocketAsync()
                .Returns(webSocketMock.Object);

            // Act
            await controller.Get();

            // Assert
            webSocketMock.Verify(ws => ws.CloseAsync(
                It.IsAny<WebSocketCloseStatus>(),
                It.IsAny<string>(),
                It.IsAny<CancellationToken>()), Times.Once);

            chatServiceMock.Verify(cs => cs.SendMessage(It.IsAny<string>()), Times.Once);
        }

        [Fact]
        public async Task Get_WithoutWebSocketRequest_ReturnsBadRequestResult()
        {
            // Arrange
            var chatServiceMock = new Mock<IChatService>();

            var controller = new WebSocketController(chatServiceMock.Object);
            controller.ControllerContext.HttpContext = new DefaultHttpContext();
            controller.ControllerContext.HttpContext.WebSockets.IsWebSocketRequest = false;

            // Act
            await controller.Get();

            // Assert
            Assert.Equal(StatusCodes.Status400BadRequest, controller.ControllerContext.HttpContext.Response.StatusCode);
            chatServiceMock.Verify(cs => cs.SendMessage(It.IsAny<string>()), Times.Never);
        }

        // More tests for other methods in the WebSocketController
    }
}
