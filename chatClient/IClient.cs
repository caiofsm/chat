using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace chatClient
{
    public interface IClient
    {
        public Task<ClientResult> Connect(string url);
        public Task<ClientResult> SendMessage(string message, User user = null);
    }
}
