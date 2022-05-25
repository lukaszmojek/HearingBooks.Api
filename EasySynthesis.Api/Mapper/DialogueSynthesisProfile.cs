using AutoMapper;
using EasySynthesis.Api.Syntheses.DialogueSyntheses;
using EasySynthesis.Api.Syntheses.DialogueSyntheses.RequestDialogueSynthesis;
using EasySynthesis.Contracts.DialogueSynthesis;
using EasySynthesis.Domain.Entities;

namespace EasySynthesis.Api.Mapper;

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
				options => options.MapFrom(x => x.SecondSpeakerVoice.Name));
		CreateMap<DialogueSyntehsisRequest, DialogueSynthesisData>();
	}
}