using Domain;
using Domain.DTO;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using HttpMultipartParser;
using Azure.Storage.Blobs;
using Azure.Storage.Blobs.Models;

namespace Infrastructure.Service
{
    public class ProductService : IProductService
    {
        private BlobServiceClient blobServiceClient;
        private BlobContainerClient containerClient;
        //private readonly BlobCredentialOptions _blobCredentialOptions;

        private readonly ICosmosReadRepository<Product> _productReadRepository;
        private readonly ICosmosWriteRepository<Product> _productWriteRepository;

        public ProductService(ICosmosReadRepository<Product> productReadRepository, ICosmosWriteRepository<Product> productWriteRepository)
        {
            _productReadRepository = productReadRepository;
            _productWriteRepository = productWriteRepository;

            blobServiceClient = new BlobServiceClient("DefaultEndpointsProtocol=https;AccountName=widgetandcostorage;AccountKey=/IO1mMYd3pWglFdngLbmoezfAqvh+F5MlSY7ZyB7XIKu+r09skOqTch3nrW/cs8GZ7PAKeIJRgFqwzEc8YBKDg==;EndpointSuffix=core.windows.net");
            containerClient = blobServiceClient.GetBlobContainerClient("product-image");

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

        private async Task<Product>UpdateProduct(Product product)
        {
            return await _productWriteRepository.Update(product);
        }

        public async Task UploadProductImageAsync(string productId, FilePart file)
        {
            //check how to get image from request

            //upload the file 

            //get the url from the blob client

            //update product info wil image url


            // Get a reference to a blob
            BlobClient blobClient = containerClient.GetBlobClient(file.Name);

            // Upload the file
            await blobClient.UploadAsync(file.Data, new BlobHttpHeaders { ContentType = file.ContentType });

            //get the URL of the uploaded image
            var blobUrl = blobClient.Uri.AbsoluteUri;

            var product = await GetProductByIdAsync(productId);

            //set the new url for the existing story
            product.ImageURL = blobUrl;

            await UpdateProduct(product);
        }
    }
}
