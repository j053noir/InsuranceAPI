﻿using MongoDB.Bson;

namespace InsuranceAPI.Infrastructure.Repositories.Interfaces
{
    public interface IGenericRepository<T> where T : class
    {
        public Task<T> GetById(ObjectId id);
        public Task<IEnumerable<T>> GetAll();
        public Task Add(T entity);
        public Task Update(ObjectId id, T entity);
        public Task Delete(ObjectId id);
    }
}
