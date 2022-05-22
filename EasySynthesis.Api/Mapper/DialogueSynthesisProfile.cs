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
		CreateMap<DialogueSynthesis, DialogueSynthesisDto>();
		CreateMap<DialogueSyntehsisRequest, DialogueSynthesisData>();
	}
}