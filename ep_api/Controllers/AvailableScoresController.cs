using ep_models;
using Microsoft.AspNetCore.Mvc;


namespace EP_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class AvailableScoresController : ControllerBase
    {
        
        public AvailableScoresController()
        {
        }

        
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpGet(Name = "GetAvailableScores")]
        public ActionResult<AvailableScoresModel> Get()
        {                        
            var model = new AvailableScoresModel();
            var globals = new Globals();
            foreach(var engine in globals.AvailableEngines)
            {
                model.Scores.Add(new Score { EngineName = engine.EngineName, EngineVersion = engine.EngineVersion, EngineUri = engine.EngineUri });
            }            
            
            return model;
        }
    }
}