using AutoMapper;
using EasySynthesis.Api.Syntheses.TextSyntheses;
using EasySynthesis.Api.Syntheses.TextSyntheses.RequestTextSynthesis;
using EasySynthesis.Contracts.TextSynthesis;
using EasySynthesis.Domain.Entities;

#pragma warning disable CS1591

namespace EasySynthesis.Api.Mapper;

public class TextSynthesisProfile : Profile
{
	public TextSynthesisProfile()
	{
		CreateMap<TextSynthesis, TextSynthesisDto>();
		CreateMap<TextSynthesisRequest, TextSynthesisData>();
	}
}