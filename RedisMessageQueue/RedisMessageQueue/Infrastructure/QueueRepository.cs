using RedisMessageQueue.Domain.Interfaces;
using RedisMessageQueue.Domain.Models;
using StackExchange.Redis;

namespace RedisMessageQueue.Infrastructure
{
    public class QueueRepository : IQueueRepository
    {
        public QueueRepository(IConnectionMultiplexer redisCache)
        {
            if (null == redisCache)
                throw new ArgumentNullException(nameof(redisCache));

            _redisCache = redisCache;
            InitializeDatabase();
        }

        public async Task<IEnumerable<Message>> GetAllMessagesAsync()
        {
            await RefreshQueue();

            return _messages;
        }

        public Task<Message> GetMessageContentAsync()
        {
            throw new NotImplementedException();
        }

        public async Task<bool> SaveMessageAsync(Message message)
        {
            if (null == message)
                throw new ArgumentNullException(nameof(message));

            return await _redisDatabase.StringSetAsync(message.CreationDate.ToString(), message.Content);
        }

        public Task<bool> DeleteMessageContentAsync(DateTime key)
        {
            throw new NotImplementedException();
        }

        private IConnectionMultiplexer _redisCache;
        private IDatabase _redisDatabase;
        private IEnumerable<Message> _messages = new List<Message>();

        private void InitializeDatabase()
        {
            _redisDatabase = _redisCache.GetDatabase(0);
        }

        private async Task RefreshQueue()
        {
            var server = _redisCache.GetServer(_redisCache.GetEndPoints().FirstOrDefault());

            if (null != server)
            {
                var redisKeys = server.Keys();
                var unorderedMessages = new List<Message>();

                foreach (var key in redisKeys)
                {
                    unorderedMessages.Add(new Message
                    {
                        CreationDate = Convert.ToDateTime(key),
                        Content = await _redisDatabase.StringGetAsync(key)
                    });
                }

                _messages = unorderedMessages.OrderBy(msg => msg.CreationDate);
            }
        }
    }
}
