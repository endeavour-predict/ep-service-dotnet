// we have to alias the calculatoir DLLs because they all contain things in the same namespace,
// see: https://stackoverflow.com/questions/9194495/type-exists-in-2-assemblies
extern alias qrisk;
extern alias qdiab;
extern alias qfrac;

using Microsoft.VisualBasic;
using System.Reflection;
using System.Text.Json.Serialization;
using static Core.EPStandardDefinitions;

namespace EP_API.Models
{

    /// <summary>
    /// Calculation Result Model
    /// Contains the results from a single Calculator Engine
    /// </summary>
    public class EngineResultModel
    {

        public string EngineName { get; set; }


        public string EngineVersion { get; set; }

        /// <summary>
        /// List of Prediction Scores
        /// </summary>
        public List<PredictionResult> Results { get; set; } = new List<PredictionResult>();

        /// <summary>
        /// List of DataQuality checks, showing any substituted values
        /// </summary>
        public List<DataQuality> Quality { get; set; } = new List<DataQuality>();

        /// <summary>
        /// Metadata about this call to the Engine
        /// </summary>
        public Meta CalculationMeta { get; set; } = new Meta();

        /// <summary>
        /// The data provided by the user for this Prediction. Identifiable fields are stripped of data and marked as **PI** 
        /// </summary>
        public APIInputModel InputModel { get; set; }


        /// <summary>
        /// Contains the ID, Score and Typical score (if available)
        /// </summary>
        public class PredictionResult
        {
            public PredictionResult()
            {
            }
            [JsonPropertyName("@id")]
            public Uri id { get; set; }
            public double score { get; set; }
            public double? typicalScore { get; set; }
            public int predictionYears { get; set; }
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


        /// <summary>
        /// Contains MetaData about the prediction: timings, versions, etc
        /// </summary>
        public class Meta
        {
            public Meta()
            {
            }
            
            /// <summary>
            /// Status result from the Calculator.
            /// </summary>
            public ResultStatus EngineResultStatus { get; set; }
            /// <summary>
            /// Used to hold extra information if the Calculator was unable to produce a Score (reason for failure).
            /// </summary>
            public ReasonInvalid EngineResultStatusReason { get; set; }
        }





        // Constructors, one for each supported engine

        public EngineResultModel(qrisk::QRISK3Engine.QRiskCVDResults calcResult, QRiskInputModel calcInputModel)
        {            
            var globals = new Globals();
            var engineName = EPStandardDefinitions.Engines.QRisk3.ToString();
            Engine engine = GetEngine(globals, engineName);
            this.EngineName = engine.EngineName;
            this.EngineVersion = engine.EngineVersion;

            var meta = new Meta();
            meta.EngineResultStatus = (EPStandardDefinitions.ResultStatus)calcResult.resultStatus;
            meta.EngineResultStatusReason = (EPStandardDefinitions.ReasonInvalid)calcResult.reason;
            this.CalculationMeta = meta;

            // map the calculator specific results output to the generic API engine results model
            // QRisk3 returns 2 scores 
            var result1 = new PredictionResult();
            result1.id = new Uri(engine.EngineUri);
            result1.score = calcResult.score; 
            result1.typicalScore = calcResult.typical_score;
            result1.predictionYears = 10;            
            this.Results.Add(result1);

            
            if (calcResult.qHeartAge != null)
            {
                var result2 = new PredictionResult();
                result2.id = new Uri(engine.EngineUri + "HeartAge");
                result2.score = (double)calcResult.qHeartAge;
                result2.predictionYears = 10;
                this.Results.Add(result2);
            }            
            

            var smokingQuality = new DataQuality();
            smokingQuality.Parameter = "smokingStatus";
            smokingQuality.Quality = (ParameterQuality)Enum.Parse(typeof(ParameterQuality), calcResult.DataQuality.smokingStatus.data.ToString());
            smokingQuality.SubstituteValue = calcResult.DataQuality.smokingStatus.substitute_value;
            Quality.Add(smokingQuality);

            var sbpQuality = new DataQuality();
            sbpQuality.Parameter = "systolicBloodPressureMean";
            sbpQuality.Quality = (ParameterQuality)Enum.Parse(typeof(ParameterQuality), calcResult.DataQuality.sbp.data.ToString());
            sbpQuality.SubstituteValue = calcResult.DataQuality.sbp.substitute_value.ToString();
            Quality.Add(sbpQuality);

            var sbp5sQuality = new DataQuality();
            sbp5sQuality.Parameter = "systolicBloodPressureStDev";
            sbp5sQuality.Quality = (ParameterQuality)Enum.Parse(typeof(ParameterQuality), calcResult.DataQuality.sbps5.data.ToString());
            sbp5sQuality.SubstituteValue = calcResult.DataQuality.sbps5.substitute_value.ToString();
            Quality.Add(sbp5sQuality);

            var ratioQuality = new DataQuality();
            ratioQuality.Parameter = "ratio";
            ratioQuality.Quality = (ParameterQuality)Enum.Parse(typeof(ParameterQuality), calcResult.DataQuality.ratio.data.ToString());
            ratioQuality.SubstituteValue = calcResult.DataQuality.ratio.substitute_value.ToString();
            Quality.Add(ratioQuality);

            var ethnicityQuality = new DataQuality();
            ethnicityQuality.Parameter = "ethnicity";
            ethnicityQuality.Quality = (ParameterQuality)Enum.Parse(typeof(ParameterQuality), calcResult.DataQuality.ethnicity.data.ToString());
            ethnicityQuality.SubstituteValue = calcResult.DataQuality.ethnicity.substitute_value.ToString();
            Quality.Add(ethnicityQuality);


            var bmiQuality = new DataQuality();
            bmiQuality.Parameter = "BMI";
            bmiQuality.Quality = (ParameterQuality)Enum.Parse(typeof(ParameterQuality), calcResult.DataQuality.bmi.data.ToString());
            bmiQuality.SubstituteValue = calcResult.DataQuality.bmi.substitute_value.ToString();
            Quality.Add(bmiQuality);

            var townsendQuality = new DataQuality();
            townsendQuality.Parameter = "townsendScore";
            townsendQuality.Quality = (ParameterQuality)Enum.Parse(typeof(ParameterQuality), calcResult.DataQuality.townsend.data.ToString());
            townsendQuality.SubstituteValue = calcResult.DataQuality.townsend.substitute_value.ToString();
            Quality.Add(townsendQuality);

            // use the calculator specific input model to load back into the API input model, this will show the data used in the calc, and nothing more.
            PropertyInfo[] calcInputProperties = typeof(QRiskInputModel).GetProperties();
            PropertyInfo[] apiInputProperties = typeof(APIInputModel).GetProperties();
            APIInputModel apiInputModel = new APIInputModel();
            foreach (PropertyInfo calcProperty in calcInputProperties)
            {
                // map the calc param to the genmeric input param, showing the ones used
                var apiInputProperty = apiInputProperties.Where(p => p.PropertyType.Name == calcProperty.Name).SingleOrDefault();
                if (apiInputProperty != null)
                {                    
                    calcProperty.SetValue(calcInputModel, apiInputProperty.GetValue(apiInputModel));
                }
            }
            this.InputModel = apiInputModel;

        }


        public EngineResultModel(qdiab::QDiabetesEngine.QDiabetesResults calcResult, QDiabetesInputModel calcInputModel)
        {
            var globals = new Globals();
            var engineName = EPStandardDefinitions.Engines.QDiabetes.ToString();
            Engine engine = GetEngine(globals, engineName);            
            this.EngineName = engine.EngineName;
            this.EngineVersion = engine.EngineVersion;

            var meta = new Meta();
            meta.EngineResultStatus = (EPStandardDefinitions.ResultStatus)calcResult.resultStatus;
            meta.EngineResultStatusReason = (EPStandardDefinitions.ReasonInvalid)calcResult.reason;
            this.CalculationMeta = meta;

            var result1 = new PredictionResult();
            result1.id = new Uri(engine.EngineUri);
            result1.score = calcResult.patient_score;
            result1.typicalScore = calcResult.reference_score;
            result1.predictionYears = 10;
            this.Results.Add(result1);

            var smokingQuality = new DataQuality();
            smokingQuality.Parameter = "smokingStatus";
            smokingQuality.Quality = (ParameterQuality)Enum.Parse(typeof(ParameterQuality), calcResult.dataQuality.smokingStatus.data.ToString());
            smokingQuality.SubstituteValue = calcResult.dataQuality.smokingStatus.substitute_value;
            Quality.Add(smokingQuality);

            var ethnicityQuality = new DataQuality();
            ethnicityQuality.Parameter = "ethnicity";
            ethnicityQuality.Quality = (ParameterQuality)Enum.Parse(typeof(ParameterQuality), calcResult.dataQuality.ethnicity.data.ToString());
            ethnicityQuality.SubstituteValue = calcResult.dataQuality.ethnicity.substitute_value.ToString();
            Quality.Add(ethnicityQuality);


            var bmiQuality = new DataQuality();
            bmiQuality.Parameter = "BMI";
            bmiQuality.Quality = (ParameterQuality)Enum.Parse(typeof(ParameterQuality), calcResult.dataQuality.bmi.data.ToString());
            bmiQuality.SubstituteValue = calcResult.dataQuality.bmi.substitute_value.ToString();
            Quality.Add(bmiQuality);

            var townsendQuality = new DataQuality();
            townsendQuality.Parameter = "townsendScore";
            townsendQuality.Quality = (ParameterQuality)Enum.Parse(typeof(ParameterQuality), calcResult.dataQuality.town.data.ToString());
            townsendQuality.SubstituteValue = calcResult.dataQuality.town.substitute_value.ToString();
            Quality.Add(townsendQuality);

            // use the calculator specific input model to load back into the API input model, this will show the data used in the calc, and nothing more.
            PropertyInfo[] calcInputProperties = typeof(QDiabetesInputModel).GetProperties();
            PropertyInfo[] apiInputProperties = typeof(APIInputModel).GetProperties();
            APIInputModel apiInputModel = new APIInputModel();
            foreach (PropertyInfo calcProperty in calcInputProperties)
            {
                // map the calc param to the genmeric input param, showing the ones used
                var apiInputProperty = apiInputProperties.Where(p => p.PropertyType.Name == calcProperty.Name).SingleOrDefault();
                if (apiInputProperty != null)
                {
                    calcProperty.SetValue(calcInputModel, apiInputProperty.GetValue(apiInputModel));
                }
            }
            this.InputModel = apiInputModel;

        }


        public EngineResultModel(qfrac::QFractureEngine.QFractureResults calcResult, QFractureInputModel calcInputModel)
        {
            var globals = new Globals();
            var engineName = EPStandardDefinitions.Engines.QFracture.ToString();
            Engine engine = GetEngine(globals, engineName);
            this.EngineName = engine.EngineName;
            this.EngineVersion = engine.EngineVersion;

            var meta = new Meta();
            meta.EngineResultStatus = (EPStandardDefinitions.ResultStatus)calcResult.resultStatus;
            meta.EngineResultStatusReason = (EPStandardDefinitions.ReasonInvalid)calcResult.reason;
            this.CalculationMeta = meta;

            // map the calculator specific results output to the generic API engine results model
            // QFrac returns two scores
            var result1 = new PredictionResult();
            result1.id = new Uri(engine.EngineUri);
            result1.score = calcResult.fracture4_score;
            result1.typicalScore = calcResult.reference_fracture4_score;
            result1.predictionYears = calcInputModel.predictionYears;
            this.Results.Add(result1);

            var result2 = new PredictionResult();
            result2.id = new Uri(engine.EngineUri + "Hip");
            result2.score = calcResult.nof_score;
            result2.typicalScore = calcResult.reference_nof_score;
            result2.predictionYears = calcInputModel.predictionYears;
            this.Results.Add(result2);

            var smokingQuality = new DataQuality();
            smokingQuality.Parameter = "smokingStatus";
            smokingQuality.Quality = (ParameterQuality)Enum.Parse(typeof(ParameterQuality), calcResult.dataQuality.smokingStatus.data.ToString());
            smokingQuality.SubstituteValue = calcResult.dataQuality.smokingStatus.substitute_value;
            Quality.Add(smokingQuality);

            var ethnicityQuality = new DataQuality();
            ethnicityQuality.Parameter = "ethnicity";
            ethnicityQuality.Quality = (ParameterQuality)Enum.Parse(typeof(ParameterQuality), calcResult.dataQuality.ethnicity.data.ToString());
            ethnicityQuality.SubstituteValue = calcResult.dataQuality.ethnicity.substitute_value.ToString();
            Quality.Add(ethnicityQuality);


            var bmiQuality = new DataQuality();
            bmiQuality.Parameter = "BMI";
            bmiQuality.Quality = (ParameterQuality)Enum.Parse(typeof(ParameterQuality), calcResult.dataQuality.bmi.data.ToString());
            bmiQuality.SubstituteValue = calcResult.dataQuality.bmi.substitute_value.ToString();
            Quality.Add(bmiQuality);

            // use the calculator specific input model to load back into the API input model, this will show the data used in the calc, and nothing more.
            PropertyInfo[] calcInputProperties = typeof(QFractureInputModel).GetProperties();
            PropertyInfo[] apiInputProperties = typeof(APIInputModel).GetProperties();
            APIInputModel apiInputModel = new APIInputModel();
            foreach (PropertyInfo calcProperty in calcInputProperties)
            {
                // map the calc param to the genmeric input param, showing the ones used
                var apiInputProperty = apiInputProperties.Where(p => p.PropertyType.Name == calcProperty.Name).SingleOrDefault();
                if (apiInputProperty != null)
                {
                    calcProperty.SetValue(calcInputModel, apiInputProperty.GetValue(apiInputModel));
                }
            }
            this.InputModel = apiInputModel;

        }




        private static Engine GetEngine(Globals globals, string engineName)
        {
            var engine = globals.AvailableEngines.Where(engine => engine.EngineName == engineName).SingleOrDefault();
            if (engine == null) throw new ApplicationException("No engine was found with the name: " + engineName);
            return engine;
        }



    }


}



