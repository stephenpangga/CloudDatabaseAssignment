using Domain;
using Domain.DTO;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
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

        public async Task DeleteUserAsync(string productId)
        {
            Product product = await GetProductByIdAsync(productId);
            await _productWriteRepository.Delete(product);
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _productReadRepository.GetAll().ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(string productId)
        {
            Guid id = Guid.Parse(productId);
            return await _productReadRepository.GetAll().FirstOrDefaultAsync(p => p.ProductId == id);
        }

        public async Task<Product> UpdateProductAsync(ProductDTO productDTO)
        {
            Product updateProduct = new Product();
            return await _productWriteRepository.Update(updateProduct);
        }
    }
}
