using Fest_form.data.Entity;

namespace Fest_form.Repositories.DeanseTeamRepos
{
    public interface IDanceTeamRepos<T>
    {
        public void CreateTeam(DanceTeam team);
        public void CheckTeam(ref DanceTeam team);
        public List<DanceTeam> GetTeamList();
    }
}
