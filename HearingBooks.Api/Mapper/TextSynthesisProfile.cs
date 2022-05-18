using AutoMapper;
using HearingBooks.Api.Syntheses.TextSyntheses;
using HearingBooks.Domain.Entities;
#pragma warning disable CS1591

namespace HearingBooks.Api.Mapper;

public class TextSynthesisProfile : Profile
{
	public TextSynthesisProfile()
	{
		CreateMap<TextSynthesis, TextSynthesisDto>();
	}
}