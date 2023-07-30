using InsuranceAPI.Infrastructure.Models;
using InsuranceAPI.Infrastructure.Respositories.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace InsuranceAPI.Infrastructure.Repositories
{
    public class InspectionsRepository : IInspectionsRepository
    {
        private readonly IMongoCollection<Inspection> _collection;

        public InspectionsRepository(IMongoDatabase database, IOptions<Models.MongoDatabaseSettings> settings)
        {
            string? inspectionsCollectionName = settings.Value.InspectionsCollectionName;
            if (string.IsNullOrEmpty(inspectionsCollectionName)) throw new ArgumentNullException(nameof(inspectionsCollectionName));

            _collection = database.GetCollection<Inspection>(inspectionsCollectionName);
        }

        public async Task Add(Inspection entity)
        {
            await _collection.InsertOneAsync(entity);
        }

        public async Task Delete(ObjectId id)
        {
            await _collection.DeleteOneAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Inspection>> GetAll()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<Inspection> GetById(ObjectId id)
        {
            return await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task Update(ObjectId id, Inspection entity)
        {
            await _collection.ReplaceOneAsync(x => x.Id == id, entity);
        }
    }
}
