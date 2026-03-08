using Microsoft.EntityFrameworkCore;
using PostService.Core.Abstractions;
using PostService.Core.Models;
using PostService.DataAccess.Entities;

namespace PostService.DataAccess.Repositories
{
    public class PostsRepository : IPostsRepository
    {
        private readonly PostServiceDbContext _context;

        public PostsRepository(PostServiceDbContext context)
        {
            this._context = context;
        }

        public async Task<List<(Post, string)>> GetAll()
        {
            var msgEntities = await _context.Posts.AsNoTracking().ToListAsync();

            var messages = msgEntities.Select(x => Post.Create(x.ID, x.ThreadID, x.UserID, x.Message, x.LikeQuantity, x.DislikeQuantity, x.CreateTime, x.ParentMessageID)).ToList();

            return messages;
        }

        public async Task<(Post, string)> GetOne(Guid Id)
        {
            var x = await _context.Posts.SingleOrDefaultAsync(x => x.ID == Id);

            if (x == null) 
                return (null, string.Empty);

            var msg = Post.Create(x.ID, x.ThreadID, x.UserID, x.Message, x.LikeQuantity, x.DislikeQuantity, x.CreateTime, x.ParentMessageID);

            return msg;
        }

        public async Task<List<(Post, string)>> GetAllInThread(Guid threadId)
        {
            var entities = await _context.Posts.AsNoTracking().Where(x => x.ThreadID == threadId).ToListAsync();

            var messages = entities.Select(x => Post.Create(x.ID, x.ThreadID, x.UserID, x.Message, x.LikeQuantity, x.DislikeQuantity, x.CreateTime, x.ParentMessageID)).ToList();

            return messages;
        }

        public async Task<Guid> Create(Post msg)
        {
            var entity = new PostEntity
            {
                ID = msg.ID,
                ThreadID = msg.ThreadID,
                Message = msg.Msg,
                LikeQuantity = msg.LikeQuantity,
                DislikeQuantity = msg.DislikeQuantity,
                CreateTime = msg.CreateTime,
                ParentMessageID = msg.ParentMessageID,
                UserID = msg.UserID,
            };

            await _context.Posts.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity.ID;
        }

        public async Task<Guid> Update(Guid id, string msg, int likesQuantity, int dislikesQuantity)
        {
            await _context.Posts
                .Where(x => x.ID == id)
                .ExecuteUpdateAsync(x => x
                    .SetProperty(r => r.Message, r => msg)
                    .SetProperty(r => r.LikeQuantity, r => likesQuantity)
                    .SetProperty(r => r.DislikeQuantity, r => dislikesQuantity)
                    );

            return id;
        }

        public async Task<Guid> Delete(Guid id)
        {
            await _context.Posts
                .Where(x => x.ID == id)
                .ExecuteDeleteAsync();

            return id;
        }


    }
}
