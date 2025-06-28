using AiPlayground.DataAccess.Entities;
using OpenAI.Chat;

namespace AIPlayground.BusinessLogic.AIProcessing.Processors;

public class OpenAIProcessor : IAIProcessor
{
    public async Task<Run> ProcessAsync(Prompt prompt, Model model, float temperature)
    {
        ChatClient client = new(model: model.Name, apiKey: Environment.GetEnvironmentVariable("OPENAI_API_KEY"));

        var systemMessage = new SystemChatMessage(prompt.SystemMessage);
        var userMessage = new UserChatMessage(prompt.UserMessage);

        var messages = new List<ChatMessage>
        {
            systemMessage,
            userMessage,
        };

        var options = new ChatCompletionOptions
        {
            Temperature = temperature,
        };

        ChatCompletion completion = await client.CompleteChatAsync(messages, options);
        var actualResponse = completion.Content.First().Text;

        var ratingSystemMessage = new SystemChatMessage("You are rating a machine responses compared to the user expected response. Give a decimal number, 1 - 100, where 100 is the expected user response, be generous.");
        var expectedResultMessage = new UserChatMessage($"Expected response: {prompt.ExpectedResult}. Machine response: {actualResponse}.");

        var ratingMessages = new List<ChatMessage>
        {
            ratingSystemMessage,
            expectedResultMessage
        };

        ChatCompletion ratingCompletion = await client.CompleteChatAsync(ratingMessages);
        double.TryParse(ratingCompletion.Content.First().Text, out double ratingResponse);

        return new Run
        {
            ModelId = model.Id,
            PromptId = prompt.Id,
            ActualResponse = actualResponse,
            Temperature = temperature,
            Rating = ratingResponse
        };
    }
}
