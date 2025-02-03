using Microsoft.EntityFrameworkCore;

namespace ExerciseTracker.athallie.Model
{
    public class ExerciseContext : DbContext
    {
        public DbSet<Exercise> Exercises { get; set; }

        public ExerciseContext() { }
        public ExerciseContext(DbContextOptions options) : base(options) { }
    }
}
