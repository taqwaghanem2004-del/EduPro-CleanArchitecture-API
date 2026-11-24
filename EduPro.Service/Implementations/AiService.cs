using EduPro.Service.Abstracts;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using System.Text;
using System.Text.Json;

namespace EduPro.Service.Implementations
{
    public class AiService : IAiService
    {
        private readonly HttpClient _httpClient;
        private readonly string _baseUrl;

        public AiService(HttpClient httpClient, IConfiguration configuration)
        {
            _httpClient = httpClient;
            _baseUrl = configuration["AiService:BaseUrl"];
            _httpClient.DefaultRequestHeaders.Add("ngrok-skip-browser-warning", "true");
        }

        public async Task<string> UploadAndProcessFileAsync(int fileId, string absoluteFilePath, string fileName)
        {
            try
            {
                var projectId = fileId.ToString();

                using var fileStream = File.OpenRead(absoluteFilePath);
                using var uploadContent = new MultipartFormDataContent();
                uploadContent.Add(new StreamContent(fileStream), "file", fileName);

                var uploadResponse = await _httpClient.PostAsync(
                    $"{_baseUrl}/api/v1/data/upload/{projectId}", uploadContent);

                if (!uploadResponse.IsSuccessStatusCode) return "Upload Failed";

                var uploadResult = await uploadResponse.Content.ReadAsStringAsync();
                var uploadJson = JsonSerializer.Deserialize<JsonElement>(uploadResult);
                var actualFileId = uploadJson.GetProperty("file_id").GetString();

                var processBody = JsonSerializer.Serialize(new
                {
                    file_id = actualFileId,
                    chunk_size = 1000,
                    overlap_size = 100,
                    do_reset = 1
                });
                var processContent = new StringContent(processBody, Encoding.UTF8, "application/json");

                var processResponse = await _httpClient.PostAsync(
                    $"{_baseUrl}/api/v1/data/process/{projectId}", processContent);

                if (!processResponse.IsSuccessStatusCode) return "Process Failed";

                var pushBody = JsonSerializer.Serialize(new { do_reset = 0 });
                var pushContent = new StringContent(pushBody, Encoding.UTF8, "application/json");

                var pushResponse = await _httpClient.PostAsync(
                    $"{_baseUrl}/api/v1/nlp/index/push/{projectId}", pushContent);

                if (!pushResponse.IsSuccessStatusCode) return "Push Failed";

                return "Success";
            }
            catch (Exception ex)
            {
                return $"AI Error: {ex.Message}";
            }
        }

        public async Task<string> AskQuestionAsync(int fileId, string question)
        {
            try
            {
                var projectId = fileId.ToString();

                var body = new { text = question, limit = 5 };
                var json = JsonSerializer.Serialize(body);
                var content = new StringContent(json, Encoding.UTF8, "application/json");

                var response = await _httpClient.PostAsync(
                    $"{_baseUrl}/api/v1/nlp/index/answer/{projectId}", content);

                if (!response.IsSuccessStatusCode)
                    return "لم أتمكن من الإجابة على سؤالك";

                var result = await response.Content.ReadAsStringAsync();
                var resultJson = JsonSerializer.Deserialize<JsonElement>(result);

                // استخرج الـ answer من الـ response
                if (resultJson.TryGetProperty("answer", out var answer))
                    return answer.GetString() ?? "لا توجد إجابة";

                return result;
            }
            catch (Exception ex)
            {
                return $"AI Error: {ex.Message}";
            }
        }
    }
}