using MongoDB.Bson;

namespace InsuranceAPI.Infrastructure.Respositories.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        public Task<T> GetById(ObjectId id);
        public Task<IEnumerable<T>> GetAll();
        public Task Add(T entity);
        public Task Update(T entity);
        public Task Delete(ObjectId id);
    }
}
