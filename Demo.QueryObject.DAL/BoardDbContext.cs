using Microsoft.EntityFrameworkCore;

namespace Demo.QueryObject.DAL
{
    public class BoardDbContext : DbContext
    {
        public BoardDbContext(DbContextOptions<BoardDbContext> options) : base(options)
        {
        }

        public DbSet<Board> Boards { get; set; }
        public DbSet<Post> Posts { get; set; }
    }
}
