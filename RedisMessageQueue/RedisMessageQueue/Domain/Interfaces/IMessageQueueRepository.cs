using RedisMessageQueue.Domain.Models;

namespace RedisMessageQueue.Domain.Interfaces
{
    public interface IMessageQueueRepository
    {
        Task<int> GetAllMessagesAsync();

        Task<Message> GetMessageContentAsync();

        Task<bool> SaveMessage(Message message);
    }
}
