using System;
using System.Collections.Generic;
using System.Text;

namespace chatClient
{
    public interface IClient
    {
        public ClientResult Connect(string host, string port);
        //public bool (string host, string port);
        public ClientResult SendMessage(string message, User user = null);
    }
}
