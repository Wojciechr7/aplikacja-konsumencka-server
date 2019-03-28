using AutoMapper;
using System;
using WebApplication.Commends;
using WebApplication.DTO;
using WebApplication.Models;

namespace WebApplication.AutoMapper
{
    public class MappingProfile : Profile
    {
        public MappingProfile()
        {
            CreateMap<Advertisement, AdvertisementsDTO>();
            CreateMap<AdvertisementCOM, Advertisement>()
                .ForMember(
                    x => x.Date,
                    y => y.MapFrom(src => DateTime.Now)
                );
            CreateMap<ImageCOM, AdvertisementImage>()
                .ForMember(
                    x => x.Id,
                    y => y.MapFrom(src => Guid.NewGuid())
                );
            CreateMap<Account, AccountDTO>();
            CreateMap<AccountCOM, Account>()
                .ForMember(
                    x => x.Id,
                    y => y.MapFrom(src => Guid.NewGuid())
                );
            CreateMap<Advertisement, AdvertisementDetailsDTO>();
            CreateMap<AdvertisementImage, ImageDTO>();
            CreateMap<Cities, CitiesDTO>();
            CreateMap<Account, AdvertisementDetailsDTO>()
                .ForMember(
                    x => x.UserId,
                    y => y.MapFrom(src => src.Id)
                );
            CreateMap<Messages, MessagesDTO>();
        }
    }
}
