using Microsoft.AspNetCore.Mvc;
using ProjectManagerSimulatorApi.DataObjects;
using ProjectManagerSimulatorApi.Repositories;

namespace ProjectManagerSimulatorApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class EstimatesController(EstimateRepository repository) : ControllerBase
{
    [EndpointName("GetAllEstimates")]
    [HttpGet]
    public ActionResult<IEnumerable<Estimate>> GetAll([FromQuery] Guid? projectId)
    {
        if (projectId.HasValue)
        {
            var estimates = repository.GetByProjectId(projectId.Value);
            return Ok(estimates);
        }
        return Ok(repository.GetAll());
    }

    [EndpointName("CreateEstimate")]
    [HttpPost]
    public ActionResult<Estimate> Create([FromBody] Estimate estimate)
    {
        if (estimate is null)
            return BadRequest();

        repository.Save(estimate);
        return CreatedAtAction(null, null, estimate);
    }

    [EndpointName("CreateEstimateBatch")]
    [HttpPost("batch")]
    public IActionResult CreateBatch([FromBody] IEnumerable<Estimate> estimates)
    {
        if (estimates is null)
            return BadRequest();

        repository.Save(estimates);
        return NoContent();
    }
}
