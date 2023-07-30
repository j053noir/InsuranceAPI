namespace InsuranceAPI.Infrastructure.Models
{
    public class AppSettings
    {
        public string JWTSecret { get; set; }
        public double JWTExpirationInMinutes { get; set; } = 60;
        public double JWTRefreshTokenLifetimeInDays { get; set; } = 60;
    }
}
