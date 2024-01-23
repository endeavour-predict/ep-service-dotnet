using Microsoft.OpenApi.Models;
using System.Text.Json.Serialization;

var builder = WebApplication.CreateBuilder(args);
builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();

// include the comments for the methods/properties in the swagger docs (name and path is set in the Project Build properties)
var xmlFilename = "CommentsAutoDoc.xml";
builder.Services.AddSwaggerGen(
    options => {
        options.IncludeXmlComments(xmlFilename);
        options.SwaggerDoc("v1",
                new OpenApiInfo
                {
                    Title = "Endeavour Predict API",
                    Version = "v1",
                    Description = "Endeavour Predict API",
                    Contact = new OpenApiContact
                    {
                        Email = "enquiries@endeavourhealth.org",
                        Name = "Endeavour Health",
                        Url = new Uri("https://github.com/endeavour-predict")
                    },
                    License = new OpenApiLicense
                    {
                        Name = "AGPL-3.0",
                        Url = new Uri("https://www.gnu.org/licenses/agpl-3.0.en.html")
                    }
                }); ;

    });

// This is so we can present the ENUMs as strings, rather than Array [int]
builder.Services.AddControllers()
    .AddJsonOptions(options => { options.JsonSerializerOptions.Converters.Add(new JsonStringEnumConverter()); });


// AWS lambda suport
builder.Services.AddAWSLambdaHosting(LambdaEventSource.HttpApi);
var app = builder.Build();
app.UseStaticFiles(); // required for swagger customisation

app.UseSwagger(c => {
    c.RouteTemplate = "{documentname}/swagger.json";
});

app.UseSwaggerUI(c => {
    c.SwaggerEndpoint("/v1/swagger.json", "Endeavour Predict: API");
    c.RoutePrefix = ""; // we want the swagger docs at the root
    c.InjectStylesheet("/css/swagger-theme-muted.css"); // Inject our swagger CSS
    c.DocumentTitle = "Endeavour Predict API";

});


app.UseHttpsRedirection();
app.UseAuthorization();
app.MapControllers();
app.Run();


