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
            var app = new App(client, "ws://localhost:8080/chat/");

            var returnCode = await app.Boot();

            Console.ReadLine();
            return (int)returnCode;
        }
    }
}
