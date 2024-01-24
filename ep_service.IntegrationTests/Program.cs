using ep_models;
using Newtonsoft.Json;
using RestSharp;
using System.Security.Principal;
using static System.Net.Mime.MediaTypeNames;
using Formatting = Newtonsoft.Json.Formatting;

if (args.Length != 2)
{
    Console.WriteLine("Usage: ep_service.IntegrationTests.exe <pathToJSONFiles> <EP_ApiEndPointURL>");
    return;
}
if (!Directory.Exists(args[0]))
{
    Console.WriteLine("Cannot find directory.");
    return;
}
Console.WriteLine("Batch processing...");

List<string> resultsLines = new List<string>();
resultsLines.Add("---------------------------");
resultsLines.Add("STARTING");
resultsLines.Add("---------------------------");
int testsRun = 0;

var resultFile = args[0] + @"/results_" + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".log";

var serializer = new Newtonsoft.Json.JsonSerializer();
EPInputModel epInputModel = new EPInputModel();
PredictionModel expectedPredictionModel = new PredictionModel();
bool anythingFailed = false;

string testFilePath = args[0];
string endpoint = args[1];

foreach (var inputFile in Directory.GetFiles(testFilePath))
{
    if (inputFile=="") continue;
    if (!inputFile.Contains(".input."))
    {
        Console.WriteLine("Skipping File: " + inputFile);
        continue;
    }

    Console.WriteLine("Testing file: " + inputFile);
    resultsLines.Add("Testing : " + inputFile);

    // make sure we have a corresponding output file (expected results)
    var outputFile = inputFile.Replace("input", "output");
    if (!File.Exists(outputFile))
    {
        Console.WriteLine("ERROR. outputFile file not found: " + outputFile);
    }


    // get the expected result from the output file
    
    Stream outputFileStream = File.OpenRead(outputFile);
    using (var sr = new StreamReader(outputFileStream))
    {
        using (var jsonTextReader = new JsonTextReader(sr))
        {
            expectedPredictionModel = serializer.Deserialize<PredictionModel>(jsonTextReader);
        }
    }



    // call the API for the actual result

    string jsonInput = File.ReadAllText(inputFile);

    var client = new RestClient(endpoint);
    var request = new RestRequest("Prediction", Method.Post);
    request.AddJsonBody(jsonInput);
    var response = client.ExecutePost(request);
    PredictionModel actualPredictionModel = JsonConvert.DeserializeObject<PredictionModel>(response.Content);

    // compare results and write out to file/ console
    var actual_serviceResult = actualPredictionModel;
    var actual_engineScores = actual_serviceResult.EngineResults.Where(p => p.EngineName == Core.EPStandardDefinitions.Engines.QRisk3).Single();
    // having problems with the @id field in the response, so just going ot pull them out by index here
    //var actual_QRisk3Score = actual_engineScores.Results.First().Where(p => p.id.ToString() == Globals.QRiskScoreUri).Single();
    //var actual_QRisk3HeartAgeScore = actual_engineScores.Results.Where(p => p.id.ToString() == Globals.QRiskScoreUri + "HeartAge").SingleOrDefault();
    var actual_QRisk3Score = actual_engineScores.Results[0];
    var actual_QRisk3HeartAgeScore = actual_engineScores.Results[1];

    var actual_Meta = actual_engineScores.CalculationMeta;

    var expected_serviceResult = expectedPredictionModel;
    var expected_engineScores = expected_serviceResult.EngineResults[0];
    var expected_QRisk3Score = expected_engineScores.Results[0];
    var expected_QRisk3HeartAgeScore = expected_engineScores.Results[1];
    var expected_Meta = expected_engineScores.CalculationMeta;


    // Test 1 - compare QRisk3 scores    
    if (expected_QRisk3Score.score != actual_QRisk3Score.score)
    {
        resultsLines.Add("FAIL: " + inputFile + " -- expected_QRisk3Score.score: " + expected_QRisk3Score.score + " : actual_QRisk3Score.score" + actual_QRisk3Score.score);
        anythingFailed = true;
    }

    // Test 2 - compare QRisk3 HeartAge scores    
    // we don't always get a heart age score (like when CVD = true) so we need to check whether we're expecting one
    if (expected_QRisk3HeartAgeScore != null)
    {
        if (expected_QRisk3HeartAgeScore.score != actual_QRisk3HeartAgeScore.score)
        {
            resultsLines.Add("FAIL: " + inputFile + " -- expected_QRisk3HeartAgeScore.score: " + expected_QRisk3HeartAgeScore.score + " : actual_QRisk3HeartAgeScore.score" + actual_QRisk3HeartAgeScore.score);
            anythingFailed = true;
        }
    }

    // Test 3 - Compare resultstatus 
    if (expected_Meta.EngineResultStatus != actual_Meta.EngineResultStatus)
    {
        resultsLines.Add("FAIL: " + inputFile + " -- expected_Meta.EngineResultStatus: " + expected_Meta.EngineResultStatus + " : actual_Meta.EngineResultStatus" + actual_Meta.EngineResultStatus);
        anythingFailed = true;
    }

    // Test 4 - Compare resultreason
    if (expected_Meta.EngineResultStatusReason != actual_Meta.EngineResultStatusReason)
    {
        resultsLines.Add("FAIL: " + inputFile + " -- expected_Meta.EngineResultStatusReason: " + expected_Meta.EngineResultStatusReason + " : actual_Meta.EngineResultStatusReason" + actual_Meta.EngineResultStatusReason);
        anythingFailed = true;
    }
    testsRun++;
    
}
resultsLines.Add("---------------------------");
resultsLines.Add("FINISHED");
resultsLines.Add("---------------------------");
resultsLines.Add("Tests Run: " + testsRun);
resultsLines.Add("---------------------------");

if (anythingFailed)
{
    resultsLines.Add("FINAL RESULT: FAIL");
}
else
{
    resultsLines.Add("FINAL RESULT: OK");
}

File.WriteAllLines(resultFile, resultsLines);

