
using Fest_form.data.Entity;
using Fest_form.GlobalData.Collections;
using Fest_form.Interface;
using Fest_form.Models;

using Microsoft.Extensions.Caching.Memory;
using Microsoft.AspNetCore.Mvc;
using System.Diagnostics;
using System.Globalization;

using Fest_form.GlobalData.Enum;
using Fest_form.Services.Bucket;
using Fest_form.Services.MailSend;
using Newtonsoft.Json;
using Fest_form.Repositories;
using Fest_form.Repositories.FileRepos;
using Fest_form.Repositories.DeanseTeamRepos;


namespace Fest_form.Controllers
{
    
    public class RegistrationController : Controller
    {
        private readonly ILogger<RegistrationController> _logger;
        private readonly IGenreRepos<Genre> _genreRepos;
        private readonly ICategory<Category> _categoryRepos;
        private readonly IParticipantsNumber<ParticipantsNumber> _participantsNumber;
        private readonly IMemoryCache _cache;
        private readonly IDanceTeamRepos<DanceTeam> _danceTeamRepos;
       
        
        private readonly IFileRepos _fileRepos;

        public RegistrationController(ILogger<RegistrationController> logger,
            IGenreRepos<Genre> genreRepos,
            ICategory<Category> category,
            IParticipantsNumber<ParticipantsNumber> participantsNumber,
            IMemoryCache memoryCache,
            IDanceTeamRepos<DanceTeam> danceTeamRepos,
            IFileRepos fileRepos
            )
        {
            _logger = logger;
            _genreRepos = genreRepos;
            _categoryRepos = category;
            _participantsNumber = participantsNumber;
            _cache = memoryCache;
            _danceTeamRepos = danceTeamRepos;
            _fileRepos = fileRepos;
        }
        private const long MaxFileSize = 25 * 1024 * 1024; // 25MB in bytes
       
        public IActionResult Index(string? num)
        {
            try
            {
                HttpContext.Session.SetString("path", Request.Path);
                var langValue = Request.Cookies["Language"];

                if (!_cache.TryGetValue("GenreList", out List<Genre> genreList))
                {
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
                
                if (!string.IsNullOrEmpty(num))
                {
                    ViewData["num"] = num;
                }
                if (!string.IsNullOrEmpty(langValue))
                {
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

                        CategoryId = -1,
                        GenreId = -1,
                        ParticipantsNumberId = -3,

                        ParticipantsNameList = new ParticipantsList
                        {
                            Person1 = new Person {
                                  PersonName = "Janet",
                                  PersonLastName = "Peltroo"
                             },
                            Person2 = new Person
                            {
                                  PersonName = "Carla",
                                  PersonLastName = "Black"
                            },
                            Person3 = new Person
                            {
                                  PersonName = "Shiba",
                                  PersonLastName = "Inno"
                            }
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
            catch (Exception ex) {
                _logger.LogError(ex, "Index Controler fatal error");
                return RedirectToAction("Error","Error");
            }
        }

        [HttpPost]
        public  IActionResult Index(DanceTeam team /*, List<IFormFile> files*/) {

            // "FileRequiredError"
            List<IFormFile> files = new List<IFormFile>();
            for (var i = 0; i < team.Performances.Count; i++) {
                if ((Request.Form.Files[$"Performances[{i}].PhonogramFileURL"] is { } file && file.Length > 0))
                {
                 
                    if (file.Length > MaxFileSize)
                    {
                        ModelState.AddModelError($"Performances_{i}_PhonogramFileURL", Resources.Resource.FileSizeError);
                        break;
                    }
                    var ext = Path.GetExtension(file.FileName);
                    var fileName = $"{team.TeamName}-{team.Performances[i].PerformanceName}{ext}";
                    team.Performances[i].PhonogramFileURL = Uri.EscapeDataString(fileName.Replace(" ", "_"));

                    files.Add(file);
                  
                }else ModelState.AddModelError($"Performances_{i}_PhonogramFileURL", Resources.Resource.FileRequiredError);
            }

            HttpContext.Session.SetString("path", Request.Path);
            if (!ModelState.IsValid)
            {
                ViewData["GenreList"] = _cache.Get<List<Genre>>("GenreList"); ;
                ViewData["CategoryList"] = _cache.Get<List<Category>>("CategoryList");
                ViewData["ParticipantsNumberList"] = _cache.Get<List<ParticipantsNumber>>("ParticipantsNumberList");
                return View(team);
            }
            try
            {
                // _fileRepos.FileSender(team, files);
                // _mail.SendMultipleEmailsAsync(team, files);
                //_danceTeamRepos.CreateTeam(team);

            }
            catch (Exception) {
                ViewData["MailSendError"] = "Mail Send Error";
                return View(team);
            }
            HttpContext.Session.SetString("team",JsonConvert.SerializeObject(team));
            return RedirectToAction("Success");
           
        }
        public IActionResult Success()
        {
            var  _dtJeson = HttpContext.Session.GetString("team");
            DanceTeam _dt = new DanceTeam();
            if (_dtJeson != null) {
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
                _dt.Performances[i].PerformanceGroup = categoryList?
                    .First(item => _dt.Performances[i].CategoryId == item.Id);
           };

            HttpContext.Session.SetString("path", Request.Path);
            return View(_dt);

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
