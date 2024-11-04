
using Fest_form.data.Entity;
using Fest_form.GlobalData.Collections;
using Fest_form.Interface;
using Fest_form.Models;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Globalization;

using Fest_form.GlobalData.Enum;


namespace Fest_form.Controllers
{
    
    public class RegistrationController : Controller
    {
        private readonly ILogger<RegistrationController> _logger;
        private readonly IGenreRepos<Genre> _genreRepos;
        private readonly ICategory<Category> _categoryRepos;
        private readonly IParticipantsNumber<ParticipantsNumber> _participantsNumber;
        private readonly IMemoryCache _cache;
        private readonly IPerformanceValidationService _performanceValidationService;
        public RegistrationController(ILogger<RegistrationController> logger,
            IGenreRepos<Genre> genreRepos,
            ICategory<Category> category,
            IParticipantsNumber<ParticipantsNumber> participantsNumber,
            IMemoryCache memoryCache,
            IPerformanceValidationService performanceValidationService
            )
        {
            _logger = logger;
            _genreRepos = genreRepos;
            _categoryRepos = category;
            _participantsNumber = participantsNumber;
            _cache = memoryCache;
            _performanceValidationService = performanceValidationService;
            }

        public IActionResult Index(string? num)
        {
            
            HttpContext.Session.SetString("path", Request.Path);
            var langValue = Request.Cookies["Language"];

            if (!_cache.TryGetValue("GenreList", out List<Genre> genreList)) {
                genreList = _genreRepos.GetGenreItems();
                _cache.Set("GenreList", genreList, TimeSpan.FromHours(5));
            }
            if (!_cache.TryGetValue("CategoryList", out List<Category> categoryList))
            {
                categoryList = _categoryRepos.GetCategories();
                _cache.Set("CategoryList", categoryList, TimeSpan.FromHours(5));
            }
            if (!_cache.TryGetValue("ParticipantsNumberList", out List<ParticipantsNumber> partNumb))
            {
                partNumb = _participantsNumber.GetList();
                _cache.Set("ParticipantsNumberList", partNumb, TimeSpan.FromHours(5));
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


            var danceTeam = new DanceTeam
            {
                TeamId = Guid.NewGuid(),
                TeamName = "Rhythmic Dancers",
                TeamLeader = new Person
                {
                    PersonId = Guid.NewGuid(),
                    PersonName = "John",
                    PersonLastName = "Doe",
                    PersonFatherName = "Smith"
                },
                Mail = "teamleader@example.com",
                TeamPhoneNumber = "+123456789012",
                TeamLevel = TeamLevelEnum.Professional,
                Organization = "Local Dance Club",
                Performances = new List<Performance>
                {
                    new Performance
                    {
                        PerformanceId = Guid.NewGuid(),
                        DanceTeamId = Guid.NewGuid(), // Link this to the DanceTeam's TeamId as needed
                        PerformanceName = "Opening Dance",
                        ChoreographerDirector = new Person
                        {
                            PersonId = Guid.NewGuid(),
                            PersonName = "Jane",
                            PersonLastName = "Smith"
                        },
                        Concertmaster = new Person
                        {
                            PersonId = Guid.NewGuid(),
                            PersonName = "Emily",
                            PersonLastName = "White"
                        },
                        PerformanceGroup = new Category
                        {
                            Id = -1,
                            Name = "group1",
                            Description = "DescriptionGroup1"
                        },
                        Genre = new Genre
                        {
                            Id = -1,
                            Name = "Classic"
                        },
                        ParticipantsNumber = new ParticipantsNumber
                        {
                            Id = -4,
                            Name = "Ensemble(Small)"
                        },
                        ParticipantsNameList = new ParticipantsList
                        {
                            // Add participants as required
                        },
                        PerformanceTime = "10:30",
                        StartPoint = StartPointEnum.Point,
                        PhonogramFileURL = "",
                        YouTubeVideoURL = "https://youtube.com/example"
                    }
                }
            };


            return View(danceTeam);
          
        }

        [HttpPost]
        public IActionResult Index(DanceTeam team /*, List<IFormFile> files*/) {

            for (var i = 0; i < team.Performances.Count; i++) {
                if ((Request.Form.Files[$"Performances[{i}].PhonogramFileURL"] is { } file && file.Length > 0))
                {
                    team.Performances[i].PhonogramFileURL = file.FileName;
                }
            }

            Dictionary<string, string>? errors = new ();

               
           

                for (int i = 0; i < team.Performances.Count; i++)
                {
                    errors = _performanceValidationService.validationPerformance(team.Performances[i], i);
                    if (errors?.Count > 0)
                    {
                        foreach (var item in errors)
                        {
                            ModelState.AddModelError(item.Key, item.Value);
                        }
                    }
                }


                

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
