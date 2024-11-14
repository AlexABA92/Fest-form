using Fest_form.data;
using Fest_form.data.Entity;


namespace Fest_form.Repositories.DeanseTeamRepos
{
    public class DanceTeamRepos(FestDataContext context, ILogger<DanceTeamRepos> logger) 
        : IDanceTeamRepos<DanceTeam>  
    {
        private readonly FestDataContext _context = context;
        private ILogger<DanceTeamRepos> _logger = logger;
      
        public void CreateTeam(DanceTeam team) {
            try
            {
                _context.DanceTeams.Add(team);
                _context.SaveChanges();
            }
            catch (Exception ex) {
                _logger.LogError(ex, "Fatal Eroor"); 
            }
        }


    }
}
