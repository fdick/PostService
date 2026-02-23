using PostService.Core.Models;

namespace PostService.Core.Abstractions
{
    public interface IMessagesRepository
    {
        Task<Guid> Create(Message msg);
        Task<Guid> Delete(Guid id);
        Task<List<(Message, string)>> GetAll();
        Task<List<(Message, string)>> GetAllInThread(Guid threadId);
        Task<(Message, string)> GetOne(Guid id);
        Task<Guid> Update(Guid id, string msg, int likesQuantity, int dislikesQuantity);
    }
}