using Fest_form.data.Entity;

namespace Fest_form.Repositories.ParticipantRepos
{
    public interface IParticipantRepos<T> where T : class
    {
        public void AddParticipant(List<Participant> participant);
        public List<Participant>? GetParticipants(Guid performanceId);
    }
}
