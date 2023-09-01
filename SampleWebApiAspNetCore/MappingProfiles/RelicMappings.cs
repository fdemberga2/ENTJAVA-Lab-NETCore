using AutoMapper;
using SampleWebApiAspNetCore.Dtos;
using SampleWebApiAspNetCore.Entities;

namespace SampleWebApiAspNetCore.MappingProfiles
{
    public class RelicMappings : Profile
    {
        public RelicMappings()
        {
            CreateMap<RelicEntity, RelicDto>().ReverseMap();
            CreateMap<RelicEntity, RelicUpdateDto>().ReverseMap();
            CreateMap<RelicEntity, RelicCreateDto>().ReverseMap();
        }
    }
}
