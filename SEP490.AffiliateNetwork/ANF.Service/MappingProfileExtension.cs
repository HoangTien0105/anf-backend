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
            CreateMap<User, LoginResponse>()
                .ForMember(dest => dest.AccessToken, opt => opt.MapFrom((src, dest, destMember, context) => context.Items["Token"]))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.PublisherProfile.ImageUrl))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.AdvertiserProfile.ImageUrl));

            CreateMap<User, DetailedUserResponse>()
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => src.Role.ToString()))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.PublisherProfile.ImageUrl))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom(src => src.AdvertiserProfile.ImageUrl));

            CreateMap<AccountCreateRequest, User>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => IdHelper.GenerateRandomLong()))
                .ForMember(dest => dest.UserCode, opt => opt.MapFrom(_ => StringHelper.GenerateUniqueCode()))
                .ForMember(dest => dest.Role, opt => opt.MapFrom(src => Enum.Parse<UserRoles>(src.Role, true))) //Case-insensitive parsing
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => UserStatus.Pending))
                .ForMember(dest => dest.EmailConfirmed, opt => opt.MapFrom(src => false));

            CreateMap<SubscriptionRequest, Subscription>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => IdHelper.GenerateRandomLong()))
                .ForMember(dest => dest.PricePerMonth, opt => opt.MapFrom(src => Math.Floor(src.Price)));

            CreateMap<PublisherProfileCreatedRequest, PublisherProfile>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom((src, dest, destMember, context) => context.Items["ImageUrl"]));

            CreateMap<PublisherProfileUpdatedRequest, User>()
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.CitizenId, opt => opt.MapFrom(src => src.CitizenId))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth));

            CreateMap<PublisherProfileUpdatedRequest, PublisherProfile>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom((src, dest, destMember, context) => context.Items["ImageUrl"]))
                .ForMember(dest => dest.Bio, opt => opt.MapFrom(src => src.Bio))
                .ForMember(dest => dest.Specialization, opt => opt.MapFrom(src => src.Specialization));

            CreateMap<AdvertiserProfileRequest, AdvertiserProfile>()
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom((src, dest, destMember, context) => context.Items["ImageUrl"]));

            CreateMap<User, AdvertiserResponse>();

            CreateMap<User, PublisherResponse>();
            
            CreateMap<UpdatePasswordRequest, User>()
                .ForMember(dest => dest.Password, opt => opt.MapFrom(src => src.NewPassword))
                .ForMember(dest => dest.ResetPasswordToken, opt => opt.MapFrom(_ => string.Empty))
                .ForMember(dest => dest.ExpiryDate, opt => opt.MapFrom(_ => DateTime.MinValue));

            CreateMap<CategoryCreateRequest, Category>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => IdHelper.GenerateRandomLong()));
            CreateMap<CategoryUpdateRequest, Category>()
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name))
                .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));
            CreateMap<Category, CategoryResponse>();


            CreateMap<AffiliateSourceCreateRequest, TrafficSource>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => TrackingSourceStatus.Pending))
                .ForMember(dest => dest.CreatedAt, opt => opt.MapFrom(_ => DateTime.Now))
                .ForMember(dest => dest.PublisherId, opt => opt.MapFrom((src, dest, destMember, context) => (long)context.Items["PublisherId"]));

            CreateMap<AffiliateSourceUpdateRequest, TrafficSource>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore());

            CreateMap<OfferUpdateRequest, Offer>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.CampaignId, opt => opt.Ignore())
                .ForMember(dest => dest.OrderReturnTime, opt => opt.Ignore())
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore());

            CreateMap<CampaignCreateRequest, Campaign>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => IdHelper.GenerateRandomLong()))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => CampaignStatus.Pending))
                .ForMember(dest => dest.AdvertiserCode, opt => opt.MapFrom(src => src.AdvertiserCode))
                .ForMember(dest => dest.Offers, opt => opt.Ignore());

            CreateMap<CampaignImgCreateRequest, CampaignImage>()
                .ForMember(dest => dest.AddedAt, opt => opt.MapFrom(_ => DateTime.Now))
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            CreateMap<CampaignUpdateRequest, Campaign>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Offers, opt => opt.Ignore());

            CreateMap<Campaign, CampaignDetailedResponse>()
                .ForMember(dest => dest.CampImages, opt => opt.MapFrom(src => src.Images.Select(x => x.ImageUrl)))
                .ForMember(dest => dest.Offers, opt => opt.MapFrom(src => src.Offers))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()));

            CreateMap<Campaign, CampaignPubDetailedResponse>()
                .ForMember(dest => dest.CampImages, opt => opt.MapFrom(src => src.Images.Select(x => x.ImageUrl)))
                .ForMember(dest => dest.Offers, opt => opt.MapFrom(src => src.Offers
                    .Where(offer => offer.Status == OfferStatus.Started || offer.Status == OfferStatus.Approved)));

            CreateMap<OfferForCampaignCreateRequest, OfferCreateRequest>();

            CreateMap<OfferCreateRequest, Offer>()
                .ForMember(dest => dest.Status, opt => opt.MapFrom(_ => OfferStatus.Pending))
                .ForMember(dest => dest.ImageUrl, opt => opt.Ignore())
                .ForMember(dest => dest.OrderReturnTime, opt => opt.Ignore());
                

            CreateMap<TrafficSource, AffiliateSourceResponse>();

            CreateMap<Subscription, SubscriptionResponse>();

            CreateMap<Campaign, CampaignResponse>()
                .ForMember(dest => dest.CampImages, opt => opt.MapFrom(src => src.Images.Select(x => x.ImageUrl)))
                .ForMember(dest => dest.Offers, opt => opt.MapFrom(src => src.Offers))
                .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.ToString()))
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name));
            
            CreateMap<CampaignImage, CampaignImageResponse>();
            
            CreateMap<Offer, OfferResponse>();
            
            CreateMap<AdvertiserProfileUpdatedRequest, User>()
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.CitizenId, opt => opt.MapFrom(src => src.CitizenId))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth));

            CreateMap<AdvertiserProfileUpdatedRequest, AdvertiserProfile>()
                .ForMember(dest => dest.CompanyName, opt => opt.MapFrom(src => src.CompanyName))
                .ForMember(dest => dest.Industry, opt => opt.MapFrom(src => src.Industry))
                .ForMember(dest => dest.ImageUrl, opt => opt.MapFrom((src, dest, destMember, context) => context.Items["ImageUrl"]))
                .ForMember(dest => dest.Bio, opt => opt.MapFrom(src => src.Bio));

            CreateMap<UserBankCreateRequest, UserBank>()
                .ForMember(dest => dest.UserCode, opt => opt.MapFrom((src, dest, destMember, context) => (Guid)context.Items["UserCode"]))
                .ForMember(dest => dest.AddedDate, opt => opt.MapFrom(_ => DateTime.Now));

            CreateMap<UserBankUpdatedRequest, UserBank>()
                .ForMember(dest => dest.UserCode, opt => opt.Ignore())
                .ForMember(dest => dest.AddedDate, opt => opt.Ignore());

            CreateMap<Transaction, WithdrawalResponse>();

            CreateMap<BatchPayment, ExportedBatchDataResponse>();

            CreateMap<PolicyCreateRequest, Policy>();

            CreateMap<PolicyUpdateRequest, Policy>();

            CreateMap<Policy, PolicyResponse>();

            CreateMap<PostbackRequest, PostbackData>();

            CreateMap<PurchaseLogRequest, PurchaseLog>();

            CreateMap<Transaction, UserTransactionResponse>();

            CreateMap<AdvertiserOfferStats, AdvertiserOfferStatsResponse>();

            CreateMap<PublisherOfferStats, PublisherOfferStatsResponse>();

            CreateMap<PostbackLogUpdateRequest, PurchaseLog>();
        }
    }
}
