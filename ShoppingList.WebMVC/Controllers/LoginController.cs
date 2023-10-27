
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json;
using ShoppingList.Application.ViewModels.AuthViewModel;
using ShoppingList.Application.ViewModels.UserViewModel;
using ShoppingList.Domain.Entities;
using System.Net;
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
                string token = await response.Content.ReadAsStringAsync();
                TempData["Token"] = token;
                return RedirectToAction("Index", "Home");

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


        public class ErrorResponse
        {
            public string type { get; set; }
            public string title { get; set; }
            public int status { get; set; }
            public string traceId { get; set; }
            public Dictionary<string, List<string>> errors { get; set; }
        }

    }
}
