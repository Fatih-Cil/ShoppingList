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

            DashboardViewModel dashboardViewModel = new DashboardViewModel();

            var httpClient = new HttpClient();

            var responseProduct = await httpClient.GetAsync("https://localhost:44344/api/Products");
            if (responseProduct.StatusCode != System.Net.HttpStatusCode.NotFound)
            {
                var jsonStringProduct = await responseProduct.Content.ReadAsStringAsync();
                var productList = JsonConvert.DeserializeObject<List<ProductDetailDTOVM>>(jsonStringProduct);
                dashboardViewModel.ProductCount = productList.Count();
            }
            else dashboardViewModel.ProductCount = 0;

            var responseCategory = await httpClient.GetAsync("https://localhost:44344/api/Categories");
            if (responseCategory.StatusCode != System.Net.HttpStatusCode.NotFound)
            {
                var jsonStringCategory = await responseCategory.Content.ReadAsStringAsync();
                var categoryList = JsonConvert.DeserializeObject<List<Category>>(jsonStringCategory);
                dashboardViewModel.CategoryCount = categoryList.Count();
            }
            else dashboardViewModel.CategoryCount = 0;
            
            var responseUser = await httpClient.GetAsync("https://localhost:44344/api/Users");
            if (responseUser.StatusCode != System.Net.HttpStatusCode.NotFound)
            {
                var jsonStringUser = await responseUser.Content.ReadAsStringAsync();
                var userList = JsonConvert.DeserializeObject<List<Category>>(jsonStringUser);
                dashboardViewModel.UserCount = userList.Count();
            }
            else dashboardViewModel.UserCount = 0;
                    

            return View(dashboardViewModel);
        }
    }
}
