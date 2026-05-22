using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace app.Models
{
    public class Cart
    {
        public Guid Id { get; set; } = Guid.NewGuid();
        public DateTime UpdatedAt { get; set; } = DateTime.UtcNow;

        // Foreign Key
        public Guid UserId { get; set; }
        public User User { get; set; } = null!;

        // Navigation
        public ICollection<CartItem> CartItems { get; set; } = new List<CartItem>();
    }
}