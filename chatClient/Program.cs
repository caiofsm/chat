using chatCommon;
using System;
using System.Text;
using System.Threading.Tasks;

namespace chatClient
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            var client = new Client(new UTF8Encoding());
            var parser = new Parser();
            var terminal = new Terminal();
            var app = new App(client, parser, terminal, "ws://localhost:8080/chat/");

            var returnCode = await app.Start();
            return (int)returnCode;
        }
    }
}
