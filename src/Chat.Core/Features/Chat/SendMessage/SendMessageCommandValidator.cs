using FluentValidation;

namespace Chat.Core.Features.Chat.SendMessage
{
    public class SendMessageCommandValidator : AbstractValidator<SendMessageCommand>
    {
        public SendMessageCommandValidator()
        {
            RuleFor(x => x.Message).NotNull();
            RuleFor(x => x.Sender).NotNull();
            RuleFor(x => x.Receiver).NotNull();
        }
    }
    
}