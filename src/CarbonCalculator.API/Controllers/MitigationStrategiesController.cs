using Microsoft.AspNetCore.Mvc;
using CarbonCalculator.Core.Interfaces;
using CarbonCalculator.Core.Entities;

namespace CarbonCalculator.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class MitigationStrategiesController : ControllerBase
    {
        private readonly IMitigationStrategyService _mitigationStrategyService;

        public MitigationStrategiesController(IMitigationStrategyService mitigationStrategyService)
        {
            _mitigationStrategyService = mitigationStrategyService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<MitigationStrategy>>> GetMitigationStrategies(
            [FromQuery] string? category = null,
            [FromQuery] string? costCategory = null,
            [FromQuery] string? difficulty = null)
        {
            try
            {
                var strategies = await _mitigationStrategyService.GetMitigationStrategiesAsync(category, costCategory, difficulty);
                return Ok(strategies);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while retrieving mitigation strategies.", details = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<MitigationStrategy>> GetMitigationStrategy(int id)
        {
            try
            {
                var strategy = await _mitigationStrategyService.GetMitigationStrategyByIdAsync(id);
                if (strategy == null)
                {
                    return NotFound(new { error = "Mitigation strategy not found." });
                }
                return Ok(strategy);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while retrieving mitigation strategy.", details = ex.Message });
            }
        }
    }
}
