using AutoMapper;
using HearingBooks.Contracts;
using HearingBooks.Contracts.DialogueSynthesis;
using HearingBooks.Domain.Entities;

namespace HearingBooks.Common.Mapper;

public class DialogueSynthesisProfile : Profile
{
	public DialogueSynthesisProfile()
	{
		CreateMap<DialogueSynthesis, DialogueSynthesisDto>()
			.ForMember(
				destination => destination.Language, 
				options => options.MapFrom(x => x.Language.Name))
			.ForMember(
				destination => destination.FirstSpeakerVoice, 
				options => options.MapFrom(x => x.FirstSpeakerVoice.Name))
			.ForMember(
				destination => destination.SecondSpeakerVoice, 
				options => options.MapFrom(x => x.SecondSpeakerVoice.Name))
			.ForMember(
				destination => destination.RequestingUserId, 
				options => options.MapFrom(x => x.User.Id));
		CreateMap<DialogueSyntehsisRequest, DialogueSynthesisData>();
	}
}