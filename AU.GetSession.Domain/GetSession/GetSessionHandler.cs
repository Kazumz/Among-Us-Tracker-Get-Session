using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using AU.GetSession.Domain.Entities;
using AU.GetSession.Domain.Services.Repositories;

namespace AU.GetSession.Domain.GetSession
{
    public class GetSessionHandler : IGetSessionHandler
    {
        private readonly IPlayerRepository playerRepository;

        public GetSessionHandler(IPlayerRepository playerRepository)
        {
            this.playerRepository = playerRepository;
        }

        public async Task<List<Player>> GetSession(Guid sessionId)
        {
            return await playerRepository.GetPlayers(sessionId);
        }
    }
}
