using FluentValidation;

namespace InsuranceAPI.Infrastructure.DTOs.Users
{
    public class AuthenticationValidator : AbstractValidator<AuthenticationRequestDTO>
    {
        public AuthenticationValidator()
        {
            RuleFor(model => model.UserName).NotEmpty().MinimumLength(4);
            RuleFor(model => model.Password).NotEmpty().MinimumLength(4);
        }
    }
}
