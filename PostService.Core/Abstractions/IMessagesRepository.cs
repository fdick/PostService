using PostService.Core.Models;

namespace PostService.Core.Abstractions
{
    public interface IMessagesRepository
    {
        Task<Guid> Create(Message msg);
        Task<Guid> Delete(Guid id);
        Task<List<Message>> GetAll();
        Task<Guid> Update(Guid id, string msg, int likesQuantity, int dislikesQuantity);
    }
}