using Newtonsoft.Json;

namespace Stockfighter.Core.Api
{
    public class OrderRequest
    {
        public string Account { get; set; }
        public string Venue { get; set; }
        public string Stock { get; set; }
        public int Price { get; set; }
        [JsonProperty("qty")]
        public int Quantity { get; set; }
        public OrderDirection Direction { get; set; }
        public string OrderType { get; set; }
    }
}