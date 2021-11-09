using FluentValidation;

namespace Chat.Core.Features.Chat.LoadMessagesByCount
{
    public class LoadMessageByCountQueryValidator : AbstractValidator<LoadMessageByCountQuery>
    {
        public LoadMessageByCountQueryValidator()
        {
            RuleFor(x => x.UserName).NotNull();
        }
    }
}