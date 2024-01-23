using ep_models;
using Newtonsoft.Json;
using RestSharp;
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

var serializer = new Newtonsoft.Json.JsonSerializer();
EPInputModel epInputModel = new EPInputModel();

foreach (var file in Directory.GetFiles(args[0]))
{
    if (!file.Contains(".input."))
    {
        Console.WriteLine("Skipping File: " + file);
        continue;
    }

    Console.WriteLine("Processing file: " + file);
    using (StreamReader sr = File.OpenText(file))
    {
        using (JsonTextReader reader = new JsonTextReader(sr))
        {
            epInputModel = serializer.Deserialize<EPInputModel>(reader);
        }
    }

    // call the API    
    string endpoint = args[1];
    string jsonInput = File.ReadAllText(file);

    var client = new RestClient(endpoint);
    var request = new RestRequest("Prediction", Method.Post);
    request.AddJsonBody(new{ jsonInput });
    var response = client.ExecutePost(request);

    var outputFile = file.Replace("input", "output");
    File.WriteAllText(outputFile, response.Content);

    // TODO JC compare the result with the output file. Write a line to teh results.csv showing integration test result..
}

