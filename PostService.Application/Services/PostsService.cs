using PostService.Core.Abstractions;
using PostService.Core.Models;

namespace PostService.Application.Services
{
    public class PostsService : IPostsService
    {
        private readonly IPostsRepository _msgRepository;

        public PostsService(IPostsRepository msgRepository)
        {
            this._msgRepository = msgRepository;
        }

        public async Task<List<(Post, string)>> GetAllPostsAsync()
        {
            return await _msgRepository.GetAll();
        }

        public async Task<List<(Post, string)>> GetPostsInThreadAsync(Guid threadId)
        {
            return await _msgRepository.GetAllInThread(threadId);

        }

        public async Task<Guid> CreatePostAsync(Post msg)
        {
            try
            {
                //check for existing parent message by provided ID
                if (msg.ParentMessageID != null)
                {
                    var m = await _msgRepository.GetOne(msg.ParentMessageID.Value);
                    if (m.Item1 == null)
                        throw new Exception("Provided ID doesn't exist!");

                }

                return await _msgRepository.Create(msg);
            }
            catch (Exception e)
            {
                return Guid.Empty;
            }
        }

        public async Task<Guid> UpdatePostAsync(Guid id, string msg, int likesQuantity, int dislikesQuantity)
        {
            return await _msgRepository.Update(id, msg, likesQuantity, dislikesQuantity);
        }

        public async Task<Guid> DeletePostAsync(Guid ID)
        {
            return await _msgRepository.Delete(ID);
        }

    }
}
