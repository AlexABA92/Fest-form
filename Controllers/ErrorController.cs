using Microsoft.AspNetCore.Mvc;

namespace Fest_form.Controllers
{
    public class ErrorController : Controller
    {
        [Route("Error")] 
        public IActionResult Index() 
        { return View(); }
        
        
        
        [Route("Error/500")]
        public IActionResult ServerError() 
        { 
            Response.StatusCode = 500;
            return View("ServerError"); 
        }
    }
}
