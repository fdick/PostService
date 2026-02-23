using Grpc.Core;
using PostService.API.Protos;
using PostService.Core.Abstractions;

namespace PostService.Application.Services
{
    public class GRPCMessagesService : API.Protos.GRPCMessagesService.GRPCMessagesServiceBase
    {
        private readonly IMessagesService _msgService;

        public GRPCMessagesService(IMessagesService msgService)
        {
            this._msgService = msgService;
        }
        public override async Task<MessageResponse> GetMessages(MessageRequest request, ServerCallContext context)
        {
            if (!Guid.TryParse(request.ThreadID, out var threadId))
            {
                throw new RpcException(new Status(StatusCode.InvalidArgument, "Invalid thread ID format"));
            }

            var messages = await _msgService.GetMessagesInThreadAsync(threadId);

            var response = new MessageResponse();
            response.Messages.AddRange(messages.Select(m => new API.Protos.Message() { 
                ThreadId = m.Item1.ThreadID.ToString(),
                UserId = m.Item1.UserID.ToString(),
                Content = m.Item1.Msg,
                DislikesQuantity = m.Item1.DislikeQuantity,
                LikesQuantity = m.Item1.LikeQuantity,
                CreatedAt = Google.Protobuf.WellKnownTypes.Timestamp.FromDateTime(DateTime.SpecifyKind(m.Item1.CreateTime, DateTimeKind.Utc)),
            }
            ));

            return response;

            //return Task.FromResult(new MessageResponse
            //{
            //    Content = "TEST RESPONSE MESSAGE",
            //});
        }
    }
}
