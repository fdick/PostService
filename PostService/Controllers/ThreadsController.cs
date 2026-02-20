using Microsoft.AspNetCore.Mvc;
using PostService.API.Contracts;
using PostService.Core.Abstractions;

namespace PostService.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class ThreadsController : ControllerBase
    {
        private readonly IThreadService _service;

        public ThreadsController(IThreadService service)
        {
            this._service = service;
        }

        [HttpGet]
        public async Task<ActionResult<List<ThreadResponse>>> GetAllUsers()
        {
            var users = await _service.GetAllThreads();

            var respons = users.Select(x => new ThreadResponse(x.Item1.ID, x.Item1.Name, x.Item1.AuthorID)).ToList();

            return Ok(respons);
        }

        [HttpPost]
        public async Task<ActionResult<UserRequest>> CreateThread([FromBody] ThreadRequest request)
        {
            var (thread, error) = Core.Models.Thread.Create(
                Guid.NewGuid(),
                request.name,
                request.authorId
                );

            if (!string.IsNullOrEmpty(error))
            {
                return BadRequest(error);
            }

            var id = await _service.CreateThread(thread);

            if (id == Guid.Empty)
            {
                return BadRequest("Stupid request\n" + id);
            }

            return Ok(id);
        }
    }
}
