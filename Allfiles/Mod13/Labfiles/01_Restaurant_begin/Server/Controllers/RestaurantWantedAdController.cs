using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Server.Data;
using Server.Models;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class RestaurantWantedAdController : ControllerBase
    {
        private readonly RestaurantContext _context;

        public RestaurantWantedAdController(RestaurantContext context)
        {
            _context = context;
        }

        [HttpGet]
        public ActionResult<List<EmployeeRequirements>> Get()
        {
            var requirements = _context.EmployeesRequirements
                .OrderBy(r => r.JobTitle);
            return requirements.ToList();
        }
    }
}
