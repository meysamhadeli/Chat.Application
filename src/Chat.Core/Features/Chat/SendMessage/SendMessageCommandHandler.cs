using System;
using System.Threading;
using System.Threading.Tasks;
using AutoMapper;
using Chat.Core.Dtos;
using Chat.Core.Infrastructure.Data;
using Chat.Core.Infrastructure.Exception;
using Chat.Core.Infrastructure.Nats;
using Chat.Core.Models;
using Humanizer;
using MediatR;

namespace Chat.Core.Features.Chat.SendMessage
{
    public class SendMessageCommandHandler : IRequestHandler<SendMessageCommand ,long>
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;
        private readonly INatsBus _natsBus;

        public SendMessageCommandHandler(ApplicationDbContext applicationDbContext, IMapper mapper, INatsBus natsBus)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
            _natsBus = natsBus;
        }
        
        public async Task<long> Handle(SendMessageCommand command, CancellationToken cancellationToken)
        {
            if (command.Sender == command.Receiver)
                throw new AppException("The sender and receiver username cannot be the same!");
            
            var chatMessage = _mapper.Map<ChatMessage>(command);
            var entityEntry = await _applicationDbContext.AddAsync(chatMessage, cancellationToken);
            await _applicationDbContext.SaveChangesAsync(cancellationToken);

            _natsBus.Publish(new ChatMessageDto()
            {
                Message = chatMessage.Message,
                Sender = chatMessage.Sender,
                Receiver = chatMessage.Receiver,
                MessageDate = chatMessage.CreatedDate
            }, nameof(ChatMessage).Underscore());
            
            return entityEntry.Entity.Id;
        }
    }
}