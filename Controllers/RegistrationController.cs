
using Fest_form.data.Entity;
using Fest_form.GlobalData.Collections;
using Fest_form.Interface;
using Fest_form.Models;
using Fest_form.Repositories.DeanseTeamRepos;
using Fest_form.Repositories.FileRepos;
using Fest_form.Repositories.ParticipantRepos;
using Fest_form.Repositories.PerformanceRepos;
using Fest_form.Repositories.PersonRepos;
using Fest_form.Services.MailSend;

using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Caching.Memory;

using Newtonsoft.Json;

using System.Diagnostics;
using System.Globalization;




namespace Fest_form.Controllers
{

    public class RegistrationController(
            ILogger<RegistrationController> logger,
            IGenreRepos<Genre> genreRepos,
            ICategory<Category> category,
            IParticipantsNumber<ParticipantsNumber> participantsNumber,
            IPerformanceRepos<Performance> performanceRepos,
            IParticipantRepos<Participant> participantRepos,
            IDanceTeamRepos<DanceTeam> danceTeamRepos,
            IMemoryCache memoryCache,
            IFileRepos fileRepos,
            IMailSend mailSend,
            IPersonRepos<Person> personRepos
) : Controller
    {
        private readonly ILogger<RegistrationController> _logger = logger;
        private readonly IGenreRepos<Genre> _genreRepos = genreRepos;
        private readonly ICategory<Category> _categoryRepos = category;
        private readonly IParticipantsNumber<ParticipantsNumber> _participantsNumber = participantsNumber;
        private readonly IDanceTeamRepos<DanceTeam> _danceTeamRepos = danceTeamRepos;
        private readonly IPerformanceRepos<Performance> _performanceRepos = performanceRepos;
        private readonly IMemoryCache _cache = memoryCache;
        private readonly IFileRepos _fileRepos = fileRepos;
        private readonly IMailSend _mailSend = mailSend;
        private readonly IPersonRepos<Person>_personRepos =personRepos;
        private readonly IParticipantRepos<Participant> _participantRepos = participantRepos;

        private const long MaxFileSize = 25 * 1024 * 1024; // 25MB in bytes


        public IActionResult Index(string? num)
        {
            try
            {
                var  _people = _personRepos.GetPersonList();
                if (_people != null && _people.Count > 0)
                    HttpContext.Session.SetString("people", JsonConvert.SerializeObject(_people));

                if (!string.IsNullOrEmpty(num))
                {
                    ViewData["num"] = num;
                }

                SetDataToView();

                // if return from success for adding performances
                if (HttpContext.Session.GetString("return") != null) {
                    var team = JsonConvert.DeserializeObject<DanceTeam>(HttpContext.Session.GetString("return") ?? "");
                    if(team != null)
                        return View(team);
                }

                return View();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Index Controler fatal error");
                return RedirectToAction("Error", "Error");
            }
        }
        

        [HttpPost]
        public IActionResult Index(DanceTeam team)
        {
            Debug.WriteLine(JsonConvert.SerializeObject(team));
            // set data for view select list
            void setViewDataCollection()
            {
                ViewData["GenreList"] = _cache.Get<List<Genre>>("GenreList"); ;
                ViewData["CategoryList"] = _cache.Get<List<Category>>("CategoryList");
                ViewData["ParticipantsNumberList"] = _cache.Get<List<ParticipantsNumber>>("ParticipantsNumberList");
            }
            try
            {
                //check file for validation and set encrypt name to url safety
                List < IFormFile > files = new List<IFormFile>();
                for (var i = 0; i < team.Performances.Count; i++)
                {
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

                    }
                    
                }

                // set path for language 
                HttpContext.Session.SetString("path", Request.Path);
                
                if (!ModelState.IsValid)
                {
                    setViewDataCollection();
                    return View(team);
                }

                // set person to list for chack persen in bd
                List<Person> _people = new();
                if (HttpContext.Session.GetString("people") != null)
                    _people = JsonConvert.DeserializeObject<List<Person>>(HttpContext.Session.GetString("people")!)!;

                if (_people.Count > 0)
                    _personRepos.CheckTeamForPerson(ref team, ref _people);

                _danceTeamRepos.CheckTeam(ref team);
              

                if (team.TeamId == Guid.Empty)
                {
                     
                    _fileRepos.TeamInfoMail(team);
                    _danceTeamRepos.CreateTeam(team);
                }
                else
                {
                    var per = _performanceRepos.GetPerformances(team.TeamId);
                    if (per != null)
                    {
                        var match = from perName in per
                                    join tPerfname in team.Performances
                                    on perName.PerformanceName
                                    equals tPerfname.PerformanceName
                                    select perName;
                        if (!match.Any())
                        {
                            team.Performances.ForEach(item => item.DanceTeamId = team.TeamId);
                            _performanceRepos.AddPerformance(team.Performances);
                            _participantRepos.AddParticipant(team.Performances.SelectMany(p => p.Participants).ToList());
                        }
                        else
                        {

                            foreach (var item in match)
                            {
                                var index = team.Performances.FindIndex(pitem => pitem.PerformanceName == item.PerformanceName);
                                ModelState.AddModelError($"Performances_{index}_PerformanceName", Resources.Resource.PerformanceUniqueError);
                            }

                            setViewDataCollection();
                            return View(team);
                        }
                    }
                    else
                    {
                        _performanceRepos.AddPerformance(team.Performances);
                        _participantRepos.AddParticipant(team.Performances.SelectMany(p => p.Participants).ToList());
                    }
                }


                //send files to bucket and mail
                if (files.Count > 0)
                    _fileRepos.TeamInfoMail(team, files);
                else _fileRepos.TeamInfoMail(team);

                if (HttpContext.Session.GetString("team") != null)
                    HttpContext.Session.Clear();
                HttpContext.Session.SetString("team", JsonConvert.SerializeObject(team));
                return RedirectToAction("index", "Success");
            }
            catch (Exception ex)
            {
                Debug.WriteLine(JsonConvert.SerializeObject(ex));
                _logger.LogError(ex, "Index(DanceTeam team ) Error");
                return View();
            }
        }
        


        public IActionResult ChangeLanguage(string lang)
        {
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
        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }

       
        private void SetDataToView() {

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

            
            if (!string.IsNullOrEmpty(langValue))
            {
                ViewData["lang"] = langValue;
            }
        }
    }
}
