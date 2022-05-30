using AutoMapper;
using EasySynthesis.Api.Languages.GetLanguages;
using EasySynthesis.Domain.Entities;

namespace EasySynthesis.Common.Mapper;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>();
    }
}