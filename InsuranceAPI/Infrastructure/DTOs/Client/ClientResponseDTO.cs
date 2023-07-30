namespace InsuranceAPI.Infrastructure.DTOs.Client
{
    public class ClientResponseDTO
    {
        public string Id { get; set; }
        public string GivenName { get; set; }
        public string FamilyName { get; set; }
        public string Identification { get; set; }
        public string IdentificationType { get; set; }
        public string FullName => $"{GivenName} {FamilyName}";
        public DateTime DateOfBirth { get; set; }
        public string UserName { get; set; }
        public bool IsActive { get; set; }
        public string Email { get; set; }
        public string Phone { get; set; }
        public string Street { get; set; }
        public string City { get; set; }
        public string State { get; set; }
    }
}
