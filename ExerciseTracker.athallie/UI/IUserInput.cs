namespace ExerciseTracker.athallie.UI
{
    public interface IUserInput
    {
        string GetInput(string prompt, string columnName, DateTime? startDate = null);
        bool ValidateInput(string prompt, string columnName, DateTime? startDate = null);
    }
}
