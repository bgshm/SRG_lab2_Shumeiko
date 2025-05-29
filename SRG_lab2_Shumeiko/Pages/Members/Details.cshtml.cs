using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using SRG_lab2_Shumeiko.Data;
using SRG_lab2_Shumeiko.Models;

namespace SRG_lab2_Shumeiko.Pages_Members
{
    public class DetailsModel : PageModel
    {
        private readonly SRG_lab2_Shumeiko.Data.AppDbContext _context;

        public DetailsModel(SRG_lab2_Shumeiko.Data.AppDbContext context)
        {
            _context = context;
        }

        public Member Member { get; set; } = default!;

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            var member = await _context.Members.FirstOrDefaultAsync(m => m.MemberID == id);

            if (member is not null)
            {
                Member = member;

                return Page();
            }

            return NotFound();
        }
    }
}
