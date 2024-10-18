
using Fest_form.data.Entity;
using Fest_form.GlobalData.Collections;
using Fest_form.Interface;
using Fest_form.Models;
using Fest_form.Services;

using Google.Apis.Drive.v3;

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

        public RegistrationController(ILogger<RegistrationController> logger,
            IGenreRepos<Genre> genreRepos,
            ICategory<Category> category,
            IParticipantsNumber<ParticipantsNumber> participantsNumber)
        {
            _logger = logger;
            _genreRepos = genreRepos;
            _categoryRepos = category;
            _participantsNumber = participantsNumber;
            }

        public IActionResult Index(string? num)
        {
            
            HttpContext.Session.SetString("path", Request.Path);
            var langValue = Request.Cookies["Language"];
            
            ViewData["GenreList"] = _genreRepos.GetGenreItems(); ;
            ViewData["CategoryList"] = _categoryRepos.GetCategories(); 
            ViewData["ParticipantsNumberList"] = _participantsNumber.GetList();
            if (!string.IsNullOrEmpty(num)) {
                ViewData["num"] = num;
            }
            if (!string.IsNullOrEmpty(langValue)) {
                ViewData["lang"] = langValue;
            }
            var model = new DanceTeam();
            model.TeamLeader = new Person() {PersonFatherName = "lol" };
            
            return View(model);
          
        }

        [HttpPost]
        public IActionResult Index(DanceTeam team) {
            HttpContext.Session.SetString("path", Request.Path);
            if (!ModelState.IsValid)
            {
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
