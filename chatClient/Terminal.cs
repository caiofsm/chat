using System;
using System.Collections.Generic;
using System.Text;

namespace chatClient
{
    public class Terminal : IUserInterface
    {
        public virtual void ShowWelcomeMessage()
        {
            ShowMessage(GetWelcomeMessage());
        }

        public virtual void ShowMessage(string str)
        {
            Console.WriteLine(str);
        }

        public virtual string PrintPromptAndGetInput()
        {
            Console.Write(GetPrompt());
            return Console.ReadLine();
        }

        public string GetWelcomeMessage()
        {
            return @"***Welcome to our chat server. Please provide a nickname:";
        }

        public string GetPrompt()
        {
            return "> ";
        }
    }
}
