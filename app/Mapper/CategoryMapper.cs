using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using app.Dtos.Category;
using app.Models;

namespace app.Mapper
{
    public static class CategoryMapper
    {
        public static CategoryResponseDto ToCategoryResponseDto(this Category category)
        {
            return new CategoryResponseDto
            {
                Id = category.Id,
                Name = category.Name,
                Description = category.Description
            };
        }
    }
}