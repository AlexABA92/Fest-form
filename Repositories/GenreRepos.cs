using Fest_form.data;
using Fest_form.data.Entity;
using Fest_form.Interface;

namespace Fest_form.Repositories
{
    public class GenreRepos(FestDataContext context) : IGenreRepos<Genre>
    {
        private readonly FestDataContext _dataContext = context;
        public List<Genre> GetGenreItems() => _dataContext.Genres.ToList();
      
    }
}
