using System;
using System.Collections.Generic;
using Chat.Core.Dtos;
using MediatR;

namespace Chat.Core.Features.Chat.LoadReceivedMessages
{
    public class LoadReceivedMessagesQuery : IRequest<IEnumerable<ChatMessageDto>>
    {

        public LoadReceivedMessagesQuery(string userName, DateTime? dateTime)
        {
            UserName = userName;
            DateTime = dateTime;
        }

        public string UserName { get;}
        public DateTime? DateTime { get;}

    }
}