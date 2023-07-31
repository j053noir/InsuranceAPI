using InsuranceAPI.Infrastructure.Models;
using InsuranceAPI.Infrastructure.Repositories.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace InsuranceAPI.Infrastructure.Repositories
{
    public class UsersRepository : IUsersRepository
    {
        private readonly IMongoCollection<User> _collection;

        public UsersRepository(IMongoDatabase database, IOptions<Models.MongoDatabaseSettings> settings)
        {
            string? usersCollectionName = settings.Value.UsersCollectionName;
            if (string.IsNullOrEmpty(usersCollectionName)) throw new ArgumentNullException(nameof(usersCollectionName));

            _collection = database.GetCollection<User>(usersCollectionName);
        }

        public async Task Add(User entity)
        {
            await _collection.InsertOneAsync(entity);
        }

        public async Task Delete(ObjectId id)
        {
            await _collection.DeleteOneAsync(x => x.Id == id);
        }

        public async Task<User> GetByUsername(string username)
        {
            return await _collection.Find(x => x.UserName == username).FirstOrDefaultAsync();
        }

        public async Task<IEnumerable<User>> GetAll()
        {
            return await _collection.Find(x => x.IsActive).ToListAsync();
        }

        public async Task<User> GetById(ObjectId id)
        {
            return await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task Update(ObjectId id, User entity)
        {
            await _collection.ReplaceOneAsync(x => x.Id == id, entity);
        }

        public async Task<User> GetByRefreshToken(string token)
        {
            return await _collection.Find(x => x.RefreshTokens.Any(y => y.Token == token)).FirstOrDefaultAsync();
        }
    }
}
