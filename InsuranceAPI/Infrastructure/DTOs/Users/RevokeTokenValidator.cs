using FluentValidation;

namespace InsuranceAPI.Infrastructure.DTOs.Users
{
    public class RevokeTokenValidator : AbstractValidator<RevokeRefreshTokenDTO>
    {
        public RevokeTokenValidator()
        {
            RuleFor(model => model.Token).NotEmpty();
        }
    }
}
