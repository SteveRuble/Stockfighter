using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using MediatR;
using Stockfighter.Core.Api;
using Stockfighter.Core.Api.Interfaces;

namespace Stockfighter.Core.Handlers
{
    public class BlockOrderRequest : IAsyncRequest<CompletionResponse>,
        IMessageWithVenue,
        IMessageWithSymbol, IAsyncRequest
    {
        public int Amount { get; set; }
        public string Venue { get; set; }
        public string Symbol { get; set; }
    }


    public class CompletionResponse
    {
        public bool Success { get; set; }
    }

    public class BlockOrderHandler : IAsyncRequestHandler<BlockOrderRequest, CompletionResponse>
    {
        private readonly IStockfighterClient _client;
        public BlockOrderHandler(IStockfighterClient client)
        {
            _client = client;
        }

        public Task<CompletionResponse> Handle(BlockOrderRequest message)
        {
            return null;
        }
    }
}
