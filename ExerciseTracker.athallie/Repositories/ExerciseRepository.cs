using ExerciseTracker.athallie.Model;
using ExerciseTracker.athallie.Models;
using Microsoft.EntityFrameworkCore;

namespace ExerciseTracker.athallie.Repositories
{
    public class ExerciseRepository : Repository<Exercise>, IExerciseRepository
    {
        public ExerciseRepository(ExerciseTrackerContext context) : base(context)
        {
        }

        public async Task<List<Exercise>> GetExercisesByDuration(TimeSpan duration)
        {
            try
            {
                return await Context.Exercises.Where(e => e.Duration == duration).ToListAsync();
            } catch(Exception ex) 
            {
                throw new Exception($"Error: {ex.Message}");
            }
        }

        public async Task<List<Exercise>> GetExercisesByEndDate(DateTime endDate)
        {
            try
            {
                return await Context.Exercises.Where(e => e.DateEnd == endDate).ToListAsync();
            }
            catch(Exception ex)
            {
                throw new Exception($"Error: {ex.Message}");
            }
        }

        public async Task<Exercise> GetExerciseByIdAsync(int id)
        {
            try
            {
                return await Context.Exercises.FindAsync(id);
            } catch(Exception ex)
            {
                throw new Exception($"Error: {ex.Message}");
            }
        }

        public async Task<List<Exercise>> GetExercisesByStartDate(DateTime startDate)
        {
            try
            {
                return await Context.Exercises.Where(e => e.DateStart == startDate).ToListAsync();
            } catch(Exception ex)
            {
                throw new Exception($"Error: {ex.Message}");
            }
        }
    }
}
