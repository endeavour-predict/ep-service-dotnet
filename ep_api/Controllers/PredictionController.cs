using Microsoft.AspNetCore.Mvc;
using EP_API.Models;


namespace EP_API.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class PredictionController : ControllerBase
    {

        public string apiVersion { get { return "EP API v0.0.1"; } }

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
        public ActionResult<PredictionModel> Post(APIInputModel inputModel)
        {
            PredictionModel outputModel;
            var calculationService = new CalculationService();
            calculationService.PerformCalculations(inputModel, out outputModel);

            outputModel.ApiMeta.ApiVersion = apiVersion;
            outputModel.ApiMeta.ApiTimeStampUTC = DateTime.UtcNow;
            outputModel.ApiInputModel = inputModel;
            return outputModel;
        }
    }
}