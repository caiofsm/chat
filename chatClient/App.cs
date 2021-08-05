using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace chatClient
{
    public class App
    {
        public App(IClient client, string url)
        {
            this.Client = client;
            Url = url;
        }
        private IClient Client { get; set; }

        private string Url { get; set; }
        public async Task<AppResult> Boot()
        {
            var shouldContinue= true;

            showWelcomeMessage();
            var userName = printPromptAndgetInput();
            //showMessage($"Conectando ao servidor {Host}:{Port}");
            var connectionResult= await Client.Connect(Url);
            if (!connectionResult.Success)
            {
                showMessage($"Falha ao conectar ao {Url}");
                return AppResult.FalhaAoConectar;
            }

            showMessage($"Você foi registrado como {userName}. Entrando no canal #general");
            
            while (shouldContinue)
            {
                var input= printPromptAndgetInput();
                var result = Parser.parseTextInput(input);

                switch (result.Command)
                {
                    case Command.GlobalMessage:
                        //TODO: Mandar para o servidor
                        Client.SendMessage(result.Message);
                        break;
                    case Command.PrivateMessage:
                        //TODO: Mandar para o servidor
                        Client.SendMessage(result.Message, new User { Name = result.UserName });
                        break;
                    case Command.Exit:
                        return AppResult.Sair;
                        break;
                    case Command.InvalidCommand:
                        break;
                }

                await Task.Delay(100);
            }

            return 0;
        }


        public void showWelcomeMessage()
        {
            showMessage("*** Bem vindo ao chat. Insira seu usuário:");
        }

        public void showMessage(string str)
        {
            Console.WriteLine(str);
        }

        public string printPromptAndgetInput()
        {
            Console.Write("> ");
            return Console.ReadLine();
        }
    }

    public enum AppResult
    {
        FalhaAoConectar = 1,
        Sair = 2
    }

    public enum Command
    {
        GlobalMessage = 1,
        PrivateMessage = 2,
        Exit = 3,
        InvalidCommand= 4
    }


}
