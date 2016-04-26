using Stockfighter.Core.Api.Interfaces;

namespace Stockfighter.Core.Api
{
    public class StockfighterClientFactory : IStockfighterClientFactory
    {
        private readonly StockfighterClientFactoryConfig _config;

        public StockfighterClientFactory(StockfighterClientFactoryConfig config)
        {
            _config = config;
        }

        public IStockfighterClient Create(string venue, string account)
        {
            return new StockfighterClient(venue, account, _config.BaseUri, _config.Key);
        }
    }
}