using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AU.GetSession.Domain.Entities;

namespace AU.GetSession.Domain.Services.Repositories
{
    public interface IPlayerRepository
    {
        Task<List<Player>> GetPlayers(Guid sessionId);
    }
}
