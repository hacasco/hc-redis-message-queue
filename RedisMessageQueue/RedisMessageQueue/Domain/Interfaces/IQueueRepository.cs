using RedisMessageQueue.Domain.Models;

namespace RedisMessageQueue.Domain.Interfaces
{
    public interface IQueueRepository
    {
        Task<IEnumerable<Message>> GetAllMessagesAsync();

        Task<Message> GetMessageContentAsync();

        Task<bool> SaveMessage(Message message);
    }
}
