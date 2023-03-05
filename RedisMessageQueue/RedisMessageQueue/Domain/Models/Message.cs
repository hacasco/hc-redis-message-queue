namespace RedisMessageQueue.Domain.Models
{
    public class Message
    {
        public string Content { get; set; }

        public DateTime CreationDate { get; set; }
    }
}
