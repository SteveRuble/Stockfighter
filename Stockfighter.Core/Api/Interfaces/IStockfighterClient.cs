using System;
using System.Threading.Tasks;

namespace Stockfighter.Core.Api.Interfaces
{
    public interface IStockfighterClient
    {
        Task<bool> GetGlobalHeartbeatAsync();
        Task<bool> GetVenueHeartbeatAsync();
        Task<StocksResponse> GetAllStocksAsync();
        Task<OrderBookResponse> GetOrderbookAsync(string symbol);
        Task<OrderResponse> PlaceOrderAsync(string symbol, int quantity, int price, OrderDirection direction, string orderType = OrderType.Limit);
        Task<OrderResponse> GetOrderAsync(string symbol, int id);
        Task<OrderResponse> CancelOrderAsync(string symbol, int id);
        Task<OrderResponse[]> GetAllOrdersAsync();
        Task<OrderResponse[]> GetAllOrdersAsync(string symbol);
        Task<QuoteResponse> GetQuoteAsync(string symbol);
        IObservable<QuoteResponse> ObserveQuotes(string symbol = null);
        IObservable<ExecutionMessage> ObserveExecutions(string symbol = null);
    }
}