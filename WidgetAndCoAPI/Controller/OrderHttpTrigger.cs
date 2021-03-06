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
            HttpResponseData response = req.CreateResponse();
            try
            {
                await response.WriteAsJsonAsync(_orderService.GetAllOrdersAsync());
                response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                await response.WriteStringAsync(e.Message, Encoding.UTF8);
            }
            return response;
        }

        [Function("GetByOrderId")]
        public async Task<HttpResponseData> GetAOrderById([HttpTrigger(AuthorizationLevel.Anonymous, "GET", Route = "orders/{OrderId}")] HttpRequestData req, string OrderId, FunctionContext executionContext)
        {
            HttpResponseData response = req.CreateResponse();
            try
            {
                await response.WriteAsJsonAsync(_orderService.GetOrderByIdAsync(OrderId));
                response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                await response.WriteStringAsync(e.Message, Encoding.UTF8);
            }
            return response;
        }

        [Function("AddOrder")]
        public async Task<HttpResponseData> AddOrder([HttpTrigger(AuthorizationLevel.Anonymous, "Post", Route = "orders")] HttpRequestData req, FunctionContext executionContext)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            OrderDTO orderDTO = JsonConvert.DeserializeObject<OrderDTO>(requestBody);
            HttpResponseData response = req.CreateResponse();
            try
            {
                await response.WriteAsJsonAsync(_orderService.AddOrderAsync(orderDTO));
                response.StatusCode = HttpStatusCode.Created;
            }
            catch (Exception e)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                await response.WriteStringAsync(e.Message, Encoding.UTF8);
            }
            return response;
        }

        [Function("UpdateOrder")]
        public async Task<HttpResponseData> UpdateOrder([HttpTrigger(AuthorizationLevel.Anonymous, "Put", Route = "orders/{OrderId}")] HttpRequestData req, string orderId, FunctionContext executionContext)
        {
            string requestBody = await new StreamReader(req.Body).ReadToEndAsync();
            OrderDTO orderDTO = JsonConvert.DeserializeObject<OrderDTO>(requestBody);
            HttpResponseData response = req.CreateResponse();
            try
            {
                await response.WriteAsJsonAsync(_orderService.UpdateOrderAsync(orderDTO, orderId));
                response.StatusCode = HttpStatusCode.OK;
            }
            catch (Exception e)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                await response.WriteStringAsync(e.Message, Encoding.UTF8);
            }
            return response;
        }

        [Function("DeleteOrder")]
        public async Task<HttpResponseData> DeleteOrder([HttpTrigger(AuthorizationLevel.Anonymous, "Delete", Route = "orders/{OrderId}")] HttpRequestData req, string orderId, FunctionContext executionContext)
        {
            HttpResponseData response = req.CreateResponse();
            try
            {
                await _orderService.DeleteOrderAsync(orderId);
                await response.WriteStringAsync("Order has been deleted successfully!", Encoding.UTF8);
                response.StatusCode = HttpStatusCode.Accepted;
            }
            catch (Exception e)
            {
                response.StatusCode = HttpStatusCode.BadRequest;
                await response.WriteStringAsync(e.Message, Encoding.UTF8);
            }
            return response;
        }

        //this is only called when the product is going to be shipped
        //example when the barcode of the order is scanned the order id will be sent
        //and shipping time will be update to the current time when the endpoint is called
        [Function("UpdateShippingTime")]
        public async Task<HttpResponseData> UpdateOrdersShippingDate([HttpTrigger(AuthorizationLevel.Anonymous, "Put", Route = "orders/updateshipping/{OrderId}")] HttpRequestData req, string orderId, FunctionContext executionContext)
        {
            HttpResponseData response = req.CreateResponse();
            try
            {
                await response.WriteAsJsonAsync(_orderService.UpdateShippingDate(orderId));
                response.StatusCode = HttpStatusCode.OK;
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
