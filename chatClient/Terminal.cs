using System;
using System.Collections.Generic;
using System.Text;

namespace chatClient
{
    public class Terminal : IUserInterface
    {
        public void ShowWelcomeMessage()
        {
            ShowMessage("*** Welcome to our chat server. Please provide a nickname:");
        }

        public void ShowMessage(string str)
        {
            Console.WriteLine(str);
        }

        public string PrintPromptAndgetInput()
        {
            Console.Write("> ");
            return Console.ReadLine();
        }
    }
}
