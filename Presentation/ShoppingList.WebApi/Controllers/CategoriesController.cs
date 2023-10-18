using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingList.Application.Abstractions.IServices;
using ShoppingList.Application.ViewModels.CategoryViewModel;
using ShoppingList.Domain.Entities;
using System.Reflection.Metadata.Ecma335;

namespace ShoppingList.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        ICategoryService _categoryService;
        IProductService _productService;

        public CategoriesController(ICategoryService categoryService, IProductService productService)
        {
            _categoryService = categoryService;
            _productService = productService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {

            List<Category> result = _categoryService.GetAll();

            return (result.Count == 0)
                 ? NotFound("Kategor bulunamadı")
                 : Ok(result);

        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {

            Category result = _categoryService.GetById(id);

            return (result == null)
                 ? NotFound("Kategori bulunamadı")
                 : Ok(result);

        }

        [HttpPost]
        public IActionResult Add(AddCategoryViewModel addCategoryViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            Category category = new Category();
            category.Name = addCategoryViewModel.Name;

            var result = _categoryService.Add(category);
            if (result.kod == 0) return NotFound(result.message);

            if (result.kod == 500) return StatusCode(StatusCodes.Status500InternalServerError);

            return CreatedAtAction(nameof(GetById), new { id = result.Item1.Id }, addCategoryViewModel);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteById(int id)
        {
            Category category = _categoryService.GetById(id);
            if (category == null) return NotFound("Kategori Bulunamadı");

           var result= _productService.GetProductByCategoryId(id);
            if (result.Count != 0) return NotFound("Bu kategoriye ait ürünler olduğu için silinemez.");
            
            return (_categoryService.Delete(category))
                ? NoContent() 
                : StatusCode(StatusCodes.Status500InternalServerError);

            
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, UpdateCategoryViewModel updateCategoryViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            Category category = _categoryService.GetById(id);
            if (category == null) return NotFound("Kategori Bulunamadı");

            category.Id = id;
            category.Name = updateCategoryViewModel.Name;

            var result = _categoryService.Update(category);
            if (result.kod == 0) return NotFound(result.message);

            if (result.kod == 500) return StatusCode(StatusCodes.Status500InternalServerError);

            return Ok();
        }
    }
}
