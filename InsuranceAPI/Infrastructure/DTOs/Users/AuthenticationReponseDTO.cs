namespace InsuranceAPI.Infrastructure.DTOs.Users
{
    public class AuthenticationReponseDTO
    {
        public string Token { get; set; }

        public string RefreshToken { get; set; }
    }
}
