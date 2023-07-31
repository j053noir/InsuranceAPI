using AutoMapper;
using InsuranceAPI.Infrastructure.DTOs.Users;
using InsuranceAPI.Infrastructure.Models;
using InsuranceAPI.Infrastructure.Repositories.Interfaces;
using InsuranceAPI.Infrastructure.Services.Interfaces;
using Microsoft.Extensions.Options;
using MongoDB.Bson;

namespace InsuranceAPI.Infrastructure.Services
{
    public class UserService : IUserService
    {
        private readonly IUsersRepository _usersRepository;
        private readonly IAuthenticationService _authenticationService;
        private readonly IMapper _mapper;
        private readonly AppSettings _appSettings;


        public UserService
        (
            IUsersRepository usersRepository,
            IAuthenticationService authenticationService,
            IMapper mapper,
            IOptions<AppSettings> appSettings
        )
        {
            _usersRepository = usersRepository;
            _authenticationService = authenticationService;
            _mapper = mapper;
            _appSettings = appSettings.Value;
        }

        #region public methods

        public async Task<AuthenticationReponseDTO> Authenticate(AuthenticationRequestDTO model, string ipAddress)
        {
            var user = await _usersRepository.GetByUsername(model.UserName);

            if (user == null || !BCrypt.Net.BCrypt.Verify(model.Password, user.Password))
                throw new ApplicationException("Please provide both valid username and password");


            var refreshToken = await _authenticationService.GenerateRefreshToken(ipAddress);
            user.RefreshTokens.Add(refreshToken);

            RemoveOldRefreshTokens(user);

            await _usersRepository.Update(user.Id, user);

            return new AuthenticationReponseDTO
            {
                Token = _authenticationService.GenerateToken(user),
                RefreshToken = refreshToken.Token,
            };
        }

        public async Task<AuthenticationReponseDTO> RefreshToken(string token, string ipAddress)
        {
            var user = await GetUserByRefreshToken(token);
            var refreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == token);

            if (refreshToken.IsRevoked)
            {
                RevokeAllChildrenTokens(refreshToken, user, ipAddress, $"Attempted reuse of revoked ancestor token: {token}");
                await _usersRepository.Update(user.Id, user);
            }

            if (!refreshToken.IsActive) throw new ApplicationException("Invalid token");

            var newRefreshToken = await RotateRefreshToken(refreshToken, ipAddress);
            user.RefreshTokens.Add(newRefreshToken);

            RemoveOldRefreshTokens(user);

            await _usersRepository.Update(user.Id, user);

            return new AuthenticationReponseDTO
            {
                Token = _authenticationService.GenerateToken(user),
                RefreshToken = newRefreshToken.Token,
            };
        }

        public async Task RevokeToken(string token, string ipAddress)
        {
            var user = await GetUserByRefreshToken(token);
            var refreshToken = user.RefreshTokens.SingleOrDefault(x => x.Token == token);

            if (refreshToken != null && !refreshToken.IsActive)
                throw new ApplicationException("Invalid token");

            // revoke token and save
            RevokeRefreshToken(refreshToken, ipAddress, "Revoked without replacement");
            await _usersRepository.Update(user.Id, user);
        }

        public async Task Delete(ObjectId id)
        {
            var user = await GetById(id);

            await _usersRepository.Delete(id);
        }

        public async Task<IEnumerable<User>> GetAll() // TODO: Add filters, and pagination
        {
            return await _usersRepository.GetAll();
        }

        public async Task<User> GetById(ObjectId id)
        {
            var user = await _usersRepository.GetById(id);

            return user ?? throw new KeyNotFoundException(id.ToString());
        }

        public async Task<User> GetByUsername(string username)
        {
            var user = await _usersRepository.GetByUsername(username);

            return user ?? throw new KeyNotFoundException(username);
        }

        public async Task Register(RegistrationRequestDTO model)
        {
            await IsUserNameAvailable(model.UserName);
            var user = _mapper.Map<User>(model);
            user.RefreshTokens = new List<RefreshToken>();

            // hash password
            user.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);

            await _usersRepository.Add(user);
        }

        public async Task Update(ObjectId id, UpdateUserRequestDTO model)
        {
            var user = await GetById(id);

            if (!string.IsNullOrEmpty(model.UserName))
            {
                await IsUserNameAvailable(model.UserName, user);
                user.UserName = model.UserName;
            }

            if (!string.IsNullOrEmpty(model.Password))
            {
                user.Password = BCrypt.Net.BCrypt.HashPassword(model.Password);
            }

            await _usersRepository.Update(id, user);
        }

        #endregion

        #region Private methods

        private async Task<bool> IsUserNameAvailable(string username, User? user = null)
        {
            var isAvailable = false;

            if (user == null)
            {
                isAvailable = await _usersRepository.GetByUsername(username) == null;
            }
            else if (user != null)
            {
                var existingUser = await _usersRepository.GetByUsername(username);

                isAvailable = existingUser.Id == user.Id;
            }

            if (!isAvailable) throw new ApplicationException("Username is already taken");

            return isAvailable;
        }

        private async Task<RefreshToken> RotateRefreshToken(RefreshToken refreshToken, string ipAddress)
        {
            var newRefreshToken = await _authenticationService.GenerateRefreshToken(ipAddress);
            RevokeRefreshToken(refreshToken, ipAddress, "Replaced by new token", newRefreshToken.Token);
            return newRefreshToken;
        }

        private void RemoveOldRefreshTokens(User user)
        {
            user.RefreshTokens.RemoveAll
            (x => !x.IsActive && x.CreationDate.AddDays(_appSettings.JWTRefreshTokenLifetimeInDays) <= DateTime.UtcNow);
        }

        private void RevokeAllChildrenTokens(RefreshToken refreshToken, User user, string ipAddress, string reason)
        {
            if (!string.IsNullOrEmpty(refreshToken.ReplacedByToken))
            {
                var childToken = user.RefreshTokens.SingleOrDefault(x => x.Token == refreshToken.ReplacedByToken);

                if (childToken != null && childToken.IsActive)
                    RevokeRefreshToken(childToken, ipAddress, reason);
                else
                    RevokeAllChildrenTokens(childToken, user, ipAddress, reason);
            }   
        }

        private async Task<User> GetUserByRefreshToken(string token)
        {
            var user = await _usersRepository.GetByRefreshToken(token);

            return user ?? throw new ApplicationException("Token is not valid");
        }

        private void RevokeRefreshToken(RefreshToken? refreshToken, string ipAddress, string reason = null, string replacedByToken = null)
        {
            if (refreshToken == null) return;

            refreshToken.Revoked = DateTime.UtcNow;
            refreshToken.RevokedByIp = ipAddress;
            refreshToken.ReasonRevoked = reason;
            refreshToken.ReplacedByToken = replacedByToken;
        }

        #endregion
    }
}
