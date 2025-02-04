using System.Text.RegularExpressions;
using Spectre.Console;

namespace ExerciseTracker.athallie.UI
{
    public class UserInput : IUserInput
    {
        public UserInput() { }
        public string GetInput(string prompt, string columnName)
        {
            var input = AnsiConsole.Prompt(
                new TextPrompt<string>(prompt)
            );
            return input;
        }

        public bool ValidateInput(string input, string columnName, DateTime? startDate = null)
        {
            switch (columnName.ToLower())
            {
                case "datestart":
                    //Format: MM/dd/YYYY
                    var isValid = DateTime.TryParse(input, out DateTime dateStart);
                    return isValid == true ? true : false;
                case "dateend":
                    //Format: MM/dd/YYYY
                    if (startDate == null)
                    {
                        throw new ArgumentNullException($"{nameof(startDate)} can't be null for {nameof(columnName)} column.");
                    }

                    isValid = DateTime.TryParse(input, out DateTime dateEnd);
                    if (!isValid)
                    {
                        return false;
                    }
                    else if (dateEnd < startDate)
                    {
                        throw new ArgumentException($"End date cannot be less than start date.");
                    }
                    else
                    {
                        return true;
                    }
                case "duration":
                    //Format: HH:MM:SS
                    isValid = TimeSpan.TryParse(input, out TimeSpan duration);
                    return isValid == true ? true : false;
                case "comments":
                    return true;
                default:
                    return false;
            }
        }
    }
}
