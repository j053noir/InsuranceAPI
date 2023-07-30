using InsuranceAPI.Infrastructure.Models;
using System.ComponentModel.DataAnnotations;

namespace InsuranceAPI.Infrastructure.DTOs.Users
{
    public class RegistrationRequestDTO
    {
        [Required]
        public string? UserName { get; set; }

        [Required]
        public string? Password { get; set; }

        public UserRole Role { get; set; } = UserRole.Client;
    }
}
