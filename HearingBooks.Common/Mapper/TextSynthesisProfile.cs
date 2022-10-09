using AutoMapper;
using HearingBooks.Contracts;
using HearingBooks.Contracts.TextSynthesis;
using HearingBooks.Domain.Entities;

#pragma warning disable CS1591

namespace HearingBooks.Common.Mapper;

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