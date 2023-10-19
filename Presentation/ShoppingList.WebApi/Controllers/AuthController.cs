using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using ShoppingList.Application.Abstractions.IServices;
using ShoppingList.Application.ViewModels.AuthViewModel;
using ShoppingList.Application.ViewModels.UserViewModel;
using ShoppingList.Domain.Entities;
using ShoppingList.WebApi.Models;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace ShoppingList.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {

        IAuthService _authService;
        IOptions<TokenOption> _tokenOption;
        public AuthController(IAuthService authService, IOptions<TokenOption> tokenOption)
        {
            _authService = authService;
            _tokenOption = tokenOption;
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

            if (result == null)
            {
                return NotFound();
            }
            else
            {
                string role;
                role= (result.IsAdmin == true) ? "Admin" : "User";
                //token üretiliyor
                List<Claim> claims=new List<Claim>();
                claims.Add(new Claim(ClaimTypes.Name,result.Name));
                claims.Add(new Claim(ClaimTypes.Email,result.Email));
                claims.Add(new Claim(ClaimTypes.NameIdentifier ,result.Email));
                claims.Add(new Claim(ClaimTypes.Role, role));



                JwtSecurityToken token = new JwtSecurityToken(
                    issuer:_tokenOption.Value.Issuer,
                    audience:_tokenOption.Value.Audience,
                    claims: claims,
                expires:DateTime.Now.AddDays(_tokenOption.Value.Expiration),
                    signingCredentials: new Microsoft.IdentityModel.Tokens.SigningCredentials(new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_tokenOption.Value.SecretKey)),SecurityAlgorithms.HmacSha256)

                    );
                JwtSecurityTokenHandler handler = new JwtSecurityTokenHandler();
                string userToken=handler.WriteToken(token);

              return  Ok(userToken);
            }
                 

        }


    }
}
