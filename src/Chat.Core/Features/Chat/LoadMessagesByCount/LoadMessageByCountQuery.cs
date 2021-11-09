using System.Collections.Generic;
using Chat.Core.Dtos;
using MediatR;

namespace Chat.Core.Features.Chat.LoadMessagesByCount
{
    public class LoadMessageByCountQuery: IRequest<IEnumerable<ChatMessageDto>>
    {
        public LoadMessageByCountQuery(string userName, int? numMessages)
        {
            UserName = userName;
            NumMessages = numMessages;
        }

        public string UserName { get;}
        public int? NumMessages { get;}
    }
}