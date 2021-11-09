using System.Collections.Generic;
using Chat.Core.Dtos;
using MediatR;

namespace Chat.Core.Features.Chat.LoadReceivedMessages
{
    public class LoadReceivedMessagesQuery : IRequest<IEnumerable<ChatMessageDto>>
    {
        public LoadReceivedMessagesQuery(string userName)
        {
            UserName = userName;
        }

        public string UserName { get;}
    }
}