using Microsoft.AspNetCore.Mvc;
using Producer.Services;

namespace Producer.Controllers;

[ApiController]
[Route("[controller]")]
public class ProducerController : ControllerBase
{
    private readonly IQueuePublisher _queuePublisher;

    public ProducerController(IQueuePublisher queuePublisher)
    {
        _queuePublisher = queuePublisher;
    }
    
    [HttpPost]
    public async Task<IActionResult> PostAsync(string queueName, string message)
    {
        await _queuePublisher.PublishAsync(queueName, message);
        return Ok();
    }
}