using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using app.Dtos.Product;
using app.Helper;
using app.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace app.Controllers
{
    [ApiController]
    [Route("api/product")]
    public class ProductController : ControllerBase
    {
        private readonly IProductRepository _productRepo;

        public ProductController(IProductRepository productRepo)
        {
            _productRepo = productRepo;
        }

        // GET api/product
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var products = await _productRepo.GetAllAsync();
            return Ok(ApiResponse<List<ProductResponseDto>>.SuccessResponse(
                products, "Products retrieved successfully."));
        }

        // GET api/product/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var product = await _productRepo.GetByIdAsync(id);
            if (product == null)
                return NotFound(ApiResponse<string>.FailResponse("Product not found.", 404));

            return Ok(ApiResponse<ProductResponseDto>.SuccessResponse(
                product, "Product retrieved successfully."));
        }

        // GET api/product/category/{categoryId}
        [HttpGet("category/{categoryId}")]
        public async Task<IActionResult> GetByCategory(Guid categoryId)
        {
            var products = await _productRepo.GetByCategoryAsync(categoryId);
            return Ok(ApiResponse<List<ProductResponseDto>>.SuccessResponse(
                products, "Products retrieved successfully."));
        }

        // POST api/product
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateProductDto dto)
        {
            var product = await _productRepo.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = product.Id },
                ApiResponse<ProductResponseDto>.SuccessResponse(
                    product, "Product created successfully.", 201));
        }

        // PUT api/product/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateProductDto dto)
        {
            var product = await _productRepo.UpdateAsync(id, dto);
            if (product == null)
                return NotFound(ApiResponse<string>.FailResponse("Product not found.", 404));

            return Ok(ApiResponse<ProductResponseDto>.SuccessResponse(
                product, "Product updated successfully."));
        }

        // DELETE api/product/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _productRepo.DeleteAsync(id);
            if (!result)
                return NotFound(ApiResponse<string>.FailResponse("Product not found.", 404));

            return Ok(ApiResponse<string>.SuccessResponse(
                null!, "Product deleted successfully."));
        }
    }
}