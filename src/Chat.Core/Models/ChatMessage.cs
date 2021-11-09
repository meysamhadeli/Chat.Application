using System;

namespace Chat.Core.Models
{
    public class ChatMessage
    {
        public long Id { get; init; }
        public string Sender { get; init; }
        public string Receiver { get; init; } 
        public string Message { get; init; }
        public DateTime CreatedDate { get; init; }
    }
}