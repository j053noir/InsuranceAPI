using FluentValidation;

namespace InsuranceAPI.Infrastructure.DTOs.Client
{
    public class ClientValidator : AbstractValidator<CreateClientRequestDTO>
    {
        public ClientValidator()
        {
            RuleFor(model => model.Role).NotNull().IsInEnum();
            RuleFor(model => model.UserName).NotEmpty().MinimumLength(4);
            RuleFor(model => model.Password).NotEmpty().MinimumLength(4);
            RuleFor(model => model.GivenName).NotEmpty();
            RuleFor(model => model.FamilyName).NotEmpty();
            RuleFor(model => model.Identification).NotEmpty();
            RuleFor(model => model.IdentificationType).NotEmpty();
            RuleFor(model => model.DateOfBirth).NotEmpty();
            RuleFor(model => model.Email).NotEmpty().EmailAddress();
            RuleFor(model => model.CofirmEmail).NotEmpty().EmailAddress().Equal("Email", StringComparer.OrdinalIgnoreCase);
            RuleFor(model => model.Phone).NotEmpty();
            RuleFor(model => model.ConfirmPhone).NotEmpty().Equal("Phone", StringComparer.OrdinalIgnoreCase);
            RuleFor(model => model.Street).NotEmpty();
            RuleFor(model => model.City).NotEmpty();
            RuleFor(model => model.State).NotEmpty();
        }
    }
}
