using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Azure.Functions.Worker;
using Microsoft.Azure.Functions.Worker.Http;
using Microsoft.Extensions.Logging;
using Infrastructure.Service;
using System.IO;
using Domain.DTO;
using Newtonsoft.Json;
using System.Net;

namespace WidgetAndCoAPI
{
    public class OrderHttpTrigger
    {
        private ILogger Logger { get; }

        private readonly IOrderService _orderService;

        public OrderHttpTrigger( ILogger<OrderHttpTrigger> Logger, IOrderService orderService)
        {
            this.Logger = Logger;
            _orderService = orderService;
        }

        [Function("GetAllOrder")]
        public async Task<HttpResponseData> GetAllOrders([HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "orders")] HttpRequestData req, FunctionContext executionContext)
        {
            HttpResponseData response = req.CreateResponse(System.Net.HttpStatusCode.OK);
            await response.WriteAsJsonAsync(_orderService.GetAllOrdersAsync());
            return response;
        }
        [Function("GetByOrderId")]
        public async Task<HttpResponseData> GetAOrderById([HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "orders/{OrderId}")] HttpRequestData req, string OrderId, FunctionContext executionContext)
        {
            HttpResponseData response = req.CreateResponse(System.Net.HttpStatusCode.OK);
            await response.WriteAsJsonAsync(_orderService.GetOrderByIdAsync(OrderId));
            return response;
        }

        [Function("AddOrder")]
        public async Task<HttpResponseData> AddOrder([HttpTrigger(AuthorizationLevel.Anonymous, "Post", Route = "orders")] HttpRequestData req, FunctionContext executionContext)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            OrderDTO orderDTO = JsonConvert.DeserializeObject<OrderDTO>(requestBody);
            HttpResponseData response = req.CreateResponse(System.Net.HttpStatusCode.OK);
            await response.WriteAsJsonAsync(_orderService.AddOrderAsync(orderDTO));
            return response;
        }


        [Function("UpdateOrder")]
        public async Task<HttpResponseData> UpdateOrder([HttpTrigger(AuthorizationLevel.Anonymous, "Put", Route = "orders/{OrderId}")] HttpRequestData req, string orderId, FunctionContext executionContext)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            OrderDTO orderDTO = JsonConvert.DeserializeObject<OrderDTO>(requestBody);
            HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(_orderService.UpdateOrderAsync(orderDTO));
            return response;
        }

        [Function("DeleteOrder")]
        public async Task<HttpResponseData> DeleteOrder([HttpTrigger(AuthorizationLevel.Anonymous, "Delete", Route = "orders/{OrderId}")] HttpRequestData req, string orderId, FunctionContext executionContext)
        {
            HttpResponseData response = req.CreateResponse();
            await _orderService.DeleteOrderAsync(orderId);
            response.StatusCode = HttpStatusCode.Accepted;
            await response.WriteStringAsync("Project deleted successfully!", Encoding.UTF8);
            return response;
        }

        [Function("UpdateShippingTime")]
        public async Task<HttpResponseData> UpdateOrdersShippingDate([HttpTrigger(AuthorizationLevel.Anonymous, "Put", Route = "orders/updateshipping/{OrderId}")] HttpRequestData req, string orderId, FunctionContext executionContext)
        {
            HttpResponseData response = req.CreateResponse(HttpStatusCode.OK);
            await response.WriteAsJsonAsync(_orderService.UpdateShippingDate(orderId));
            return response;
        }
    }
}
