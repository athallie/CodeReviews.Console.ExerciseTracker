using Microsoft.EntityFrameworkCore;

namespace ExerciseTracker.athallie.Model
{
    public class Context<TEntity> : DbContext where TEntity : class
    {
        public DbSet<TEntity> Data { get; set; }

        public Context() { }
        public Context(DbContextOptions<Context<TEntity>> options) : base(options) { }
    }
}
