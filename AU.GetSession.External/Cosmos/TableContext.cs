using System.Collections.Generic;
using System.Threading.Tasks;
using AU.GetSession.Services.External.Cosmos;
using Microsoft.Azure.Cosmos.Table;
using Microsoft.Extensions.Logging;

namespace AU.GetSession.External.Cosmos
{
    public class TableContext : ITableContext
    {
        private readonly ILogger<TableContext> logger;

        public TableContext(ILogger<TableContext> logger)
        {
            this.logger = logger;
        }

        public async Task<IEnumerable<T>> RetrieveByPartitionKey<T>(CloudTable cloudTable, string partitionKey) where T : ITableEntity, new()
        {
            List<T> collection = new List<T>();

            TableQuery<T> partitionScanQuery = new TableQuery<T>().Where(TableQuery.GenerateFilterCondition("PartitionKey", QueryComparisons.Equal, partitionKey));

            TableContinuationToken token = null;
            do
            {
                TableQuerySegment<T> segment = await cloudTable.ExecuteQuerySegmentedAsync(partitionScanQuery, token);
                token = segment.ContinuationToken;

                if (segment.RequestCharge.HasValue)
                {
                    logger.LogInformation($"Request Charge of RetrieveByPartitionKey Operation: {segment.RequestCharge}");
                }

                collection.AddRange(segment.Results);
            }
            while (token != null);

            return collection;
        }
    }
}
