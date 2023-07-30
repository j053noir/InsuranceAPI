using AutoMapper;
using InsuranceAPI.Infrastructure.DTOs.Client;
using InsuranceAPI.Infrastructure.DTOs.Users;
using InsuranceAPI.Infrastructure.Models;

namespace InsuranceAPI.Infrastructure.DTOs
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            // User
            CreateMap<RegistrationRequestDTO, User>();
            CreateMap<RegistrationRequestDTO, AuthenticationRequestDTO>();
            CreateMap<User, UserReponseDTO>();
            CreateMap<CreateClientRequestDTO, RegistrationRequestDTO>()
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.Password))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => UserRole.Client));

            // Client
            CreateMap<Models.Client, ClientResponseDTO>()
                .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.ContactInformation.Email))
                .ForMember(dest => dest.Phone, opt => opt.MapFrom(src => src.ContactInformation.Phone))
                .ForMember(dest => dest.Street, opt => opt.MapFrom(src => src.ContactInformation.Address.Street))
                .ForMember(dest => dest.City, opt => opt.MapFrom(src => src.ContactInformation.Address.City))
                .ForMember(dest => dest.State, opt => opt.MapFrom(src => src.ContactInformation.Address.State))
                .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.User.UserName))
                .ForMember(dest => dest.IsActive, opt => opt.MapFrom(src => src.User.IsActive));
            CreateMap<CreateClientRequestDTO, Models.Client>();
            CreateMap<CreateClientRequestDTO, ContactInformation>();
            CreateMap<CreateClientRequestDTO, Address>();
        }
    }
}
