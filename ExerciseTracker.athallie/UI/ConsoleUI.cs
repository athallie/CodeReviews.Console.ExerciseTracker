using ExerciseTracker.athallie.Model;
using ExerciseTracker.athallie.Utils;
using Microsoft.IdentityModel.Tokens;
using Spectre.Console;

namespace ExerciseTracker.athallie.UI
{
    public class ConsoleUI : IConsoleUI
    {
        private readonly HttpUtils _httpUtils;
        private readonly UserInput _userInput;
        public ConsoleUI(HttpUtils httpUtils, UserInput userInput) 
        { 
            _httpUtils = httpUtils;
            _userInput = userInput;
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
                    ShowAll();
                    break;
                case "add":
                    Add();
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

        private async void ShowAll()
        {
            List<Exercise> data = (List<Exercise>) await _httpUtils.GetExercises();
            GetTitle();
            data.ForEach(e => Console.WriteLine(e.DateStart));
        }

        private async void Add()
        {
            int columnAmount = 4;
            string[] data = new string[columnAmount];
            string[] prompts = { 
                """
                Date Format: MM/DD/YYYY
                Example: 12/01/2022 -> (1 December 2022)
                P.S.: The date also has to be valid for the month in that year!

                Start Date:
                """,
                """"
                Date Format: MM/DD/YYYY
                Example: 12/01/2022 -> (1 December 2022)
                P.S: The date also has to be valid for the month in that year!
                
                End Date:
                """",
                """
                Duration Format: HH:MM:SS
                Example: 01:30:25 (1 Hour, 30 Minutes, 25 Seconds)
                P.S.: Hour must be around 0 - 24, Minutes & Second must be around 0 - 60 

                Duration:
                """,
                "Comments (can be empty):"
            };
            string[] types = { 
                "DateStart",
                "DateEnd",
                "Duration",
                "Comments"
            };

            for (int i = 0; i < columnAmount; i++)
            {
                string input;
                if (i == 1)
                {
                    input = _userInput.GetInput(prompts[i], types[i], DateTime.Parse(data[0]));
                }
                else
                {
                    input = _userInput.GetInput(prompts[i], types[i]);
                }
                data[i] = input;
                Console.WriteLine();
            }

            string response = await _httpUtils.AddExercise(
                DateTime.Parse(data[0]),
                DateTime.Parse(data[1]),
                TimeSpan.Parse(data[2]),
                data[3]
            );

            Console.WriteLine("\n" + response);
        }
    }
}
