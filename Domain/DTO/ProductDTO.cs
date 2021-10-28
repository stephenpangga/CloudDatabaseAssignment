using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain.DTO
{
    public class ProductDTO
    {
        public string ProductName { get; set; }
        public double Price { get; set; }
        public string ProductSpecification { get; set; }
        public List<ProductImage> ImageURLs { get; set; }

        public ProductDTO()
        {

        }
    }
}
