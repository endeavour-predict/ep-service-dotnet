using ep_models;
using Newtonsoft.Json;
using RestSharp;
using System.Security.Principal;
using static System.Net.Mime.MediaTypeNames;
using Formatting = Newtonsoft.Json.Formatting;

/*
    Examples of use:
    dotnet run "/Users/johncroasdale/Documents/IntegrationTestData" https://api.endeavourpredict.org/epredict/ <token>
    dotnet run TestPack https://api.endeavourpredict.org/epredict/ <token>
 */

Console.WriteLine("---------------------------");
if (args.Length != 3)
{
    Console.WriteLine("Windows Usage: ep_service.IntegrationTests.exe <pathToJSONFilesOr'TestPack'> <EP_ApiEndPointURL> <token>");
    Console.WriteLine("Mac/Linux Usage: (in bin folder) dotnet run <pathToJSONFilesOr'TestPack'> <EP_ApiEndPointURL> <token>");
    Console.WriteLine("");
    Console.WriteLine("First Parameter should be the path to the JSON input files, OR the word TestPack, if you want to run the examples from the test pack.");
    Console.WriteLine("Second Parameter should be the URL of the service, e.g..https://api.endeavourpredict.org/epredict/ ");
    Console.WriteLine("Third Parameter should be your token (see https://dev.endeavourpredict.org/) ");
    Console.WriteLine("---------------------------");
    return;
}

string testFilePath = args[0];
string endpoint = args[1];
string token = args[2];

bool testPackMode = false;

if (testFilePath.ToLower() == "testpack")
{
    Console.WriteLine("Running in TestPack mode.");
    testPackMode = true;
}
else
{
    if (!Directory.Exists(testFilePath))
    {
        Console.WriteLine("Cannot find directory: '" + testFilePath + "'");
        return;
    }
    Console.WriteLine("Running in directory mode. Found directory: '" + testFilePath + "'");
}

List<string> resultsLines = new List<string>();
Console.WriteLine("---------------------------");
Console.WriteLine("TESTS STARTING");
Console.WriteLine(DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss"));
Console.WriteLine("---------------------------");
int testsRun = 0;

var serializer = new Newtonsoft.Json.JsonSerializer();
EPInputModel epInputModel = new EPInputModel();
PredictionModel expectedPredictionModel = new PredictionModel();
bool anythingFailed = false;

if (testPackMode)
{
    var tests = test_packs.QRisk3_Resources.FileTests;

    foreach (var test in tests)
    {        
        var expected_serviceResult = test.PredictionModel;        
        string inputFile = JsonConvert.SerializeObject(test.EPInputModel);        
        RunTest(ref testsRun, expected_serviceResult, ref anythingFailed, endpoint, token, inputFile, test.TestName);
    }
}
else
{
    // folder mode
    foreach (var inputFile in Directory.GetFiles(testFilePath))
    {
        if (inputFile == "") continue;
        if (!inputFile.Contains(".input."))
        {
            continue;
        }
        
        // make sure we have a corresponding output file (expected results)
        var outputFile = inputFile.Replace("input", "output");
        if (!File.Exists(outputFile))
        {
            Console.WriteLine("ERROR. Correspnding outputFile file not found: " + outputFile);
            anythingFailed = true;
            continue;
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
        string jsonInput = File.ReadAllText(inputFile);
        RunTest(ref testsRun, expectedPredictionModel, ref anythingFailed, endpoint, token, jsonInput, inputFile);
    }
}


Console.WriteLine("---------------------------");
Console.WriteLine("TESTS FINISHED");
Console.WriteLine(DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss"));
Console.WriteLine("---------------------------");
Console.WriteLine("Number of Tests Run: " + testsRun);
Console.WriteLine("---------------------------");

if (anythingFailed)
{
    Console.WriteLine("FINAL RESULT: FAIL");
}
else
{
    Console.WriteLine("FINAL RESULT: OK");
}
Console.WriteLine("---------------------------");

static void RunTest(ref int testsRun, PredictionModel expectedPredictionModel, ref bool anythingFailed, string endpoint, string token, string jsonInput, string testName)
{
    Console.WriteLine("Testing : " + testName);

    // call the API for the actual result    
    var client = new RestClient(endpoint);
    var request = new RestRequest("Prediction", Method.Post);
    request.AddHeader("X-Gravitee-Api-Key", token);
    request.AddJsonBody(jsonInput);
    var response = client.ExecutePost(request);
    
    if (!response.IsSuccessful)
    {
        string extraInfo = response.ErrorMessage;
        extraInfo = extraInfo + response.Content;        
        Console.WriteLine("API FAIL with status code: " + response.StatusCode + " and message: '" + extraInfo + "' for test: " + testName);
        anythingFailed = true;
        return;
    }

    PredictionModel actualPredictionModel = JsonConvert.DeserializeObject<PredictionModel>(response.Content);

    // compare results and write out to file/ console
    var actual_serviceResult = actualPredictionModel;
    var actual_engineScores = actual_serviceResult.EngineResults.Where(p => p.EngineName == Core.EPStandardDefinitions.Engines.QRisk3).Single();



    var actual_QRisk3Score = actual_engineScores.Results.Where(p => p.id.ToString() == "http://endhealth.info/im#Qrisk3").Single();

    EngineResultModel.PredictionResult actual_QRisk3HeartAgeScore;
    if (actual_engineScores.Results.Count > 1)
    {
        actual_QRisk3HeartAgeScore = actual_engineScores.Results.Where(p => p.id.ToString() == "http://endhealth.info/im#Qrisk3HeartAge").Single();
    }
    else 
    { 
        actual_QRisk3HeartAgeScore = null; 
    }
    var actual_Meta = actual_engineScores.CalculationMeta;
    var expected_serviceResult = expectedPredictionModel;
    var expected_engineScores = expected_serviceResult.EngineResults[0];
    var expected_QRisk3Score = expected_engineScores.Results[0];

    EngineResultModel.PredictionResult expected_QRisk3HeartAgeScore;
    if (expected_engineScores.Results.Count > 1)
    {
        expected_QRisk3HeartAgeScore = expected_engineScores.Results[1];
    }
    else { expected_QRisk3HeartAgeScore = null; }

   
    var expected_Meta = expected_engineScores.CalculationMeta;


    // Test 1 - compare QRisk3 scores    
    if (expected_QRisk3Score.score != actual_QRisk3Score.score)
    {
        Console.WriteLine("FAIL: " + testName + " -- expected_QRisk3Score.score: " + expected_QRisk3Score.score + " : actual_QRisk3Score.score" + actual_QRisk3Score.score);
        anythingFailed = true;
    }

    // Test 2 - compare QRisk3 HeartAge scores    
    // we don't always get a heart age score (like when CVD = true) so we need to check whether we're expecting one
    if (expected_QRisk3HeartAgeScore != null)
    {
        if (expected_QRisk3HeartAgeScore.score != actual_QRisk3HeartAgeScore.score)
        {
            Console.WriteLine("FAIL: " + testName + " -- expected_QRisk3HeartAgeScore.score: " + expected_QRisk3HeartAgeScore.score + " : actual_QRisk3HeartAgeScore.score" + actual_QRisk3HeartAgeScore.score);
            anythingFailed = true;
        }
    }

    // Test 3 - Compare resultstatus 
    if (expected_Meta.EngineResultStatus != actual_Meta.EngineResultStatus)
    {
        Console.WriteLine("FAIL: " + testName + " -- expected_Meta.EngineResultStatus: " + expected_Meta.EngineResultStatus + " : actual_Meta.EngineResultStatus" + actual_Meta.EngineResultStatus);
        anythingFailed = true;
    }

    // Test 4 - Compare resultreason
    if (expected_Meta.EngineResultStatusReason != actual_Meta.EngineResultStatusReason)
    {
        Console.WriteLine("FAIL: " + testName + " -- expected_Meta.EngineResultStatusReason: " + expected_Meta.EngineResultStatusReason + " : actual_Meta.EngineResultStatusReason" + actual_Meta.EngineResultStatusReason);
        anythingFailed = true;
    }
    testsRun++;
}