# chat

WebSockets foi a solução escolhida de comunicação entre o client e servidor.

Abrir e buildar a solution

Executar primeiro o projeto chatServer.

Em seguida executar o chatClient.
* Informe o usuário.

Os comandos disponíveis são:
* Digitar qualquer mensagem vai enviar para todos os usuários conectados.

* Digitar /p NomeDeUmUsuario Bom dia!
    Envia "Bom dia!" para o "NomeDeUmUsuario" mas todos os outros usuários conseguem ver a mensagem.

* Digitar /x NomeDeUmUsuario Bom dia!
    Envia "Bom dia!" apenas para "NomeDeUmUsuario". Outros usuários não conseguem ver a mensagem.
   
* Digitar /exit encerra o client.
