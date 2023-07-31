using System.ComponentModel.DataAnnotations;

namespace InsuranceAPI.Infrastructure.DTOs.Users
{
    public class AuthenticationRequestDTO
    {
        [Required]
        public string? UserName { get; set; }

        [Required]
        public string? Password { get; set; }
    }
}
