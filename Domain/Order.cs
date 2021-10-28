using System;

namespace Domain
{
    public class Order
    {
        public Guid OrderId { get; set; }

        public Guid ProductId { get; set; }

        public Guid UserId { get; set; }

        public DateTime OrderDate { get; set; }

        //initial creation of order has shipping null,
        //once product is scanned for shipment, this datetime will be updated to the day of shipment.
        public DateTime? ShippingDate { get; set; }

        public string PartitionKey { get; set; }

        public Order()
        {

        }
    }
}
