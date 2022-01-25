using AutoMapper;
using HearingBooks.Api.Languages;
using HearingBooks.Domain.Entities;

namespace HearingBooks.Api.Mapper;

public class LangaugeProfile : Profile
{
	public LangaugeProfile()
	{
		CreateMap<Language, LangaugeDto>();
	}
}