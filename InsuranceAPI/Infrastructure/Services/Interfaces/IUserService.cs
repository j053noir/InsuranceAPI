﻿
using InsuranceAPI.Infrastructure.DTOs.Users;
using InsuranceAPI.Infrastructure.Models;
using MongoDB.Bson;

namespace InsuranceAPI.Infrastructure.Services.Interfaces
{
    public interface IUserService
    {
        Task<AuthenticationReponseDTO> Authenticate(AuthenticationRequestDTO model, string ipAddress);
        Task<AuthenticationReponseDTO> RefreshToken(string token, string ipAddress);
        Task RevokeToken(string token, string ipAddress);
        Task<IEnumerable<User>> GetAll();
        Task<User> GetById(ObjectId id);
        Task<User> GetByUsername(string username);
        Task Register(RegistrationRequestDTO model);
        Task Update(ObjectId id, UpdateUserRequestDTO model);
        Task Delete(ObjectId id);
    }
}
