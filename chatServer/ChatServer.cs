using System;
using System.Collections.Generic;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace ChatServer
{
    public class ChatServer
    {
        public async Task Boot(string url)
        {
            var httpListener = new HttpListener();
            httpListener.Prefixes.Add(url);
            httpListener.Start();

            Console.WriteLine($"Escutando {url}");

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

            //server.
            return;
        }

        private async Task HandleHttpRequest(HttpListenerContext httpListenerContext)
        {
            HttpListenerWebSocketContext webSocketContext;
            var client = String.Empty;
            try
            {
                webSocketContext = await httpListenerContext.AcceptWebSocketAsync(null);
                var ipConnected = httpListenerContext.Request.RemoteEndPoint.Address.ToString();
                var sourcePort = httpListenerContext.Request.RemoteEndPoint.Port.ToString();
                client = $"{ipConnected}:{sourcePort}";

                Console.WriteLine($"Novo cliente conectado: {client}");

            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception: {ex.Message}");
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
                        var teste = UTF8Encoding.UTF8.GetString(receiveBuffer);
                        Console.WriteLine($"client {client}: {teste}");
                        Console.WriteLine($"mensagem {teste}");
                        await webSocket.SendAsync(new ArraySegment<byte>(receiveBuffer, 0, result.Count), 
                            WebSocketMessageType.Binary, result.EndOfMessage, CancellationToken.None);
                    }
                }
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Exception1: {ex.Message}");
            }
            finally
            {
                if (webSocket != null)
                {
                    webSocket.Dispose();
                }
            }
        }
    }
}
