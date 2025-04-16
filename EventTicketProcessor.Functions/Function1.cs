using Microsoft.Azure.Functions.Worker;
using Microsoft.Extensions.Logging;
using System.Text.Json;
using Microsoft.Data.SqlClient;
using EventTicketProcessor.Models;
using System;

namespace EventTicketProcessor.Functions
{
    public class Function1
    {
        private readonly ILogger _logger;

        public Function1(ILoggerFactory loggerFactory)
        {
            _logger = loggerFactory.CreateLogger<Function1>();
        }

        [Function("QueueTriggerFunction")]
        public void Run([QueueTrigger("ticket-orders", Connection = "AzureWebJobsStorage")] string queueItem)
        {
            _logger.LogInformation($"Received queue message: {queueItem}");

            var order = JsonSerializer.Deserialize<TicketOrder>(queueItem);

            string connectionString = Environment.GetEnvironmentVariable("SqlConnectionString");

            using var conn = new SqlConnection(connectionString);
            conn.Open();

            var sql = @"
                INSERT INTO TicketOrders (Name, Email, Concert, Quantity, PurchaseDate)
                VALUES (@Name, @Email, @Concert, @Quantity, @PurchaseDate)";

            using var cmd = new SqlCommand(sql, conn);
            cmd.Parameters.AddWithValue("@Name", order.Name);
            cmd.Parameters.AddWithValue("@Email", order.Email);
            cmd.Parameters.AddWithValue("@Concert", order.Concert);
            cmd.Parameters.AddWithValue("@Quantity", order.Quantity);
            cmd.Parameters.AddWithValue("@PurchaseDate", order.PurchaseDate);

            cmd.ExecuteNonQuery();
        }
    }
}
