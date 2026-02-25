using Fest_form.data;
using Fest_form.data.Entity;

using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;


namespace Fest_form.Repositories.DeanseTeamRepos
{
    public class DanceTeamRepos(FestDataContext context, ILogger<DanceTeamRepos> logger) 
        : IDanceTeamRepos<DanceTeam>  
    {
        private readonly FestDataContext _context = context;
        private ILogger<DanceTeamRepos> _logger = logger;
        [HttpGet]
        public List<DanceTeam> GetTeamList() {
        
            return _context.DanceTeams
                .Include(dt => dt.TeamLeader)
                .Include(dt => dt.Performances).ThenInclude(p => p.Concertmaster)
                .Include(dt => dt.Performances).ThenInclude(p => p.ChoreographerDirector)
                .Include(dt => dt.Performances).ThenInclude(p => p.Genre)
                .Include(dt => dt.Performances).ThenInclude(p => p.ParticipantsNumber)
                .Include(dt => dt.Performances).ThenInclude(p => p.Category)


                .Include(p => p.Performances).ThenInclude(p => p.ParticipantsNameList).ThenInclude(per=>per.Person1)
                .Include(p => p.Performances).ThenInclude(p => p.ParticipantsNameList).ThenInclude(per => per.Person2)
                .Include(p => p.Performances).ThenInclude(p => p.ParticipantsNameList).ThenInclude(per => per.Person3)


                .ToList();
        }
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
                    dt => dt.TeamName == teamTemp.TeamName &&
                    (dt.TeamLeader.PersonName == teamTemp.TeamLeader.PersonName &&
                    dt.TeamLeader.PersonLastName == teamTemp.TeamLeader.PersonLastName
                    ));

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
