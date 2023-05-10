using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace ApiUsageExamples.Performance.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class PerformanceTestsController : ControllerBase
    {
        private readonly StorageAccess _storageAccess;


        public PerformanceTestsController(StorageAccess storageAccess)
        {
            _storageAccess = storageAccess;
        }

        [HttpGet(Name = "TestClusteredCollections")]
        public async Task<ActionResult> TestClusteredCollectionsAsync()
        {
            await _storageAccess.RunTest();

            return Ok();
        }
    }
}
