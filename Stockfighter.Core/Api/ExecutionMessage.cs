using System;
using Stockfighter.Core.Api.Interfaces;

namespace Stockfighter.Core.Api
{
    public class ExecutionMessage :
        IMessageWithAccount,
        IMessageWithStatus,
        IMessageWithSymbol,
        IMessageWithVenue
    {
        public bool OK { get; set; }
        public string Error { get; set; }

        public string Account { get; set; }
        public string Symbol { get; set; }
        public string Venue { get; set; }

        public OrderResponse Order { get; set; }
        public int StandingId { get; set; }
        public int IncomingId { get; set; }
        public int Price { get; set; }
        public int Filled { get; set; }
        public DateTime FilledAt { get; set; }
        /// <summary>
        /// Whether the order that was on the book is now complete
        /// </summary>
        public bool StandingComplete { get; set; }
        /// <summary>
        /// Whether the incoming order is complete (as of this execution)
        /// </summary>
        public bool IncomingComplete { get; set; }
    }
}