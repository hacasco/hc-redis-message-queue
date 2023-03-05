namespace RedisMessageQueue.Domain.Interfaces
{
    public interface IQueueRepository
    {
        Task<string> PopMessageAsync();

        Task<bool> PushMessageAsync(string message);

        Task<int> CountMessagesAsync();
    }
}
