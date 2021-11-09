using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Chat.Core.Dtos;
using Chat.Core.Infrastructure.Data;
using Chat.Core.Models;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Chat.Core.Features.Chat.LoadMessagesByCount
{
    public class LoadMessageByCountQueryHandler: IRequestHandler<LoadMessageByCountQuery, IEnumerable<ChatMessageDto>>
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public LoadMessageByCountQueryHandler(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<IEnumerable<ChatMessageDto>> Handle(LoadMessageByCountQuery query, CancellationToken cancellationToken)
        {
            IList<ChatMessage> entityResults = null;
            
            var queryResults =  await _applicationDbContext.ChatMessages
                .Where(x => x.Sender == query.UserName || x.Receiver == query.UserName)
                .OrderBy(x => x.CreatedDate)?.ToListAsync(cancellationToken);
            
            if (query.NumMessages is not null)
                entityResults = queryResults.Take((int)query.NumMessages)?.ToList();

            var dtoResults = entityResults?.Select(x => new ChatMessageDto
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