using AspNetCoreWebApiMongoDB.Models;
using AspNetCoreWebApiMongoDB.Services;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace AspNetCoreWebApiMongoDB.Controllers
{
    [ApiController]
    [Route("[controller]")]
    public class CrewController : Controller
    {
        private CrewService _crewService;

        public CrewController(CrewService crewService)
        {
            this._crewService = crewService;
        }

        [HttpGet]
        public ActionResult<List<Crew>> Get() => this._crewService.GetCrews();
    }
}
