using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using app.Dtos.Order;
using app.Models;

namespace app.Mapper
{
    public static class OrderMapper
    {
        public static OrderResponseDto ToOrderResponseDto(Order order)
         => new()
         {
             OrderId = order.Id,
             Status = order.Status,
             ShippingAddress = order.ShippingAddress ?? string.Empty,
             TotalAmount = order.TotalAmount,
             CreatedAt = order.CreatedAt,
             Items = order.OrderItems.Select(oi => new OrderItemResponseDto
             {
                 OrderItemId = oi.Id,
                 ProductId = oi.ProductId,
                 ProductName = oi.Product?.Name ?? string.Empty,
                 ImageUrl = oi.Product?.ImageUrl,
                 UnitPrice = oi.UnitPrice,
                 Quantity = oi.Quantity,
                 SubTotal = oi.UnitPrice * oi.Quantity
             }).ToList()
         };
    }
}