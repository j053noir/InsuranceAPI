using InsuranceAPI.Infrastructure.Models;
using InsuranceAPI.Infrastructure.Repositories.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Driver;

namespace InsuranceAPI.Infrastructure.Repositories
{
    public class RefreshTokenRepository : IRefreshTokenRespository
    {
        private readonly IMongoCollection<RefreshToken> _collection;

        public RefreshTokenRepository(IMongoDatabase database, IOptions<Models.MongoDatabaseSettings> settings)
        {
            string? refreshTokensCollectionName = settings.Value.RefreshTokensCollectionName;
            if (string.IsNullOrEmpty(refreshTokensCollectionName)) throw new ArgumentNullException(nameof(refreshTokensCollectionName));

            _collection = database.GetCollection<RefreshToken>(refreshTokensCollectionName);
        }

        public async Task Create(RefreshToken token)
        {
            await _collection.InsertOneAsync(token);
        }

        public async Task<RefreshToken> GetToken(string token)
        {
            return await _collection.Find(x => x.Token == token).FirstOrDefaultAsync();
        }

        public async Task<long> GetTokenCount(string token)
        {
            return await _collection.CountDocumentsAsync(x => x.Token == token);
        }
    }
}
