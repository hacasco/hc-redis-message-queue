using Microsoft.Extensions.Caching.Distributed;
using Newtonsoft.Json;
using RedisMessageQueue.Domain.Interfaces;
using RedisMessageQueue.Domain.Models;

namespace RedisMessageQueue.Infrastructure
{
    public class QueueRepository : IQueueRepository
    {
        public QueueRepository(IDistributedCache distributedCache)
        {
            _redisCache = distributedCache;
        }

        public async Task<IEnumerable<Message>> GetAllMessagesAsync()
        {
            var messages = await _redisCache.GetStringAsync(QueueKey);

            if (messages == null)
            {
                return JsonConvert.DeserializeObject<IEnumerable<Message>>(messages);
            }

            return null;
        }

        public async Task<Message> GetMessageContentAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SaveMessage(Message message)
        {
            throw new NotImplementedException();
        }

        private IDistributedCache _redisCache;
        private const string QueueKey = "Queue";
    }
}
