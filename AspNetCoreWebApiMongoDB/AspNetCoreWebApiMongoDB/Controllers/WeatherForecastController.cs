using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Newtonsoft.Json.Converters;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreWebApiMongoDB.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
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

        [HttpGet]
        public IEnumerable<WeatherForecast> Get()
        {
            var rng = new Random();
            return Enumerable.Range(1, 5).Select(index => new WeatherForecast
            {
                Date = DateTime.Now.AddDays(index),
                TemperatureC = rng.Next(-20, 55),
                Summary = Summaries[rng.Next(Summaries.Length)]
            })
            .ToArray();
        }

        [HttpGet("/test-dict")]
        public Dictionary<string, int> TestDictionaryResult()
        {
            var result = new Dictionary<string, int>()
            {
                { "AAA", 0 }
            };

            result.Add(Guid.NewGuid().ToString(), 1);
            result.Add(Guid.NewGuid().ToString(), 2);
            result.Add(Guid.NewGuid().ToString(), 3);

            return result;
        }


        [HttpGet("/temp-schema")]
        public MultipleBatchesDeleteResult TempSchema()
        {
            var result = new MultipleBatchesDeleteResult();


            result.Add(Guid.NewGuid().ToString(), BatchDeleteResult.Deleted);
            result.Add(Guid.NewGuid().ToString(), BatchDeleteResult.Suspending);
            result.Add(Guid.NewGuid().ToString(), BatchDeleteResult.RejectedBecauseOfLock);

            return result;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public sealed class MultipleBatchesDeleteResult : Dictionary<string, BatchDeleteResult>
    {

    }

    /// <summary>
    /// 
    /// </summary>
    [JsonConverter(typeof(StringEnumConverter))]
    public enum BatchDeleteResult
    {
        /// <summary>
        /// 
        /// </summary>
        Suspending = 0,
        /// <summary>
        /// 
        /// </summary>
        Deleted = 1,
        /// <summary>
        /// 
        /// </summary>
        RejectedBecauseOfLock = 2
    }
}
