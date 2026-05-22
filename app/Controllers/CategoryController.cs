using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using app.Dtos.Category;
using app.Helper;
using app.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace app.Controllers
{
    [Route("api/category")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly ICategoryRepository _categoryRepo;

        public CategoryController(ICategoryRepository categoryRepo)
        {
            _categoryRepo = categoryRepo;
        }

        // GET api/category
        [HttpGet]
        public async Task<IActionResult> GetAll()
        {
            var categories = await _categoryRepo.GetAllAsync();
            return Ok(ApiResponse<List<CategoryResponseDto>>.SuccessResponse(
                categories, "Categories retrieved successfully."));
        }

        // GET api/category/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var category = await _categoryRepo.GetByIdAsync(id);
            if (category == null)
                return NotFound(ApiResponse<string>.FailResponse("Category not found.", 404));

            return Ok(ApiResponse<CategoryResponseDto>.SuccessResponse(
                category, "Category retrieved successfully."));
        }

        // POST api/category
        [HttpPost]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Create([FromBody] CreateCategoryDto dto)
        {
            var category = await _categoryRepo.CreateAsync(dto);
            return CreatedAtAction(nameof(GetById), new { id = category.Id },
                ApiResponse<CategoryResponseDto>.SuccessResponse(
                    category, "Category created successfully.", 201));
        }

        // PUT api/category/{id}
        [HttpPut("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Update(Guid id, [FromBody] UpdateCategoryDto dto)
        {
            var category = await _categoryRepo.UpdateAsync(id, dto);
            if (category == null)
                return NotFound(ApiResponse<string>.FailResponse("Category not found.", 404));

            return Ok(ApiResponse<CategoryResponseDto>.SuccessResponse(
                category, "Category updated successfully."));
        }

        // DELETE api/category/{id}
        [HttpDelete("{id}")]
        [Authorize(Roles = "Admin")]
        public async Task<IActionResult> Delete(Guid id)
        {
            var result = await _categoryRepo.DeleteAsync(id);
            if (!result)
                return NotFound(ApiResponse<string>.FailResponse("Category not found.", 404));

            return Ok(ApiResponse<string>.SuccessResponse(
                null!, "Category deleted successfully."));
        }
    }
}