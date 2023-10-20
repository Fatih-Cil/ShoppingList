using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using ShoppingList.Application.Abstractions.IServices;
using ShoppingList.Application.ViewModels.ListViewModel;
using ShoppingList.Application.ViewModels.ProductViewModel;
using ShoppingList.Domain.Entities;

namespace ShoppingList.WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ListsController : ControllerBase
    {
        IListService _listService;
        IUserService _userService;

        public ListsController(IListService listService, IUserService userService)
        {
            _listService = listService;
            _userService = userService;
        }

        [HttpGet]
        public IActionResult GetAllByUser(int userId)
        {
           User user= _userService.GetById(userId);
            if (user == null) { return NotFound("Kullanıcı bulunamadı"); }

            List<List> result = _listService.GetAllListByUserId(userId);

            return (result.Count == 0)
                 ? NotFound("Liste bulunamadı")
                 : Ok(result);

        }

        [HttpGet("{id}")]
        public IActionResult GetById(int id)
        {

            List result = _listService.GetById(id);

            return (result == null)
                 ? NotFound("Liste bulunamadı")
                 : Ok(result);

        }

        [HttpDelete("{id}")]
        public IActionResult DeleteById(int id)
        {
            List list = _listService.GetById(id);
            if (list == null) return NotFound("Liste bulunamadı");

            return (_listService.Delete(list))
                ? NoContent()
                : StatusCode(StatusCodes.Status500InternalServerError);
        }

        [HttpPost]
        public IActionResult Add(AddListViewModel addListViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            List list = new List();
            list.Name = addListViewModel.Name;
            list.UserId = addListViewModel.UserId;
            list.Status = true;


            var result = _listService.Add(list);

            if (result.kod == 0) return NotFound(result.message);

            if (result.kod == 500) return StatusCode(StatusCodes.Status500InternalServerError);

            return CreatedAtAction(nameof(GetById), new { id = result.Item1.Id }, addListViewModel);
        }

        [HttpPut("{id}")]
        public IActionResult Update(int id, UpdateListViewModel updateListViewModel)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }
            List list = _listService.GetById(id);
            if (list == null) return NotFound("Liste bulunamadı");

            list.Id = id;
            list.UserId=updateListViewModel.UserId;
            list.Name = updateListViewModel.Name;
            list.Status = updateListViewModel.Status;

            var result = _listService.Update(list);

            if (result.kod == 0) return NotFound(result.message);

            if (result.kod == 500) return StatusCode(StatusCodes.Status500InternalServerError);

            return Ok();
        }
    }
}
