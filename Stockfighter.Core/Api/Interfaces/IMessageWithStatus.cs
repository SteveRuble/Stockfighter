namespace Stockfighter.Core.Api.Interfaces
{
    public interface IMessageWithStatus
    {
        bool OK { get; set; }
        string Error { get; set; }
    }
}