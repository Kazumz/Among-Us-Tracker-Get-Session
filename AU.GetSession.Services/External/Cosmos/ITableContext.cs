using System.Collections.Generic;
using System.Threading.Tasks;
using Microsoft.Azure.Cosmos.Table;

namespace AU.GetSession.Services.External.Cosmos
{
    public interface ITableContext
    {
        Task<IEnumerable<T>> RetrieveByPartitionKey<T>(CloudTable cloudTable, string partitionKey) where T : ITableEntity, new();
    }
}
