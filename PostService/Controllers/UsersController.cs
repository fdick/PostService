using Microsoft.AspNetCore.Mvc;
using PostService.API.Contracts;
using PostService.Core.Abstractions;

namespace PostService.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _service;

        public UsersController(IUserService service)
        {
            this._service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<UserResponse>>> GetAllUsers()
        {
            var users = await _service.GetAllUsersAsync();

            var respons = users.Select(x => new UserResponse(x.Item1.ID, x.Item1.Name, x.Item1.LastName, x.Item1.Nickname, x.Item1.Email)).ToList();

            return Ok(respons);
        }

        [HttpPost]
        public async Task<ActionResult<UserRequest>> Create([FromBody] UserRequest request)
        {
            var (user, error) = Core.Models.User.Create(
                Guid.NewGuid(),
                request.nickname,
                request.name,
                request.lastname,
                request.email
                );

            if (!string.IsNullOrEmpty(error))
            {
                return BadRequest(error);
            }

            var id = await _service.CreateUserAsync(user);

            if (id == Guid.Empty)
            {
                return BadRequest("Stupid request\n" + id);
            }

            return Ok(id);
        }
    }
}
