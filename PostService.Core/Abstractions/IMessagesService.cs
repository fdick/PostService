using PostService.Core.Models;

namespace PostService.Application.Services
{
    public interface IMessagesService
    {
        Task<Guid> CreateMessage(Message msg);
        Task<Guid> DeleteMessage(Guid ID);
        Task<List<(Message, string)>> GetAllMessages();
        Task<Guid> UpdateMessage(Guid id, string msg, int likesQuantity, int dislikesQuantity);
    }
}