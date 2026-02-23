using Grpc.Core;
using PostService.API.GRPC.Protos;
using PostService.Core.Abstractions;
using static PostService.API.GRPC.Protos.GRPCMessagesController;

namespace PostService.API.GRPC
{
    public class GRPCMessagesController : GRPCMessagesControllerBase
    {
        private readonly IMessagesService _msgService;

        public GRPCMessagesController(IMessagesService msgService)
        {
            this._msgService = msgService;
        }
        public override async Task<GRPCMessageResponse> GetMessages(GRPCMessageRequest request, ServerCallContext context)
        {
            if (!Guid.TryParse(request.ThreadID, out var threadId))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid thread ID format"));
            }

            var messages = await _msgService.GetMessagesInThreadAsync(threadId);

            var response = new GRPCMessageResponse();
            response.Messages.AddRange(messages.Select(m => new GRPCMessage()
            {
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
