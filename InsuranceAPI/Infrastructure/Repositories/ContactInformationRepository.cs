using InsuranceAPI.Infrastructure.Models;
using InsuranceAPI.Infrastructure.Respositories.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace InsuranceAPI.Infrastructure.Repositories
{
    public class ContactInformationRepository : IContactInformationRepository
    {
        private readonly IMongoCollection<ContactInformation> _collection;

        public ContactInformationRepository(IMongoDatabase database, IOptions<Models.MongoDatabaseSettings> settings)
        {
            string? contactInformationCollectionName = settings.Value.ContactInformationCollectionName;
            if (string.IsNullOrEmpty(contactInformationCollectionName)) throw new ArgumentNullException(nameof(contactInformationCollectionName));

            _collection = database.GetCollection<ContactInformation>(contactInformationCollectionName);
        }

        public async Task Add(ContactInformation entity)
        {
            await _collection.InsertOneAsync(entity);
        }

        public async Task Delete(ObjectId id)
        {
            await _collection.DeleteOneAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<ContactInformation>> GetAll()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<ContactInformation> GetById(ObjectId id)
        {
            return await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task Update(ObjectId id, ContactInformation entity)
        {
            await _collection.ReplaceOneAsync(x => x.Id == id, entity);
        }
    }
}
