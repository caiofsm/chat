using System;
using System.Threading.Tasks;

namespace chatClient
{
    class Program
    {
        static async Task<int> Main(string[] args)
        {
            var client = new Client();
            var app = new App(client, "127.0.0.1", "8080");

            var returnCode = await app.Boot();

            Console.ReadLine();
            return (int)returnCode;
        }
    }
}
