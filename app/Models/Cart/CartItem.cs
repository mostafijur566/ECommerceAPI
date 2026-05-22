using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace app.Models
{
    public class CartItem
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public int Quantity { get; set; }

        // Foreign Keys
        public Guid CartId { get; set; }
        public Cart Cart { get; set; } = null!;

        public Guid ProductId { get; set; }
        public Product Product { get; set; } = null!;
    }
}