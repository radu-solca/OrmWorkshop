using System;
using System.Collections.Generic;
using System.Text;

namespace eShop.Domain
{
    public class OrderItem : Entity
    {
        public Guid OrderId { get; set; }
        public Guid ProductId { get; set; }
        public int Quantity { get; set; }

        public Product Product { get; set; }
    }
}
