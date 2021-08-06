using System;
using System.Linq;

namespace chatClient
{
    public static class Parser
    {
        public static ParserResult parseTextInput(string input)
        {
            var result = new ParserResult();
            var inputSplit = input.Split(' ').ToList();
            
            try
            {
                if (inputSplit.Count >= 1 && inputSplit[0].Equals("/p"))
                {
                    result.Command = Command.PrivateMessage;
                    result.Message = String.Join(' ', inputSplit.Skip(2));
                    result.UserName = inputSplit[1];
                }
                else if (inputSplit.Count >= 1 && inputSplit[0].Equals("/exit"))
                {
                    result.Command = Command.Exit;
                }
                else
                {
                    result.Command = Command.GlobalMessage;
                    result.Message = input;
                }
            }
            catch (Exception)
            {
                result.Command = Command.InvalidCommand;
                result.Message = String.Empty;
                result.UserName = String.Empty;
            }
            

            return result;
        }
    }

    public class ParserResult
    {
        public Command Command { get; set; }
        public string Message { get; set; }
        public string UserName { get; set; }
    }
}