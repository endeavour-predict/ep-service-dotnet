using Microsoft.AspNetCore.Mvc;
using ep_models;

namespace EP_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PredictionController : ControllerBase
    {        
        public PredictionController()
        {
        }
        /// <summary>
        /// Return EP Scores
        /// </summary>        
        /// <response code="200">200 is returned if a Prediction score has been calculated.</response>
        /// <response code="400">400 is returned if input cannot be processed (for example if the Age is out of range). Details will be in the returned response body.</response>                
        [Produces("application/json")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [HttpPost(Name = "GetPrediction")]
        public ActionResult<PredictionModel> Post(EPInputModel inputModel)
        {                        
            return new ep_service.PredictionService().GetScore(inputModel);
        }
    }
}