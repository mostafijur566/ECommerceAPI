using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using app.Data;
using app.Dtos.Order;
using app.Interfaces;
using app.Mapper;
using app.Models;
using Microsoft.EntityFrameworkCore;

namespace app.Repository
{
    public class OrderRepository : IOrderRepository
    {
        private readonly ApplicationDbContext _context;

        public OrderRepository(ApplicationDbContext context)
        {
            _context = context;
        }

        public async Task<OrderResponseDto> PlaceOrderAsync(Guid userId)
        {
            // Get user's cart with items
            var cart = await _context.Carts
                .Include(c => c.CartItems)
                    .ThenInclude(ci => ci.Product)
                .FirstOrDefaultAsync(c => c.UserId == userId);

            if (cart == null || !cart.CartItems.Any())
                throw new InvalidOperationException("Cart is empty.");

            // Check stock availability
            foreach (var item in cart.CartItems)
            {
                if (item.Product.Stock < item.Quantity)
                    throw new InvalidOperationException(
                        $"Insufficient stock for product: {item.Product.Name}");
            }

            // Create order
            var order = new Order
            {
                Id = Guid.NewGuid(),
                UserId = userId,
                Status = "Pending",
                CreatedAt = DateTime.UtcNow,
                TotalAmount = cart.CartItems.Sum(ci => ci.Product.Price * ci.Quantity),
                OrderItems = cart.CartItems.Select(ci => new OrderItem
                {
                    Id = Guid.NewGuid(),
                    ProductId = ci.ProductId,
                    Quantity = ci.Quantity,
                    UnitPrice = ci.Product.Price // snapshot price at time of order
                }).ToList()
            };

            // Deduct stock
            foreach (var item in cart.CartItems)
            {
                item.Product.Stock -= item.Quantity;
            }

            // Save order & clear cart
            _context.Orders.Add(order);
            _context.CartItems.RemoveRange(cart.CartItems);
            await _context.SaveChangesAsync();

            // Reload with product details
            return await GetOrderByIdInternalAsync(order.Id);
        }

        public async Task<List<OrderResponseDto>> GetUserOrdersAsync(Guid userId)
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .Where(o => o.UserId == userId)
                .OrderByDescending(o => o.CreatedAt)
                .Select(o => OrderMapper.ToOrderResponseDto(o))
                .ToListAsync();
        }

        public async Task<OrderResponseDto?> GetOrderByIdAsync(Guid orderId, Guid userId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);

            if (order == null) return null;
            return OrderMapper.ToOrderResponseDto(order);
        }

        public async Task<List<OrderResponseDto>> GetAllOrdersAsync()
        {
            return await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .OrderByDescending(o => o.CreatedAt)
                .Select(o => OrderMapper.ToOrderResponseDto(o))
                .ToListAsync();
        }

        public async Task<OrderResponseDto?> UpdateOrderStatusAsync(Guid orderId, UpdateOrderStatusDto dto)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null) return null;

            order.Status = dto.Status;
            await _context.SaveChangesAsync();
            return OrderMapper.ToOrderResponseDto(order);
        }

        public async Task<bool> CancelOrderAsync(Guid orderId, Guid userId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == orderId && o.UserId == userId);

            if (order == null) return false;

            if (order.Status != "Pending")
                throw new InvalidOperationException("Only pending orders can be cancelled.");

            // Restore stock
            foreach (var item in order.OrderItems)
            {
                item.Product.Stock += item.Quantity;
            }

            order.Status = "Cancelled";
            await _context.SaveChangesAsync();
            return true;
        }

        // ── Helpers ───────────────────────────────────────────

        private async Task<OrderResponseDto> GetOrderByIdInternalAsync(Guid orderId)
        {
            var order = await _context.Orders
                .Include(o => o.OrderItems)
                    .ThenInclude(oi => oi.Product)
                .FirstOrDefaultAsync(o => o.Id == orderId);

            if (order == null) throw new InvalidOperationException("Order not found.");

            return OrderMapper.ToOrderResponseDto(order!);
        }

    }
}