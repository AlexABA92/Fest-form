
using Fest_form.data.Entity;
using Fest_form.GlobalData.Collections;
using Fest_form.Interface;
using Fest_form.Models;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Globalization;


namespace Fest_form.Controllers
{
    
    public class RegistrationController : Controller
    {
        private readonly ILogger<RegistrationController> _logger;
        private readonly IGenreRepos<Genre> _genreRepos;
        private readonly ICategory<Category> _categoryRepos;
        private readonly IParticipantsNumber<ParticipantsNumber> _participantsNumber;
        private readonly IMemoryCache _cache;
        public RegistrationController(ILogger<RegistrationController> logger,
            IGenreRepos<Genre> genreRepos,
            ICategory<Category> category,
            IParticipantsNumber<ParticipantsNumber> participantsNumber,
            IMemoryCache memoryCache
            )
        {
            _logger = logger;
            _genreRepos = genreRepos;
            _categoryRepos = category;
            _participantsNumber = participantsNumber;
            _cache = memoryCache;
            }

        public IActionResult Index(string? num)
        {
            
            HttpContext.Session.SetString("path", Request.Path);
            var langValue = Request.Cookies["Language"];

            if (!_cache.TryGetValue("GenreList", out List<Genre> genreList)) {
                genreList = _genreRepos.GetGenreItems();
                _cache.Set("GenreList", genreList, TimeSpan.FromHours(1));
            }
            if (!_cache.TryGetValue("CategoryList", out List<Category> categoryList))
            {
                categoryList = _categoryRepos.GetCategories();
                _cache.Set("CategoryList", categoryList, TimeSpan.FromHours(1));
            }
            if (!_cache.TryGetValue("ParticipantsNumberList", out List<ParticipantsNumber> partNumb))
            {
                partNumb = _participantsNumber.GetList();
                _cache.Set("ParticipantsNumberList", partNumb, TimeSpan.FromHours(1));
            }
            ViewData["GenreList"] = genreList;
            ViewData["CategoryList"] = categoryList; 
            ViewData["ParticipantsNumberList"] = partNumb;
            if (!string.IsNullOrEmpty(num)) {
                ViewData["num"] = num;
            }
            if (!string.IsNullOrEmpty(langValue)) {
                ViewData["lang"] = langValue;
            }
           
            return View();
          
        }

        [HttpPost]
        public IActionResult Index(DanceTeam team, IFormFile file) {

            if (file == null) ModelState.AddModelError("", @Resources.Resource.AddTrack);
            

            HttpContext.Session.SetString("path", Request.Path);
            if (!ModelState.IsValid)
            {
                ViewData["GenreList"] = _cache.Get<List<Genre>>("GenreList"); 
                ViewData["CategoryList"] = _cache.Get<List<Category>>("CategoryList");
                ViewData["ParticipantsNumberList"] = _cache.Get<List<ParticipantsNumber>>("ParticipantsNumberList");
                return View(team);
            }
                return RedirectToAction("Success"); 
        }

        public IActionResult Success()
        {

            HttpContext.Session.SetString("path", Request.Path);
            return View();

        }


        public IActionResult ChangeLanguage(string lang) {
            string? returnUrl = HttpContext.Session.GetString("path") ?? "/Registration/Index";
            List<string> cultures = LanguageCollections.Cultures();
            if (!cultures.Contains(lang)) lang = "uk";

            CookieOptions options = new CookieOptions();
            options.Expires = DateTime.Now.AddDays(10);
            if (!string.IsNullOrEmpty(lang))
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture(lang);
                Thread.CurrentThread.CurrentUICulture = new CultureInfo(lang);
            }
            else
            {
                Thread.CurrentThread.CurrentCulture = CultureInfo.CreateSpecificCulture("uk");
                Thread.CurrentThread.CurrentUICulture = new CultureInfo("uk");
                lang = "uk";
            }
            Response.Cookies.Append("Language", lang);
            return Redirect(returnUrl);
        }

       
        //}

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

    }
}
