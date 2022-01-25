using AutoMapper;
using HearingBooks.Api.Languages;
using HearingBooks.Domain.Entities;

namespace HearingBooks.Api.Mapper;

public class VoiceProfile : Profile
{
	public VoiceProfile()
	{
		CreateMap<Voice, VoiceDto>();
	}
}