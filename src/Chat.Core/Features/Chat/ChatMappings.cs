using System;
using AutoMapper;
using Chat.Core.Features.Chat.SendMessage;

namespace Chat.Core.Features.Chat
{
    public class ChatMappings : Profile
    {
        public ChatMappings()
        {
            CreateMap<SendMessageCommand, Models.ChatMessage>()
                .ForMember(d => d.CreatedDate, o =>
                    o.MapFrom(s => DateTime.Now));
        }
    }
}