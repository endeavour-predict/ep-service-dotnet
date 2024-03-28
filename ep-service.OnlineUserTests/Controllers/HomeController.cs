using Microsoft.AspNetCore.Mvc;
using ep_service.OnlineTests.Models;
using System.Diagnostics;

namespace ep_service.OnlineTests.Controllers
{
    public class HomeController : Controller
    {
        

        public HomeController()
        {
            
        }

        public IActionResult Index()
        {
            return View();
        }
        
    }
}
