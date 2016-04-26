using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Stockfighter.Core.Api;
using Stockfighter.Core.Api.Interfaces;
using Stockfighter.Core.Book;
using Stockfighter.Core.ValueTypes;

namespace Stockfighter.Core.Jobs
{
    public class MarketMaker
    {
        private readonly IStockfighterClient _client;
        public MarketMaker(IStockfighterClient client)
        {
            _client = client;
        }

        public async Task Run(Symbol symbol)
        {
            var position = new Position(symbol);


        }
    }
}
