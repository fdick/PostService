using Microsoft.EntityFrameworkCore;
using PostService.DataAccess.Entities;

namespace PostService.DataAccess
{
    public class PostServiceDbContext : DbContext
    {
        public PostServiceDbContext(DbContextOptions<PostServiceDbContext> options) : base(options)
        {
        }

        public DbSet<MessageEntity> Messages { get; set; }
    }
}
