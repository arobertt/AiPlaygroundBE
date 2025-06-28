using AiPlayground.DataAccess.Entities;
using AiPlayground.DataAccess.Repositories;
using AIPlayground.BusinessLogic.DTOs;
using AIPlayground.BusinessLogic.Interfaces;

namespace AIPlayground.BusinessLogic.Services
{
    public class ModelService : IModelService
    {
        private readonly IRepository<Model> _modelRepository;

        public ModelService(IRepository<Model> modelRepository)
        {
            _modelRepository = modelRepository;
        }

        public async Task<IEnumerable<ModelDto>> GetModelsAsync()
        {
            var models = await _modelRepository.GetAllAsync();

            return models.Select(m => new ModelDto
            {
                Id = m.Id,
                Name = m.Name
            });
        }

        public async Task<ModelDto?> GetByIdAsync(int id)
        {
            var model = await _modelRepository.GetByIdAsync(id);

            if (model == null)
            {
                return null;
            }

            return new ModelDto
            {
                Id = model.Id,
                Name = model.Name
            };
        }
    }
}
