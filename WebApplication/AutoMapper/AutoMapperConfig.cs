using AutoMapper;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using WebApplication.Commends;
using WebApplication.Models;

namespace WebApplication.AutoMapper
{
    public static class AutoMapperConfig
    {
        public static IMapper Initialize()
            => new MapperConfiguration(cfg =>
             {
                 //cfg.CreateMap<AdvertisementCOM, Advertisement>();
             }  
            ).CreateMapper();
    }
}
