using FluentValidation;
using Shared.Dtos.Auth;

namespace Shared.Validations.Auth
{
    public class RegisterDtoValidator : AbstractValidator<RegisterDto>
    {
        public RegisterDtoValidator()
        {
            RuleFor(x => x.email).NotNull().EmailAddress();
            RuleFor(x => x.username).NotEmpty().WithMessage("Username must not be empty");
            RuleFor(x => x.password).NotNull().MinimumLength(8).MaximumLength(25);
        }
    }
}
