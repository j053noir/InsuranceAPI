using InsuranceAPI.Infrastructure.Models;
using InsuranceAPI.Infrastructure.Respositories.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace InsuranceAPI.Infrastructure.Repositories
{
    public class InsurancePoliciesRepository : IInsurancePoliciesRepository
    {
        private readonly IMongoCollection<InsurancePolicy> _collection;

        public InsurancePoliciesRepository(IMongoDatabase database, IOptions<Models.MongoDatabaseSettings> settings)
        {
            string? insurancePoliciesCollectionName = settings.Value.InsurancePoliciesCollectionName;
            if (string.IsNullOrEmpty(insurancePoliciesCollectionName)) throw new ArgumentNullException(nameof(insurancePoliciesCollectionName));

            _collection = database.GetCollection<InsurancePolicy>(insurancePoliciesCollectionName);
        }

        public async Task Add(InsurancePolicy entity)
        {
            await _collection.InsertOneAsync(entity);
        }

        public async Task Delete(ObjectId id)
        {
            await _collection.DeleteOneAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<InsurancePolicy>> GetAll()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<InsurancePolicy> GetById(ObjectId id)
        {
            return await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task Update(ObjectId id, InsurancePolicy entity)
        {
            await _collection.ReplaceOneAsync(x => x.Id == id, entity);
        }
    }
}
