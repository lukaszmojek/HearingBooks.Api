using AutoMapper;
using HearingBooks.Contracts;
using HearingBooks.Domain.Entities;

namespace HearingBooks.Common.Mapper;

public class LangaugeProfile : Profile
{
	public LangaugeProfile()
	{
		CreateMap<Language, LangaugeDto>();
	}
}