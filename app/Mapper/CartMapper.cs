using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using app.Dtos.Cart;
using app.Models;

namespace app.Mapper
{
    public static class CartMapper
    {
        public static CartResponseDto ToCartDto(this Cart cart)
        {
            return new CartResponseDto
            {
                CartId = cart.Id,
                Items = cart.CartItems.Select(ci => new CartItemResponseDto
                {
                    CartItemId = ci.Id,
                    ProductId = ci.ProductId,
                    ProductName = ci.Product?.Name ?? string.Empty,
                    ImageUrl = ci.Product?.ImageUrl,
                    UnitPrice = ci.Product?.Price ?? 0,
                    Quantity = ci.Quantity,
                    SubTotal = (ci.Product?.Price ?? 0) * ci.Quantity
                }).ToList(),
                TotalAmount = cart.CartItems.Sum(ci => (ci.Product?.Price ?? 0) * ci.Quantity),
                TotalItems = cart.CartItems.Sum(ci => ci.Quantity)
            };
        }
    }
}