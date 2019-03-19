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
        }
    }
}
