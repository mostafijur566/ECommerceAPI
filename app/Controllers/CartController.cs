using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using app.Dtos.Cart;
using app.Helper;
using app.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace app.Controllers
{
    [ApiController]
    [Route("api/cart")]
    [Authorize] 
    public class CartController : ControllerBase
    {
        private readonly ICartRepository _cartRepo;

        public CartController(ICartRepository cartRepo)
        {
            _cartRepo = cartRepo;
        }

        // GET api/cart
        [HttpGet]
        public async Task<IActionResult> GetCart()
        {
            var userId = GetUserId();
            var cart = await _cartRepo.GetCartAsync(userId);
            return Ok(ApiResponse<CartResponseDto>.SuccessResponse(
                cart, "Cart retrieved successfully."));
        }

        // POST api/cart
        [HttpPost]
        public async Task<IActionResult> AddToCart([FromBody] AddToCartDto dto)
        {
            var userId = GetUserId();
            var cart = await _cartRepo.AddToCartAsync(userId, dto);
            return Ok(ApiResponse<CartResponseDto>.SuccessResponse(
                cart, "Item added to cart successfully."));
        }

        // PUT api/cart/{cartItemId}
        [HttpPut("{cartItemId}")]
        public async Task<IActionResult> UpdateCartItem(Guid cartItemId, [FromBody] UpdateCartItemDto dto)
        {
            var userId = GetUserId();
            var cart = await _cartRepo.UpdateCartItemAsync(userId, cartItemId, dto);
            return Ok(ApiResponse<CartResponseDto>.SuccessResponse(
                cart, "Cart item updated successfully."));
        }

        // DELETE api/cart/{cartItemId}
        [HttpDelete("{cartItemId}")]
        public async Task<IActionResult> RemoveFromCart(Guid cartItemId)
        {
            var userId = GetUserId();
            var cart = await _cartRepo.RemoveFromCartAsync(userId, cartItemId);
            return Ok(ApiResponse<CartResponseDto>.SuccessResponse(
                cart, "Item removed from cart successfully."));
        }

        // DELETE api/cart/clear
        [HttpDelete("clear")]
        public async Task<IActionResult> ClearCart()
        {
            var userId = GetUserId();
            await _cartRepo.ClearCartAsync(userId);
            return Ok(ApiResponse<string>.SuccessResponse(
                null!, "Cart cleared successfully."));
        }


        // Helper
        private Guid GetUserId()
        {
            var claim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.Parse(claim!);
        }
    }
}