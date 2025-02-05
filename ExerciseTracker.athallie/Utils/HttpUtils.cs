using ExerciseTracker.athallie.Model;
using System.Net.Http.Headers;
using System.Text;
using System.Text.Json;

namespace ExerciseTracker.athallie.Utils
{
    public class HttpUtils
    {
        private readonly HttpClient _httpClient;
        public string? ApiEndpoint { get; set; }
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
        public async Task<string> AddExercise(DateTime dateStart, DateTime dateEnd, TimeSpan duration, string? comments)
        {
            if (ApiEndpoint != null)
            {
                using StringContent jsonContent = new(
                    JsonSerializer.Serialize(new Exercise
                    {
                        DateStart = dateStart,
                        DateEnd = dateEnd,
                        Duration = duration,
                        Comments = comments
                    }),
                    Encoding.UTF8,
                    "application/json"
                );

                using HttpResponseMessage response = await
                    _httpClient.PostAsync(ApiEndpoint, jsonContent);

                var jsonResponse = await response.Content.ReadAsStringAsync();
                return jsonResponse;
            }

            throw new Exception("API endpoint has not been set.");
        }

        public async Task<string> DeleteExercise(int id)
        {
            using HttpResponseMessage response = await _httpClient.DeleteAsync($"{ApiEndpoint}/{id}");
            string jsonResponse = await response.Content.ReadAsStringAsync();
            return jsonResponse;
        }

        public async Task<Exercise?> UpdateExercise(int id, Exercise exercise)
        {
            using StringContent jsonContent = new(
                JsonSerializer.Serialize(exercise),
                Encoding.UTF8,
                "application/json"
            );
            using HttpResponseMessage response = await _httpClient.PutAsync(
                $"{ApiEndpoint}/{id}",
                jsonContent
            );
            //var jsonResponse = await response.Content.ReadFromJsonAsync<Exercise>();
            return exercise;
        }
    }
}
