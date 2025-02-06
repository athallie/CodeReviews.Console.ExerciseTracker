using System.Text.RegularExpressions;
using Microsoft.IdentityModel.Tokens;
using Spectre.Console;

namespace ExerciseTracker.athallie.UI
{
    public class UserInput : IUserInput
    {
        public UserInput() { }

        public string[] GetAllInput()
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
                    input = GetInput(prompts[i], types[i], DateTime.Parse(data[0]));
                }
                else
                {
                    input = GetInput(prompts[i], types[i]);
                }

                if (input.IsNullOrEmpty())
                {
                    return new[] {"quit"};
                }

                data[i] = input;
            }

            return data;
        }
        public string GetInput(string prompt, string columnName, DateTime? startDate = null)
        {
            string input;
            while (true)
            {
                input = GetInput(prompt);

                if (input.IsNullOrEmpty())
                {
                    return "quit";
                }
                {
                    
                }

                var inputIsValid = ValidateInput(input, columnName, startDate);
                if (inputIsValid) { break; }
            }
            return input;
        }

        public int GetIDInput(string prompt)
        {
            string input;
            while (true)
            {
                input = GetInput(prompt);

                if (input.IsNullOrEmpty())
                {
                    return -1;
                }

                var inputIsValid = ValidateInput(input);
                if (inputIsValid) { break; }
            }
            return Int32.Parse(input);
        }
            
        private string GetInput(string prompt)
        {
            return AnsiConsole.Prompt(
                    new TextPrompt<string>(prompt)
                    .AllowEmpty()
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
