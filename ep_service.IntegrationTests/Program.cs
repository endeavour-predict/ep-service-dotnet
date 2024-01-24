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

var resultFile = args[0] + @"/results_" + DateTime.Now.ToString("yyyy-dd-M--HH-mm-ss") + ".log";

var serializer = new Newtonsoft.Json.JsonSerializer();
EPInputModel epInputModel = new EPInputModel();
PredictionModel expectedPredictionModel = new PredictionModel();
bool anythingFailed = false;


foreach (var inputFile in Directory.GetFiles(args[0]))
{
    if (!inputFile.Contains(".input."))
    {
        //Console.WriteLine("Skipping File: " + inputFile);
        continue;
    }

    Console.WriteLine("Processing file: " + inputFile);
    File.AppendAllText(resultFile, "Processing: " + inputFile);

    // make sure we have a corresponding output file (expected results)
    var outputFile = inputFile.Replace("input", "output");
    if (!File.Exists(outputFile))
    {
        Console.WriteLine("ERROR. outputFile file nto found: " + outputFile);
    }


    // get the expected result from the output file
    using (StreamReader sr = File.OpenText(outputFile))
    {
        using (JsonTextReader reader = new JsonTextReader(sr))
        {
            expectedPredictionModel = serializer.Deserialize<PredictionModel>(reader);
        }
    }


    // call the API for the actual result
    string endpoint = args[1];
    string jsonInput = File.ReadAllText(inputFile);

    var client = new RestClient(endpoint);
    var request = new RestRequest("Prediction", Method.Post);
    request.AddJsonBody(new{ jsonInput });
    var response = client.ExecutePost(request);
    PredictionModel actualPredictionModel = JsonConvert.DeserializeObject<PredictionModel>(response.Content);

    // compare results and write out to file/ console
    var actual_serviceResult = actualPredictionModel;
    var actual_engineScores = actual_serviceResult.EngineResults.Where(p => p.EngineName == Core.EPStandardDefinitions.Engines.QRisk3).Single();
    var actual_QRisk3Score = actual_engineScores.Results.Where(p => p.id.ToString() == Globals.QRiskScoreUri).Single();
    var actual_QRisk3HeartAgeScore = actual_engineScores.Results.Where(p => p.id.ToString() == Globals.QRiskScoreUri + "HeartAge").SingleOrDefault();
    var actual_Meta = actual_engineScores.CalculationMeta;

    var expected_serviceResult = expectedPredictionModel;
    var expected_engineScores = expected_serviceResult.EngineResults.Where(p => p.EngineName == Core.EPStandardDefinitions.Engines.QRisk3).Single();
    var expected_QRisk3Score = expected_engineScores.Results.Where(p => p.id.ToString() == Globals.QRiskScoreUri).Single();
    var expected_QRisk3HeartAgeScore = expected_engineScores.Results.Where(p => p.id.ToString() == Globals.QRiskScoreUri + "HeartAge").SingleOrDefault();
    var expected_Meta = expected_engineScores.CalculationMeta;

    
    if (expected_QRisk3Score.score != actual_QRisk3Score.score)
    {
        File.AppendAllText(resultFile, "FAIL: " + inputFile + " -- expected_QRisk3Score.score: " + expected_QRisk3Score.score + " : actual_QRisk3Score.score" + actual_QRisk3Score.score);
        anythingFailed = true;
    }

    if (anythingFailed)
    {
        File.AppendAllText(resultFile, "FINAL RESULT: FAIL");
    }
    else
    {
        File.AppendAllText(resultFile, "FINAL RESULT: OK");
    }

    //// we always get a score, even if it's 0.0
    //Assert.AreEqual(expected_QRisk3Score.score, actual_QRisk3Score.score, test.TestName);

    //// we don't always get a heart age score (like when CVD = true) so we need to check whether we're expecting one
    //if (expected_QRisk3HeartAgeScore != null)
    //{
    //    Assert.AreEqual(expected_QRisk3HeartAgeScore.score, actual_QRisk3HeartAgeScore.score, test.TestName);
    //}

    //// Final assertion is that the calc reasons match
    //Assert.AreEqual(expected_Meta.EngineResultStatus, actual_Meta.EngineResultStatus, test.TestName);
    //Assert.AreEqual(expected_Meta.EngineResultStatusReason, actual_Meta.EngineResultStatusReason, test.TestName);
    //testsRun++;


    // TODO JC compare the result with the output file. Write a line to teh results.csv showing integration test result..
}

