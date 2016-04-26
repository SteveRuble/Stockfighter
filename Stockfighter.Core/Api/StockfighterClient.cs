using System;
using System.ComponentModel;
using System.Net.Http;
using System.Reactive.Linq;
using System.Reactive.Subjects;
using System.Security.Cryptography.X509Certificates;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Newtonsoft.Json;
using Newtonsoft.Json.Linq;
using Stockfighter.Core.Api.Interfaces;
using WebSocket4Net;

namespace Stockfighter.Core.Api
{
    public class StockfighterClient : IStockfighterClient
    {
        private readonly HttpClient _client;
        private readonly string _venue;
        private readonly string _account;

        public StockfighterClient(string venue, string account, string baseUri, string key)
        {
            _venue = venue;
            _account = account;
            _client = new HttpClient();
            _client.DefaultRequestHeaders.Add("X-Starfighter-Authorization", key);
            _client.BaseAddress = new Uri(baseUri);
        }

        public Task<bool> GetGlobalHeartbeatAsync() => GetAsync<ApiResponse>("/heartbeat").ContinueWith(r => r.Result.OK);

        public Task<bool> GetVenueHeartbeatAsync() => GetAsync<ApiResponse>($"/venues/{_venue}/heartbeat").ContinueWith(r => r.Result.OK);

        public Task<StocksResponse> GetAllStocksAsync() => GetAsync<StocksResponse>($"/venues/{_venue}/stocks");

        public Task<OrderBookResponse> GetOrderbookAsync(string symbol) => GetAsync<OrderBookResponse>($"/venues/{_venue}/stocks/{symbol}");

        public Task<OrderResponse> PlaceOrderAsync(string symbol, int quantity, int price, OrderDirection direction, string orderType = OrderType.Limit)
         => PostAsync<OrderResponse>($"/venues/{_venue}/stocks/{symbol}/orders", new OrderRequest()
         {
             Account = _account,
             Venue = _venue,
             Direction = direction,
             OrderType = orderType,
             Price = price,
             Quantity = quantity,
             Stock = symbol
         });

        public Task<OrderResponse> GetOrderAsync(string symbol, int id) => GetAsync<OrderResponse>($"/venues/{_venue}/stocks/{symbol}/orders/{id}");
        public Task<OrderResponse> CancelOrderAsync(string symbol, int id) => PostAsync<OrderResponse>($"/venues/{_venue}/stocks/{symbol}/orders/{id}/cancel", "dummy");
        public Task<OrderResponse[]> GetAllOrdersAsync() => GetAsync<MultiOrderResponse>($"/venues/{_venue}/accounts/{_account}/orders").ContinueWith(r => r.Result.Orders);

        public Task<OrderResponse[]> GetAllOrdersAsync(string symbol)
            => GetAsync<MultiOrderResponse>($"/venues/{_venue}/accounts/{_account}/stock/{symbol}/orders").ContinueWith(r => r.Result.Orders);

        public Task<QuoteResponse> GetQuoteAsync(string symbol) => GetAsync<QuoteResponse>($"/venues/{_venue}/stocks/{symbol}/quote");

        public IObservable<QuoteResponse> ObserveQuotes(string symbol = null)
        {
            var url = symbol == null
                ? $"wss://api.stockfighter.io/ob/api/ws/{_account}/venues/{_venue}/tickertape"
                : $"wss://api.stockfighter.io/ob/api/ws/{_account}/venues/{_venue}/tickertape/stocks/{symbol}";

            var observable = ObserveOverWebSocket(url, message =>
            {
                var payload = JsonConvert.DeserializeObject<TickerMessage>(message, CoreModule.JsonSerializerSettings);
                if (payload.OK)
                {
                    return payload.Quote;
                }
                return null;
            });

            return observable;
        }

        public IObservable<ExecutionMessage> ObserveExecutions(string symbol = null)
        {
            var url = symbol == null
                ? $"wss://api.stockfighter.io/ob/api/ws/{_account}/venues/{_venue}/executions"
                : $"wss://api.stockfighter.io/ob/api/ws/{_account}/venues/{_venue}/executions/stocks/{symbol}";

            var observable = ObserveOverWebSocket(url, message =>
            {
                var payload = JsonConvert.DeserializeObject<ExecutionMessage>(message, CoreModule.JsonSerializerSettings);
                if (payload.OK && payload.Order.OK)
                {
                    return payload;
                }
                return null;
            });

            return observable;
        }

        private IObservable<TMessage> ObserveOverWebSocket<TMessage>(string uri, Func<string, TMessage> payloadResolver)
        {

            var subject = new Subject<TMessage>();
            var socket = new WebSocket(uri);

            socket.MessageReceived += (sender, args) =>
            {
                var payload = payloadResolver(args.Message);
                if (payload != null)
                {
                    subject.OnNext(payload);
                }
            };

            socket.Error += (sender, args) => subject.OnError(args.Exception);

            socket.Closed += (sender, args) => subject.OnCompleted();

            socket.Open();

            subject.Subscribe(_ => { }, _ =>
            {
                socket.Dispose();
            }, () =>
            {
                socket.Dispose();
            });


            return subject;
        }


        private async Task<T> GetAsync<T>(string path) where T : ApiResponse
        {
            var response = await _client.GetAsync("/ob/api" + path);
            var json = await response.Content.ReadAsStringAsync();
            var payload = JsonConvert.DeserializeObject<T>(json, CoreModule.JsonSerializerSettings);
            if (!string.IsNullOrEmpty(payload.Error))
            {
                throw new ApplicationException($"Error from GET to '{path}': {payload.Error}");
            }
            return payload;
        }

        private async Task<T> PostAsync<T>(string path, object payload) where T : ApiResponse
        {
            var jsonPayload = JsonConvert.SerializeObject(payload, CoreModule.JsonSerializerSettings);
            var response = await _client.PostAsync("/ob/api" + path, new StringContent(jsonPayload));
            var jsonResponse = await response.Content.ReadAsStringAsync();
            var responsePayload = JsonConvert.DeserializeObject<T>(jsonResponse, CoreModule.JsonSerializerSettings);
            if (!string.IsNullOrEmpty(responsePayload.Error))
            {
                throw new ApplicationException($"Error from POST to '{path}' with body '{jsonPayload}': {responsePayload.Error}");
            }
            return responsePayload;
        }
    }

    public enum OrderDirection
    {
        Buy,
        Sell
    }

    public static class OrderType
    {
        public const string Limit = "limit";
        public const string Market = "market";
        public const string FillOrKill = "fill-or-kill";
        public const string ImmediateOrCancel = "immediate-or-cancel";
    }
}
