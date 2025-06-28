using AIPlayground.BusinessLogic.DTOs;

namespace AIPlayground.BusinessLogic.Interfaces
{
    public interface IRunService
    {
        Task<List<RunDto>> CreateRunsAsync(RunCreateDto runCreateDto);
        Task<IEnumerable<RunDto>> GetRunsByPromptIdAsync(int promptId);
        Task<IEnumerable<RunDto>> GetRunsByModelIdAsync(int modelId);
        Task<IEnumerable<RunDto>> GetAllAsync();
        Task<RunDto?> UpdateRunRatingAsync(int runId, float userRating);
    }
}
