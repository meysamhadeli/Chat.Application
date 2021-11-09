using Chat.Core.Dtos;
using MediatR;

namespace Chat.Core.Features.Chat.GetChatById
{
    public class GetChatByIdQuery: IRequest<ChatMessageDto>
    {
        public GetChatByIdQuery(long id)
        {
            Id = id;
        }

        public long Id { get;}
    }
}