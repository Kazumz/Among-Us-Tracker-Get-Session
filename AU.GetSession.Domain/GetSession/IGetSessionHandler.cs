using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AU.GetSession.Domain.Entities;

namespace AU.GetSession.Domain.GetSession
{
    public interface IGetSessionHandler
    {
        Task<List<Player>> GetSession(Guid sessionId);
    }
}
