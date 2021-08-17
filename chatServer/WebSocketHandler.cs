using chatCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace chatServer
{
    public class WebSocketHandler : IServer
    {
        public WebSocketHandler(UTF8Encoding utf8Encoding, string url)
        {
            this.Encoding = utf8Encoding;
            this.Url = url;
        }

        private string Url;
        private Encoding Encoding { get; set; }
        private Action<string, WebSocket> PayloadMessageHandler { get; set; }
        private Action<string> RemoveSocket { get; set; }
        public async Task Boot(Action<string, WebSocket> payloadMessageHandler, Action<string> removeSocket)
        {
            RemoveSocket = removeSocket;
            PayloadMessageHandler = payloadMessageHandler;
            var httpListener = new HttpListener();
            httpListener.Prefixes.Add(Url);
            httpListener.Start();

            Console.WriteLine($"Listening {Url}");

            while (true)
            {
                var httpListenerContext = await httpListener.GetContextAsync();

                if (httpListenerContext.Request.IsWebSocketRequest)
                {
                    HandleHttpRequest(httpListenerContext);
                }
                else
                {
                    httpListenerContext.Response.StatusCode = 400;
                    httpListenerContext.Response.Close();
                }
                await Task.Delay(100);
            }
        }

        public async Task HandleHttpRequest(HttpListenerContext httpListenerContext)
        {
            HttpListenerWebSocketContext webSocketContext;
            var client = String.Empty;
            try
            {
                webSocketContext = await httpListenerContext.AcceptWebSocketAsync(null);
                var ipConnected = httpListenerContext.Request.RemoteEndPoint.Address.ToString();
                var sourcePort = httpListenerContext.Request.RemoteEndPoint.Port.ToString();
                client = $"{ipConnected}:{sourcePort}";

                Console.WriteLine($"New client connected: {client}");

            }
            catch (Exception ex)
            {
                Console.WriteLine("{0} Exception: {1}", "HandleHttpRequest_1", ex.Message);
                return;
            }

            WebSocket webSocket = null;
            try
            {
                webSocket = webSocketContext.WebSocket;
                byte[] receiveBuffer = new byte[1024];

                while (webSocket.State == WebSocketState.Open)
                {
                    var result = await webSocket.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);

                    if (result.MessageType == WebSocketMessageType.Close)
                    {
                        await webSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "NormalClosure", CancellationToken.None);
                    }
                    else
                    {

                        var payload = UTF8Encoding.UTF8.GetString(receiveBuffer.Take(result.Count).ToArray()).TrimEnd();
                        //App.HandlePayloadMessage(payload, webSocket);
                        PayloadMessageHandler.Invoke(payload, webSocket);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0} Exception: {1}", "HandleHttpRequest_2", ex.Message);
            }
            finally
            {
                if (webSocket != null)
                {
                    webSocket.Dispose();
                }
            }
        }

        public async Task<bool> SendMessage(Payload payload, KeyValuePair<string, WebSocket> userAndDestination)
        {
            var success = false;
            var destinationUserName = userAndDestination.Key;
            var webSocket = userAndDestination.Value;

            try
            {
                var sendBuffer = Encoding.GetBytes(JsonSerializer.Serialize(payload));

                await webSocket.SendAsync(new ArraySegment<byte>(sendBuffer),
                 WebSocketMessageType.Text, false, CancellationToken.None);
                success = true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("{0} Exception: {1}", "SendMessage", ex.Message);
                RemoveSocket.Invoke(destinationUserName);
                success = false;
                throw ex;
            }
            return success;
        }
    }
}
