using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Domain;
using Domain.DTO;

namespace Infrastructure.Service
{
    public interface IOrderService
    {
        Task<IEnumerable<Order>> GetAllOrdersAsync();
        Task<Order> GetOrderByIdAsync(string orderId);
        Task<Order> AddOrderAsync(OrderDTO orderDTO);

        Task<Order> UpdateOrderAsync(OrderDTO orderDTO, string orderId);

        Task DeleteOrderAsync(string orderId);

        Task<Order> UpdateShippingDate(string orderId);

    }
}
