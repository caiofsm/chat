﻿using chatClient;
using System;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;

namespace chatCommon
{
    public class App
    {
        private IClient Client { get; set; }

        private string Url { get; set; }
        private string UserName { get; set; }
        private bool? UserRegistered { get; set; }
        private IParser Parser { get; set; }
        private IUserInterface UserInterface { get; set; }

        public App(IClient client, IParser parser, IUserInterface userInterface, string url)
        {
            Client = client;
            Url = url;
            UserRegistered = null;
            Parser = parser;
            UserInterface = userInterface;
        }
        public async Task<AppResult> Start()
        {
            UserInterface.ShowWelcomeMessage();

            var connectionResult = await Client.Connect(Url, this.HandlePayloadMessage);
            if (!connectionResult.Success)
            {
                UserInterface.ShowMessage($"Connection failed :( Url:{Url}\n");
                return AppResult.FalhaAoConectar;
            }

            

            while (UserRegistered != true)
            {
                UserName = UserInterface.PrintPromptAndgetInput().Trim();
                this.UserRegistered = false;
                TryRegisterNickName();
                CheckUntilResponseOrTimeout(100).Wait(2000);
                this.UserRegistered = null;
            }

            while (true)
            {
                var input = UserInterface.PrintPromptAndgetInput();
                var parserResult = Parser.ParseTextInput(input);
                var payload = new Payload()
                {
                    Command = parserResult.Command,
                    Message = parserResult.Message,
                    CommandArgument = parserResult.CommandArgument,
                    UserName = UserName
                };

                switch (parserResult.Command)
                {

                    case Command.GlobalMessage:
                    case Command.AnotherUserMessage:
                    case Command.PrivateMessage:
                        Client.SendMessage(payload);
                        break;
                    case Command.Exit:
                        return AppResult.Sair;
                        break;
                    case Command.InvalidCommand:
                        break;
                }

                await Task.Delay(100);
            }
        }

        private async Task CheckUntilResponseOrTimeout(int pollingInterval)
        {
            var shouldCheck = true;
            while (shouldCheck)
            {
                if (this.UserRegistered == true)
                {
                    shouldCheck = false;
                }
                await Task.Delay(pollingInterval);
            }
        }

        private void TryRegisterNickName()
        {
            Client.SendMessage(new Payload()
            {
                UserName=UserName,
                Command=Command.RegisterNickName
            }).Wait();
        }

        public void HandlePayloadMessage(Payload payload)
        {
            var message = String.Empty;

            switch (payload.Command)
            {
                case Command.RegisterNickName:
                    if (this.UserRegistered == false)
                    {
                        UserInterface.ShowMessage(payload.Message);
                        UserRegistered = payload.Success;
                    }
                    return;
                case Command.GlobalMessage:
                    message = $"{payload.UserName} says: {payload.Message}";

                    break;
                case Command.PrivateMessage:
                    message = $"{payload.UserName} privately says to {payload.CommandArgument}: {payload.Message}";
                    break;
                case Command.AnotherUserMessage:
                    message = $"{payload.UserName} says to {payload.CommandArgument}: {payload.Message}";
                    break;
                default:
                    return;
            }

            UserInterface.ShowMessage(message);
        }
        
    }

    public enum AppResult
    {
        FalhaAoConectar = 1,
        Sair = 2
    }




}
