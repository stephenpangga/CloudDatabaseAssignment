using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Domain.DTO;

namespace Infrastructure.Service
{
    public interface IProductService
    {
        Task<IEnumerable<Product>> GetAllProductsAsync();

        Task<Product> GetProductByIdAsync(string ProductId);

        Task<Product> AddProductAsync(ProductDTO productDTO);

        Task<Product> UpdateProductAsync(ProductDTO productDTO, string productId);

        Task DeleteUserAsync(string ProductId);
    }
}
