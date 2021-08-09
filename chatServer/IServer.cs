using chatCommon;
using System;
using System.Collections.Generic;
using System.Net.WebSockets;
using System.Threading.Tasks;

namespace ChatServer
{
    public interface IServer
    {
        public Task Boot(Action<string, WebSocket> payloadMessageHandler, Action<string> removeSocket);
        public Task<bool> SendMessage(Payload payload, KeyValuePair<string, WebSocket> userAndDestination);
    }
}