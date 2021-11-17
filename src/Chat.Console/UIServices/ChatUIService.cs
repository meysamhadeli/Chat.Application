using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;
using Chat.Core.Dtos;
using Chat.Core.Features.Chat.SendMessage;
using Chat.Core.Infrastructure.Nats;
using Chat.Core.Models;
using Humanizer;

namespace Chat.Console
{
    public class ChatUIService : IChatUIService
    {
        public async Task LoadReceivedMessages(string userName, IHttpClientFactory factory, APIOptions apiOptions)
        {
            var client = factory.CreateClient("APIClient");
            var result =
                await client.GetFromJsonAsync<IEnumerable<ChatMessageDto>>
                    ($"{apiOptions.LoadMessagesEndpoint}/{userName}?numberOfMessages={20}");

            if (result != null)
            {
                //Print history of messages from sender and receiver not received.
                foreach (var chatMessageDto in result)
                {
                    if (chatMessageDto.Sender == userName)
                    {
                        System.Console.ForegroundColor = ConsoleColor.Yellow;
                        System.Console.WriteLine(
                            $"UserName: {userName}, {chatMessageDto.MessageDate}: {chatMessageDto.Message}");
                        System.Console.ResetColor();
                    }
                    else
                    {
                        System.Console.ForegroundColor = ConsoleColor.Blue;
                        System.Console.WriteLine(
                            $"UserName: {chatMessageDto.Sender}, {chatMessageDto.MessageDate}: {chatMessageDto.Message}");
                        System.Console.ResetColor();
                    }
                }
            }
        }

        public void SubscribeOnChatMessage(string userName, INatsBus bus)
        {
            var subscription = bus.Subscribe<ChatMessageDto>(
                chatMessage =>
                {
                    if (chatMessage.Receiver == userName)
                    {
                        System.Console.ForegroundColor = ConsoleColor.Blue;
                        System.Console.WriteLine(
                            $"UserName: {chatMessage.Sender}, {chatMessage.MessageDate} : {chatMessage.Message}");
                        System.Console.ResetColor();
                    }
                }, nameof(ChatMessage).Underscore());
        }

        public async Task SendMessage(string userName, string receiver, string message, IHttpClientFactory factory,
            APIOptions apiOptions)
        {
            var client = factory.CreateClient("APIClient");
            var sendMessageCommand = new SendMessageCommand(message, userName, receiver);

            var response = await client.PostAsJsonAsync(apiOptions.SendMessageEndpoint, sendMessageCommand);

            response.EnsureSuccessStatusCode();

            System.Console.ForegroundColor = ConsoleColor.Yellow;
            System.Console.WriteLine($"UserName: {userName}, {DateTime.Now}: {sendMessageCommand.Message}");
            System.Console.ResetColor();
        }
    }
}