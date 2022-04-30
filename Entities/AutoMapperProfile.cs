using AutoMapper;
using Entities.Common;
using Entities.DbModels;

namespace Entities
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserItem>().ReverseMap();
            CreateMap<UserWord, WordLearnItem>()
                .ForMember(d => d.Id, x => x.MapFrom(s => s.WordTranslation.Id))
                .ForMember(d => d.Eng, x => x.MapFrom(s => s.WordTranslation.Eng))
                .ForMember(d => d.Rus, x => x.MapFrom(s => s.WordTranslation.Rus));
            CreateMap<WordTranslation, WordItem>().ReverseMap();
        }
    }
}
