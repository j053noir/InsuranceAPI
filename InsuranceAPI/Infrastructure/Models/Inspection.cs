using MongoDB.Bson;

namespace InsuranceAPI.Infrastructure.Models
{
    public class Inspection
    {
        public ObjectId Id { get; set; }
        public DateTime InspectionDate { get; set; }
        public string InspectorName { get; set; }
        public string Notes { get; set; }
        public Dictionary<string, string> AdditionalInfo { get; set; }
    }
}
