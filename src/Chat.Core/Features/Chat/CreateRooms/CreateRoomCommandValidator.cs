using FluentValidation;
namespace Chat.Core.Features.Chat.CreateRooms
{
    public class CreateRoomCommandValidator : AbstractValidator<CreateRoomCommand>
    {
        public CreateRoomCommandValidator()
        {
            RuleFor(x => x.MemberNames).NotNull();
            RuleFor(x => x.RoomName).NotNull();
            RuleFor(x => x.Creator).NotNull();
        }
    }
}
