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
        [Range(1, int.MaxValue, ErrorMessage = "CartId must be greater than 0.")]
        public string ShippingAddress { get; set; } = string.Empty;
    }
}