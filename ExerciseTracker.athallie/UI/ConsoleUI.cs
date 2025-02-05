using ExerciseTracker.athallie.Model;
using ExerciseTracker.athallie.Utils;
using Microsoft.IdentityModel.Tokens;
using Spectre.Console;

namespace ExerciseTracker.athallie.UI
{
    public class ConsoleUI : IConsoleUI
    {
        private readonly HttpUtils _httpUtils;
        public ConsoleUI(HttpUtils httpUtils) 
        { 
            _httpUtils = httpUtils;
        }
        public string ShowMenuAndGetChosenAction()
        {
            var action = AnsiConsole.Prompt(
                new SelectionPrompt<string>()
                .PageSize(4)
                .AddChoices(new[] {
                    "All Records",
                    "Add Record",
                    "Edit Record",
                    "Delete Record"
                })
            );
            return action;
        }

        public void GetTitle()
        {
            AnsiConsole.Clear();
            string title = "Exercise Tracker";
            AnsiConsole.MarkupLine($"[bold yellow]{title}[/]");
            AnsiConsole.MarkupLine("-".PadLeft(title.Length));
        }

        public async void ExecuteAction(string action)
        {
            switch (action.ToLower().Split()[0])
            {
                case "all":
                    List<Exercise> data = (List<Exercise>) await _httpUtils.GetExercises();
                    GetTitle();
                    data.ForEach(e => Console.WriteLine(e.DateStart));
                    break;
                case "add":
                    break;
                case "edit":
                    break;
                case "delete":
                    break;
            }
        }

        public void Run()
        {
            GetTitle();
            string action;
            while(true)
            {
                action = ShowMenuAndGetChosenAction();
                if (!action.IsNullOrEmpty()) { break; }
            }
            ExecuteAction(action);
        }
    }
}
