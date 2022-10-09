using HearingBooks.Domain.Entities;

namespace HearingBooks.Infrastructure.Repositories;

public interface IVoiceRepository
{
    Task<Voice> GetVoiceByName(string name);
}