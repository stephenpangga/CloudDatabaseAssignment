using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Infrastructure.Repository;
using Microsoft.EntityFrameworkCore;
using Domain.DTO;

namespace Infrastructure.Service
{
    public class OrderService : IOrderService
    {
        private readonly ICosmosReadRepository<Order> _orderReadRepository;
        private readonly ICosmosWriteRepository<Order> _orderWriteRepository;

        public OrderService(ICosmosReadRepository<Order> orderReadRepository, ICosmosWriteRepository<Order> orderWriteRepository)
        {
            _orderReadRepository = orderReadRepository;
            _orderWriteRepository = orderWriteRepository;
        }

        public async Task<Order> AddOrderAsync(OrderDTO orderDTO)
        {
            if(orderDTO.ProductId != Guid.Empty)
            {
                Order order = new Order();
                order.OrderId = Guid.NewGuid();
                order.ProductId = orderDTO.ProductId;
                order.UserId = orderDTO.UserId;
                order.OrderDate = DateTime.Now;
                order.ShippingDate = null; // set to null, but can be updated when updateShipping endpoint is called.
                order.PartitionKey = orderDTO.ProductId.ToString();

                return await _orderWriteRepository.AddAsync(order);
            }
            else
            {
                throw new Exception("Please enter a product ID");
            }
            
        }
        public async Task DeleteOrderAsync(string orderId)
        {
            if (!string.IsNullOrEmpty(orderId))
            {
                Order order = await GetOrderByIdAsync(orderId);
                await _orderWriteRepository.Delete(order);
            }
            else
            {
                throw new Exception("Order Id provided does not exist");
            }
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _orderReadRepository.GetAll().ToListAsync();
        }

        public async Task<Order> GetOrderByIdAsync(string orderId)
        {
            try
            {
                Guid id = Guid.Parse(orderId);
                var order = await _orderReadRepository.GetAll().FirstOrDefaultAsync(o => o.OrderId == id);
                
                if (order == null)
                {
                    throw new Exception("The order you are looking for does not exist");
                }
                return order;
            }
            catch
            {
                throw new Exception("Please provide a proper ID");
            }
        }

        public async Task<Order> UpdateOrderAsync(OrderDTO orderDTO, string orderId)
        {
            Order updateOrder = await GetOrderByIdAsync(orderId);
            updateOrder.ProductId = orderDTO.ProductId;
            updateOrder.UserId = orderDTO.UserId;
            updateOrder.OrderDate = orderDTO.OrderDate;
            updateOrder.ShippingDate = orderDTO.ShippingDate;
            return await _orderWriteRepository.Update(updateOrder);
        }

        public async Task<Order> UpdateShippingDate(string orderId)
        {
            Order orderTochangeShippingTime = await GetOrderByIdAsync(orderId);
            orderTochangeShippingTime.ShippingDate = DateTime.Today.AddDays(5);
            return await _orderWriteRepository.Update(orderTochangeShippingTime);
        }

    }
}
