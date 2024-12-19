using AutoMapper;
using TechNationAPI.Converter;
using TechNationAPI.Dtos;
using TechNationAPI.Models;

namespace TechNationAPI.Profiles
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<CreateLogDto, Log>()
                .ReverseMap(); 
            CreateMap<MinhaCdnLog, CreateLogDto>()
                //.ForMember(dest => dest.ResponseSize, opt => opt.MapFrom(src => src.ResponseSize))
                //.ForMember(dest => dest.CacheStatus, opt => opt.MapFrom(src => src.CacheStatus))
                //.ForMember(dest => dest.Date, opt => opt.MapFrom(src => src.Date))
                //.ForMember(dest => dest.TimeTaken, opt => opt.MapFrom(src => src.TimeTaken))
                //.ForMember(dest => dest.Version, opt => opt.MapFrom(src => src.Version))
                .ForMember(dest => dest.Url, opt => opt.MapFrom(src => src.UriPath))
                .ReverseMap();
          
        }
    }
}
