using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using SRG_lab2_Shumeiko.Data;
using SRG_lab2_Shumeiko.Models;

namespace SRG_lab2_Shumeiko.Pages_TaskExecutors
{
    public class CreateModel : PageModel
    {
        private readonly SRG_lab2_Shumeiko.Data.AppDbContext _context;

        public CreateModel(SRG_lab2_Shumeiko.Data.AppDbContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
        ViewData["MemberID"] = new SelectList(_context.Members, "MemberID", "Name");
        ViewData["TaskID"] = new SelectList(_context.Tasks, "TaskID", "Title");
            return Page();
        }

        [BindProperty]
        public TaskExecutor TaskExecutor { get; set; } = default!;

        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.TaskExecutors.Add(TaskExecutor);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
