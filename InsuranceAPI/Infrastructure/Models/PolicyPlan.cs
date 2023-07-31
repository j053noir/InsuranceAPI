using MongoDB.Bson;

namespace InsuranceAPI.Infrastructure.Models
{
    public class PolicyPlan
    {
        public ObjectId Id { get; set; }
        public string Name { get; set; }
        public Dictionary<string, string> Coverage { get; set; }
        public Dictionary<string, string> AdditionalInfo { get; set; }
    }
}
