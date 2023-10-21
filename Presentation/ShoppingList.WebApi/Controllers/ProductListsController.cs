using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.Mvc;
using ShoppingList.Application.Abstractions.IServices;
using ShoppingList.Application.ViewModels.ProductListViewModel;
using ShoppingList.Domain.Entities;

namespace ShoppingList.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ProductListsController : ControllerBase
    {
        IProductListService _productListService;

        public ProductListsController(IProductListService productListService)
        {
            _productListService = productListService;
        }

        [HttpGet("{id}")]
        public IActionResult Get(int id)
        {
            ProductList result = _productListService.GetById(id);

            return (result == null)
                 ? NotFound("Kayıt bulunamadı")
                 : Ok(result);

        }

        [HttpGet]
        public IActionResult GetAllByListId(int listid)
        {

            List<ProductList> result = _productListService.GetAllProductListByListId(listid);
            return (result.Count == 0)
                ? NotFound("Kayıt bulunamadı")
                : Ok(result);

        }

        [HttpDelete("{id}")]
        public IActionResult Delete(int id)
        {

            ProductList productList = _productListService.GetById(id);
            if (productList == null) { return NotFound("Kayıt bulunamadı"); }

            return (_productListService.Delete(productList))
                ? NoContent()
                : StatusCode(StatusCodes.Status500InternalServerError);

        }

        [HttpPost]
        public IActionResult Add(AddProductListViewModel addProductListViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ProductList productList = new ProductList();
            productList.ProductId = addProductListViewModel.ProductId;
            productList.ListId = addProductListViewModel.ListId;
            productList.Description = addProductListViewModel.Description;
            productList.Status = true;



            var result = _productListService.Add(productList);

            if (result.kod == 0) return NotFound(result.message);

            if (result.kod == 500) return StatusCode(StatusCodes.Status500InternalServerError);

            return CreatedAtAction(nameof(Get), new { id = result.Item1.Id }, addProductListViewModel);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, UpdateProductListModel updateProductListModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            ProductList productList = _productListService.GetById(id);

            if (productList == null) return NotFound("Kayıt Bulunamadı");
            productList.Id = id;

            productList.Description = updateProductListModel.Description;


            var result = _productListService.Update(productList);


            if (result.kod == 500) return StatusCode(StatusCodes.Status500InternalServerError);

            return Ok();
        }
    }
}
