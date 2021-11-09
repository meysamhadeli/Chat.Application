using FluentValidation;

namespace Chat.Core.Features.Chat.LoadMessagesByTime
{
    public class LoadMessageByTimeQueryValidator : AbstractValidator<LoadMessageByTimeQuery>
    {
        public LoadMessageByTimeQueryValidator()
        {
            RuleFor(x => x.UserName).NotNull();
            RuleFor(x => x.DateTime).NotNull();
        }
    }
}