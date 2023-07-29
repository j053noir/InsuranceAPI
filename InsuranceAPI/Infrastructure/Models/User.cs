using MongoDB.Bson;
using System.ComponentModel;

namespace InsuranceAPI.Infrastructure.Models
{
    public class User
    {
        public ObjectId Id { get; set; }
        public string UserName { get; set; }
        public string Password { get; set; }
        public UserRole Role { get; set; } = UserRole.Client;
        public bool IsActive { get; set; }
    }

    public enum UserRole
    {
        [Description("Administrator")]
        Administrator = 0,
        [Description("Client")]
        Client,
        [Description("Sales Team")]
        SalesTeam,
        [Description("Inspector")]
        Inspector,
    }
}
