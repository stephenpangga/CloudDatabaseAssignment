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
using HttpMultipartParser;

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
            HttpResponseData response = req.CreateResponse();
            try
            {
                await response.WriteAsJsonAsync(_productService.GetAllProductsAsync());
                response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                await response.WriteStringAsync(e.Message, Encoding.UTF8);
            }
            return response;
        }
        [Function("GetByProductId")]
        public async Task<HttpResponseData> GetAProductById([HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "products/{ProductId}")] HttpRequestData req, string productId, FunctionContext executionContext)
        {
            HttpResponseData response = req.CreateResponse();
            try
            {
                await response.WriteAsJsonAsync(_productService.GetProductByIdAsync(productId));
                response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                await response.WriteStringAsync(e.Message, Encoding.UTF8);
            }
            return response;
        }

        [Function("AddProduct")]
        public async Task<HttpResponseData> AddProduct([HttpTrigger(AuthorizationLevel.Anonymous, "Post", Route = "products")] HttpRequestData req, FunctionContext executionContext)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            ProductDTO productDTO = JsonConvert.DeserializeObject<ProductDTO>(requestBody);
            HttpResponseData response = req.CreateResponse();
            try
            {
                await response.WriteAsJsonAsync(_productService.AddProductAsync(productDTO));
                response.StatusCode = HttpStatusCode.Created;
            }
            catch (Exception e)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                await response.WriteStringAsync(e.Message, Encoding.UTF8);
            }
            return response;
        }


        [Function("UpdateProduct")]
        public async Task<HttpResponseData> UpdateProduct([HttpTrigger(AuthorizationLevel.Anonymous, "Put", Route = "products/{ProductId}")] HttpRequestData req, string productId, FunctionContext executionContext)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            ProductDTO productDTO = JsonConvert.DeserializeObject<ProductDTO>(requestBody);
            HttpResponseData response = req.CreateResponse();
            try
            {
                await response.WriteAsJsonAsync(_productService.UpdateProductAsync(productDTO, productId));
                response.StatusCode = HttpStatusCode.Accepted;
            }
            catch (Exception e)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                await response.WriteStringAsync(e.Message, Encoding.UTF8);
            }
            return response;
        }

        [Function("DeleteProduct")]
        public async Task<HttpResponseData> DeleteUser([HttpTrigger(AuthorizationLevel.Anonymous, "Delete", Route = "products/{ProductId}")] HttpRequestData req, string productId, FunctionContext executionContext)
        {
            HttpResponseData response = req.CreateResponse();
            try
            {
                await _productService.DeleteUserAsync(productId);
                response.StatusCode = HttpStatusCode.Accepted;
                await response.WriteStringAsync("Product has been deleted successfully!", Encoding.UTF8);
            }
            catch (Exception e)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                await response.WriteStringAsync(e.Message, Encoding.UTF8);
            }
            return response;
        }

        [Function("UploadImage")]
        public async Task<HttpResponseData> UploadProductImage([HttpTrigger(AuthorizationLevel.Anonymous, "put", Route = "upload/{ProductId}")] HttpRequestData req, string productId, FunctionContext executionContext)
        {
            var parsedFormBody = MultipartFormDataParser.ParseAsync(req.Body);
            var Imagefile = parsedFormBody.Result.Files[0];
            HttpResponseData response = req.CreateResponse();
            try
            {
                await _productService.UploadProductImageAsync(productId, Imagefile);
                response.StatusCode = HttpStatusCode.Accepted;
                await response.WriteStringAsync("Product Image has been uploaded successfully!", Encoding.UTF8);
            }
            catch (Exception e)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                await response.WriteStringAsync(e.Message, Encoding.UTF8);
            }
            return response;
        }
    }
}
