using Microsoft.AspNetCore.Mvc;
using CarbonCalculator.Core.Interfaces;
using CarbonCalculator.Core.Entities;

namespace CarbonCalculator.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class CalculatorController : ControllerBase
    {
        private readonly ICalculationService _calculationService;

        public CalculatorController(ICalculationService calculationService)
        {
            _calculationService = calculationService;
        }

        [HttpPost("calculate")]
        public async Task<ActionResult<CalculationResult>> CalculateCarbonFootprint([FromBody] CalculationRequest request)
        {
            try
            {
                if (request == null || !request.Activities.Any())
                {
                    return BadRequest(new { error = "Invalid request. Activities are required." });
                }

                var result = await _calculationService.CalculateCarbonFootprintAsync(request);
                return Ok(result);
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new { error = ex.Message });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while calculating carbon footprint.", details = ex.Message });
            }
        }

        [HttpGet("history")]
        public async Task<ActionResult<IEnumerable<CalculationHistory>>> GetCalculationHistory(
            [FromQuery] string? trialId = null,
            [FromQuery] string? userId = null)
        {
            try
            {
                var history = await _calculationService.GetCalculationHistoryAsync(trialId, userId);
                return Ok(history);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while retrieving calculation history.", details = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<CalculationDetails>> GetCalculationDetails(Guid id)
        {
            try
            {
                var details = await _calculationService.GetCalculationDetailsAsync(id);
                if (details == null)
                {
                    return NotFound(new { error = "Calculation not found." });
                }
                return Ok(details);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while retrieving calculation details.", details = ex.Message });
            }
        }
    }
}
