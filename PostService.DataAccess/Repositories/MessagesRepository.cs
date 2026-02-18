using Microsoft.EntityFrameworkCore;
using PostService.Core.Abstractions;
using PostService.Core.Models;
using PostService.DataAccess.Entities;

namespace PostService.DataAccess.Repositories
{
    public class MessagesRepository : IMessagesRepository
    {
        private readonly PostServiceDbContext _context;

        public MessagesRepository(PostServiceDbContext context)
        {
            this._context = context;
        }

        public async Task<List<(Message, string)>> GetAll()
        {
            var msgEntities = await _context.Messages.AsNoTracking().ToListAsync();

            var messages = msgEntities.Select(x => Message.Create(x.ID, x.ThreadID, x.Message, x.LikeQuantity, x.DislikeQuantity, x.CreateTime, x.SubMessagesIDs)).ToList();

            return messages;
        }

        public async Task<Guid> Create(Message msg)
        {
            var entity = new MessageEntity
            {
                ID = msg.ID,
                ThreadID = msg.ThreadID,
                Message = msg.Msg,
                LikeQuantity = msg.LikeQuantity,
                DislikeQuantity = msg.DislikeQuantity,
                CreateTime = msg.CreateTime,
                SubMessagesIDs = msg.SubMessagesIDs
            };

            await _context.Messages.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity.ID;
        }

        public async Task<Guid> Update(Guid id, string msg, int likesQuantity, int dislikesQuantity)
        {
            await _context.Messages
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
            await _context.Messages
                .Where(x => x.ID == id)
                .ExecuteDeleteAsync();

            return id;
        }
    }
}
