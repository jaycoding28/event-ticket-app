using System;

namespace EventTicketProcessor.Models
{
    public class TicketOrder
    {
        public string Name { get; set; }
        public string Email { get; set; }
        public string Concert { get; set; }
        public int Quantity { get; set; }
        public DateTime PurchaseDate { get; set; }
    }
}
