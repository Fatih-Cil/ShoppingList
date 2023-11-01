using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Newtonsoft.Json;
using ShoppingList.Domain.Entities;
using ShoppingList.WebMVC.Areas.AdminPanel.Models.ProductVM;

namespace ShoppingList.WebMVC.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class ProductsController : Controller
    {
        [HttpGet]
        public async Task<IActionResult> Index()
        {

            var httpClient = new HttpClient();

            var responseMessage = await httpClient.GetAsync("https://localhost:44344/api/Products");
            var jsonString = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<ProductDetailDTOVM>>(jsonString);
            return View(values);
          
        }

        [HttpGet]
        public async Task<IActionResult> Add()
        {

            var httpClient = new HttpClient();

            var responseMessage = await httpClient.GetAsync("https://localhost:44344/api/Categories");
            var jsonString = await responseMessage.Content.ReadAsStringAsync();
            var values = JsonConvert.DeserializeObject<List<Category>>(jsonString);

           

            TempData["CategoryList"] = new SelectList(values, "Id", "Name"); 
            return View();

        }
    }
}
