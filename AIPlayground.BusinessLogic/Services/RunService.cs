using AiPlayground.DataAccess.Entities;
using AiPlayground.DataAccess.Repositories;
using AIPlayground.BusinessLogic.AIProcessing.Factories;
using AIPlayground.BusinessLogic.DTOs;
using AIPlayground.BusinessLogic.Enums;
using AIPlayground.BusinessLogic.Interfaces;

namespace AIPlayground.BusinessLogic.Services
{
    public class RunService : IRunService
    {
        private readonly IRepository<Run> _runRepository;
        private readonly IRepository<Model> _modelRepository;
        private readonly IRepository<Prompt> _promptRepository;

        public RunService(IRepository<Run> runRepository, IRepository<Model> modelRepository,
            IRepository<Prompt> promptRepository)
        {
            _runRepository = runRepository;
            _modelRepository = modelRepository;
            _promptRepository = promptRepository;
        }

        public async Task<List<RunDto>> CreateRunsAsync(RunCreateDto runCreateDto)
        {
            var runs = new List<RunDto>();

            var AIProcessorFactory = new AIProcessorFactory();

            var prompt = await _promptRepository.GetByIdAsync(runCreateDto.PromptId);

            if (prompt == null)
            {
                throw new Exception($"Prompt with id {runCreateDto.PromptId} not found");
            }

            foreach (var modelToRun in runCreateDto.ModelsToRun)
            {
                var model = await _modelRepository.GetByIdAsync(modelToRun.ModelId);

                if (model == null)
                {
                    throw new Exception($"Model with id {modelToRun.ModelId} not found");
                }

                var platformType = (PlatformType)model.PlatformId;

                var processor = AIProcessorFactory.CreateAIProcessor(platformType);

                var run = await processor.ProcessAsync(prompt, model, (float)modelToRun.Temperature);

                await _runRepository.AddAsync(run);

                runs.Add(new RunDto
                {
                    Id = run.Id,
                    Model = run.ModelId,
                    Prompt = run.PromptId,
                    ActualResponse = run.ActualResponse,
                    Temperature = run.Temperature,
                    Rating = run.Rating,
                    UserRating = run.UserRating
                });
            }

            return runs;
        }

        public async Task<IEnumerable<RunDto>> GetAllAsync()
        {
            var runs = await _runRepository.GetAllAsync();
            return runs.Select(r => new RunDto
            {
                Id = r.Id,
                Model = r.ModelId,
                Prompt = r.PromptId,
                ActualResponse = r.ActualResponse,
                Temperature = r.Temperature,
                Rating = r.Rating,
                UserRating = r.UserRating
            });
        }
        public async Task<IEnumerable<RunDto>> GetRunsByPromptIdAsync(int promptId)
        {
            var runs = await _runRepository.GetAllAsync();
            var filteredRuns = runs.Where(r => r.PromptId == promptId);

            return filteredRuns.Select(r => new RunDto
            {
                Id = r.Id,
                Model = r.ModelId,
                Prompt = r.PromptId,
                ActualResponse = r.ActualResponse,
                Temperature = r.Temperature,
                Rating = r.Rating,
                UserRating = r.UserRating
            });
        }

        public async Task<IEnumerable<RunDto>> GetRunsByModelIdAsync(int modelId)
        {
            var runs = await _runRepository.GetAllAsync();
            var filteredRuns = runs.Where(r => r.ModelId == modelId);

            return filteredRuns.Select(r => new RunDto
            {
                Id = r.Id,
                Model = r.ModelId,
                Prompt = r.PromptId,
                ActualResponse = r.ActualResponse,
                Temperature = r.Temperature,
                Rating = r.Rating,
                UserRating = r.UserRating
            });
        }

        public async Task<RunDto?> UpdateRunRatingAsync(int runId, float userRating)
        {
            var run = await _runRepository.GetByIdAsync(runId);
            if (run == null)
            {
                return null;
            }

            run.UserRating = userRating;

            var updatedRun = await _runRepository.UpdateAsync(run);

            return new RunDto
            {
               Id = updatedRun.Id,
               UserRating = updatedRun.UserRating,
            };
        }
    }
}
