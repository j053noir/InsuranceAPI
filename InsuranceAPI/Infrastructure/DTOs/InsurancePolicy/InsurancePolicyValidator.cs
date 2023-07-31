using FluentValidation;

namespace InsuranceAPI.Infrastructure.DTOs.InsurancePolicy
{
    public class InsurancePolicyValidator : AbstractValidator<CreateInsurancePolicyRequestDTO>
    {
        public InsurancePolicyValidator()
        {
            RuleFor(model => model.PolicyNumber).NotEmpty();
            RuleFor(model => model.CreatedDate).NotEmpty();
            RuleFor(model => model.StartDate).NotEmpty().LessThan(d => d.EndDate);
            RuleFor(model => model.EndDate).NotEmpty().GreaterThan(d => d.StartDate);
            RuleFor(model => model.Clients).NotEmpty().Must(x => x.Any());
            RuleForEach(model => model.Clients.Keys).NotEmpty();
            RuleForEach(model => model.Clients.Values).IsInEnum();
            RuleFor(model => model.PolicyPlanName).NotEmpty();
            RuleFor(model => model.Coverage).NotEmpty().Must(x => x.Any());
            RuleForEach(model => model.Coverage.Keys).NotEmpty();
            RuleForEach(model => model.Coverage.Values).NotEmpty();
            RuleFor(model => model.Make).NotEmpty();
            RuleFor(model => model.Model).NotEmpty();
            RuleFor(model => model.Year).NotEmpty().GreaterThan(1900);
            RuleFor(model => model.RegistrationPlate).NotEmpty();
            RuleFor(model => model.VehicleType).IsInEnum();
            RuleFor(model => model.InspectionDate).NotEmpty().When(x => !string.IsNullOrEmpty(x.InspectorName) || !string.IsNullOrEmpty(x.Notes));
            RuleFor(model => model.InspectorName).NotEmpty().When(x => x.InspectionDate.HasValue || !string.IsNullOrEmpty(x.Notes));
            RuleFor(model => model.Notes).NotEmpty().When(x => x.InspectionDate.HasValue || !string.IsNullOrEmpty(x.InspectorName));
        }
    }
}
