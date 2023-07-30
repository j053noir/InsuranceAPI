using InsuranceAPI.Infrastructure.Models;
using System.ComponentModel.DataAnnotations;

namespace InsuranceAPI.Infrastructure.DTOs.Client
{
    public class CreateClientRequestDTO
    {
        [Required]
        public ClientRole Role { get; set; }

        [Required]
        public string UserName { get; set; }

        [Required]
        public string Password { get; set; }

        [Required]
        public string GivenName { get; set; }

        [Required]
        public string FamilyName { get; set; }

        [Required]
        public string Identification { get; set; }

        [Required]
        public string IdentificationType { get; set; }

        [Required]
        public DateTime DateOfBirth { get; set; }

        [Required]
        public string Email { get; set; }

        [Required]
        public string CofirmEmail { get; set; }

        [Required]
        public string Phone { get; set; }

        [Required]
        public string CofirmPhone { get; set; }

        [Required]
        public string Street { get; set; }

        [Required]
        public string City { get; set; }

        [Required]
        public string State { get; set; }
    }
}
