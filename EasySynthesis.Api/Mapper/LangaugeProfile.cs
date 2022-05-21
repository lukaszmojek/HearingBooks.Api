using AutoMapper;
using EasySynthesis.Api.Languages.GetLanguages;
using EasySynthesis.Domain.Entities;

namespace EasySynthesis.Api.Mapper;

public class LangaugeProfile : Profile
{
	public LangaugeProfile()
	{
		CreateMap<Language, LangaugeDto>();
	}
}