using ProjectManagerSimulatorApi.DataObjects;

namespace ProjectManagerSimulatorApi.Repositories;

public class EstimateRepository
{
    private readonly List<Estimate> _estimates =
   [
       new Estimate { EstimatedBy = "Alice", EstimatedTimespan = TimeSpan.FromDays(5), ProjectId = Guid.Parse("11111111-1111-1111-1111-111111111111") },
       new Estimate { EstimatedBy = "Bob", EstimatedTimespan = TimeSpan.FromDays(3), ProjectId = Guid.Parse("11111111-1111-1111-1111-111111111111") },
       new Estimate { EstimatedBy = "Charlie", EstimatedTimespan = TimeSpan.FromDays(7), ProjectId = Guid.Parse("11111111-1111-1111-1111-111111111111") },
       new Estimate { EstimatedBy = "Diana", EstimatedTimespan = TimeSpan.FromDays(4), ProjectId = Guid.Parse("22222222-2222-2222-2222-222222222222") },
       new Estimate { EstimatedBy = "Eve", EstimatedTimespan = TimeSpan.FromDays(6), ProjectId = Guid.Parse("22222222-2222-2222-2222-222222222222") }
   ];

    public void Save(Estimate estimate)
    {
        _estimates.Add(estimate);
    }

    public void Save(IEnumerable<Estimate> estimates)
    {
        _estimates.AddRange(estimates);
    }

    public IEnumerable<Estimate> GetAll()
    {
        return _estimates;
    }

    public IEnumerable<Estimate> GetByProjectId(Guid projectId)
    {
        return _estimates.Where(e => e.ProjectId == projectId);
    }
}
