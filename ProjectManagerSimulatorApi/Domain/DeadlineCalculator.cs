using ProjectManagerSimulatorApi.DataObjects;

namespace ProjectManagerSimulatorApi.Domain;

public static class DeadlineCalculator
{
    public static Deadline CalculateDeadline(IEnumerable<Estimate> estimates, string name, Guid projectId)
    {
        if (!estimates.Any())
        {
            throw new ArgumentException("Estimates collection cannot be empty.", nameof(estimates));
        }

        var meanTimespan = TimeSpan.FromTicks((long)estimates.Average(e => e.EstimatedTimespan.Ticks));

        var deadlineDate = DateTimeOffset.Now.AddTicks((long)(meanTimespan.Ticks * 0.8));

        return new Deadline
        {
            Name = name,
            DeadlineDate = deadlineDate,
            ProjectId = projectId
        };
    }
}
