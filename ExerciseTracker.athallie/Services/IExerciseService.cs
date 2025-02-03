using ExerciseTracker.athallie.Model;

namespace ExerciseTracker.athallie.Services
{
    public interface IExerciseService
    {
        IQueryable<Exercise> GetAllExercises();
        Task<Exercise> GetExerciseByIdAsync(int id);
        Task<List<Exercise>> GetExercisesByStartDateAsync(DateTime startDate);
        Task<List<Exercise>> GetExercisesByEndDateAsync(DateTime endDate);
        Task<List<Exercise>> GetExercisesByDurationAsync(TimeSpan duration);
        Task<Exercise> AddExerciseAsync(Exercise exercise);
        Task<Exercise> UpdateExerciseAsync(Exercise exercise);
    }
}
