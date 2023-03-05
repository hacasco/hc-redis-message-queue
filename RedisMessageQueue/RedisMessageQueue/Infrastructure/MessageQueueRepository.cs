using RedisMessageQueue.Domain.Interfaces;
using RedisMessageQueue.Domain.Models;

namespace RedisMessageQueue.Infrastructure
{
    public class MessageQueueRepository : IMessageQueueRepository
    {
        public Task<int> GetAllMessagesAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Message> GetMessageContentAsync()
        {
            throw new NotImplementedException();
        }

        public Task<bool> SaveMessage(Message message)
        {
            throw new NotImplementedException();
        }
    }
}
