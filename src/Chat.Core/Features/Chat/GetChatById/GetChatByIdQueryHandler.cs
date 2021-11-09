using System.Threading;
using System.Threading.Tasks;
using Chat.Core.Dtos;
using Chat.Core.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Chat.Core.Features.Chat.GetChatById
{
    public class GetChatByIdQueryHandler: IRequestHandler<GetChatByIdQuery, ChatMessageDto>
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public GetChatByIdQueryHandler(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<ChatMessageDto> Handle(GetChatByIdQuery query, CancellationToken cancellationToken)
        {
            var result = await _applicationDbContext.ChatMessages
                .SingleOrDefaultAsync(x => x.Id == query.Id, cancellationToken: cancellationToken);

            var dtoResult = new ChatMessageDto
            {
                Message = result.Message,
                MessageDate = result.CreatedDate,
                Sender = result.Sender,
                Receiver = result.Receiver
            };
            
            return dtoResult;
        }
    }
}