using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using app.Dtos.Product;
using app.Helper;

namespace app.Interfaces
{
    public interface IProductRepository
    {
        Task<PagedResponse<ProductResponseDto>> GetAllAsync(ProductQueryParams queryParams); 
        Task<ProductResponseDto?> GetByIdAsync(Guid id);
        Task<List<ProductResponseDto>> GetByCategoryAsync(Guid categoryId);
        Task<ProductResponseDto> CreateAsync(CreateProductDto dto);
        Task<ProductResponseDto?> UpdateAsync(Guid id, UpdateProductDto dto);
        Task<bool> DeleteAsync(Guid id);
        Task<bool> ExistsAsync(Guid id);
    }
}