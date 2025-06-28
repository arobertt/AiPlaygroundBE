using AIPlayground.BusinessLogic.AIProcessing.Processors;
using AIPlayground.BusinessLogic.Enums;

namespace AIPlayground.BusinessLogic.AIProcessing.Factories;

public class AIProcessorFactory
{
    public IAIProcessor CreateAIProcessor(PlatformType platformType)
    {
        switch (platformType)
        {
            case PlatformType.OpenAI:
                return new OpenAIProcessor();
            case PlatformType.DeepSeek:
                return new DeepSeekProcessor();
            case PlatformType.Gemini:
                return new GeminiProcessor();
            default:
                throw new ArgumentException($"No AI processor found for platform type: {platformType}.");
        }
    }
}