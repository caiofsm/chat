using System;
using System.Threading.Tasks;

namespace ChatServer
{
    class Program
    {
        static int Main(string[] args)
        {
            Console.WriteLine("ChatServer");
            BootStrap();
            Console.ReadLine();

            return 0;
        }

        private static void BootStrap()
        {
            var app = new ChatServer();
            app.Boot("http://localhost:8080/chat/");
        }
    }
}
