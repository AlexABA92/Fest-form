using Fest_form.data.Entity;

namespace Fest_form.Repositories.PersonRepos
{
    public interface IPersonRepos <T> 
    {
        public  List<Person> GetPersonList();
        public void CheckTeamForPerson(ref DanceTeam team, ref List<Person> list);
    }
}
