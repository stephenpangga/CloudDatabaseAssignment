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
            Order order = new Order();
            order.OrderId = Guid.NewGuid();
            order.ProductId = orderDTO.ProductId;
            order.UserId = orderDTO.UserId;
            order.OrderDate = DateTime.Now;
            //order.ShippingDate = DateTime.Now;
            order.PartitionKey = orderDTO.ProductId.ToString();

            return await _orderWriteRepository.AddAsync(order);
        }
        public async Task DeleteOrderAsync(string orderId)
        {
            Order order = await GetOrderByIdAsync(orderId);
            await _orderWriteRepository.Delete(order);
        }

        public async Task<IEnumerable<Order>> GetAllOrdersAsync()
        {
            return await _orderReadRepository.GetAll().ToListAsync();
        }

        public async Task<Order> GetOrderByIdAsync(string orderId)
        {
            Guid id = Guid.Parse(orderId);
            return await _orderReadRepository.GetAll().FirstOrDefaultAsync(o => o.OrderId == id);
        }

        public async Task<Order> UpdateOrderAsync(OrderDTO orderDTO)
        {
            Order updateOrder = new Order();
            return await _orderWriteRepository.Update(updateOrder);
        }
    }
}
