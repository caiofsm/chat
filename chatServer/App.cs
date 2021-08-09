using chatCommon;
using chatServer;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Net;
using System.Net.WebSockets;
using System.Text;
using System.Text.Json;
using System.Threading;
using System.Threading.Tasks;

namespace ChatServer
{
    public class App
    {
        public App(IServer server)
        {
            _sockets = new ConcurrentDictionary<string, WebSocket>();
            Server = server;
        }
        private IServer Server { get; set; }
        private ConcurrentDictionary<string, WebSocket> _sockets { get; set; }

        public async Task Boot()
        {
            await Server.Boot(this.HandlePayloadMessage, this.RemoveSocket);
        }
        public void HandlePayloadMessage(string message, WebSocket webSocket)
        {
            var clientPayload = JsonSerializer.Deserialize<Payload>(message);

            switch (clientPayload.Command)
            {
                case Command.RegisterNickName:
                    var registerResult = TryRegisterNickName(clientPayload.UserName, webSocket);
                    var responsePayload = new Payload()
                    {
                        Command = Command.RegisterNickName,
                        Success = registerResult,
                        UserName = clientPayload.UserName,
                        Message = registerResult ?
                        $"*** You are registered as {clientPayload.UserName}. Joining #general" :
                        $"*** Sorry, the nickname {clientPayload.UserName} is already taken. Please choose a different one:"
                    };
                    Server.SendMessage(responsePayload, new KeyValuePair<string, WebSocket>(" ", webSocket));
                    break;
                case Command.AnotherUserMessage:
                case Command.GlobalMessage:
                case Command.PrivateMessage:
                    PrepareAndSendMessage(clientPayload);
                    break;
                default:
                    break;
            }
        }

        private void PrepareAndSendMessage(Payload payload)
        {
            var destinations = _sockets;

            if (InvalidPayload(payload))
            {
                return;
            }

            if (payload.Command == Command.RegisterNickName
                || payload.Command == Command.PrivateMessage)
            {
                destinations = GetFilteredDestination(payload);
            }

            foreach (var destination in destinations)
            {
                try
                {
                    Server.SendMessage(payload, destination);
                }
                catch (Exception ex)
                {
                    Console.WriteLine("{0} Exception: {1}", "PrepareMessage", ex.Message);
                    RemoveSocket(destination.Key);
                }
            }
        }

        private bool InvalidPayload(Payload payload)
        {
            if ((payload.Command == Command.AnotherUserMessage
                || payload.Command == Command.PrivateMessage) && !_sockets.ContainsKey(payload.CommandArgument))
            {
                return true;
            }

            return false;
        }

        private ConcurrentDictionary<string, WebSocket> GetFilteredDestination(Payload clientPayload)
        {
            var destination = new ConcurrentDictionary<string, WebSocket>();
            List<string> keys= new List<string>();

            switch (clientPayload.Command)
            {
                case Command.RegisterNickName:
                    keys.Add(clientPayload.UserName);
                    break;
                case Command.PrivateMessage:
                    keys.Add(clientPayload.UserName);
                    keys.Add(clientPayload.CommandArgument);
                    break;
                default:
                    return destination;
            }

            return GetSocketsByKeys(keys);
        }

        private ConcurrentDictionary<string, WebSocket> GetSocketsByKeys(List<string> keys)
        {
            WebSocket privateWebSocket;
            var result = new ConcurrentDictionary<string, WebSocket>();

            foreach (var key in keys)
            {
                if (!string.IsNullOrWhiteSpace(key) && _sockets.TryGetValue(key, out privateWebSocket))
                {
                    result.TryAdd(key, privateWebSocket);
                }
            }

            return result;
        }

        public void RemoveSocket(string userName)
        {
            Console.WriteLine($"Removing {userName}");
            _sockets.TryRemove(userName, out _);
        }

        public bool TryRegisterNickName(string userName, WebSocket webSocket)
        {
            var success = false;

            if (!string.IsNullOrWhiteSpace(userName) && _sockets.TryAdd(userName, webSocket))
            {
                success = true;
            }

            return success;
        }
    }
}
