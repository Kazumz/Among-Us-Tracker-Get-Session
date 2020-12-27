using System;
using Microsoft.Azure.Cosmos.Table;

namespace AU.GetSession.Services.Repositories.Player.DataModel
{
    public class Player : TableEntity
    {
        public Player()
        {
        }

        public Player(Guid guid, string name)
        {
            PartitionKey = guid.ToString();
            RowKey = name;
        }

        public int Position { get; set; }

        public int Colour { get; set; }
    }
}
