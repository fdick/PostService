using Microsoft.AspNetCore.Mvc;
using PostService.API.Contracts;
using PostService.Core.Abstractions;
using PostService.Core.Models;

namespace PostService.API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class MessagesController : ControllerBase
    {
        private readonly IMessagesService _messagesService;

        public MessagesController(IMessagesService messagesService)
        {
            this._messagesService = messagesService;
        }

        [HttpGet]
        public async Task<ActionResult<List<MessagesResponse>>> GetMessages()
        {
            var messages = await _messagesService.GetAllMessages();

            var response = messages.Select(x => new MessagesResponse(
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
        public async Task<ActionResult<Guid>> CreateMessage([FromBody] MessagesRequest request)
        {
            var (msg, error) = Message.Create(
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

            var msgID = await _messagesService.CreateMessage(msg);

            if (msgID == Guid.Empty)
            {
                return BadRequest("Stupid request\n" + request);
            }

            return Ok(msgID);
        }

        [HttpDelete("{id:guid}")]
        public async Task<ActionResult<Guid>> DeleteMessage(Guid id)
        {
            var guid = await _messagesService.DeleteMessage(id);

            return Ok(guid);
        }

        [HttpPut("{id:guid}")]
        public async Task<ActionResult<Guid>> UpdateMessage(Guid id, [FromBody] MessagesRequest request)
        {
            var guid = await _messagesService.UpdateMessage(id, request.msg, request.likesQuantity, request.dislikeQuantity);

            return Ok(guid);
        }
    }
}
