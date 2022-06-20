using AutoMapper;
using EasySynthesis.Contracts;
using EasySynthesis.Contracts.TextSynthesis;
using EasySynthesis.Domain.Entities;

#pragma warning disable CS1591

namespace EasySynthesis.Common.Mapper;

public class TextSynthesisProfile : Profile
{
	public TextSynthesisProfile()
	{
		CreateMap<TextSynthesis, TextSynthesisDto>()
			.ForMember(
				destination => destination.Language, 
				options => options.MapFrom(x => x.Language.Name))
			.ForMember(
				destination => destination.Voice, 
				options => options.MapFrom(x => x.Voice.Name))
			.ForMember(
				destination => destination.RequestingUserId, 
				options => options.MapFrom(x => x.User.Id));
		CreateMap<TextSynthesisRequest, TextSynthesisData>();
	}
}