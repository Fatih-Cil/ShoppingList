using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingList.Application.Abstractions.IServices;
using ShoppingList.Application.ViewModels.AuthViewModel;
using ShoppingList.Application.ViewModels.UserViewModel;
using ShoppingList.Domain.Entities;

namespace ShoppingList.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        IAuthService _authService;
        public AuthController(IAuthService authService)
        {
            _authService = authService;
        }

        [HttpPost]
        public IActionResult Login(LoginViewModel loginViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            User user = new User();

            user.Email = loginViewModel.Email;
            user.Password = loginViewModel.Password;

            var result = _authService.LoginCheck(user);

            return (result == null)
                 ? NotFound(false)
                 : Ok(true);

        }


    }
}
