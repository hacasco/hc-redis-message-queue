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
            if (null == queueRepository)
                throw new ArgumentNullException(nameof(queueRepository));

            _queueRepository = queueRepository;
        }

        [HttpPost("push")]
        public async Task<IActionResult> PushAsync([FromBody] string message)
        {
            if (string.IsNullOrWhiteSpace(message))
            {
                return BadRequest("Message cannot be null or empty.");
            }

            await _queueRepository.PushMessageAsync(message);

            return Ok();
        }

        [HttpPost("pop")]
        public async Task<IActionResult> PopAsync()
        {
            var message = await _queueRepository.PopMessageAsync();

            if (null == message)
                return NotFound("Queue is empty.");

            return Ok(message);
        }

        [HttpGet("count")]
        public async Task<IActionResult> CountAsync()
        {
            return Ok(await _queueRepository.CountMessagesAsync());
        }

        private IQueueRepository _queueRepository;
    }
}
