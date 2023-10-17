using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingList.Application.Abstractions.IServices;
using ShoppingList.Application.ViewModels.CategoryViewModel;
using ShoppingList.Domain.Entities;


namespace ShoppingList.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoriesController : ControllerBase
    {
        ICategoryService _categoryService;

        public CategoriesController(ICategoryService categoryService)
        {
            _categoryService = categoryService;

        }

        [HttpGet]
        public IActionResult GetAll()
        {

         List<Category> result=  _categoryService.GetAll();

           return (result.Count == 0)
                ? NotFound("Kategor bulunamadı") 
                : Ok(result);
           
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {

            Category result = _categoryService.GetById(id);

            return (result==null)
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
            category.Name=addCategoryViewModel.Name;

           var result= _categoryService.Add(category);
            if (result.kod==0) return NotFound(result.message);

            if (result.kod == 500) return StatusCode(StatusCodes.Status500InternalServerError);

            return CreatedAtAction(nameof(GetById), new { id = result.Item1.Id }, addCategoryViewModel);
        }
    }
}
