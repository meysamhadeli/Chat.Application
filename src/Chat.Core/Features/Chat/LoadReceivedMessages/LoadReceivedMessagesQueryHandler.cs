using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Chat.Core.Dtos;
using Chat.Core.Infrastructure.Data;
using Chat.Core.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Chat.Core.Features.Chat.LoadReceivedMessages
{
    public class LoadReceivedMessagesQueryHandler : IRequestHandler<LoadReceivedMessagesQuery, IEnumerable<ChatMessageDto>>
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public LoadReceivedMessagesQueryHandler(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }

        public async Task<IEnumerable<ChatMessageDto>> Handle(LoadReceivedMessagesQuery query,
            CancellationToken cancellationToken)
        {
            List<ChatMessage> entityResults;

            if (query.DateTime is not null)
                entityResults =  await _applicationDbContext.ChatMessages
                    .Where(x => x.Receiver == query.UserName)
                    .Where(x => x.CreatedDate >= query.DateTime)
                    .OrderBy(x => x.CreatedDate)?.ToListAsync(cancellationToken);
            
            else
                entityResults = await _applicationDbContext.ChatMessages
                    .Where(x => x.Receiver == query.UserName)
                    .OrderBy(x => x.CreatedDate)?.ToListAsync(cancellationToken);   
            

            var dtoResults = entityResults.Select(x => new ChatMessageDto
            {
                Message = x.Message,
                MessageDate = x.CreatedDate,
                Sender = x.Sender,
                Receiver = x.Receiver
            });

            return dtoResults;
        }
    }
}