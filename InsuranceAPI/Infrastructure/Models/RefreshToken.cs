using MongoDB.Bson;

namespace InsuranceAPI.Infrastructure.Models
{
    public class RefreshToken
    {
        public ObjectId Id { get; set; }
        public string Token { get; set; }
        public DateTime ExpirationDate { get; set; }
        public DateTime CreationDate { get; set; }
        public string CreatedByIp { get; set; }
        public DateTime? Revoked { get; set; }
        public string RevokedByIp { get; set; }
        public string ReplacedByToken { get; set; }
        public string ReasonRevoked { get; set; }
        public bool IsExpired => DateTime.UtcNow >= ExpirationDate;
        public bool IsRevoked => Revoked.HasValue;
        public bool IsActive => !IsRevoked && !IsExpired;
    }
}
