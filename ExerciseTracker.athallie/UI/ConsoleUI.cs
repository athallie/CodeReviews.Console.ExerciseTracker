using ExerciseTracker.athallie.Model;
using ExerciseTracker.athallie.Utils;
using Microsoft.Identity.Client;
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
            AnsiConsole.MarkupLine($"[bold yellow]Exercise Tracker[/]");
            AnsiConsole.WriteLine();
        }

        public async void ExecuteAction(string action)
        {
            switch (action.ToLower().Split()[0])
            {
                case "all":
                    await ShowAllRecords(true);
                    break;
                case "add":
                    AddRecord();
                    break;
                case "edit":
                    UpdateRecord();
                    break;
                case "delete":
                    DeleteRecord();
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

        private async Task ShowAllRecords(bool showBackPrompt)
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
            AnsiConsole.WriteLine();
            if (showBackPrompt)
            {
                while (true)
                {
                    var goBack = AskNextAction("Go back?");
                    if (goBack)
                    {
                        Run();
                        break;
                    }
                }
            }
        }

        private async void AddRecord()
        {
            while (true)
            {
                string[] data = _userInput.GetAllInput();

                if (data[0].Equals("quit"))
                {
                    Run();
                    break;
                }

                bool response = await _httpUtils.AddExercise(
                    DateTime.Parse(data[0]),
                    DateTime.Parse(data[1]),
                    TimeSpan.Parse(data[2]),
                    data[3]
                );

                ShowResponse(
                    response, "Record added!",
                    "Error adding record. Please try again。"
                );

                AnsiConsole.WriteLine();

                bool addMore = AskNextAction("Add another record?");

                if (addMore == false)
                {
                    Run();
                    break;
                } else
                {
                    AnsiConsole.WriteLine();
                }
            }
        }

        private async void DeleteRecord()
        {
            while (true)
            {
                await ShowAllRecords(false);
                int id = _userInput.GetIDInput("ID of Item to be Deleted (enter nothing to quit):");
                if (id == -1)
                {
                    Run();
                    break;
                }

                var response = await _httpUtils.DeleteExercise(id);

                ShowResponse(
                    response, "Delete success!",
                    "Delete failed, please try again."
                );

                AnsiConsole.WriteLine();

                var deleteMore = AskNextAction("Delete another record?");

                if (deleteMore == false)
                {
                    Run();
                    break;
                } else { AnsiConsole.WriteLine(); continue; }
            }
        }

        private async void UpdateRecord()
        {
            while(true)
            {
                await ShowAllRecords(false);
                int id = _userInput.GetIDInput("ID of Item to be Updated (enter nothing to quit):");

                if (id == -1) { Run(); break; }

                string[] newData = _userInput.GetAllInput();

                if (newData[0].Equals("quit")) { Run(); break; }

                var response = await _httpUtils.UpdateExercise(id, new Exercise()
                {
                    Id = id,
                    DateStart = DateTime.Parse(newData[0]),
                    DateEnd = DateTime.Parse(newData[1]),
                    Duration = TimeSpan.Parse(newData[2]),
                    Comments = newData[3]
                });

                ShowResponse(
                    response, "Record updated!",
                    "Error updating the record. Please try again."
                );

                var updateMore = AskNextAction("Update another record?");

                if (updateMore == false) { Run(); break; }
                else { AnsiConsole.WriteLine(); }
            }
        }

        private bool AskNextAction(string prompt)
        {
            return AnsiConsole.Prompt(
                new TextPrompt<bool>(prompt)
                .AddChoice(true)
                .AddChoice(false)
                .DefaultValue(true)
                .WithConverter(choice => choice ? "y" : "n")
            );
        }

        private void ShowResponse(bool response, string successMessage, string failedMessage)
        {
            if (response == true)
            {
                AnsiConsole.MarkupLine($"[green]{successMessage}[/]");
            }
            else
            {
                AnsiConsole.MarkupLine($"[red]{failedMessage}[/]");
            }
            AnsiConsole.WriteLine();
        }
    }
}
