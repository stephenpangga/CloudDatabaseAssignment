using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain
{
    public class Product
    {
        public Guid ProductId { get; set; }

        public string ProductName { get; set; } 

        public double Price { get; set; }

        public string ProductSpecification { get; set; }

        //this needs to accept multiple array
        public string ImageURL { get; set; }

        public string PartitionKey { get; set; }
    }
}
