using Microsoft.AspNetCore.Mvc;
using PostService.API.Contracts;
using PostService.Core.Abstractions;
using PostService.Core.Models;

namespace PostService.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PostsController : ControllerBase
    {
        private readonly IPostsService _messagesService;

        public PostsController(IPostsService messagesService)
        {
            this._messagesService = messagesService;
        }

        [HttpGet]
        [Route("/allposts")]
        public async Task<ActionResult<List<PostsResponse>>> GetPosts()
        {
            var messages = await _messagesService.GetAllPostsAsync();

            var response = messages.Select(x => new PostsResponse(
                x.Item1.ID,
                x.Item1.ThreadID,
                x.Item1.UserID,
                x.Item1.Msg,
                x.Item1.LikeQuantity,
                x.Item1.DislikeQuantity,
                x.Item1.CreateTime,
                x.Item1.ParentMessageID
                ));

            return Ok(response);
        }

        [HttpPost]
        public async Task<ActionResult<Guid>> CreatePost([FromBody] PostsRequest request)
        {
            var (msg, error) = Post.Create(
                Guid.NewGuid(),
                request.threadId,
                request.userId,
                request.msg,
                request.likesQuantity,
                request.dislikeQuantity,
                DateTime.UtcNow,
                request.parentMsgId
                );



            if (!string.IsNullOrEmpty(error))
            {
                return BadRequest(error);
            }

            var msgID = await _messagesService.CreatePostAsync(msg);

            if (msgID == Guid.Empty)
            {
                return BadRequest("Stupid request\n" + request);
            }

            return Ok(msgID);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<Guid>> DeletePost(Guid id)
        {
            var guid = await _messagesService.DeletePostAsync(id);

            return Ok(guid);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<Guid>> UpdatePost(Guid id, [FromBody] PostsRequest request)
        {
            var guid = await _messagesService.UpdatePostAsync(id, request.msg, request.likesQuantity, request.dislikeQuantity);

            return Ok(guid);
        }
    }
}
