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
            product.ProductSpecification = productDTO.ProductSpecification;
            product.ImageURL = productDTO.ImageURL;
            product.PartitionKey = productDTO.ProductName; //maybe change this to producttype

            return await _productWriteRepository.AddAsync(product);
        }

        public async Task DeleteUserAsync(string productId)
        {
            if (!string.IsNullOrEmpty(productId))
            {
                Product product = await GetProductByIdAsync(productId);
                await _productWriteRepository.Delete(product);
            }
            else
            {
                throw new Exception("Product Id provided does not exist");
            }
            
        }

        public async Task<IEnumerable<Product>> GetAllProductsAsync()
        {
            return await _productReadRepository.GetAll().ToListAsync();
        }

        public async Task<Product> GetProductByIdAsync(string productId)
        {
            try
            {
                Guid id = Guid.Parse(productId);
                var product =  await _productReadRepository.GetAll().FirstOrDefaultAsync(p => p.ProductId == id);

                if (product == null)
                {
                    throw new Exception("The product you are looking for does not exist");
                }
                return product;
            }
            catch
            {
                throw new Exception("Please provide a proper ID");
            }
            
        }

        public async Task<Product> UpdateProductAsync(ProductDTO productDTO, string productId)
        {
            Product updateProduct = await GetProductByIdAsync(productId);
            updateProduct.ProductName = productDTO.ProductName;
            updateProduct.Price = productDTO.Price;
            updateProduct.ProductSpecification = productDTO.ProductSpecification;
            updateProduct.ImageURL = productDTO.ImageURL;
            return await _productWriteRepository.Update(updateProduct);
        }

        /*public uploadProductImage(string ProductId)
        {

        }*/
    }
}
