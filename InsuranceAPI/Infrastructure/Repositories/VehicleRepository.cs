using InsuranceAPI.Infrastructure.Models;
using InsuranceAPI.Infrastructure.Respositories.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Bson;
using MongoDB.Driver;

namespace InsuranceAPI.Infrastructure.Repositories
{
    public class VehicleRepository : IVehiclesRepository
    {
        private readonly IMongoCollection<Vehicle> _collection;

        public VehicleRepository(IMongoDatabase database, IOptions<Models.MongoDatabaseSettings> settings)
        {
            string? vehiclesCollectionName = settings.Value.VehiclesCollectionName;
            if (string.IsNullOrEmpty(vehiclesCollectionName)) throw new ArgumentNullException(nameof(vehiclesCollectionName));

            _collection = database.GetCollection<Vehicle>(vehiclesCollectionName);
        }

        public async Task Add(Vehicle entity)
        {
            await _collection.InsertOneAsync(entity);
        }

        public async Task Delete(ObjectId id)
        {
            await _collection.DeleteOneAsync(x => x.Id == id);
        }

        public async Task<IEnumerable<Vehicle>> GetAll()
        {
            return await _collection.Find(_ => true).ToListAsync();
        }

        public async Task<Vehicle> GetById(ObjectId id)
        {
            return await _collection.Find(x => x.Id == id).FirstOrDefaultAsync();
        }

        public async Task Update(ObjectId id, Vehicle entity)
        {
            await _collection.ReplaceOneAsync(x => x.Id == id, entity);
        }
    }
}
