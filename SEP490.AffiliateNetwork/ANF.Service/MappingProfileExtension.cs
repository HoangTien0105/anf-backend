using ANF.Core.Enums;
using ANF.Core.Models.Entities;
using ANF.Core.Models.Requests;
using ANF.Core.Models.Responses;
using ANF.Infrastructure;
using AutoMapper;

namespace ANF.Service
{
    public class MappingProfileExtension : Profile
    {
        public MappingProfileExtension()
        {
            CreateMap<User, LoginResponse>();

            CreateMap<PublisherCreateRequest, User>()
                .ForMember(dest => dest.PasswordHash, opt => opt.MapFrom(src => PasswordHasher.HashPassword(src.Password)))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => UserRoles.Publisher));
        }
    }
}
