using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using ShoppingList.Application.ViewModels.AuthViewModel;
using ShoppingList.Application.ViewModels.UserViewModel;
using ShoppingList.Domain.Entities;
using ShoppingList.WebMVC.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Net;
using System.Security.Claims;
using System.Text;
using System.Text.Json.Nodes;
using System.Windows.Markup;

namespace ShoppingList.WebMVC.Controllers
{
    public class LoginController : Controller
    {


        public async Task<IActionResult> Index()
        {
            return View();


        }

        [HttpPost]
        public async Task<IActionResult> Index(LoginViewModel loginViewModel)
        {


            HttpClient client = new HttpClient();

            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://localhost:44344/api/Auth");
            // requestMessage.Headers.Add("Authorization", "key=AAAAG...:APA91bH7U...");

            var jsonAsStringContent = JsonConvert.SerializeObject(loginViewModel);
            requestMessage.Content = new StringContent(jsonAsStringContent, Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.SendAsync(requestMessage).GetAwaiter().GetResult();

            if (response.IsSuccessStatusCode)
            {
                string jwt = await response.Content.ReadAsStringAsync();

                HttpContext.Session.SetString("SessionUser", jwt);
               TempData["Token"] = jwt;
                var handler = new JwtSecurityTokenHandler();
                var token = handler.ReadJwtToken(jwt);
                var roleClaim = token.Claims.FirstOrDefault(claim => claim.Type == ClaimTypes.Role);
                if (roleClaim != null)
                {
                   
                        return RedirectToAction("Index", "Home", new { area = roleClaim.Value+"Panel" });
                    
                }

                TempData["Role"] = "Token role değerine ulaşılamadı";
                return View(loginViewModel);
                
            }



            var jsonString = await response.Content.ReadAsStringAsync();
            var validation = JsonConvert.DeserializeObject<ErrorResponse>(jsonString);

            if (validation.status == 404)
            {
                TempData["Error"] = "Giriş başarısız. Lütfen tekrar deneyiniz.";

                return View(loginViewModel);
            }
            foreach (var key in validation.errors.Keys)
            {
                foreach (var error in validation.errors[key])
                {
                    ModelState.AddModelError(key, error);
                }

            }

            return View(loginViewModel);

        }


        public async Task<IActionResult> Register()
        {
            return View();

        }

        [HttpPost]
        public async Task<IActionResult> Register(AddUserViewModel addUserViewModel)
        {

            HttpClient client = new HttpClient();

            HttpRequestMessage requestMessage = new HttpRequestMessage(HttpMethod.Post, "https://localhost:44344/api/Users");
            // requestMessage.Headers.Add("Authorization", "key=AAAAG...:APA91bH7U...");

            var jsonAsStringContent = JsonConvert.SerializeObject(addUserViewModel);
            requestMessage.Content = new StringContent(jsonAsStringContent, Encoding.UTF8, "application/json");

            HttpResponseMessage response = client.SendAsync(requestMessage).GetAwaiter().GetResult();


            if (response.IsSuccessStatusCode)
            {

                TempData["Message"] = "Kullanıcı başarılı şekilde kayıt edildi.";
                return RedirectToAction("Register", "Login");

            }


            var jsonString = await response.Content.ReadAsStringAsync();
            var validation = JsonConvert.DeserializeObject<ErrorResponse>(jsonString);


            if (validation.status == 404)
            {
                TempData["Error"] = "Bu mail adresi daha önce kullanılmış. Lütfen tekrar deneyiniz.";

                return View(addUserViewModel);
            }
            foreach (var key in validation.errors.Keys)
            {
                foreach (var error in validation.errors[key])
                {
                    ModelState.AddModelError(key, error);
                }

            }

            return View(addUserViewModel);


        }


    }
}
