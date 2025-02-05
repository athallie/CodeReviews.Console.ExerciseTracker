using System.Text.RegularExpressions;
using Spectre.Console;

namespace ExerciseTracker.athallie.UI
{
    public class UserInput : IUserInput
    {
        public UserInput() { }
        public string GetInput(string prompt, string columnName, DateTime? startDate = null)
        {
            string input;
            while (true)
            {
                input = GetInput(prompt);
                var inputIsValid = ValidateInput(input, columnName, startDate);
                if (inputIsValid) { break; }
            }
            return input;
        }

        public int GetIDInput()
        {
            string input;
            while (true)
            {
                input = GetInput("ID of Item To Be Deleted:");
                var inputIsValid = ValidateInput(input);
                if (inputIsValid) { break; }
            }
            return Int32.Parse(input);
        }
            
        private string GetInput(string prompt)
        {
            return AnsiConsole.Prompt(
                    new TextPrompt<string>(prompt)
            );
        }
        
        //Validate ID (for deletion)
        private bool ValidateInput(string id)
        {
            if (Int32.TryParse(id, out var itemId))
            {
                return true;
            } else
            {
                return false;
            }
        }

        //Validate Input for Addition based on Exercise class
        private bool ValidateInput(string input, string columnName, DateTime? startDate = null)
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
                        Console.WriteLine($"{nameof(startDate)} can't be null for {nameof(columnName)} column.");
                        return false;
                    }

                    isValid = DateTime.TryParse(input, out DateTime dateEnd);
                    if (!isValid)
                    {
                        return false;
                    }
                    else if (dateEnd < startDate)
                    {
                        Console.WriteLine($"End date cannot be less than start date.");
                        return false;
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
