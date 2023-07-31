namespace InsuranceAPI.Infrastructure.Models
{
    public class Address
    {
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public Dictionary<string, string> AdditionalInfo { get; set; }
    }
}
