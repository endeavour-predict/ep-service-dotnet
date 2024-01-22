
using ep_service.Models;
using System.Text.Json.Serialization;

namespace ep_service.Models
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
        /// Metadata about this call
        /// </summary>
        public Meta Meta { get; set; } = new Meta();

        /// <summary>
        /// The data provided by the user for this Prediction.
        /// </summary>
        public EPInputModel EPInputModel { get; set; }

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
public class Meta
{
    public Meta()
    {
    }
    /// <summary>
    /// Build version of the Service.
    /// </summary>
    public string ServiceVersion { get; set; }
    
    /// <summary>
    /// ISO DateTime (UTC) that the Service was invoked
    /// </summary>
    public DateTime RequestTimeStampUTC { get; set; }    
}

