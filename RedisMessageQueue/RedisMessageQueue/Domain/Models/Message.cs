namespace RedisMessageQueue.Domain.Models
{
    public class Message
    {
        public Guid Id { get; set; }

        public string Content { get; set; }
    }
}
