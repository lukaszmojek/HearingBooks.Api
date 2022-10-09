using AutoMapper;
using HearingBooks.Contracts;
using HearingBooks.Domain.Entities;

namespace HearingBooks.Common.Mapper;

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