using MediatR;

namespace Chat.Core.Features.Chat.SendMessage
{
    public record SendMessageCommand(string Message, string Sender, string Receiver): IRequest<long>;
}