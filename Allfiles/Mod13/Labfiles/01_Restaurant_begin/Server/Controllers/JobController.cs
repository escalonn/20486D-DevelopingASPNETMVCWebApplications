using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Server.Data;
using Server.Models;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class JobController : ControllerBase
    {
        private readonly RestaurantContext _context;

        public JobController(RestaurantContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public ActionResult<JobApplication> GetById(int id)
        {
            if (_context.JobApplications.FirstOrDefault(p => p.Id == id) is var apply)
            {
                return apply;
            }
            return NotFound();
        }

        [HttpPost]
        public ActionResult<JobApplication> Post(JobApplication jobApplication)
        {
            _context.Add(jobApplication);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), routeValues: new { id = jobApplication.Id }, value: jobApplication);
        }
    }
}
