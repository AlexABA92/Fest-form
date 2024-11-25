using Fest_form.data.Entity;

namespace Fest_form.Repositories.FileRepos
{
    public interface IFileRepos
    {
        public Task FileSender(DanceTeam team, List<IFormFile> files);
        public void TeamInfoMail(DanceTeam team);
    }
}
