using Fest_form.data.Entity;
namespace Fest_form.Interface
{
    public interface IGenreRepos<Genre> 
    {
        public List<Genre> GetGenreItems();
    }
}
