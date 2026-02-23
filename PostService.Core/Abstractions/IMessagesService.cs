using PostService.Core.Models;

namespace PostService.Core.Abstractions
{
    public interface IMessagesService
    {
        Task<Guid> CreateMessage(Message msg);
        Task<Guid> DeleteMessage(Guid ID);
        Task<List<(Message, string)>> GetAllMessages();
        Task<List<(Message, string)>> GetMessagesInThreadAsync(Guid threadId);
        Task<Guid> UpdateMessage(Guid id, string msg, int likesQuantity, int dislikesQuantity);
    }
}