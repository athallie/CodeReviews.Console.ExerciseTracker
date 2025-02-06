using System.Text.Json.Serialization;
using Microsoft.EntityFrameworkCore;

namespace ExerciseTracker.athallie.Model
{
    public class Exercise
    {
        [property: JsonPropertyName("id")] public int Id { get; set; }
        [property: JsonPropertyName("datestart")] public DateTime DateStart { get; set; }
        [property: JsonPropertyName("dateend")] public DateTime DateEnd { get; set; }
        [property: JsonPropertyName("duration")] public TimeSpan Duration { get; set; }
        [property: JsonPropertyName("comments")] public string? Comments { get; set; }
    }
}
