namespace Stockfighter.Core.Api
{
    public class StocksResponse : ApiResponse
    {
        public SymbolModel[] Symbols { get; set; }
    }
}