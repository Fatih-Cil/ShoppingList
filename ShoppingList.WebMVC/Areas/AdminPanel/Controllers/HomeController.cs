using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ShoppingList.Domain.Entities;
using ShoppingList.WebMVC.Areas.AdminPanel.Models.DashboardVM;
using ShoppingList.WebMVC.Areas.AdminPanel.Models.ProductVM;

namespace ShoppingList.WebMVC.Areas.AdminPanel.Controllers
{
    [Area("AdminPanel")]
    public class HomeController : Controller
    {
        public async Task<IActionResult> Index()
        {

            var httpClient = new HttpClient();

            var responseProduct = await httpClient.GetAsync("https://localhost:44344/api/Products");
            var responseCategory = await httpClient.GetAsync("https://localhost:44344/api/Categories");
            var responseUser = await httpClient.GetAsync("https://localhost:44344/api/Users");


            var jsonStringProduct = await responseProduct.Content.ReadAsStringAsync();
            var productList = JsonConvert.DeserializeObject<List<ProductDetailDTOVM>>(jsonStringProduct);

            var jsonStringCategory = await responseCategory.Content.ReadAsStringAsync();
            var categoryList = JsonConvert.DeserializeObject<List<Category>>(jsonStringCategory);

            var jsonStringUser = await responseUser.Content.ReadAsStringAsync();
            var userList = JsonConvert.DeserializeObject<List<Category>>(jsonStringUser);

            DashboardViewModel dashboardViewModel = new DashboardViewModel();
            dashboardViewModel.ProductCount = productList.Count();
            dashboardViewModel.CategoryCount = categoryList.Count();
            dashboardViewModel.UserCount = userList.Count();

            return View(dashboardViewModel);
        }
    }
}
