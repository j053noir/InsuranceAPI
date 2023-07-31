using FluentValidation;

namespace InsuranceAPI.Infrastructure.DTOs.Users
{
    public class UpdateUserValidator : AbstractValidator<UpdateUserRequestDTO>
    {
        public UpdateUserValidator()
        {
            RuleFor(model => model.UserName).Must(x => string.IsNullOrEmpty(x) || x.Length >= 4);
            RuleFor(model => model.Password).Must(x => string.IsNullOrEmpty(x) || x.Length >= 4);
        }
    }
}
