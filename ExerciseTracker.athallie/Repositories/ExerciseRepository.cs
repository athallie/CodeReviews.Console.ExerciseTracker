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

        public async Task<bool> DeleteExerciseById(int id)
        {
            try
            {
                using var transaction = await Context.Database.BeginTransactionAsync();
                await Context.Exercises.Where(e => e.Id == id).ExecuteDeleteAsync();
                await transaction.CommitAsync();
            } catch (Exception ex)
            {
                throw new Exception($"Couldn't delete element: {ex.Message}");
            }

            return await Context.Exercises.FindAsync(id) == null ? true : false;
        }
    }
}
