using Fest_form.data;
using Fest_form.data.Entity;
using Fest_form.Interface;

namespace Fest_form.Repositories
{
    public class CategoryRepos(FestDataContext context) : ICategory<Category>
    {
        private readonly FestDataContext _context = context;
        public List<Category> GetCategories() => _context.Categories.ToList(); 
    }
}
