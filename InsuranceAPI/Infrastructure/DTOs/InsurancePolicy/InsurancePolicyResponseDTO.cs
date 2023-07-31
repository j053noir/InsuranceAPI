using InsuranceAPI.Infrastructure.Models;

namespace InsuranceAPI.Infrastructure.DTOs.InsurancePolicy
{
    public class InsurancePolicyResponseDTO
    {
        public string Id { get; set; }
        public string PolicyNumber { get; set; }
        public DateTime CreatedDate { get; set; } = DateTime.Now;
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public IDictionary<string, string> Clients { get; set; }
        public string PolicyPlanName { get; set; }
        public IDictionary<string, string> Coverage { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string RegistrationPlate { get; set; }
        public VehicleType VehicleType { get; set; }
        public DateTime InspectionDate { get; set; }
        public bool IsInspected { get; set; }
    }
}
