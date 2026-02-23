using PostService.Core.Models;

namespace PostService.Core.Abstractions
{
    public interface IMessagesService
    {
        Task<Guid> CreateMessageAsync(Message msg);
        Task<Guid> DeleteMessageAsync(Guid ID);
        Task<List<(Message, string)>> GetAllMessagesAsync();
        Task<List<(Message, string)>> GetMessagesInThreadAsync(Guid threadId);
        Task<Guid> UpdateMessageAsync(Guid id, string msg, int likesQuantity, int dislikesQuantity);
    }
}