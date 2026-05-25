using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using app.Dtos.Order;

namespace app.Interfaces
{
    public interface IOrderRepository
    {
        Task<OrderResponseDto> PlaceOrderAsync(Guid userId);
        Task<List<OrderResponseDto>> GetUserOrdersAsync(Guid userId);
        Task<OrderResponseDto?> GetOrderByIdAsync(Guid orderId, Guid userId);
        Task<List<OrderResponseDto>> GetAllOrdersAsync(); // Admin
        Task<OrderResponseDto?> UpdateOrderStatusAsync(Guid orderId, UpdateOrderStatusDto dto); // Admin
        Task<bool> CancelOrderAsync(Guid orderId, Guid userId);
    }
}