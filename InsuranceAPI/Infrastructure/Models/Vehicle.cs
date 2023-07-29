using MongoDB.Bson;

namespace InsuranceAPI.Infrastructure.Models
{
    public class Vehicle
    {
        public ObjectId Id { get; set; }
        public string Make { get; set; }
        public string Model { get; set; }
        public int Year { get; set; }
        public string RegistrationPlate { get; set; }
        public Dictionary<string, string> AdditionalInfo { get; set; }
    }
}
