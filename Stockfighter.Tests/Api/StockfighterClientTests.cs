using System;
using System.Collections.Generic;
using System.Linq;
using System.Reactive.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using Stockfighter.Core.Api;
using FluentAssertions;
using Xunit;

namespace Stockfighter.Tests.Api
{
    public class StockfighterClientTests
    {
        public const int TimestampResolution = 1000 * 60 * 5; // Five minutes

        public StockfighterClientTests()
        {

            Target = new StockfighterClient(TestConstants.Venue, TestConstants.Account);           
        }


        public StockfighterClient Target { get; set; }


        [Fact]
        public async Task Stocks()
        {
            var actual = await Target.GetAllStocksAsync();
            actual.Symbols.Should().NotBeEmpty();
        }
        [Fact]
        public async Task Orderbook()
        {
            var actual = await Target.GetOrderbookAsync(TestConstants.StockSymbol);
            //actual.Asks.Should().NotBeEmpty();
            //actual.Bids.Should().NotBeEmpty();
            actual.TimeStamp.Should().BeCloseTo(DateTime.UtcNow, TimestampResolution);
        }

        [Theory]
        [InlineData(1, 100, OrderDirection.Buy, OrderType.Limit)]
        [InlineData(1, 100, OrderDirection.Sell, OrderType.Limit)]
        public async Task PlaceOrder(int quantity, int price, OrderDirection direction, string orderType)
        {
            var actual = await Target.PlaceOrderAsync(TestConstants.StockSymbol, quantity, price, direction, orderType);
            actual.Direction.Should().Be(direction);
            actual.OrderType.Should().Be(orderType);
            actual.OriginalQuantity.Should().Be(quantity);
            actual.Price.Should().Be(price);
            actual.TimeStamp.Should().BeCloseTo(DateTime.UtcNow, TimestampResolution);
        }

        [Fact]
        public async Task OrderInteraction()
        {
            var order = await Target.PlaceOrderAsync(TestConstants.StockSymbol, 1, 100, OrderDirection.Buy);

            var orderStatus = await Target.GetOrderAsync(TestConstants.StockSymbol, order.Id);

            order.ShouldBeEquivalentTo(orderStatus, o => o.Excluding(r => r.TimeStamp));

            var orderCancel = await Target.CancelOrderAsync(TestConstants.StockSymbol, order.Id);

            orderCancel.Open.Should().BeFalse();
        }

        [Fact]
        public async Task ObserveQuotes()
        {
            var tcs = new TaskCompletionSource<QuoteResponse>();

            var subscription = Target.ObserveQuotes()
                                     .Take(1)
                                     .Subscribe(tcs.SetResult);

            var order = await Target.PlaceOrderAsync(TestConstants.StockSymbol, 1, 100, OrderDirection.Buy);

            var orderCancel = await Target.CancelOrderAsync(TestConstants.StockSymbol, order.Id);

            var completed = await Task.WhenAny(tcs.Task, Task.Delay(1000));

            subscription.Dispose();

            ReferenceEquals(completed, tcs.Task).Should().BeTrue();
            tcs.Task.Result.Symbol.Should().Be(TestConstants.StockSymbol);

        }

        [Fact]
        public async Task ObserveExecutions()
        {
            var tcs = new TaskCompletionSource<ExecutionMessage>();

            var subscription = Target.ObserveExecutions()
                                     .Take(1)
                                     .Subscribe(tcs.SetResult);

            var sellOrder = await Target.PlaceOrderAsync(TestConstants.StockSymbol, 1, 100000, OrderDirection.Sell);
            var buyOrder = await Target.PlaceOrderAsync(TestConstants.StockSymbol, 1, 100000, OrderDirection.Buy);
            
            var completed = await Task.WhenAny(tcs.Task, Task.Delay(1000));

            subscription.Dispose();

            completed.Should().BeSameAs(tcs.Task);
            var actual = tcs.Task.Result;

            actual.Symbol.Should().Be(TestConstants.StockSymbol);
            actual.IncomingId.Should().Be(buyOrder.Id, "Buy order should be filled.");

        }




    }
}
