using System.Net.Http.Headers;
using System.Text.Json;
using AiPlayground.DataAccess.Entities;
using AIPlayground.BusinessLogic.AIProcessing.Models;

namespace AIPlayground.BusinessLogic.AIProcessing.Processors;

public class GeminiProcessor : IAIProcessor
{
    public async Task<Run> ProcessAsync(Prompt prompt, Model model, float temperature)
    {
        var apiKey = Environment.GetEnvironmentVariable("GEMINI_API_KEY");

        var requestUri = $"https://generativelanguage.googleapis.com/v1beta/models/{model.Name}:generateContent?key={apiKey}";

        var client = new HttpClient();

        var content = new GeminiRequest
        {
            System_instruction = new SystemInstruction
            {
                Parts =
                [
                    new GeminiPart
                    {
                        Text = prompt.SystemMessage
                    }
                ]
            },
            Contents =
            [
                new GeminiContent
                {
                    Parts =
                    [
                        new GeminiPart
                        {
                            Text = prompt.UserMessage
                        }
                    ]
                }
            ],
            GenerationConfig = new GeminiGenerationConfig
            {
                temperature = temperature
            }
        };

        var jsonContent = JsonSerializer.Serialize(content, new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
        });
        var httpContent = new StringContent(jsonContent, System.Text.Encoding.UTF8, "application/json");

        try
        {
            Random rnd = new Random();
            Console.WriteLine(content);
            Console.WriteLine(jsonContent);
            var response = await client.PostAsync(requestUri, httpContent);
            var responseContent = await response.Content.ReadAsStringAsync();

            var geminiResponse = JsonSerializer.Deserialize<GeminiResponse>(responseContent, new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase
            });

            if (geminiResponse != null && geminiResponse.Candidates.Count > 0)
            {
                return new Run
                {
                    ModelId = model.Id,
                    PromptId = prompt.Id,
                    ActualResponse = geminiResponse.Candidates.First().Content.Parts.First().Text,
                    Temperature = temperature,
                    Rating = rnd.Next(0, 4) * 5 + 80,
                    UserRating = 0
                };
            }
            else
            {
                throw new Exception("Gemini response is null or contains no candidates");
            }
        }
        catch (Exception ex)
        {
            throw new Exception($"Error sending Gemini request: {ex.Message}");
        }
    }
}
