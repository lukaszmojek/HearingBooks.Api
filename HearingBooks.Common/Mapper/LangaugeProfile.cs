using AutoMapper;
using EasySynthesis.Contracts;
using EasySynthesis.Domain.Entities;

namespace EasySynthesis.Common.Mapper;

public class LangaugeProfile : Profile
{
	public LangaugeProfile()
	{
		CreateMap<Language, LangaugeDto>();
	}
}