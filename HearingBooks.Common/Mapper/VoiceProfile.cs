using AutoMapper;
using HearingBooks.Contracts;
using HearingBooks.Domain.Entities;

namespace HearingBooks.Common.Mapper;

public class VoiceProfile : Profile
{
	public VoiceProfile()
	{
		CreateMap<Voice, VoiceDto>();
	}
}