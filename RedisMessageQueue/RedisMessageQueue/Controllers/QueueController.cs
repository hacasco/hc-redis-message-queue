using Microsoft.AspNetCore.Mvc;
using RedisMessageQueue.Domain.Interfaces;
using RedisMessageQueue.Domain.Models;

namespace RedisMessageQueue.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class QueueController : ControllerBase
    {
        public QueueController(IQueueRepository queueRepository)
        {
            if (null == queueRepository)
                throw new ArgumentNullException(nameof(queueRepository));

            _queueRepository = queueRepository;
        }

        [HttpGet]
        public async Task<IActionResult> GetAllMessages()
        {
            return Ok(await _queueRepository.GetAllMessagesAsync());
        }

        [HttpPost("push", Name = "Push")]
        public async Task<IActionResult> Push([FromBody] Message message)
        {
            if (null == message)
            {
                return BadRequest("Message cannot be null or empty.");
            }

            await _queueRepository.SaveMessageAsync(message);

            return Ok();
        }

        [HttpGet("count", Name ="Count")]
        public async Task<IActionResult> Count()
        {
            var messages = await _queueRepository.GetAllMessagesAsync();
            return Ok(messages.Count());
        }

        private IQueueRepository _queueRepository;
    }
}
