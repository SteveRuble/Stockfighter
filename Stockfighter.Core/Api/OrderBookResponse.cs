using System;
using Newtonsoft.Json;

namespace Stockfighter.Core.Api
{
    public class OrderBookResponse : ApiResponse
    {
        public string Venue { get; set; }
        public string Symbol { get; set; }
        public OrderModel[] Bids { get; set; }
        public OrderModel[] Asks { get; set; }
        [JsonProperty("ts")]
        public DateTime TimeStamp { get; set; }
    }
}