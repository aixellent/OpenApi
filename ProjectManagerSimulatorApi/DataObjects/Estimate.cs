namespace ProjectManagerSimulatorApi.DataObjects;

public record Estimate()
{
    public string EstimatedBy { get; init; } = string.Empty;
    public TimeSpan EstimatedTimespan { get; init; }
    public Guid ProjectId { get; init; }
}