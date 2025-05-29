using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SRG_lab2_Shumeiko.Models;

namespace SRG_lab2_Shumeiko.Pages_Departments
{
    public class IndexModel : PageModel
    {
        private readonly SRG_lab2_Shumeiko.Data.AppDbContext _context;

        public IndexModel(SRG_lab2_Shumeiko.Data.AppDbContext context)
        {
            _context = context;
        }

        public IList<Department> Department { get;set; } = default!;

        public async System.Threading.Tasks.Task OnGetAsync()
        {
            Department = await _context.Departments.ToListAsync();
        }

    }
}
