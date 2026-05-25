using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace app.Dtos.Order
{
    public class UpdateOrderStatusDto
    {
        [Required]
        [RegularExpression("Pending|Processing|Shipped|Delivered|Cancelled", ErrorMessage = "Status must be one of the following: Pending, Processing, Shipped, Delivered, Cancelled.")]
        public string Status { get; set; } = string.Empty;
        // Pending, Processing, Shipped, Delivered, Cancelled
    }
}