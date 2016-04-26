namespace Stockfighter.Core.Api
{
    public class MultiOrderResponse : ApiResponse
    {
        public OrderResponse[] Orders { get; set; }
    }
}