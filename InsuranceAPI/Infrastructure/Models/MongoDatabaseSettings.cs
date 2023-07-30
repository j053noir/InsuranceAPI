namespace InsuranceAPI.Infrastructure.Models
{
    public class MongoDatabaseSettings
    {
        public string? InsurancePoliciesCollectionName { get; set; } = null;
        public string? ClientsCollectionName { get; set; } = null;
        public string? ContactInformationCollectionName { get; set; } = null;
        public string? AddressesCollectionName { get; set; } = null;
        public string? PolicyPlansCollectionName { get; set; } = null;
        public string? VehiclesCollectionName { get; set; } = null;
        public string? InspectionsCollectionName { get; set; } = null;
        public string? UsersCollectionName { get; set; } = null;
        public string? RefreshTokensCollectionName { get; set; } = null;
    }
}
