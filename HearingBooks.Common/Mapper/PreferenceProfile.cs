using AutoMapper;
using HearingBooks.Contracts;
using HearingBooks.Domain.Entities;

namespace HearingBooks.Common.Mapper;

public class PreferenceProfile : Profile
{
    public PreferenceProfile()
    {
        CreateMap<Preference, PreferenceDto>();
    }
}