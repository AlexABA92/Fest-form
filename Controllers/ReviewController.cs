using Fest_form.data;
using Fest_form.data.Entity;
using Fest_form.Repositories.DeanseTeamRepos;

using Microsoft.AspNetCore.Mvc;

namespace Fest_form.Controllers
{
    public class ReviewController(IDanceTeamRepos<DanceTeam> danceTeamRepos) : Controller
    {
        private readonly IDanceTeamRepos<DanceTeam> _danceTeamRepos = danceTeamRepos;

        public IActionResult Index()
        {
            var list = _danceTeamRepos.GetTeamList();
            return View(list);
        }
    }
}
