using InsuranceAPI.Infrastructure.Models;
using System.ComponentModel.DataAnnotations;

namespace InsuranceAPI.Infrastructure.DTOs.InsurancePolicy
{
    public class CreateInsurancePolicyRequestDTO
    {
        [Required]
        public string PolicyNumber { get; set; }

        public DateTime CreatedDate { get; set; } = DateTime.Now;

        [Required]
        public DateTime StartDate { get; set; }

        [Required]
        public DateTime EndDate { get; set; }

        [Required]
        public IDictionary<string, ClientRole> Clients { get; set; }

        [Required]
        public string PolicyPlanName { get; set; }

        [Required]
        public IDictionary<string, string> Coverage { get; set; }

        [Required]
        public string Make { get; set; }

        [Required]
        public string Model { get; set; }

        [Required]
        public int Year { get; set; }

        [Required]
        public string RegistrationPlate { get; set; }

        [Required]
        public VehicleType VehicleType { get; set; }

        public DateTime InspectionDate { get; set; }

        public string InspectorName { get; set; }

        public string Notes { get; set; }

    }
}
