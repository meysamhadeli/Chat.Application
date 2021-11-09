using System;

namespace Chat.Core.Dtos
{
    public class ChatMessageDto
    {
        public string Sender { get; init; }
        public string Receiver { get; init; }
        public string Message { get; init; }
        public DateTime MessageDate { get; init; }
    }
}