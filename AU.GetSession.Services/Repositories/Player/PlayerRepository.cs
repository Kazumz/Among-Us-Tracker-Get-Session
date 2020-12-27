using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using AU.GetSession.Domain.Services.Repositories;
using AU.GetSession.Services.External.Cosmos;
using AutoMapper;
using Microsoft.Azure.Cosmos.Table;

namespace AU.GetSession.Services.Repositories.Player
{
    public class PlayerRepository : IPlayerRepository
    {
        private readonly ITableContext tableContext;
        private readonly CloudTable table;
        private readonly IMapper mapper;

        public PlayerRepository(
            CloudTable table,
            ITableContext tableContext,
            IMapper mapper)
        {
            this.tableContext = tableContext;
            this.table = table;
            this.mapper = mapper;
        }

        public async Task<List<Domain.Entities.Player>> GetPlayers(Guid sessionId)
        {
            var results = await tableContext.RetrieveByPartitionKey<DataModel.Player>(table, sessionId.ToString());

            return mapper.Map<IEnumerable<DataModel.Player>, List<Domain.Entities.Player>>(results);
        }
    }
}
