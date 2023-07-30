using InsuranceAPI.Infrastructure.Models;
using MongoDB.Bson;

namespace InsuranceAPI.Infrastructure.Services.Interfaces
{
    public interface IAuthenticationService
    {
        public string GenerateToken(User user);
        public ObjectId? ValidateToken(string token);
        public Task<RefreshToken> GenerateRefreshToken(string ipAddress);
    }
}
