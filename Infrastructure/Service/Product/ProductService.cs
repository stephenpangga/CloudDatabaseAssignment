using Domain;
using Domain.DTO;
using Infrastructure.Repository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.Service
{
    public class ProductService : IProductService
    {
        private readonly ICosmosReadRepository<Product> _productReadRepository;
        private readonly ICosmosWriteRepository<Product> _productWriteRepository;

        public ProductService(ICosmosReadRepository<Product> productReadRepository, ICosmosWriteRepository<Product> productWriteRepository)
        {
            _productReadRepository = productReadRepository;
            _productWriteRepository = productWriteRepository;
    }

        public async Task<Product> AddProductAsync(ProductDTO productDTO)
        {
            Product product = new Product();
            product.ProductId = Guid.NewGuid();
            product.ProductName = productDTO.ProductName;
            product.Price = productDTO.Price;

            return await _productWriteRepository.AddAsync(product);
        }

        public Task DeleteUserAsync(string ProductId)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<Product> GetProductByIdAsync(string ProductId)
        {
            throw new NotImplementedException();
        }

        public Task<Product> UpdateProductAsync(ProductDTO productDTO)
        {
            throw new NotImplementedException();
        }
    }
}
