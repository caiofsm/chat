using System;
using System.Collections.Generic;
using System.Text;

namespace chatClient
{
    public class Client : IClient
    {
        public Client()
        {
        }

        public ClientResult Connect(string host, string port)
        {
            var result = new ClientResult { Success=true};

            return result;
            //throw new NotImplementedException();
        }

        public ClientResult SendMessage(string message, User user = null)
        {
            Console.WriteLine($"{message} | {user?.Name}");
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
        FalhaAoConnectar = 1,
        NickNameAlreadyTaken= 2
    }
}
