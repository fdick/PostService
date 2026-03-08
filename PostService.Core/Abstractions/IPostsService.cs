using PostService.Core.Models;

namespace PostService.Core.Abstractions
{
    public interface IPostsService
    {
        Task<Guid> CreatePostAsync(Post msg);
        Task<Guid> DeletePostAsync(Guid ID);
        Task<List<(Post, string)>> GetAllPostsAsync();
        Task<List<(Post, string)>> GetPostsInThreadAsync(Guid threadId);
        Task<Guid> UpdatePostAsync(Guid id, string msg, int likesQuantity, int dislikesQuantity);
    }
}