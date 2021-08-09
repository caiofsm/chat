using chatCommon;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace chatClient
{
    public interface IClient
    {
        public  Task<ClientResult> Connect(string url, Action<Payload> handlePayloadMessage);
        public Task<ClientResult> SendMessage(Payload message);
        public Task ReceiveMessage();

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
    }

}
