using chatClient;
using chatCommon;
using System;
using System.Linq;

namespace chatClient
{
    public class Parser: IParser
    {
        public ParserResult ParseTextInput(string input)
        {
            input = input.Trim();
            var result = new ParserResult();
            var inputSplit = input.Split(' ').ToList();
            
            try
            {
                if (inputSplit.Count >= 1 && inputSplit[0].Equals("/p"))
                {
                    result.Command = Command.AnotherUserMessage;
                    result.Message = String.Join(' ', inputSplit.Skip(2));
                    result.CommandArgument = inputSplit[1];
                }else if (inputSplit.Count >= 1 && inputSplit[0].Equals("/x"))
                {
                    result.Command = Command.PrivateMessage;
                    result.Message = String.Join(' ', inputSplit.Skip(2));
                    result.CommandArgument = inputSplit[1];
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
                result.CommandArgument = String.Empty;
            }

            if (string.IsNullOrWhiteSpace(input))
            {
                result.Command = Command.InvalidCommand;
            }

            return result;
        }
    }

    
}