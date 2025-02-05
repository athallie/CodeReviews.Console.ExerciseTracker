namespace ExerciseTracker.athallie.UI
{
    public interface IUserInput
    {
        string GetInput(string prompt, string columnName);
        bool ValidateInput(string prompt, string columnName, DateTime? startDate = null);
    }
}
