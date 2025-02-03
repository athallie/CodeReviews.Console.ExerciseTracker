using ExerciseTracker.athallie.Model;

namespace ExerciseTracker.athallie.Repositories
{
    public interface IExerciseRepository : IRepository<Exercise>
    {
        Task<Exercise> GetExerciseByIdAsync(int id);
        Task<List<Exercise>> GetExercisesByStartDate(DateTime startDate);
        Task<List<Exercise>> GetExercisesByEndDate(DateTime endDate);
        Task<List<Exercise>> GetExercisesByDuration(TimeSpan duration);
    }
}
