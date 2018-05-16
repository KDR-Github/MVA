using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace WebApplication1.Pages
{
    public class EditModel : PageModel
    {
        private readonly AppDbContext _db;

        [BindProperty]
        public Customer Customer { get; set; }

        public EditModel(AppDbContext db)
        {
            _db = db;
        }


        public async Task<IActionResult> OnGetAsync(int id)
        {
            Customer = await _db.Customers.FindAsync(id);

            if(Customer == null)
            {
                return RedirectToPage("/Index");
            }

            return Page();

        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            if(!ModelState.IsValid)
            {
                return Page();
            }

            _db.Attach(Customer).State = EntityState.Modified;

            try
            {
                await _db.SaveChangesAsync();
            } 
            catch (DbUpdateConcurrencyException e)
            {

                throw new Exception($"Customer {Customer.Id} not found", e);
            }


            return RedirectToPage("/Index");

        }
    }
}