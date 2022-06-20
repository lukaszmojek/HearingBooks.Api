using AutoMapper;
using EasySynthesis.Contracts;
using EasySynthesis.Domain.Entities;

namespace EasySynthesis.Common.Mapper;

public class UserProfile : Profile
{
    public UserProfile()
    {
        CreateMap<User, UserDto>()
            .ForSourceMember(
                x => x.PasswordHash, 
                opt => opt.DoNotValidate());
    }
}