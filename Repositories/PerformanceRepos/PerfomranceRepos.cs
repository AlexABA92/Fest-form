using Fest_form.data;
using Fest_form.data.Entity;


namespace Fest_form.Repositories.PerformanceRepos
{
    public class PerformanceRepos(FestDataContext context,ILogger<PerformanceRepos> logger) : IPerformanceRepos<Performance>
    {
        private readonly FestDataContext _context = context;
        private readonly ILogger<PerformanceRepos> _logger = logger;
        public void AddPerformance(List<Performance> performance) {
            try
            {
                _context.Performances.AddRange(performance);
                _context.SaveChanges();
            }
            catch (Exception ex) {
                _logger.LogError(ex, "AddPerformance Error");
            }
        }
        public void CheckPerformance(Performance performance) {
            
        
        }
        public List<Performance>? GetPerformances(Guid teamId) {
             return  _context.Performances.Where(dt=> dt.DanceTeamId == teamId).ToList();
        }
    }
}
