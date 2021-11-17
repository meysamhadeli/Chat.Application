using FluentValidation;

namespace Chat.Core.Features.Chat.LoadReceivedMessages
{
    public class LoadReceivedMessagesQueryValidator: AbstractValidator<LoadReceivedMessagesQuery>
    {
        public LoadReceivedMessagesQueryValidator()
        {
            RuleFor(x => x.UserName).NotNull();
        }
    }
}