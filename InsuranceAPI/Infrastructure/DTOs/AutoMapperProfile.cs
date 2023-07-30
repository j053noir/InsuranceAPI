using AutoMapper;
using InsuranceAPI.Infrastructure.DTOs.Users;
using InsuranceAPI.Infrastructure.Models;

namespace InsuranceAPI.Infrastructure.DTOs
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<RegistrationRequestDTO, User>();
            CreateMap<RegistrationRequestDTO, AuthenticationRequestDTO>();
            CreateMap<User, UserReponseDTO>();
        }
    }
}
