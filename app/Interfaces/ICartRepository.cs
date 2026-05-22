using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using app.Dtos.Cart;

namespace app.Interfaces
{
    public interface ICartRepository
    {
        Task<CartResponseDto> GetCartAsync(Guid userId);
        Task<CartResponseDto> AddToCartAsync(Guid userId, AddToCartDto dto);
        Task<CartResponseDto> UpdateCartItemAsync(Guid userId, Guid cartItemId, UpdateCartItemDto dto);
        Task<CartResponseDto> RemoveFromCartAsync(Guid userId, Guid cartItemId);
        Task<bool> ClearCartAsync(Guid userId);
    }
}