using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Text;
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
        public async Task<ClientResult> Connect(string url)
        {
            var result = new ClientResult();
            WebSocket = new ClientWebSocket();

            try
            {
                await WebSocket.ConnectAsync(new Uri(url), CancellationToken.None);
                result.Success = true;
                result.ErrorMessage = String.Empty;
                result.ClientError = ClientError.NoError;
            }
            catch (Exception ex)
            {
                result.Success = false;
                result.ErrorMessage = ex.Message;
                result.ClientError = ClientError.FailedToConnect;
            }
            
            

            return result;
        }

        public async Task<ClientResult> SendMessage(string message, User user = null)
        {
            //Console.WriteLine($"{message} | {user?.Name}");
            var payload = Encoding.GetBytes(message);
            await WebSocket.SendAsync(new ArraySegment<byte>(payload), WebSocketMessageType.Binary, false, CancellationToken.None);
            return new ClientResult { Success= true};
        }
    }

    public class ClientResult
    {
        public bool Success { get; set; }
        public string ErrorMessage { get; set; }
        public ClientError ClientError { get; set; }
    }

    public enum ClientError
    {
        NoError = 0,
        FailedToConnect = 1,
        NickNameAlreadyTaken= 2
    }


}
