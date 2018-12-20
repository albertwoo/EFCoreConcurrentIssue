using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace EFCoreConcurrentIssue.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ValuesController : ControllerBase
    {
        private readonly MyDbContext db;

        public ValuesController(MyDbContext db)
        {
            this.db = db;
        }

        [HttpGet]
        public IActionResult Get()
        {
            Console.Write(".");
            var ids = db.Events.Select(x => x.Id).Take(10).ToList();
            return Ok(db.Events.Include(x => x.Location).Where(x => ids.Contains(x.Id)).ToList());
        }

        [HttpGet("Test2")]
        public IActionResult GetTest2() =>
            Ok(db.Locations.Include(x => x.Events)
                .Select(x =>
                    new 
                    {
                        Id = x.Id,
                        EventCounts = x.Events.Count()
                    })
                .ToList());
    }
}
