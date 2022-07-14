using AutoMapper;
using EasySynthesis.Contracts;
using EasySynthesis.Domain.Entities;

namespace EasySynthesis.Common.Mapper;

public class VoiceProfile : Profile
{
	public VoiceProfile()
	{
		CreateMap<Voice, VoiceDto>();
	}
}