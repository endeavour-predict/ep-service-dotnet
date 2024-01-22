using ep_service.Models;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using ep_service.OnlineTests.Models;
using System.Diagnostics;
using System.Security.Principal;
using System.Text.Json;

namespace ep_service.OnlineTests.Controllers
{
    public class GetScoreController : Controller
    {
        private readonly ILogger<GetScoreController> _logger;

        public GetScoreController(ILogger<GetScoreController> logger)
        {
            _logger = logger;
        }

        [HttpPost]
        public IActionResult Index([FromForm] string inputModelJSON)
        {
            // try and deserialise the inputput model string as a EPInputModel       
            // JsonSerializer.Deserialize<EPInputModel>(inputModelJSON);
            EPInputModel inputModel  = JsonConvert.DeserializeObject<EPInputModel>(inputModelJSON);

            var service = new ep_service.PredictionService();
            var result = service.GetScore(inputModel);
       
            return View(result);
        }

        public IActionResult Privacy()
        {
            return View();
        }

        [ResponseCache(Duration = 0, Location = ResponseCacheLocation.None, NoStore = true)]
        public IActionResult Error()
        {
            return View(new ErrorViewModel { RequestId = Activity.Current?.Id ?? HttpContext.TraceIdentifier });
        }
    }
}
