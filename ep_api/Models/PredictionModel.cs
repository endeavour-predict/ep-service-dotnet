extern alias qrisk;
extern alias qdiab;
extern alias qfrac;
using System.Text.Json.Serialization;

namespace EP_API.Models
{
    public class PredictionModel
    {

        /// <summary>
        /// List of Engine Results
        /// </summary>
        public List<EngineResultModel> EngineResults { get; set; } = new List<EngineResultModel>();


        public PredictionModel()
        {
            
        }

   
        
        
        
        /// <summary>
        /// Metadata about this call to the API
        /// </summary>
        public ApiMeta ApiMeta { get; set; } = new ApiMeta();

        /// <summary>
        /// The data provided by the user for this Prediction. Identifiable fields are stripped of data and marked as **PI** 
        /// </summary>
        public APIInputModel ApiInputModel { get; set; }

    }

    public enum ParameterQuality
    {
           OK, MISSING, OUT_OF_RANGE
    }


}


public class DataQuality
{

    /// <summary>
    /// Name of the Parameter used by the Calculator
    /// </summary>
    public string Parameter { get; set; } = "";

    /// <summary>
    /// Was the parameter provided OK, out of range etc?
    /// </summary>
    public ParameterQuality Quality { get; set; }
    
    /// <summary>
    /// The value used by the calculator if the value provided was substituted
    /// </summary>
    public string SubstituteValue { get; set; } = "";

    /// <summary>
    /// Quality report from the calculator, showing any substituted values for missing or out of range parameters
    /// </summary>
    public DataQuality()
    { 
    }

}

///// <summary>
///// Contains the ID, Score and Typical score (if available)
///// </summary>
//public class PredictionResult
//{

//    public PredictionResult()
//    {        
//    }

//    [JsonPropertyName("@id")]
//    public Uri id { get; set; } 
//    public double score { get; set; }
//    public double? typicalScore { get; set; }
//    public int predictionYears { get; set; }

//}



/// <summary>
/// Contains MetaData about the prediction: timings, versions, etc
/// </summary>
public class ApiMeta
{
    public ApiMeta()
    {
    }
    /// <summary>
    /// Build version of the API.
    /// </summary>
    public string ApiVersion { get; set; }
    /// <summary>
    /// ISO DateTime (UTC) that the API was invoked
    /// </summary>
    public DateTime ApiTimeStampUTC { get; set; }    
}

