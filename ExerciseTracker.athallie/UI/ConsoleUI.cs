using ExerciseTracker.athallie.Model;
using ExerciseTracker.athallie.Utils;
using Microsoft.IdentityModel.Tokens;
using Microsoft.VisualStudio.Web.CodeGenerators.Mvc.Templates.Blazor;
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

        public void ShowTitle()
        {
            string title = "Exercise Tracker";
            AnsiConsole.MarkupLine($"[bold yellow]{title}[/]");
        }

        public async void ExecuteAction(string action)
        {
            switch (action.ToLower().Split()[0])
            {
                case "all":
                    await ShowAll(true);
                    break;
                case "add":
                    Add();
                    break;
                case "edit":
                    Update();
                    break;
                case "delete":
                    Delete();
                    break;
            }
        }

        public void Run()
        {
            AnsiConsole.Clear();
            ShowTitle();
            string action;
            while(true)
            {
                action = ShowMenuAndGetChosenAction();
                if (!action.IsNullOrEmpty()) { break; }
            }
            ExecuteAction(action);
        }

        private async Task ShowAll(bool showBackPrompt)
        {
            List<Exercise> data = (List<Exercise>) await _httpUtils.GetExercises();
            var table = new Table();
            table.AddColumns(
                "ID", "Start Date", "End Date", "Duration", "Comments"    
            );
            data.ForEach(e =>
            {
                table.AddRow(
                    $"[green]{e.Id}[/]",
                    $"{e.DateStart.ToShortDateString()}",
                    $"{e.DateEnd.ToShortDateString()}",
                    $"{e.Duration.ToString()}",
                    $"{e.Comments}"
                );
            });
            AnsiConsole.Write(table);
            if (showBackPrompt)
            {
                GoBackOrStay(false);
            }
        }

        private async void Add()
        {
            string[] data = _userInput.GetAllInput();

            string response = await _httpUtils.AddExercise(
                DateTime.Parse(data[0]),
                DateTime.Parse(data[1]),
                TimeSpan.Parse(data[2]),
                data[3]
            );

            Console.WriteLine("\n" + response);
        }

        private void GoBackOrStay(bool repeatIfStay)
        {
            var prompt = new TextPrompt<bool>("Go Back?")
                .AddChoice(true)
                .AddChoice(false)
                .DefaultValue(true)
                .WithConverter(choice => choice ? "y" : "n");
            while (true)
            {
                var goBack = AnsiConsole.Prompt(prompt);
                if (goBack)
                {
                    Run();
                    break;
                }
                else if (repeatIfStay)
                {
                    break;
                }
            }
        }

        private async void Delete()
        {
            while (true)
            {
                await ShowAll(false);
                int id = _userInput.GetIDInput("ID of Item to be Deleted (enter nothing to quit):");
                if (id == -1)
                {
                    Run();
                    break;
                }

                var response = await _httpUtils.DeleteExercise(id);
                if (response == true)
                {
                    AnsiConsole.MarkupLine($"\n[green]Delete success![/]");
                }
                else
                {
                    AnsiConsole.MarkupLine($"\n[red]Delete unsuccesful, please try again.[/]");
                }

                AnsiConsole.WriteLine();

                var deleteMore = AnsiConsole.Prompt(
                    new TextPrompt<bool>("Delete another record?")
                    .AddChoice(true)
                    .AddChoice(false)
                    .WithConverter(choice => choice ? "y" : "n")
                );

                if (deleteMore == false)
                {
                    Run();
                    break;
                } else { AnsiConsole.WriteLine(); continue; }
            }
        }

        private async void Update()
        {
            int id = _userInput.GetIDInput("ID of Item to be Updated:");
            string[] newData = _userInput.GetAllInput();
            var response = await _httpUtils.UpdateExercise(id, new Exercise()
            {
                Id = id,
                DateStart = DateTime.Parse(newData[0]),
                DateEnd = DateTime.Parse(newData[1]),
                Duration = TimeSpan.Parse(newData[2]),
                Comments = newData[3]
            });
            Console.WriteLine("\n" + response);
        }
    }
}
