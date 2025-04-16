using Microsoft.AspNetCore.Mvc;
using EventTicketProcessor.Models;
using EventTicketProcessor.Services;
using System.Threading.Tasks;

namespace EventTicketProcessor.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class TicketOrderController : ControllerBase
    {
        private readonly AzureQueueService _queueService;

        public TicketOrderController(AzureQueueService queueService)
        {
            _queueService = queueService;
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromBody] TicketOrder order)
        {
            await _queueService.SendMessageAsync(order);
            return Ok(new { message = "Ticket order sent to queue!" });
        }
    }
}
