using FluentValidation;
using InsuranceAPI.Infrastructure.Models;

namespace InsuranceAPI.Infrastructure.DTOs.Users
{
    public class RegisrationValidator : AbstractValidator<RegistrationRequestDTO>
    {
        public RegisrationValidator()
        {
            RuleFor(model => model.UserName).NotEmpty().MinimumLength(4);
            RuleFor(model => model.Password).NotEmpty().MinimumLength(4);
            RuleFor(model => model.Role).IsInEnum();
        }
    }
}
