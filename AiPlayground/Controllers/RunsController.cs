using AIPlayground.BusinessLogic.DTOs;
using AIPlayground.BusinessLogic.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace AiPlayground.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RunsController : ControllerBase
    {
        private readonly IRunService _runService;

        public RunsController(IRunService runService)
        {
            _runService = runService;
        }

        [HttpPost]
        public async Task<IActionResult> CreateRuns([FromBody] RunCreateDto runCreateDto)
        {
            if (runCreateDto.ModelsToRun.Count == 0)
            {
                return BadRequest("Invalid run data.");
            }

            var runs = await _runService.CreateRunsAsync(runCreateDto);

            return Ok(runs);
        }

        [HttpPatch("{id}")]
        public async Task<IActionResult> UpdateRunRating(int id, float userRating)
        {
            var updatedRun = await _runService.UpdateRunRatingAsync(id, userRating);
            if (updatedRun == null)
            {
                return NotFound();
            }
            return Ok(updatedRun);
        }

        [HttpGet]
        public async Task<IActionResult> GetAllAsync()
        {
            var runs = await _runService.GetAllAsync();
            return Ok(runs);
        }

        [HttpGet("prompt/{promptId}")]
        public async Task<IActionResult> GetRunsByPromptIdAsync(int promptId)
        {
            var runs = await _runService.GetRunsByPromptIdAsync(promptId);
            return Ok(runs);
        }

        [HttpGet("model/{modelId}")]
        public async Task<IActionResult> GetRunsByModelIdAsync(int modelId)
        {
            var runs = await _runService.GetRunsByModelIdAsync(modelId);
            return Ok(runs);
        }
    }
}
