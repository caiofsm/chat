using chatCommon;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace chatClient
{
    public class Client : IClient
    {
        public Client(Encoding encoding)
        {
            Encoding = encoding;
        }
        private ClientWebSocket WebSocket { get; set; }
        public Encoding Encoding { get; set; }
        public Action<Payload> ReceiveMessageHandler { get; set; }
        public async Task<ClientResult> Connect(string url, Action<Payload> handlePayloadMessage)
        {
            var result = new ClientResult();
            WebSocket = new ClientWebSocket();
            ReceiveMessageHandler = handlePayloadMessage;

            try
            {
                await WebSocket.ConnectAsync(new Uri(url), CancellationToken.None);
                result.Success = true;
                result.ErrorMessage = String.Empty;
                result.ClientError = ClientError.NoError;
                ReceiveMessage();
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
                result.ClientError = ClientError.FailedToConnect;
            }

            return result;
        }

        public async Task ReceiveMessage()
        {
            while (WebSocket.State == WebSocketState.Open)
            {
                byte[] receiveBuffer = new byte[1024];
                var result = await WebSocket.ReceiveAsync(new ArraySegment<byte>(receiveBuffer), CancellationToken.None);

                if (result.MessageType == WebSocketMessageType.Close)
                {
                    await WebSocket.CloseAsync(WebSocketCloseStatus.NormalClosure, "NormalClosure", CancellationToken.None);
                }
                else
                {

                    var payload = JsonSerializer.Deserialize<Payload>(Encoding.GetString(receiveBuffer.Take(result.Count).ToArray()));
                    ReceiveMessageHandler.Invoke(payload);
                }
            }
        }

        public async Task<ClientResult> SendMessage(Payload message)
        {
            var json = Encoding.GetBytes(JsonSerializer.Serialize(message));
            await WebSocket.SendAsync(new ArraySegment<byte>(json), WebSocketMessageType.Text, false, CancellationToken.None);
            return new ClientResult { Success = true };
        }
    }
}
