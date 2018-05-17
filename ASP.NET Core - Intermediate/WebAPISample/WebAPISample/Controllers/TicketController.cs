using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebAPISample.Controllers
{
    [Produces("application/json")]
    [Route("api/Ticket")]
    public class TicketController : Controller
    {
        private TicketContext _context;

        public TicketController(TicketContext context)
        {
            _context = context;
            
            if (_context.TicketItems.Count() == 0)
            {
                _context.TicketItems.Add(
                    new TicketItem
                    {
                        Concert = "Beyonce Concert",
                        Artist = "Beyonce",
                        Available = true
                    }
                );
                _context.SaveChanges();
            }
        }

        [HttpGet]
        public IEnumerable<TicketItem> GetAll()
        {
            return _context.TicketItems.AsNoTracking().ToList();
        }

        [HttpGet("{Id}", Name ="GetTicket")]
        public IActionResult GetById(long id)
        {
            var ticket = _context.TicketItems.FirstOrDefault(t => t.Id == id);
            if (ticket == null)
            {
                return NotFound(); //404
            }

            return new ObjectResult(ticket); //200
        }

        [HttpPost]
        public IActionResult Create([FromBody]TicketItem ticket)
        {
            if(ticket == null)
            {
                return BadRequest();
            }

            _context.TicketItems.Add(ticket);
            _context.SaveChanges();

            return CreatedAtRoute("GetTicket", new { id = ticket.Id }, ticket);
        }
    }
}