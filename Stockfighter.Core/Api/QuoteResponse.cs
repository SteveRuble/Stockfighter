using System;

namespace Stockfighter.Core.Api
{
    public class QuoteResponse : ApiResponse
    {
        public string Venue { get; set; }
        public string Symbol { get; set; }
        public int Bid { get; set; }
        public int Ask { get; set; }
        public int BidSize { get; set; }
        public int AskSize { get; set; }
        public int BidDepth { get; set; }
        public int AskDepth { get; set; }
        public int Last { get; set; }
        public int LastSize { get; set; }
        public DateTime QuoteTime { get; set; }
        public DateTime LastTrade { get; set; }
    }

    public class TickerMessage
    {
        public bool OK { get; set; }
        public QuoteResponse Quote { get; set; }
    }
}