using Microsoft.AspNetCore.Mvc;
using RedisMessageQueue.Domain.Interfaces;

namespace RedisMessageQueue.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QueueController : ControllerBase
    {
        public QueueController(IQueueRepository queueRepository)
        {
            _queueRepository = queueRepository;
        }

        public async Task<IActionResult> GetAllMessages()
        {
            return Ok(_queueRepository.GetAllMessagesAsync());
        }

        private IQueueRepository _queueRepository;
    }
}
