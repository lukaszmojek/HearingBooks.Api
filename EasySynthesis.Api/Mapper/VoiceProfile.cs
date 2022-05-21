using AutoMapper;
using EasySynthesis.Api.Languages.GetLanguages;
using EasySynthesis.Domain.Entities;

namespace EasySynthesis.Api.Mapper;

public class VoiceProfile : Profile
{
	public VoiceProfile()
	{
		CreateMap<Voice, VoiceDto>();
	}
}