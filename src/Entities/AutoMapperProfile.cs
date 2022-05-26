using AutoMapper;
using Entities.Common;
using Entities.Common.Grammar;
using Entities.DbModels;
using System;

namespace Entities
{
    public class AutoMapperProfile : Profile
    {
        public AutoMapperProfile()
        {
            CreateMap<User, UserItem>().ReverseMap();
            WordMaps();
            GrammaMaps();
        }

        private void GrammaMaps()
        {
            CreateMap<TestResult, UserTest>();
            CreateMap<GrammarTest, ThemeItem>();
            CreateMap<GrammarTest, TestInfo>();
            CreateMap<UserTest, UserTestItem>();
            CreateMap<ThemeItem, UserThemeItem>();
            CreateMap<TheoryLink, LinkItem>();
        }

        private void WordMaps()
        {
            CreateMap<UserWord, WordLearnItem>()
                            .ForMember(d => d.Id, x => x.MapFrom(s => s.WordTranslation.Id))
                            .ForMember(d => d.Eng, x => x.MapFrom(s => s.WordTranslation.Eng))
                            .ForMember(d => d.Rus, x => x.MapFrom(s => s.WordTranslation.Rus))
                            .ReverseMap();
            CreateMap<WordTranslation, WordItem>().ReverseMap();
            CreateMap<WordItem, WordLearnItem>().ReverseMap();
            CreateMap<UserWord, WordLearnInfo>();
        }
    }
}
