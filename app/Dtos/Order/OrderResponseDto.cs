using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace app.Dtos.Order
{
    public class OrderResponseDto
    {
        public Guid OrderId { get; set; }
        public string Status { get; set; } = string.Empty;
        public string ShippingAddress { get; set; } = string.Empty;
        public decimal TotalAmount { get; set; }
        public DateTime CreatedAt { get; set; }
        public List<OrderItemResponseDto> Items { get; set; } = new();
    }
}