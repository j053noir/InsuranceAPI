using AutoMapper;
using InsuranceAPI.Infrastructure.DTOs.Client;
using InsuranceAPI.Infrastructure.DTOs.InsurancePolicy;
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

            // InsurancePolicy
            CreateMap<Models.InsurancePolicy, InsurancePolicyResponseDTO>()
                .ForMember(dest => dest.Clients, opt => opt.Ignore())
                .ForMember(dest => dest.PolicyPlanName, opt => opt.MapFrom(src => src.Plan.Name))
                .ForMember(dest => dest.Coverage, opt => opt.MapFrom(src => src.Plan.Coverage))
                .ForMember(dest => dest.Make, opt => opt.MapFrom(src => src.Vehicle.Make))
                .ForMember(dest => dest.Model, opt => opt.MapFrom(src => src.Vehicle.Model))
                .ForMember(dest => dest.Year, opt => opt.MapFrom(src => src.Vehicle.Year))
                .ForMember(dest => dest.RegistrationPlate, opt => opt.MapFrom(src => src.Vehicle.RegistrationPlate))
                .ForMember(dest => dest.VehicleType, opt => opt.MapFrom(src => src.Vehicle.VehicleType.ToString()))
                .ForMember(dest => dest.IsInspected, opt => opt.MapFrom(src => src.Inspection != null))
                .ForMember(dest => dest.InspectionDate, opt => opt.MapFrom(src => src.Inspection.InspectionDate));
            CreateMap<CreateInsurancePolicyRequestDTO, Models.InsurancePolicy>()
                .ForMember(dest => dest.Clients, opt => opt.Ignore());
            CreateMap<CreateInsurancePolicyRequestDTO, PolicyPlan>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.PolicyPlanName));
            CreateMap<CreateInsurancePolicyRequestDTO, Vehicle>();
            CreateMap<CreateInsurancePolicyRequestDTO, Inspection>();
            

        }
    }
}
