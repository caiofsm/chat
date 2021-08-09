using System;

namespace chatCommon
{
    public class Payload
    {
        public string UserName { get; set; }
        public string Message { get; set; }
        public Command Command { get; set; }
        public string CommandArgument { get; set; }
        public bool Success { get; set; }
    }
}
