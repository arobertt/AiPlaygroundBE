using AIPlayground.BusinessLogic.DTOs;
using AIPlayground.BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AiPlayground.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PromptsController : ControllerBase
    {
        private readonly IPromptService _promptService;

        public PromptsController(IPromptService promptService)
        {
            _promptService = promptService;
        }

        [HttpGet]
        public async Task<IActionResult> GetPromptsAsync()
        {
            var prompts = await _promptService.GetPromptsAsync();

            return Ok(prompts);
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<PromptDto>> GetByIdAsync(int id)
        {
            var prompt = await _promptService.GetByIdAsync(id);

            if (prompt == null)
            {
                return NotFound();
            }

            return Ok(prompt);
        }

        [HttpGet("{scopeId}/prompts")]
        public async Task<IActionResult> GetPromptsByScopeIdAsync(int scopeId)
        {
            var prompts = await _promptService.GetPromptsByScopeIdAsync(scopeId);
            return Ok(prompts);
        }

        [HttpPost]
        public async Task<IActionResult> CreatePromptAsync([FromBody] PromptCreateDto promptCreateDto)
        {
            var prompt = await _promptService.CreatePromptAsync(promptCreateDto);

            return Ok(prompt);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteByIdAsync(int id)
        {
            try
            {
                await _promptService.DeleteByIdAsync(id);

                return NoContent();
            }
            catch (Exception ex)
            {
                return BadRequest(ex.Message);
            }
        }
    }
}
