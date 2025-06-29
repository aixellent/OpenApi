namespace ProjectManagerSimulatorApi.DataObjects;

public record Deadline()
{
    public string Name { get; init; } = string.Empty;
    public DateTimeOffset DeadlineDate { get; init; }
    public Guid ProjectId { get; init; }
}
