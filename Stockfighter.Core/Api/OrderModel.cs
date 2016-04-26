using Newtonsoft.Json;

namespace Stockfighter.Core.Api
{
    public class OrderModel
    {
        public int Price { get; set; }
        [JsonProperty("qty")]
        public int Quantity { get; set; }
        public bool IsBuy { get; set; }
    }
}