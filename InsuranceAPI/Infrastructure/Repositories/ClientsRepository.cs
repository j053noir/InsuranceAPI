using InsuranceAPI.Infrastructure.Models;
using InsuranceAPI.Infrastructure.Respositories.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace InsuranceAPI.Infrastructure.Repositories
{
    public class ClientsRepository : IClientsRepository
    {
        private readonly IMongoCollection<Client> _collection;

        public ClientsRepository(IMongoDatabase database, IOptions<Models.MongoDatabaseSettings> settings)
        {
            string? clientsCollectionName = settings.Value.ClientsCollectionName;
            if (string.IsNullOrEmpty(clientsCollectionName)) throw new ArgumentNullException(nameof(clientsCollectionName));

            _collection = database.GetCollection<Client>(clientsCollectionName);
        }

        public async Task Add(Client entity)
        {
            await _collection.InsertOneAsync(entity);
        }

        public async Task Delete(ObjectId id)
        {
            await _collection.DeleteOneAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Client>> GetAll()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<Client> GetById(ObjectId id)
        {
            return await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task Update(ObjectId id, Client entity)
        {
            await _collection.ReplaceOneAsync(x => x.Id == id, entity);
        }
    }
}
