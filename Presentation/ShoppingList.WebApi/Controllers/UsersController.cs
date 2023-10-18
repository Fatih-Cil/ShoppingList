using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingList.Application.Abstractions.IServices;

using ShoppingList.Application.ViewModels.UserViewModel;
using ShoppingList.Domain.Entities;

namespace ShoppingList.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class UsersController : ControllerBase
    {
        IUserService _userService;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            List<User> result = _userService.GetAll();

            return (result.Count == 0)
                 ? NotFound("Kullanıcı bulunamadı")
                 : Ok(result);
        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {

            User result = _userService.GetById(id);

            return (result == null)
                 ? NotFound("Kullanıcı bulunamadı")
                 : Ok(result);

        }

        [HttpPost]
        public IActionResult Add(AddUserViewModel addUserViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }


            User user = new User();
            user.Name = addUserViewModel.Name;
            user.Surname = addUserViewModel.Surname;
            user.Email = addUserViewModel.Email;
            user.Password = addUserViewModel.Password;
            user.Status = true;
            user.IsAdmin = false;



            var result = _userService.Add(user);
            if (result.kod == 0) return NotFound(result.message);

            if (result.kod == 500) return StatusCode(StatusCodes.Status500InternalServerError);

            return CreatedAtAction(nameof(GetById), new { id = result.Item1.Id }, addUserViewModel);
        }

        [HttpDelete("{id}")]
        public IActionResult DeleteById(int id)
        {
            User user = _userService.GetById(id);
            if (user == null) return NotFound("Kullanıcı bulunamadı");

            return (_userService.Delete(user))
                ? NoContent()
                : StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, UpdateUserViewModel updateUserViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            User user = _userService.GetById(id);
            if (user == null) return NotFound("Kullanıcı bulunamadı");
            

            user.Id = id;
            user.Name = updateUserViewModel.Name;
            user.Surname = updateUserViewModel.Surname;
            user.Email = updateUserViewModel.Email;
            user.Password = updateUserViewModel.Password;
            user.Status = updateUserViewModel.Status;

            var result = _userService.Update(user);
            if (result.kod == 0) return NotFound(result.message);

            if (result.kod == 500) return StatusCode(StatusCodes.Status500InternalServerError);

            return Ok();
        }
    }
}
