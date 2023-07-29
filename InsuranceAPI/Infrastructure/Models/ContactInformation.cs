using MongoDB.Bson;

namespace InsuranceAPI.Infrastructure.Models
{
    public class ContactInformation
    {
        public ObjectId Id { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public Address Address { get; set; }
        public Dictionary<string, string> AdditionalInfo { get; set; }
    }
}
