using System;
using Newtonsoft.Json;

namespace Stockfighter.Core.Api
{
    public class OrderResponse : ApiResponse
    {
        public string Venue { get; set; }
        public string Symbol { get; set; }
        public OrderDirection Direction { get; set; }
        [JsonProperty("originalQty")]
        public int OriginalQuantity { get; set; }
        [JsonProperty("qty")]
        public int OutstandingQuantity { get; set; }
        public int Price { get; set; }
        public string OrderType { get; set; }
        public int Id { get; set; }
        public string Account { get; set; }
        [JsonProperty("ts")]
        public DateTime TimeStamp { get; set; }
        public FillModel[] Fills { get; set; }
        public int TotalFilled { get; set; }
        public bool Open { get; set; }
    }
}