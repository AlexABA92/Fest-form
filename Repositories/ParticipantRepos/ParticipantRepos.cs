using Fest_form.data;
using Fest_form.data.Entity;

namespace Fest_form.Repositories.ParticipantRepos
{
    public class ParticipantRepos(FestDataContext context, ILogger<ParticipantRepos> logger) : IParticipantRepos<Participant>
    {
        private readonly FestDataContext _context = context ;
        private readonly ILogger<ParticipantRepos> _logger = logger;

        public void AddParticipant(List<Participant> participant) {
            try
            {
                _context.Participant.AddRange(participant);
                _context.SaveChanges();
            }
            catch (Exception ex) {
                _logger.LogError(ex, "AddParticipant Error");
            }
        }
        public void CheckParticipant(Participant participant) {
            
        
        }
         public List<Participant>? GetParticipants(Guid performanceId) {
              return  _context.Participant.Where(dt=> dt.PerformanceId == performanceId).ToList();
        }

    }
}
