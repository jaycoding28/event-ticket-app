using Microsoft.AspNetCore.Mvc;
using Azure.Storage.Queues;
using System.Text.Json;
using EventTicketProcessor.Models;


namespace EventTicketProcessor.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class TicketOrderController : ControllerBase
    {
        private readonly IConfiguration _config;

        public TicketOrderController(IConfiguration config)
        {
            _config = config;
        }

        [HttpPost]
        public async Task<IActionResult> SubmitTicket([FromBody] TicketOrder order)
        {
            var connectionString = _config["AzureStorageQueue"];
            var queueClient = new QueueClient(connectionString, "event-ticket-queue");

            await queueClient.CreateIfNotExistsAsync();

            string json = JsonSerializer.Serialize(order);
            string base64 = Convert.ToBase64String(System.Text.Encoding.UTF8.GetBytes(json));

            await queueClient.SendMessageAsync(base64);

            return Ok(new { status = "queued", data = order });
        }
    }
}
