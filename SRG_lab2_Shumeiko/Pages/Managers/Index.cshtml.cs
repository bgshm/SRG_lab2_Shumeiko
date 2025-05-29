using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SRG_lab2_Shumeiko.Data;
using SRG_lab2_Shumeiko.Models;

namespace SRG_lab2_Shumeiko.Pages_Managers
{
    public class IndexModel : PageModel
    {
        private readonly SRG_lab2_Shumeiko.Data.AppDbContext _context;

        public IndexModel(SRG_lab2_Shumeiko.Data.AppDbContext context)
        {
            _context = context;
        }

        public IList<Manager> Manager { get;set; } = default!;

        public async System.Threading.Tasks.Task OnGetAsync()
        {
            Manager = await _context.Managers
                .Include(m => m.Department).ToListAsync();
        }
    }
}
