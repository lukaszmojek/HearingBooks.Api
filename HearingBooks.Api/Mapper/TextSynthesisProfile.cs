using AutoMapper;
using HearingBooks.Api.Syntheses;
using HearingBooks.Domain.Entities;

namespace HearingBooks.Api.Mapper;

public class TextSynthesisProfile : Profile
{
	public TextSynthesisProfile()
	{
		CreateMap<TextSynthesis, TextSynthesisDto>();
	}
}