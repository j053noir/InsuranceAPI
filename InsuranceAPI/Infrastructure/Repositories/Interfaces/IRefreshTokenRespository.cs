using InsuranceAPI.Infrastructure.Models;

namespace InsuranceAPI.Infrastructure.Repositories.Interfaces
{
    public interface IRefreshTokenRespository
    {
        Task Create(RefreshToken token);
        Task<RefreshToken> GetToken(string token);
        Task<long> GetTokenCount(string token);
    }
}
