using Microsoft.AspNetCore.Mvc;
using ProjectManagerSimulatorApi.DataObjects;
using ProjectManagerSimulatorApi.Domain;
using ProjectManagerSimulatorApi.Repositories;
using System.ComponentModel;

namespace ProjectManagerSimulatorApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class DeadlinesController(DeadlineRepository deadlineRepository, EstimateRepository estimateRepository) : ControllerBase
{
    [EndpointSummary("Get all Deadlines")]
    [EndpointName("GetAllDeadlines")]
    [HttpGet]
    public ActionResult<IEnumerable<Deadline>> GetAll()
    {
        var deadlines = deadlineRepository.GetAll();
        return Ok(deadlines);
    }

    [EndpointSummary("Gets a single Deadline for a specific project")]
    [EndpointName("GetAllDeadlinesByProjectId")]
    [HttpGet("{projectId:guid}")]
    public ActionResult<Deadline> GetByProjectId([Description("This is the project ID the endpoint filteres the deadlines for.")] Guid projectId)
    {
        var deadline = deadlineRepository.GetByProjectId(projectId);
        if (deadline is null)
            return NotFound();
        return Ok(deadline);
    }

    [EndpointSummary("Calculates and saves a new deadline based on the estimates that exist for the project.")]
    [EndpointDescription("The calculation takes the mean of all available Estimates and multiplies it by 0.8")]
    [EndpointName("CalculateNewDeadline")]
    [HttpPost("calculate")]
    public ActionResult<Deadline> Calculate([FromBody] CalculateDeadlineRequest request)
    {
        if (request is null)
            return BadRequest("Request is required.");

        var estimates = estimateRepository.GetByProjectId(request.ProjectId).ToList();
        if (estimates.Count == 0)
            return BadRequest("No estimates found for the specified project.");

        var deadline = DeadlineCalculator.CalculateDeadline(estimates, request.Name, request.ProjectId);
        deadlineRepository.Save(deadline);
        return CreatedAtAction(nameof(GetByProjectId), new { projectId = deadline.ProjectId }, deadline);
    }

    [EndpointName("UpdateDeadline")]
    [HttpPut("{projectId:guid}")]
    public IActionResult Update(Guid projectId, [FromBody] UpdateDeadlineRequest request)
    {
        var existing = deadlineRepository.GetByProjectId(projectId);
        if (existing is null)
            return NotFound();

        if (request is null || request.DeadlineDate >= existing.DeadlineDate)
            return BadRequest("Deadline can only be shortened.");

        var updated = existing with { DeadlineDate = request.DeadlineDate, Name = request.Name ?? existing.Name };
        var updatedResult = deadlineRepository.Update(projectId, updated);
        if (!updatedResult)
            return NotFound();

        return NoContent();
    }
}

public record CalculateDeadlineRequest
{
    public string Name { get; init; } = string.Empty;
    public Guid ProjectId { get; init; }
}

public record UpdateDeadlineRequest
{
    public string? Name { get; init; }
    public DateTimeOffset DeadlineDate { get; init; }
}
