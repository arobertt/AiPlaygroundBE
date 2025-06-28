using AiPlayground.DataAccess.Entities;
using AiPlayground.DataAccess.Repositories;
using AIPlayground.BusinessLogic.DTOs;
using AIPlayground.BusinessLogic.Interfaces;
using Microsoft.EntityFrameworkCore;

namespace AIPlayground.BusinessLogic.Services
{
    public class PromptService : IPromptService
    {
        private readonly IRepository<Prompt> _promptRepository;

        public PromptService(IRepository<Prompt> promptRepository)
        {
            _promptRepository = promptRepository;
        }

        public async Task<IEnumerable<PromptDto>> GetPromptsAsync()
        {
            var prompts = await _promptRepository.GetAllAsync();

            return prompts.Select(p => new PromptDto
            {
                Id = p.Id,
                Name = p.Name,
                SystemMessage = p.SystemMessage,
                UserMessage = p.UserMessage,
                ExpectedResult = p.ExpectedResult
            });
        }

        public async Task<PromptDto?> GetByIdAsync(int id)
        {
            var prompt = await _promptRepository.GetByIdAsync(id);

            if (prompt == null)
            {
                return null;
            }

            return new PromptDto
            {
                Id = prompt.Id,
                Name = prompt.Name,
                SystemMessage = prompt.SystemMessage,
                UserMessage = prompt.UserMessage,
                ExpectedResult = prompt.ExpectedResult
            };
        }

        public async Task<IEnumerable<PromptDto>> GetPromptsByScopeIdAsync(int scopeId)
        {
            var prompts = await _promptRepository.GetAllAsync();
            var filteredPrompts = prompts.Where(p => p.ScopeId == scopeId);

            return filteredPrompts.Select(p => new PromptDto
            {
                Id = p.Id,
                Name = p.Name,
                SystemMessage = p.SystemMessage,
                UserMessage = p.UserMessage,
                ExpectedResult = p.ExpectedResult
            });
        }

        public async Task<PromptDto> CreatePromptAsync(PromptCreateDto promptCreateDto)
        {
            var prompt = new Prompt
            {
                ScopeId = promptCreateDto.ScopeId,
                Name = promptCreateDto.Name,
                SystemMessage = promptCreateDto.SystemMessage,
                UserMessage = promptCreateDto.UserMessage,
                ExpectedResult = promptCreateDto.ExpectedResult
            };

            var createdPrompt = await _promptRepository.AddAsync(prompt);

            return new PromptDto
            {
                Id = createdPrompt.Id,
                Name = createdPrompt.Name,
                SystemMessage = createdPrompt.SystemMessage,
                UserMessage = createdPrompt.UserMessage,
                ExpectedResult = createdPrompt.ExpectedResult
            };
        }

        public async Task DeleteByIdAsync(int id)
        {
            var prompt = await _promptRepository.GetByIdAsync(id);

            if (prompt == null)
            {
                throw new Exception($"Prompt with id {id} not found");
            }

            await _promptRepository.DeleteAsync(id);
        }
    }   
}
