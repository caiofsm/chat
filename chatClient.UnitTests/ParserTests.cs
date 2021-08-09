using Microsoft.VisualStudio.TestTools.UnitTesting;

namespace chatClient.UnitTests
{
    [TestClass]
    public class ParserTests
    {
        [TestMethod]
        public void ParseTextInput_EmptyIsInvalid_IsTrue()
        {
            var parser = new Parser();

            var parserResult = parser.ParseTextInput("");

            Assert.IsTrue(parserResult.Command == chatCommon.Command.InvalidCommand);
        }

        [TestMethod]
        public void ParseTextInput_GlobalMessageTrimmed_IsTrue()
        {
            var parser = new Parser();

            var parserResult = parser.ParseTextInput(" Olá Pessoal!! ");

            Assert.IsTrue(
                (parserResult.Command == chatCommon.Command.GlobalMessage 
                && parserResult.Message == "Olá Pessoal!!" )
                );
        }

        [TestMethod]
        public void ParseTextInput_GlobalAndBeginWithSlash_IsTrue()
        {
            var parser = new Parser();

            var parserResult = parser.ParseTextInput("/ Olá Pessoal!!");

            Assert.IsTrue(
                (parserResult.Command == chatCommon.Command.GlobalMessage
                && parserResult.Message == "/ Olá Pessoal!!")
                );
        }

        [TestMethod]
        public void ParseTextInput_AnotherUserMessage_IsTrue()
        {
            var parser = new Parser();

            var parserResult = parser.ParseTextInput("/p usuario1 Olá Pessoal!!");

            Assert.IsTrue(
                (parserResult.Command == chatCommon.Command.AnotherUserMessage
                && parserResult.Message == "Olá Pessoal!!"
                && parserResult.CommandArgument == "usuario1")
                );
        }
    }
}
