using Stockfighter.Core.Api.Interfaces;

namespace Stockfighter.Core.Api
{
    public interface IStockfighterClientFactory
    {
        IStockfighterClient Create(string venue, string account);
    }
}