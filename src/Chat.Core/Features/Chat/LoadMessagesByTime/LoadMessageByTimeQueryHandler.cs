using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;
using Chat.Core.Dtos;
using Chat.Core.Infrastructure.Data;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Chat.Core.Features.Chat.LoadMessagesByTime
{
    public class LoadMessageByTimeQueryHandler: IRequestHandler<LoadMessageByTimeQuery, IEnumerable<ChatMessageDto>>
    {
        private readonly ApplicationDbContext _applicationDbContext;

        public LoadMessageByTimeQueryHandler(ApplicationDbContext applicationDbContext)
        {
            _applicationDbContext = applicationDbContext;
        }
        public async Task<IEnumerable<ChatMessageDto>> Handle(LoadMessageByTimeQuery query, CancellationToken cancellationToken)
        {
            
            var entityResults =  await _applicationDbContext.ChatMessages
                .Where(x => x.Sender == query.UserName || x.Receiver == query.UserName)
                .Where(x => x.CreatedDate >= query.DateTime)
                .OrderBy(x => x.CreatedDate)?.ToListAsync(cancellationToken);
            
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