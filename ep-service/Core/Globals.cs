// we have to alias the engine DLLs because they all contain things in the same namespace,
// see: https://stackoverflow.com/questions/9194495/type-exists-in-2-assemblies
extern alias qrisk;
//extern alias qdiab;
//extern alias qfrac;
//extern alias qfracsd; // qfracture has a different DLL for ther StandardDefns, the other two don't!

using Core;

public class Globals
{
    public const string QDiabetesScoreUri   = "http://endhealth.info/im#QDiabetes";
    public const string QFractureScoreUri   = "http://endhealth.info/im#QFracture";
    public const string QRiskScoreUri       = "http://endhealth.info/im#Qrisk3";

    public List<Engine> AvailableEngines = new List<Engine>();


    public Globals()
    {
        this.AvailableEngines.Add(new Engine
            {   EngineName = EPStandardDefinitions.Engines.QRisk3.ToString(),
                EngineVersion = qrisk::QRISK3Engine.QRiskCVDAlgorithmCalculator.version(),
                EngineUri = "http://endhealth.info/im#Qrisk3"
            });

        //this.AvailableEngines.Add(new Engine
        //{
        //    EngineName = EPStandardDefinitions.Engines.QDiabetes.ToString(),
        //    EngineVersion = qdiab::QDiabetesEngine.QDiabetesAlgorithmCalculator.version(), 
        //    EngineUri = "http://endhealth.info/im#QDiabetes"
        //});

        //this.AvailableEngines.Add(new Engine
        //{
        //    EngineName = EPStandardDefinitions.Engines.QFracture.ToString(),
        //    EngineVersion = qfrac::QFractureEngine.QFractureAlgorithmCalculator.version(),
        //    EngineUri = "http://endhealth.info/im#QFracture"
        //});
    }

    


    public class Engine
    {
        public string EngineName { get; set; }
        public string EngineVersion { get; set; }
        public string EngineUri { get; set; }
    }

}