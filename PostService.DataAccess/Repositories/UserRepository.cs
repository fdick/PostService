using Microsoft.EntityFrameworkCore;
using PostService.Core.Abstractions;
using PostService.Core.Models;
using PostService.DataAccess.Entities;

namespace PostService.DataAccess.Repositories
{
    public class UserRepository : IUserRepository
    {
        private readonly PostServiceDbContext _context;

        public UserRepository(PostServiceDbContext context)
        {
            this._context = context;
        }

        public async Task<List<(User, string)>> GetAll()
        {
            var entities = await _context.Users.AsNoTracking().ToListAsync();

            return entities.Select(x => User.Create(x.ID, x.Nickname, x.Name, x.LastName, x.Email)).ToList();
        }

        public async Task<Guid> Create(User user)
        {
            var userEntity = new UserEntity()
            {
                ID = Guid.NewGuid(),
                Nickname = user.Nickname,
                Name = user.Name,
                LastName = user.LastName,
                Email = user.Email,
                Messages = new List<MessageEntity>(),
            };
            await _context.Users.AddAsync(userEntity);
            await _context.SaveChangesAsync();

            return userEntity.ID;
        }
        public async Task<Guid> Delete(Guid id)
        {
            await _context.Users.Where(x => x.ID == id).ExecuteDeleteAsync();
            return id;
        }

        public async Task<Guid> Update(Guid id, string name, string lastname, string nickname, string email)
        {
            await _context.Users
                .Where(x => x.ID == id)
                .ExecuteUpdateAsync(x => x
                    .SetProperty(z => z.Name, name)
                    .SetProperty(z => z.LastName, lastname)
                    .SetProperty(z => z.Nickname, nickname)
                    .SetProperty(z => z.Email, email)
            );

            return id;
        }

    }
}
