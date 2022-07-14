using EasySynthesis.Domain.Entities;

namespace EasySynthesis.Infrastructure.Repositories;

public interface IVoiceRepository
{
    Task<Voice> GetVoiceByName(string name);
}