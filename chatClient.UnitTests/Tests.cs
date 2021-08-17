using Microsoft.VisualStudio.TestTools.UnitTesting;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using chatCommon;
using chatServer;

namespace chatClient.UnitTests
{
    [TestClass]
    public class AdvTests
    {
        [TestMethod]
        public async Task ConnectAndSendMessageAndCompareResult_IsTrue()
        {
            IServer server = new WebSocketHandler(new UTF8Encoding(), "http://localhost:8080/chat/");
            var serverApp = new chatServer.App(server);
            serverApp.Boot();

            await Task.Delay(1000);

            IClient client = new Client(new UTF8Encoding());
            IParser parser = new Parser();
            var userName = "teste";
            var userName2 = "teste2";
            var message = $"Olá {userName2}!";
            var message2 = $"Olá {userName}!";
            IUserInterface terminal = new TestInteface(userName, userName2);
            var testInterface = terminal as TestInteface;
            var app = new App(client, parser, terminal, "ws://localhost:8080/chat/");
            var goal = $"{testInterface.GetWelcomeMessage()}\r\n{testInterface.GetPrompt()}{userName}\r\n" +
                $"*** You are registered as {userName}. Joining #general\r\n" +
                $"{testInterface.GetPrompt()}{message}\r\n" +
                $"{userName} says: {message}\r\n" +
                $"{userName2} says: {message2}\r\n" +
                $"{testInterface.GetPrompt()}/exit";



            IClient client2 = new Client(new UTF8Encoding());
            IParser parser2 = new Parser();


            IUserInterface terminal2 = new TestInteface(userName2, userName);
            var testInterface2 = terminal2 as TestInteface;
            var app2 = new App(client2, parser2, terminal2, "ws://localhost:8080/chat/");
            var goal2 = $"{testInterface2.GetWelcomeMessage()}\r\n{testInterface2.GetPrompt()}{userName2}\r\n" +
                $"*** You are registered as {userName2}. Joining #general\r\n" +
                $"{userName} says: {message}\r\n" +
                $"{testInterface2.GetPrompt()}{message2}\r\n" +
                $"{userName2} says: {message2}\r\n" +
                $"{testInterface2.GetPrompt()}/exit";

            app.Start();
            await Task.Delay(1000);
            app2.Start();

            await Task.Delay(5000);


            Assert.IsTrue(testInterface.Output.Contains(goal));
            Assert.IsTrue(testInterface2.Output.Contains(goal2));
        }

    }

    public class TestInteface : Terminal
    {

        public string Output = String.Empty;
        public int CommandIndex = 0;
        public string UserName { get; set; }
        public List<string> Commands { get; set; }

        public TestInteface(string userName, string anotherUser)
        {
            this.UserName = userName;
            this.Commands = new List<string>()
            {
                UserName,
                $"Olá {anotherUser}!",
                "/exit"
            };
        }

        public override string PrintPromptAndGetInput()
        {
            Task.Delay(1000).Wait();
            this.CommandIndex++;
            this.Output = this.Output + "> " + Commands[this.CommandIndex - 1] + "\r\n";
            return Commands[this.CommandIndex - 1];
        }

        public override void ShowMessage(string str)
        {
            this.Output = this.Output + str + "\r\n";
        }
    }
}
