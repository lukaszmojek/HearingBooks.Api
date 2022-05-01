using AutoMapper;
using HearingBooks.Api.Syntheses.DialogueSyntheses;
using HearingBooks.Domain.Entities;

namespace HearingBooks.Api.Mapper;

public class DialogueSynthesisProfile : Profile
{
	public DialogueSynthesisProfile()
	{
		CreateMap<DialogueSynthesis, DialogueSynthesisDto>();
	}
}