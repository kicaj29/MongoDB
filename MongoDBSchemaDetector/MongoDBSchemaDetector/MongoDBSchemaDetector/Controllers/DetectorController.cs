using Microsoft.AspNetCore.Mvc;
using System.Collections.Generic;

namespace MongoDBSchemaDetector.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class DetectorController : ControllerBase
    {

        private readonly ILogger<DetectorController> _logger;

        public DetectorController(ILogger<DetectorController> logger)
        {
            _logger = logger;
        }

        [HttpGet]
        public async Task<ActionResult<IEnumerable<DetectorResultItem>>> CollectResults()
        {
            IEnumerable<DetectorResultItem> results = await (new RulesManager()).RunAsync();

            return new ActionResult<IEnumerable<DetectorResultItem>>(results);
        }
    }
}