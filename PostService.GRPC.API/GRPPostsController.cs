using Grpc.Core;
using Microsoft.AspNetCore.Authorization;
using PostService.API.GRPC.Protos;
using PostService.Core.Abstractions;
using static PostService.API.GRPC.Protos.GRPCPostsController;

namespace PostService.API.GRPC
{
    public class GRPPostsController : GRPCPostsControllerBase
    {
        private readonly IPostsService _msgService;

        public GRPPostsController(IPostsService msgService)
        {
            this._msgService = msgService;
        }

        [Authorize(Roles = "postservice-rw-role")]
        public override async Task<GRPCPostResponse> GetPosts(GRPCPostRequest request, ServerCallContext context)
        {
            if (!Guid.TryParse(request.ThreadID, out var threadId))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid thread ID format"));
            }

            var messages = await _msgService.GetPostsInThreadAsync(threadId);

            var response = new GRPCPostResponse();
            response.Posts.AddRange(messages.Select(m => new GRPCPost()
            {
                Id = m.Item1.ID.ToString(),
                ThreadId = m.Item1.ThreadID.ToString(),
                UserId = m.Item1.UserID.ToString(),
                Content = m.Item1.Msg,
                DislikesQuantity = m.Item1.DislikeQuantity,
                LikesQuantity = m.Item1.LikeQuantity,
                CreatedAt = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(DateTime.SpecifyKind(m.Item1.CreateTime, DateTimeKind.Utc)),
            }
            ));

            return response;
        }
    }
}
