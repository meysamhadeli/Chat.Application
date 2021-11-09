using FluentValidation;

namespace Chat.Core.Features.Chat.GetChatById
{
    public class GetChatByIdQueryValidator : AbstractValidator<GetChatByIdQuery>
    {
        public GetChatByIdQueryValidator()
        {
            RuleFor(x => x.Id).NotNull();
        }
    }
}