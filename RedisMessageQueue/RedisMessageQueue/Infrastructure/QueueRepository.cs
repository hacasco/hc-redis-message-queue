using RedisMessageQueue.Domain.Interfaces;
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
            _redisDatabase = _redisCache.GetDatabase(0);
        }

        public async Task<bool> PushMessageAsync(string message)
        {
            if (string.IsNullOrWhiteSpace(message))
                throw new ArgumentException($"{nameof(message)} cannot be null or empty.");

            return await _redisDatabase.StringSetAsync(DateTime.Now.ToString(), message);
        }

        public async Task<string> PopMessageAsync()
        {
            await RefreshQueue();

            if (_messages.Count == 0)
                return null;

            var message = _messages.FirstOrDefault();
            await _redisDatabase.KeyDeleteAsync(message.Key.ToString());

            return message.Value;
        }

        public async Task<int> CountMessagesAsync()
        {
            await RefreshQueue();
            return _messages.Count();
        }

        private IConnectionMultiplexer _redisCache;
        private IDatabase _redisDatabase;
        private SortedDictionary<DateTime, string> _messages = new SortedDictionary<DateTime, string>();

        private async Task RefreshQueue()
        {
            var server = _redisCache.GetServer(_redisCache.GetEndPoints().FirstOrDefault());

            if (null != server)
            {
                var redisKeys = server.Keys();

                foreach (var key in redisKeys)
                {
                    _messages.TryAdd(
                        Convert.ToDateTime(key),
                        await _redisDatabase.StringGetAsync(key));
                }
            }
        }
    }
}
