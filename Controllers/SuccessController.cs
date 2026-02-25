using Amazon.Runtime.Internal.Util;

using Fest_form.data.Entity;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

using Newtonsoft.Json;

namespace Fest_form.Controllers
{
    public class SuccessController(IMemoryCache cache) : Controller
    {
        private readonly IMemoryCache _cache = cache;
        public IActionResult Return()
        {
            DanceTeam? dt = JsonConvert.DeserializeObject<DanceTeam>( HttpContext.Session.GetString("team")?? "");
            if (dt != null) {

                dt.Performances = new List<Performance>() { new Performance() { ParticipantsNameList = new ParticipantsList()} };
                HttpContext.Session.SetString("return", JsonConvert.SerializeObject(dt));
            }
            return RedirectToAction("Index", "Registration");
        }
        public IActionResult Index()
        {
            var _dtJeson = HttpContext.Session.GetString("team");
            DanceTeam _dt = new DanceTeam();
            if (_dtJeson != null)
            {
                _dt = JsonConvert.DeserializeObject<DanceTeam>(_dtJeson);
            }
            var genreList = _cache.Get<List<Genre>>("GenreList");
            var categoryList = _cache.Get<List<Category>>("CategoryList");
            var ParticipantsNumberList = _cache.Get<List<ParticipantsNumber>>("ParticipantsNumberList");

            for (var i = 0; i < _dt?.Performances.Count; i++)
            {
                _dt.Performances[i].Genre = genreList?
                    .First(item => _dt.Performances[i].GenreId == item.Id);
                _dt.Performances[i].ParticipantsNumber = ParticipantsNumberList?
                    .First(item => _dt.Performances[i].ParticipantsNumberId == item.Id);
                _dt.Performances[i].Category = categoryList?
                    .First(item => _dt.Performances[i].CategoryId == item.Id);
            };

            HttpContext.Session.SetString("path", Request.Path);
            return View(_dt);

        }
    }
}
