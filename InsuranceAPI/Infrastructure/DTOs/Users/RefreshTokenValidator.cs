using FluentValidation;

namespace InsuranceAPI.Infrastructure.DTOs.Users
{
    public class RefreshTokenValidator : AbstractValidator<RefreshTokenRequestDTO>
    {
        public RefreshTokenValidator()
        {
            RuleFor(model => model.Token).NotEmpty();
        }
    }
}
