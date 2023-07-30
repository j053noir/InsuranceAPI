using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;
using MongoDB.Driver;
using System.ComponentModel;

namespace InsuranceAPI.Infrastructure.Models
{
    public class Client
    {
        public ObjectId Id { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string Identification { get; set; }
        public string IdentificationType { get; set; }
        public DateTime DateOfBirth { get; set; }
        public ContactInformation ContactInformation { get; set; }
        public MongoDBRef UserId { get; set; }

        [BsonIgnore]
        public User User { get; set; }
    }

    public class ClientReference
    {
        public ObjectId Id { get; set; }
        public ClientRole Role { get; set; }
    }

    public enum ClientRole
    {
        [Description("None")]
        None = 0,
        [Description("PolicyHolder")]
        PolicyHolder,
        [Description("Beneficiary")]
        Beneficiary,
        [Description("Co_Policyholder")]
        Co_Policyholder,
    }
}
