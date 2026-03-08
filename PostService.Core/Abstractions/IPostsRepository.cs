using PostService.Core.Models;

namespace PostService.Core.Abstractions
{
    public interface IPostsRepository
    {
        Task<Guid> Create(Post msg);
        Task<Guid> Delete(Guid id);
        Task<List<(Post, string)>> GetAll();
        Task<List<(Post, string)>> GetAllInThread(Guid threadId);
        Task<(Post, string)> GetOne(Guid id);
        Task<Guid> Update(Guid id, string msg, int likesQuantity, int dislikesQuantity);
    }
}