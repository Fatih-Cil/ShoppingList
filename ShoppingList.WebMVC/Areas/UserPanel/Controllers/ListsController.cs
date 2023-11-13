using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ShoppingList.WebMVC.Areas.AdminPanel.Models.ProductVM;
using System.IdentityModel.Tokens.Jwt;
using System.Net.Http.Headers;
using System.Security.Claims;

namespace ShoppingList.WebMVC.Areas.UserPanel.Controllers
{
    [Area("UserPanel")]
    public class ListsController : Controller
    {
        string responsejwt = "";
        private void AddJwtTokenToRequest(HttpClient httpClient, string token)
        {
            httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
        }

        private bool SessionCheck()
        {
            responsejwt = HttpContext.Session.GetString("SessionUser");
            if (string.IsNullOrEmpty(responsejwt))
            {
                return false;
            }
            var handler = new JwtSecurityTokenHandler();
            var token = handler.ReadJwtToken(responsejwt);
            var roleClaim = token.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role);
            if (roleClaim == null || roleClaim.Value != "User")
            {

                return false;

            }
            return true;

        }

        public async Task<IActionResult> Index(string name = null)
        {
            
            var httpClient = new HttpClient();

            AddJwtTokenToRequest(httpClient, responsejwt);

            var responseMessage = await httpClient.GetAsync("https://localhost:44344/api/Products");

            if (responseMessage.StatusCode == System.Net.HttpStatusCode.NotFound)
            {

                return RedirectToAction("Index", "Home");

            }
            var jsonString = await responseMessage.Content.ReadAsStringAsync();
            List<ProductDetailDTOVM> values = JsonConvert.DeserializeObject<List<ProductDetailDTOVM>>(jsonString);

            if (name!=null)
            {
                var resultData = values.Where(x => x.Name.ToLower().Contains(name.ToLower()));
                ViewBag.Name = name;
                return View(resultData.ToList());
            }
           return View(values);
           
            
        }

        
    }
}
