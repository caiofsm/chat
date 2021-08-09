using chatServer;
using System;
using System.Text;
using System.Threading.Tasks;

namespace ChatServer
{
    class Program
    {
        static int Main(string[] args)
        {
            Console.WriteLine("ChatServer");
            var server = new WebSocketHandler(new UTF8Encoding(), "http://localhost:8080/chat/");
            var app = new App(server);
            app.Boot();
            //WebSocketHandler.Boot();
            Console.ReadLine();

            return 0;
        }
    }
}
