using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace app.Dtos.Order
{
    public class PlaceOrderDto
    {
        [Required]
        [StringLength(200, MinimumLength = 10, ErrorMessage = "Shipping address must be between 10 and 200 characters.")]
        public string ShippingAddress { get; set; } = string.Empty;
    }
}