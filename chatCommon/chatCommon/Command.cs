using System;
using System.Collections.Generic;
using System.Text;

namespace chatCommon
{
    public enum Command
    {

        GlobalMessage = 1,
        AnotherUserMessage = 2,
        Exit = 3,
        InvalidCommand = 4,
        PrivateMessage = 5,
        RegisterNickName = 6,
    }
}
