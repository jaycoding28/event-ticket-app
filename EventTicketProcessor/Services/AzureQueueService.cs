using Azure.Storage.Queues;
using Microsoft.Extensions.Configuration;
using System.Text.Json;
using EventTicketProcessor.Models;
using System.Threading.Tasks;

namespace EventTicketProcessor.Services
{
    public class AzureQueueService
    {
        private readonly QueueClient _queueClient;

        public AzureQueueService(IConfiguration configuration)
        {
            var connectionString = configuration.GetConnectionString("AzureQueue");
            _queueClient = new QueueClient(connectionString, "ticket-orders");
            _queueClient.CreateIfNotExists();
        }

        public async Task SendMessageAsync(TicketOrder order)
        {
            var json = JsonSerializer.Serialize(order);
            await _queueClient.SendMessageAsync(json);
        }
    }
}
