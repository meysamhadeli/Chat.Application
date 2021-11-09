using System;
using System.Collections.Generic;
using Chat.Core.Dtos;
using MediatR;

namespace Chat.Core.Features.Chat.LoadMessagesByTime
{
    public class LoadMessageByTimeQuery: IRequest<IEnumerable<ChatMessageDto>>
    {
        public LoadMessageByTimeQuery(string userName, DateTime dateTime)
        {
            UserName = userName;
            DateTime = dateTime;
        }

        public string UserName { get;}
        public DateTime DateTime { get;}
    }
}