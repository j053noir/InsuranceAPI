using Microsoft.AspNetCore.Mvc;
using MongoDB.Bson;

namespace InsuranceAPI.Infrastructure.DTOs.Users
{
    public class UserReponseDTO
    {
        public string Id { get; set; }
        public string UserName { get; set; }
        public string Role { get; set; }
        public bool IsActive { get; set; }
    }
}
