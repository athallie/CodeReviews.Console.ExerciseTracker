using ExerciseTracker.athallie.Model;
using ExerciseTracker.athallie.Repositories;

namespace ExerciseTracker.athallie.Services
{
    public class ExerciseService : IExerciseService
    {
        private readonly IExerciseRepository _exerciseRepository;
        public ExerciseService(IExerciseRepository exerciseRepository)
        {
            _exerciseRepository = exerciseRepository;
        }

        public async Task<Exercise> AddExerciseAsync(Exercise exercise)
        {
            return await _exerciseRepository.AddAsync(exercise);
        }

        public IQueryable<Exercise> GetAllExercises()
        {
            return _exerciseRepository.GetAll();
        }

        public async Task<Exercise> GetExerciseByIdAsync(int id)
        {
            return await _exerciseRepository.GetExerciseByIdAsync(id);
        }

        public async Task<List<Exercise>> GetExercisesByDurationAsync(TimeSpan duration)
        {
            return await _exerciseRepository.GetExercisesByDuration(duration);
        }

        public async Task<List<Exercise>> GetExercisesByEndDateAsync(DateTime endDate)
        {
            return await _exerciseRepository.GetExercisesByEndDate(endDate);
        }

        public async Task<List<Exercise>> GetExercisesByStartDateAsync(DateTime startDate)
        {
            return await _exerciseRepository.GetExercisesByStartDate(startDate);
        }

        public async Task<Exercise> UpdateExerciseAsync(Exercise exercise)
        {
            return await _exerciseRepository.UpdateAsync(exercise);
        }

        public async Task<bool> DeleteExerciseAsync(int id)
        {
            return await _exerciseRepository.DeleteExerciseById(id);
        }

        public bool ExerciseExists(int id)
        {
            return _exerciseRepository.GetExerciseByIdAsync(id) == null ? false : true;
        }
    }
}
