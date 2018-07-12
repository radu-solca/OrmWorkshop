using System;

namespace eShop.Domain
{
    public class Order : Entity
    {
        public Guid ProductId { get; set; }
        public Guid CustomerId { get; set; }
        public DateTime PlacedAt { get; set; }
        public DateTime? DeliveredAt { get; set; }
    }
}