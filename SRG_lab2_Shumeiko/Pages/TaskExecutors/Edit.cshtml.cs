using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SRG_lab2_Shumeiko.Data;
using SRG_lab2_Shumeiko.Models;

namespace SRG_lab2_Shumeiko.Pages_TaskExecutors
{
    public class EditModel : PageModel
    {
        private readonly SRG_lab2_Shumeiko.Data.AppDbContext _context;

        public EditModel(SRG_lab2_Shumeiko.Data.AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public TaskExecutor TaskExecutor { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var taskexecutor =  await _context.TaskExecutors.FirstOrDefaultAsync(m => m.TaskID == id);
            if (taskexecutor == null)
            {
                return NotFound();
            }
            TaskExecutor = taskexecutor;
           ViewData["MemberID"] = new SelectList(_context.Members, "MemberID", "Name");
           ViewData["TaskID"] = new SelectList(_context.Tasks, "TaskID", "Status");
            return Page();
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more information, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Attach(TaskExecutor).State = EntityState.Modified;

            try
            {
                await _context.SaveChangesAsync();
            }
            catch (DbUpdateConcurrencyException)
            {
                if (!TaskExecutorExists(TaskExecutor.TaskID))
                {
                    return NotFound();
                }
                else
                {
                    throw;
                }
            }

            return RedirectToPage("./Index");
        }

        private bool TaskExecutorExists(int id)
        {
            return _context.TaskExecutors.Any(e => e.TaskID == id);
        }
    }
}
