using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using app.Dtos.Order;
using app.Helper;
using app.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace app.Controllers
{
    [ApiController]
    [Route("api/order")]
    [Authorize]
    public class OrderController : ControllerBase
    {
        private readonly IOrderRepository _orderRepo;

        public OrderController(IOrderRepository orderRepo)
        {
            _orderRepo = orderRepo;
        }

        // POST api/order/place
        [HttpPost("place")]
        public async Task<IActionResult> PlaceOrder([FromBody] PlaceOrderDto dto)
        {
            try
            {
                var userId = GetUserId();
                var order = await _orderRepo.PlaceOrderAsync(userId);
                return Ok(ApiResponse<OrderResponseDto>.SuccessResponse(
                    order, "Order placed successfully."));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<string>.FailResponse(ex.Message));
            }
        }

        // GET api/order
        [HttpGet]
        public async Task<IActionResult> GetMyOrders()
        {
            var userId = GetUserId();
            var orders = await _orderRepo.GetUserOrdersAsync(userId);
            return Ok(ApiResponse<List<OrderResponseDto>>.SuccessResponse(
                orders, "Orders retrieved successfully."));
        }

        // GET api/order/{orderId}
        [HttpGet("{orderId}")]
        public async Task<IActionResult> GetOrderById(Guid orderId)
        {
            var userId = GetUserId();
            var order = await _orderRepo.GetOrderByIdAsync(orderId, userId);
            if (order == null)
                return NotFound(ApiResponse<string>.FailResponse("Order not found.", 404));

            return Ok(ApiResponse<OrderResponseDto>.SuccessResponse(
                order, "Order retrieved successfully."));
        }

        // DELETE api/order/{orderId}/cancel
        [HttpDelete("{orderId}/cancel")]
        public async Task<IActionResult> CancelOrder(Guid orderId)
        {
            try
            {
                var userId = GetUserId();
                var result = await _orderRepo.CancelOrderAsync(orderId, userId);
                if (!result)
                    return NotFound(ApiResponse<string>.FailResponse("Order not found.", 404));

                return Ok(ApiResponse<string>.SuccessResponse(
                    null!, "Order cancelled successfully."));
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(ApiResponse<string>.FailResponse(ex.Message));
            }
        }

        // GET api/order/admin/all
        [HttpGet("admin/all")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> GetAllOrders()
        {
            var orders = await _orderRepo.GetAllOrdersAsync();
            return Ok(ApiResponse<List<OrderResponseDto>>.SuccessResponse(
                orders, "All orders retrieved successfully."));
        }

        // PUT api/order/admin/{orderId}/status
        [HttpPut("admin/{orderId}/status")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> UpdateOrderStatus(Guid orderId, [FromBody] UpdateOrderStatusDto dto)
        {
            var validStatuses = new[] { "Pending", "Processing", "Shipped", "Delivered", "Cancelled" };
            if (!validStatuses.Contains(dto.Status))
                return BadRequest(ApiResponse<string>.FailResponse(
                    "Invalid status. Valid values: Pending, Processing, Shipped, Delivered, Cancelled"));

            var order = await _orderRepo.UpdateOrderStatusAsync(orderId, dto);
            if (order == null)
                return NotFound(ApiResponse<string>.FailResponse("Order not found.", 404));

            return Ok(ApiResponse<OrderResponseDto>.SuccessResponse(
                order, "Order status updated successfully."));
        }

        // ── Helper ────────────────────────────────────────────
        private Guid GetUserId()
        {
            var claim = User.FindFirstValue(ClaimTypes.NameIdentifier);
            return Guid.Parse(claim!);
        }
    }
}