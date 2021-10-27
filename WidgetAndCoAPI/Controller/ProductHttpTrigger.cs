using Domain.DTO;
using Infrastructure;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Infrastructure.Service;

namespace WidgetAndCoAPI
{
    public class ProductHttpTrigger
    {
        private ILogger Logger { get; }

        private readonly IProductService _productService;

        public ProductHttpTrigger(ILogger<OrderHttpTrigger> Logger, IProductService productService)
        {
            this.Logger = Logger;
            _productService = productService;
        }

        [Function("GetAllProduct")]
        public async Task<HttpResponseData> GetAllProduct([HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "products")] HttpRequestData req, FunctionContext executionContext)
        {
            HttpResponseData response = req.CreateResponse(System.Net.HttpStatusCode.OK);
            await response.WriteAsJsonAsync(_productService.GetAllProductsAsync());
            return response;
        }
        [Function("GetByProductId")]
        public async Task<HttpResponseData> GetAProductById([HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "products/{ProductId}")] HttpRequestData req, string productId, FunctionContext executionContext)
        {
            HttpResponseData response = req.CreateResponse(System.Net.HttpStatusCode.OK);
            await response.WriteAsJsonAsync(_productService.GetProductByIdAsync(productId));
            return response;
        }

        [Function("AddProduct")]
        public async Task<HttpResponseData> AddProduct([HttpTrigger(AuthorizationLevel.Anonymous, "Post", Route = "products")] HttpRequestData req, FunctionContext executionContext)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            ProductDTO productDTO = JsonConvert.DeserializeObject<ProductDTO>(requestBody);
            HttpResponseData response = req.CreateResponse(System.Net.HttpStatusCode.OK);
            await response.WriteAsJsonAsync(_productService.AddProductAsync(productDTO));
            return response;
        }


        [Function("UpdateProduct")]
        public async Task<HttpResponseData> UpdateProduct([HttpTrigger(AuthorizationLevel.Anonymous, "Put", Route = "products/{ProductId}")] HttpRequestData req, string productId, FunctionContext executionContext)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            ProductDTO productDTO = JsonConvert.DeserializeObject<ProductDTO>(requestBody);
            HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(_productService.UpdateProductAsync(productDTO));
            return response;
        }

        [Function("DeleteProduct")]
        public async Task<HttpResponseData> DeleteUser([HttpTrigger(AuthorizationLevel.Anonymous, "Delete", Route = "products/{ProductId}")] HttpRequestData req, string productId, FunctionContext executionContext)
        {
            HttpResponseData response = req.CreateResponse();
            await _productService.DeleteUserAsync(productId);
            response.StatusCode = HttpStatusCode.Accepted;
            await response.WriteStringAsync("Project deleted successfully!", Encoding.UTF8);
            return response;
        }
    }
}
