namespace ExerciseTracker.athallie.UI
{
    public interface IConsoleUI
    {
        void Run();
        string ShowMenuAndGetChosenAction();
        void ExecuteAction(string action);
        void ShowTitle();
    }
}
