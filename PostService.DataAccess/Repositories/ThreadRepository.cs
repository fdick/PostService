using Microsoft.EntityFrameworkCore;
using PostService.Core.Abstractions;
using PostService.Core.Models;
using PostService.DataAccess.Entities;

namespace PostService.DataAccess.Repositories
{
    public class ThreadRepository : IThreadRepository
    {
        private readonly PostServiceDbContext _context;

        public ThreadRepository(PostServiceDbContext context)
        {
            this._context = context;
        }

        public async Task<List<(Core.Models.Thread, string)>> GetAll()
        {
            var entities = await _context.Threads.AsNoTracking().ToListAsync();

            return entities.Select(x => Core.Models.Thread.Create(x.ID, x.Name, x.AuthorID)).ToList();
        }

        public async Task<(Core.Models.Thread, string)> GetOne(Guid Id)
        {
            var x = await _context.Threads.SingleOrDefaultAsync(x => x.ID == Id);

            if (x == null)
                return (null, string.Empty);

            var thread = Core.Models.Thread.Create(x.ID, x.Name, x.AuthorID);

            return thread;
        }

        public async Task<Guid> Create(Core.Models.Thread thread)
        {
            var entity = new ThreadEntity()
            {
                ID = Guid.NewGuid(),
                Name = thread.Name,
                AuthorID = thread.AuthorID,
            };
            await _context.Threads.AddAsync(entity);
            await _context.SaveChangesAsync();

            return entity.ID;
        }
        public async Task<Guid> Delete(Guid id)
        {
            await _context.Threads.Where(x => x.ID == id).ExecuteDeleteAsync();
            return id;
        }

        public async Task<Guid> Update(Guid id, string name, Guid authorId)
        {
            await _context.Threads
                .Where(x => x.ID == id)
                .ExecuteUpdateAsync(x => x
                    .SetProperty(z => z.Name, name)
                    .SetProperty(z => z.AuthorID, authorId)
            );

            return id;
        }

    }
}
