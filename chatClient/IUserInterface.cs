using System;
using System.Collections.Generic;
using System.Text;

namespace chatClient
{
    public interface IUserInterface
    {
        public void ShowWelcomeMessage();
        public void ShowMessage(string str);
        public string PrintPromptAndgetInput();
    }
}
