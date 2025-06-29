using ProjectManagerSimulatorApi.DataObjects;

namespace ProjectManagerSimulatorApi.Repositories;

public class DeadlineRepository
{
    private readonly Dictionary<Guid, Deadline> _deadlines = [];

    public IEnumerable<Deadline> GetAll()
    {
        return _deadlines.Values;
    }

    public Deadline? GetByProjectId(Guid projectId)
    {
        _deadlines.TryGetValue(projectId, out var deadline);
        return deadline;
    }

    public void Save(Deadline deadline)
    {
        _deadlines[deadline.ProjectId] = deadline;
    }

    public bool Update(Guid projectId, Deadline updatedDeadline)
    {
        if (!_deadlines.ContainsKey(projectId))
        {
            return false;
        }

        _deadlines[projectId] = updatedDeadline;
        return true;
    }
}
