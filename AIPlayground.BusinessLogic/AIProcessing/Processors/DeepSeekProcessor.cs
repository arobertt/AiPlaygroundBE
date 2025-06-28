using System.Net.Http.Headers;
using System.Text.Json;
using AiPlayground.DataAccess.Entities;
using AIPlayground.BusinessLogic.AIProcessing.Models;

namespace AIPlayground.BusinessLogic.AIProcessing.Processors;

public class DeepSeekProcessor : IAIProcessor
{
    public async Task<Run> ProcessAsync(Prompt prompt, Model model, float temperature)
    {
        var apiKey = Environment.GetEnvironmentVariable("DEEPSEEK_API_KEY");

        var requestUri = "https://api.deepseek.com/chat/completions";

        var client = new HttpClient();

        client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", apiKey);

        var content = new DeepSeekRequest
        {
            Model = model.Name,
            Messages =
            [
                new DeepSeekMessage
                {
                    Role = "system",
                    Content = prompt.SystemMessage
                },

                new DeepSeekMessage
                {
                    Role = "user",
                    Content = prompt.UserMessage
                }
            ],
            Stream = false,
            Temperature = temperature
      
        };

        var jsonContent = JsonSerializer.Serialize(content, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        });

        var httpContent = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

        try
        {
            Random rnd = new Random();
            var response = await client.PostAsync(requestUri, httpContent);
            var responseContent = await response.Content.ReadAsStringAsync();

            var deepSeekResponse = JsonSerializer.Deserialize<DeepSeekResponse>(responseContent, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            if (deepSeekResponse != null)
            {
                return new Run
                {
                    ModelId = model.Id,
                    PromptId = prompt.Id,
                    ActualResponse = deepSeekResponse.Choices.First().Message.Content,
                    Temperature = temperature,
                    Rating = rnd.Next(0, 3) * 5 + 70,
                    UserRating = 0
                };
            }
            else
            {
                throw new Exception("DeepSeek response is null");
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Error sending DeepSeek request: {ex.Message}");
        }
    }
}
