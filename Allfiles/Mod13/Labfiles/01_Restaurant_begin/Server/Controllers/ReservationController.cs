using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Server.Data;
using Server.Models;

namespace Server.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReservationController : ControllerBase
    {
        private readonly RestaurantContext _context;

        public ReservationController(RestaurantContext context)
        {
            _context = context;
        }

        [HttpGet("{id}")]
        public ActionResult<OrderTable> GetById(int id)
        {
            if (_context.ReservationsTables.FirstOrDefault(p => p.Id == id) is OrderTable order)
            {
                return order;
            }
            return NotFound();
        }

        [HttpPost("{id}")]
        public ActionResult<OrderTable> Create(OrderTable orderTable)
        {
            _context.Add(orderTable);
            _context.SaveChanges();
            return CreatedAtAction(nameof(GetById), routeValues: new { id = orderTable.Id }, value: orderTable);
        }
    }
}
