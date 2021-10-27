using System;

namespace Domain
{
    public class Order
    {
        public Guid OrderId { get; set; }

        public Guid ProductId { get; set; }

        public Guid UserId { get; set; }

        public DateTime? OrderDate { get; set; }
        //public DateTime ShippingDate { get; set; }

        public string PartitionKey { get; set; }

        public Order()
        {

        }
    }
}
