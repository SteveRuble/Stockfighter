using System;
using System.Threading.Tasks;
using Stockfighter.Core.Api;
using Stockfighter.Core.Api.Interfaces;

namespace Stockfighter.Tests
{
    public abstract class LevelBase
    {
        public abstract string Account { get; }
        public abstract string Venue { get; }
        public abstract string Symbol { get; }
        
        public IStockfighterClient Client { get; set; }


    }

    /// <summary>
    /// Task: buy 100,000 shares of DOI.
    /// </summary>
    public class ChockABlock : LevelBase
    {
        public override string Account => "BAD57863223";
        public override string Venue => "BBREX";
        public override string Symbol => "DBDL";

        public async Task BuyShares()
        {
            var remaining = 100000;
            var rng = new Random();

            while (remaining > 0)
            {
                var quantity = Math.Min(remaining, rng.Next(10000, 20000));

                var quote = await Client.GetQuoteAsync(Symbol);

                var price = quote.Ask;


            }
        }
    }
}
