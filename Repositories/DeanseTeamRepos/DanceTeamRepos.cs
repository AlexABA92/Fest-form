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
                _logger.LogError(ex, " CreateTeam Fatal Error"); 
            }
        }
        public void CheckTeam(ref DanceTeam team) {
            try
            {
                var teamTemp = team;
                teamTemp = _context.DanceTeams.FirstOrDefault(
                    dt => dt.TeamName == teamTemp.TeamName);

                if (teamTemp != null)
                {
                    team.TeamId = teamTemp.TeamId;
                }
               
            }
            catch (Exception ex) {
                _logger.LogError(ex, "CheckTeam Fatal Error");
            }
        }


    }
}
