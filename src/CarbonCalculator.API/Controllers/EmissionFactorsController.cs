using Microsoft.AspNetCore.Mvc;
using CarbonCalculator.Core.Interfaces;
using CarbonCalculator.Core.Entities;

namespace CarbonCalculator.API.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class EmissionFactorsController : ControllerBase
    {
        private readonly IEmissionFactorService _emissionFactorService;

        public EmissionFactorsController(IEmissionFactorService emissionFactorService)
        {
            _emissionFactorService = emissionFactorService;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<EmissionFactor>>> GetEmissionFactors(
            [FromQuery] string? category = null,
            [FromQuery] string? activityType = null)
        {
            try
            {
                var factors = await _emissionFactorService.GetEmissionFactorsAsync(category, activityType);
                return Ok(factors);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while retrieving emission factors.", details = ex.Message });
            }
        }

        [HttpGet("{id}")]
        public async Task<ActionResult<EmissionFactor>> GetEmissionFactor(int id)
        {
            try
            {
                var factor = await _emissionFactorService.GetEmissionFactorByIdAsync(id);
                if (factor == null)
                {
                    return NotFound(new { error = "Emission factor not found." });
                }
                return Ok(factor);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while retrieving emission factor.", details = ex.Message });
            }
        }

        [HttpPost]
        public async Task<ActionResult<EmissionFactor>> CreateEmissionFactor([FromBody] CreateEmissionFactorRequest request)
        {
            try
            {
                if (request == null)
                {
                    return BadRequest(new { error = "Invalid request data." });
                }

                var factor = await _emissionFactorService.CreateEmissionFactorAsync(request);
                return CreatedAtAction(nameof(GetEmissionFactor), new { id = factor.Id }, factor);
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An error occurred while creating emission factor.", details = ex.Message });
            }
        }
    }
}
