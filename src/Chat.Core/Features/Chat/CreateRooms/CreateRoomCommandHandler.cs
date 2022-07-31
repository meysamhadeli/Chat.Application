using AutoMapper;
using Chat.Core.Infrastructure.Data;
using Chat.Core.Infrastructure.Exception;
using Chat.Core.Infrastructure.Nats;
using Chat.Core.Models;
using MediatR;
using System;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Chat.Core.Features.Chat.CreateRooms
{
    public class CreateRoomCommandHandler : IRequestHandler<CreateRoomCommand, long>
    {
        private readonly ApplicationDbContext _applicationDbContext;
        private readonly IMapper _mapper;
        private readonly INatsBus _natsBus;

        public CreateRoomCommandHandler(ApplicationDbContext applicationDbContext, IMapper mapper, INatsBus natsBus)
        {
            _applicationDbContext = applicationDbContext;
            _mapper = mapper;
            _natsBus = natsBus;
        }

        async Task<long> IRequestHandler<CreateRoomCommand, long>.Handle(CreateRoomCommand command, CancellationToken cancellationToken)
        {
            try
            {
                if (!command.MemberNames.Any()) throw new AppException("Please enter the names of the recipients");

                foreach (string member in command.MemberNames)
                {
                    Room room = new Room()
                    {
                        RoomCreateor = command.Creator,
                        RoomName = command.RoomName,
                        Members = member,
                        CreatedDate = DateTime.Now
                    };
                    var entityEntry = await _applicationDbContext.AddAsync(room, cancellationToken);
                    await _applicationDbContext.SaveChangesAsync(cancellationToken);
                }
                return 0;
            }
            catch (Exception ex)
            {
                throw new AppException(ex.Message);
            }
        }
    }
}
