using chatCommon;
using System;
using System.Collections.Generic;
using System.Text;

namespace chatClient
{
    public interface IParser
    {
        public ParserResult ParseTextInput(string input);
    }
    public class ParserResult
    {
        public Command Command { get; set; }
        public string Message { get; set; }
        public string CommandArgument { get; set; }
    }
}
