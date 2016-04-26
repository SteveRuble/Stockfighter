using System;
using System.Security.Cryptography.X509Certificates;
using Newtonsoft.Json;

namespace Stockfighter.Core.Api
{
    public class FillModel
    {
        public int Price { get; set; }
        [JsonProperty("qty")]
        public int Quantity { get; set; }
        [JsonProperty("ts")]
        public DateTime TimeStamp { get; set; }
    }
}