using ANF.Core.Enums;
using ANF.Core.Models.Entities;
using ANF.Core.Models.Requests;
using ANF.Core.Models.Responses;
using ANF.Infrastructure.Helpers;
using AutoMapper;

namespace ANF.Service
{
    public class MappingProfileExtension : Profile
    {
        public MappingProfileExtension()
        {
            CreateMap<User, LoginResponse>();

            CreateMap<AccountCreateRequest, User>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => IdHelper.GenerateRandomLong()))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => Enum.Parse<UserRoles>(src.Role, true))) //Case-insensitive parsing
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => UserStatus.Pending))
                .ForMember(dest => dest.EmailConfirmed, opt => opt.MapFrom(src => false));

            CreateMap<SubscriptionRequest, Subscription>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => IdHelper.GenerateRandomLong()))
                .ForMember(dest => dest.Price, opt => opt.MapFrom(src => Math.Floor(src.Price)));

            CreateMap<PublisherProfileRequest, PublisherProfile>();
            CreateMap<AdvertiserProfileRequest, AdvertiserProfile>();

            CreateMap<User, UserResponse>();
            CreateMap<User, PublisherResponse>();
                //.ForMember(dest => dest.AffiliateSourceResponses, opt => opt.MapFrom(src => src.AffiliateSources));   //TODO: Review the mapping, current it is not used


            CreateMap<CategoryCreateRequest, Category>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => IdHelper.GenerateRandomLong()));
            CreateMap<CategoryUpdateRequest, Category>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));
            CreateMap<Category, CategoryResponse>();


            CreateMap<AffiliateSourceCreateRequest, PublisherSource>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(_ => IdHelper.GenerateRandomLong()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => AffSourceStatus.Pending))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.Now));

            CreateMap<AffiliateSourceUpdateRequest, PublisherSource>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore());

            CreateMap<PublisherSource, AffiliateSourceResponse>();
            CreateMap<Subscription, SubscriptionResponse>();
        }
    }
}
