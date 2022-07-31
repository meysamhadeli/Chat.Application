using MediatR;
using System.Collections.Generic;

namespace Chat.Core.Features.Chat.CreateRooms
{
    public record CreateRoomCommand(string RoomName, string Creator, List<string> MemberNames) : IRequest<long>;
}
