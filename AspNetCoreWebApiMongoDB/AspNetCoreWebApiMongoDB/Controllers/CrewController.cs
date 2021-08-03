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

        [HttpGet("/{id}", Name = "GetById")]
        //[Route("{id}")]
        public ActionResult<Crew> Get(string id)
        {
            return this._crewService.GetCrew(id);
        }

        [HttpPost]
        public ActionResult<Crew> Create(Crew c)
        {
            this._crewService.CreateCrew(c);

            // https://stackoverflow.com/questions/25045604/can-anyone-explain-createdatroute-to-me/25110700
            return CreatedAtRoute("GetById", new { id = c.Id }, c);
        }

    }
}
