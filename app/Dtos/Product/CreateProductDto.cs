using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace app.Dtos.Product
{
    public class CreateProductDto
    {
        [Required]
        [MaxLength(100), RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Name can only contain letters, numbers, and spaces.")]
        [MinLength(2, ErrorMessage = "Name must be at least 2 characters long.")]
        public string Name { get; set; } = string.Empty;

        [MaxLength(500), RegularExpression(@"^[a-zA-Z0-9\s.,!?'-]*$", ErrorMessage = "Description can only contain letters, numbers, spaces, and basic punctuation.")]
        [MinLength(10, ErrorMessage = "Description must be at least 10 characters long.")]
        public string? Description { get; set; }
        [Required]
        [Range(0.01, double.MaxValue, ErrorMessage = "Price must be greater than 0.")]
        public decimal Price { get; set; }
        [Required]
        [Range(0, int.MaxValue, ErrorMessage = "Stock must be 0 or greater.")]
        public int Stock { get; set; }
        public IFormFile? Image { get; set; }
        [Required]
        public Guid CategoryId { get; set; }
    }
}