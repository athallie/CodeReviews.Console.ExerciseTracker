using ExerciseTracker.athallie.Model;
using Microsoft.EntityFrameworkCore;

namespace ExerciseTracker.athallie.Models
{
    public class ExerciseTrackerContext: DbContext
    {
        public DbSet<Exercise> Exercises { get; set; }
        public ExerciseTrackerContext() { }
        public ExerciseTrackerContext(DbContextOptions<ExerciseTrackerContext> options) : base(options) { }
        protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<Exercise>()
                .Property(e => e.Duration)
                .HasConversion<long>();
        }
    }
}
