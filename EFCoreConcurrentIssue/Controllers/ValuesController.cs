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
            var ids = new List<long>();
            for (int i = 0; i < 10; i++)
            {
                ids.Add(new Random().Next(0, 10000));
            }
            return Ok(db.Events.Include(x => x.Location).Where(x => ids.Contains(x.Id)).ToList());
        }
    }
}
