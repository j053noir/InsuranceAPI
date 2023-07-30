using InsuranceAPI.Infrastructure.Models;
using InsuranceAPI.Infrastructure.Respositories.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace InsuranceAPI.Infrastructure.Repositories
{
    public class PolicyPlansRepository : IPolicyPlansRepository
    {
        private readonly IMongoCollection<PolicyPlan> _collection;

        public PolicyPlansRepository(IMongoDatabase database, IOptions<Models.MongoDatabaseSettings> settings)
        {
            string? policyPlansCollectionName = settings.Value.PolicyPlansCollectionName;
            if (string.IsNullOrEmpty(policyPlansCollectionName)) throw new ArgumentNullException(nameof(policyPlansCollectionName));

            _collection = database.GetCollection<PolicyPlan>(policyPlansCollectionName);
        }

        public async Task Add(PolicyPlan entity)
        {
            await _collection.InsertOneAsync(entity);
        }

        public async Task Delete(ObjectId id)
        {
            await _collection.DeleteOneAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<PolicyPlan>> GetAll()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<PolicyPlan> GetById(ObjectId id)
        {
            return await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task Update(ObjectId id, PolicyPlan entity)
        {
            await _collection.ReplaceOneAsync(x => x.Id == id, entity);
        }
    }
}
