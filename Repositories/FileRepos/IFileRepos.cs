using Fest_form.data.Entity;

namespace Fest_form.Repositories.FileRepos
{
    public interface IFileRepos
    {
        public Task TeamInfoMail(DanceTeam team, List<IFormFile>? files = null);
        //public void TeamInfoMail(DanceTeam team);
    }
}
