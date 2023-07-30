using MongoDB.Bson;
using MongoDB.Bson.Serialization.Attributes;

namespace InsuranceAPI.Infrastructure.Models
{
    public class InsurancePolicy
    {
        public ObjectId Id { get; set; }
        public string PolicyNumber { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public List<ClientReference> Clients { get; set; }
        public PolicyPlan Plan { get; set; }
        public Vehicle Vehicle { get; set; }

        [BsonIgnoreIfNull]
        public Inspection? Inspection { get; set; }

        public Dictionary<string, string> AdditionalInfo { get; set; }
    }
}
