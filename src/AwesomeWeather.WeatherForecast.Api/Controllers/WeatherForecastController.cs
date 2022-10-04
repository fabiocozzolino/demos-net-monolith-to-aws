using Amazon.Runtime;
using Amazon.Runtime.CredentialManagement;
using Amazon.SimpleNotificationService;
using Amazon.SimpleNotificationService.Model;
using Microsoft.AspNetCore.Mvc;

namespace AwesomeWeather.WeatherForecast.Api.Controllers;

[ApiController]
[Route("[controller]")]
public class WeatherForecastController : ControllerBase
{
    private static readonly string[] Summaries = new[]
    {
        "Freezing", "Bracing", "Chilly", "Cool", "Mild", "Warm", "Balmy", "Hot", "Sweltering", "Scorching"
    };

    private readonly ILogger<WeatherForecastController> _logger;

    public WeatherForecastController(ILogger<WeatherForecastController> logger)
    {
        _logger = logger;
    }

    [HttpGet(Name = "GetWeatherForecast")]
    public async Task<IEnumerable<WeatherForecast>> Get()
    {
        var forecasts = Enumerable.Range(1, 5).Select(index => new WeatherForecast
        {
            Date = DateTime.Now.AddDays(index),
            TemperatureC = Random.Shared.Next(-20, 55),
            Summary = Summaries[Random.Shared.Next(Summaries.Length)]
        })
        .ToArray();

        try
        {
            // create a SNS topic
            string topicArn = "arn:aws:sns:eu-west-1:979194342604:BadWeatherForecast";
            string messageText = forecasts.First(f => f.TemperatureC < 10).Date.ToString();

            var awsCredentials = new BasicAWSCredentials("AKIA6H7EH5TGECS5LUYV", "AVQki/M3As0coXWU69av37VMgLDbHSqFwwVSVmhv");
            var client = new AmazonSimpleNotificationServiceClient(awsCredentials);
            await client.PublishAsync(topicArn, messageText);
        }
        catch(Exception ex)
        {

        }

        return forecasts;
    }
}
