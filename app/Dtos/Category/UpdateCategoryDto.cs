using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace app.Dtos.Category
{
    public class UpdateCategoryDto
    {
        [Required]
        [StringLength(100, MinimumLength = 2), RegularExpression(@"^[a-zA-Z0-9\s]+$", ErrorMessage = "Name can only contain letters, numbers and spaces.")]
        public string Name { get; set; } = string.Empty;
        [StringLength(500, ErrorMessage = "Description cannot exceed 500 characters."), RegularExpression(@"^[a-zA-Z0-9\s.,!?'-]*$", ErrorMessage = "Description can only contain letters, numbers, spaces and basic punctuation.")]
        public string? Description { get; set; }
    }
}