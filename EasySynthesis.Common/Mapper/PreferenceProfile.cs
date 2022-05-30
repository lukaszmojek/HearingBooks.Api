using AutoMapper;
using EasySynthesis.Api.Languages.GetLanguages;
using EasySynthesis.Domain.Entities;

namespace EasySynthesis.Common.Mapper;

public class PreferenceProfile : Profile
{
    public PreferenceProfile()
    {
        CreateMap<Preference, PreferenceDto>();
    }
}