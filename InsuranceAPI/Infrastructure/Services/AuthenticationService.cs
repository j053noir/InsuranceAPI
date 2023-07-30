using InsuranceAPI.Infrastructure.Models;
using InsuranceAPI.Infrastructure.Repositories.Interfaces;
using InsuranceAPI.Infrastructure.Services.Interfaces;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Bson;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Security.Cryptography;
using System.Text;

namespace InsuranceAPI.Infrastructure.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly AppSettings _appSettings;
        private readonly ILogger<AuthenticationService> _logger;
        private readonly IRefreshTokenRespository _refreshTokenRespository;

        public AuthenticationService
        (
            IOptions<AppSettings> appSettings,
            ILogger<AuthenticationService> logger,
            IRefreshTokenRespository refreshTokenRespository
        )
        {
            _appSettings = appSettings.Value;
            _logger = logger;
            _refreshTokenRespository = refreshTokenRespository;
        }

        public string GenerateToken(User user)
        {
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.JWTSecret);
            var symmetricKey = new SymmetricSecurityKey(key);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new[] {
                    new Claim("id", user.Id.ToString()),
                    new Claim("username", user.UserName.ToString()),
                    new Claim("role", user.Role.ToString()),
                }),
                IssuedAt = DateTime.UtcNow,
                Expires = DateTime.UtcNow.AddMinutes(_appSettings.JWTExpirationInMinutes),
                SigningCredentials = new SigningCredentials(symmetricKey, SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }

        public ObjectId? ValidateToken(string token)
        {
            ObjectId? result = null;

            if (string.IsNullOrEmpty(token)) return result;

            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_appSettings.JWTSecret);

            try
            {
                tokenHandler.ValidateToken
                (
                    token,
                    new TokenValidationParameters
                    {
                        ValidateIssuerSigningKey = true,
                        IssuerSigningKey = new SymmetricSecurityKey(key),
                        ValidateIssuer = false,
                        ValidateAudience = false,
                        ClockSkew = TimeSpan.Zero,
                    },
                    out SecurityToken validatedToken
                );

                var jwtToken = (JwtSecurityToken)validatedToken;
                var id = jwtToken.Claims.FirstOrDefault(x => x.Type == "id")?.Value;

                if (!ObjectId.TryParse(id, out ObjectId userId))
                {
                    throw new ArgumentException("Failed parsing token", "id");
                }

                result = userId;
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error validating token");
            }

            return result;
        }

        public async Task<RefreshToken> GenerateRefreshToken(string ipAddress)
        {
            return new RefreshToken
            {
                Token = await GenerateUniqueToken(),
                ExpirationDate = DateTime.UtcNow.AddDays(_appSettings.JWTRefreshTokenLifetimeInDays),
                CreationDate = DateTime.UtcNow,
                CreatedByIp = ipAddress,
            };
        }

        private async Task<string> GenerateUniqueToken()
        {
            var token = Convert.ToBase64String(RandomNumberGenerator.GetBytes(64));

            var found = await _refreshTokenRespository.GetToken(token);

            if (found != null) return await GenerateUniqueToken();

            return token;
        }
    }
}
