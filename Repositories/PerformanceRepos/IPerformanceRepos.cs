using Fest_form.data.Entity;

namespace Fest_form.Repositories.PerformanceRepos
{
    public interface IPerformanceRepos<T>
    {
        public void AddPerformance(List<Performance> performance);
        public List<Performance>? GetPerformances(Guid teamId);
    }
}
