using InsuranceAPI.Infrastructure.Models;

namespace InsuranceAPI.Infrastructure.Repositories.Interfaces
{
    public interface IUsersRepository : IGenericRepository<User>
    {
        public Task<User> GetByUsername(string username);
        public Task<User> GetByRefreshToken(string token);
    }
}
