using ExerciseTracker.athallie.Model;
using System.Net.Http.Headers;
using System.Text.Json;

namespace ExerciseTracker.athallie.Utils
{
    public class HttpUtils
    {
        private readonly HttpClient _httpClient;
        public string? ApiEndpoint {  get; set; }
        public HttpUtils(HttpClient httpClient) { _httpClient = httpClient; Setup(); }
        private void Setup()
        {
            _httpClient.DefaultRequestHeaders.Accept.Clear();
            _httpClient.DefaultRequestHeaders.Accept.Add(
                new MediaTypeWithQualityHeaderValue("application/json")
            );
            _httpClient.DefaultRequestHeaders.Add("User-Agent", "Exercise Tracker App");
        }
        public async Task<IEnumerable<Exercise>> GetExercises()
        { 
            if (ApiEndpoint != null)
            {
                var stream = await _httpClient.GetStreamAsync(ApiEndpoint);
                List<Exercise> data = await JsonSerializer.DeserializeAsync<List<Exercise>>(stream);
                return data ?? new();
            }

            throw new Exception("API endpoint has not been set.");
        }
    }
}
