using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;

namespace WebApplication1.Pages
{
    public class CreateModel : PageModel
    {
        private readonly AppDbContext _db;

        [TempData]
        public string Message { get; set; }

        [BindProperty]
        public Customer Customer { get; set; }

        private ILogger<CreateModel> _log;

        public CreateModel(AppDbContext db, ILogger<CreateModel> log)
        {
            _db = db;
            _log = log;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }

            _db.Customers.Add(Customer);

            await _db.SaveChangesAsync();

            string msg = $"Customer {Customer.Name} added !"; ;

            Message = msg;
            _log.LogCritical(msg);

            return RedirectToPage("/Index");
        }
    }
}