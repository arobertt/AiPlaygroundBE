using AiPlayground.DataAccess.Entities;
using AiPlayground.DataAccess.Repositories;
using AIPlayground.BusinessLogic.DTOs;
using AIPlayground.BusinessLogic.Interfaces;


namespace AIPlayground.BusinessLogic.Services
{
    public class PlatformService : IPlatformService
    {
        private readonly IRepository<Platform> _platformRepository;
        private readonly IRepository<Run> _runRepository;

        public PlatformService(IRepository<Platform> platformRepository, IRepository<Run> runRepository)
        {
            _platformRepository = platformRepository;
            _runRepository = runRepository;
        }

        public async Task<IEnumerable<PlatformDto>> GetPlatformsAsync()
        {
            var platforms = await _platformRepository.GetAllAsync();
            var modelAverageRatings = await GetModelAverageRatingsAsync();

            return platforms.Select(p => new PlatformDto
            {
                ImageUrl = p.ImageUrl,
                Id = p.Id,
                Name = p.Name,
                Models = p.Models.Select(m => new ModelDto
                {
                    Id = m.Id,
                    Name = m.Name,
                    AverageRating = Math.Round(modelAverageRatings.GetValueOrDefault(m.Id, 0), 2)
                }).ToList()
            });
        }
        private async Task<Dictionary<int, double>> GetModelAverageRatingsAsync()
        {
            var runs = await _runRepository.GetAllAsync();

            return runs
                .GroupBy(r => r.ModelId)
                .ToDictionary(g => g.Key, g => g.Average(r => r.Rating));
        }
        public async Task<PlatformDto?> GetByIdAsync(int id)
        {
            var platform = await _platformRepository.GetByIdAsync(id);

            if (platform == null)
            {
                return null;
            }

            return new PlatformDto
            {
                Id = platform.Id,
                Name = platform.Name,
                Models = platform.Models.Select(m => new ModelDto
                {
                    Id = m.Id,
                    Name = m.Name,
                    AverageRating = 0 // To be calculated later
                }).ToList()
            };
        }
    }
}
