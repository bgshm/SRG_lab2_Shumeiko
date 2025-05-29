using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using SRG_lab2_Shumeiko.Data;
using SRG_lab2_Shumeiko.Models;


namespace SRG_lab2_Shumeiko.Pages
{
    public class IndexModel : PageModel
    {
        private readonly AppDbContext _context;

        public IndexModel(AppDbContext context)
        {
            _context = context;
        }

        [BindProperty]
        public QueryType SelectedQuery { get; set; }

        // Параметри запитів
        [BindProperty] public DateTime DepartmentDate { get; set; }
        [BindProperty] public int ManagerYear { get; set; }
        [BindProperty] public int MinTasksPerMonth { get; set; }
        [BindProperty] public int MaxTasksTotal { get; set; }
        [BindProperty] public string MemberRole { get; set; }
        [BindProperty] public DateTime MemberDate { get; set; }
        [BindProperty] public int ManagerId { get; set; }
        [BindProperty] public string TaskStatus { get; set; }
        [BindProperty] public int QueryMemberID { get; set; }
        [BindProperty] public int DepartmentIdParam { get; set; }
        [BindProperty] public string StructuralUnitParam { get; set; }

        // Обрані списки
        public SelectList DepartmentsList { get; set; }
        public SelectList ManagersList { get; set; }
        public SelectList MembersList { get; set; }
        public SelectList RolesList { get; set; }
        public SelectList StructuralUnitsList { get; set; }
        public SelectList StatusList { get; } = new SelectList(
            new[] { "In Progress", "Overdue", "Completed" }
        );

        // Список результатів
        public IList<object> Results { get; set; }

        public async System.Threading.Tasks.Task OnGetAsync()
        {
            await LoadSelectListsAsync();
            DepartmentDate = DepartmentDate == default ? DateTime.Today : DepartmentDate;
            MemberDate = MemberDate == default ? DateTime.Today : MemberDate;
        }

        public async Task<IActionResult> OnPostAsync()
        {
            await LoadSelectListsAsync();
            DepartmentDate = DepartmentDate == default ? DateTime.Today : DepartmentDate;
            MemberDate = MemberDate == default ? DateTime.Today : MemberDate;

            if (Request.Form.ContainsKey("selectQuery"))
            {
                Results = null;
                return Page();
            }

            switch (SelectedQuery)
            {
                case QueryType.ByDepartmentDate:
                    Results = await _context.Departments
                        .Where(d => d.CreatedDate > DepartmentDate)
                        .Select(d => new { d.DepartmentName, d.CreatedDate })
                        .ToListAsync<object>();
                    break;

                case QueryType.ByManagerEnrollmentYear:
                    Results = await _context.Managers
                        .Where(m => m.EnrollmentYear < ManagerYear)
                        .Select(m => new { m.Name, m.EnrollmentYear })
                        .ToListAsync<object>();
                    break;

                case QueryType.ByMemberTasksRange:
                    Results = await _context.Members
                        .Where(m => m.TasksPerMonth >= MinTasksPerMonth
                                 && m.TasksTotal <= MaxTasksTotal)
                        .Select(m => new { m.Name, m.TasksPerMonth, m.TasksTotal })
                        .ToListAsync<object>();
                    break;

                case QueryType.ByMemberRoleAndDate:
                    Results = await _context.Members
                        .Where(m => m.Role == MemberRole
                                 && m.LastTaskDate.HasValue
                                 && m.LastTaskDate.Value.Date == MemberDate.Date)
                        .Select(m => new {
                            m.Name,
                            m.Role,
                            LastTaskDate = m.LastTaskDate.Value.Date
                        })
                        .ToListAsync<object>();
                    break;

                case QueryType.ByManagerTasksByStatus:
                    Results = await _context.Tasks
                        .Include(t => t.Manager)
                        .Where(t => t.ManagerID == ManagerId
                                 && t.Status == TaskStatus)
                        .Select(t => new { t.Title, t.Status, ManagerName = t.Manager.Name })
                        .ToListAsync<object>();
                    break;

                case QueryType.SameTasksAsMember:
                    var targetTasks = _context.TaskExecutors
                        .Where(te => te.MemberID == QueryMemberID)
                        .Select(te => te.TaskID);

                    Results = await _context.Members
                        .Where(m => m.MemberID != QueryMemberID)
                        .Where(m =>
                            !targetTasks.Except(
                                _context.TaskExecutors
                                    .Where(te => te.MemberID == m.MemberID)
                                    .Select(te => te.TaskID)
                            ).Any()
                            &&
                            !(
                                _context.TaskExecutors
                                    .Where(te => te.MemberID == m.MemberID)
                                    .Select(te => te.TaskID)
                                .Except(targetTasks)
                            ).Any()
                        )
                        .Select(m => new { m.MemberID, m.Name })
                        .ToListAsync<object>();
                    break;

                case QueryType.MemberPairsByStructuralUnit:
                    if (string.IsNullOrEmpty(StructuralUnitParam))
                    {
                        Results = Array.Empty<object>();
                        break;
                    }

                    var membersInUnit = _context.Members
                        .Where(m => m.StructuralUnit == StructuralUnitParam);

                    Results = await membersInUnit
                        .SelectMany(m1 => membersInUnit.Where(m2 => m2.MemberID > m1.MemberID),
                            (m1, m2) => new { m1, m2 })
                        .Where(pair =>
                            !_context.TaskExecutors
                                .Where(te => te.MemberID == pair.m1.MemberID)
                                .Select(te => te.TaskID)
                                .Except(
                                    _context.TaskExecutors
                                        .Where(te => te.MemberID == pair.m2.MemberID)
                                        .Select(te => te.TaskID)
                                )
                                .Any()
                            &&
                            !_context.TaskExecutors
                                .Where(te => te.MemberID == pair.m2.MemberID)
                                .Select(te => te.TaskID)
                                .Except(
                                    _context.TaskExecutors
                                        .Where(te => te.MemberID == pair.m1.MemberID)
                                        .Select(te => te.TaskID)
                                )
                                .Any()
                        )
                        .Select(pair => new
                        {
                            MemberA = pair.m1.Name,
                            MemberB = pair.m2.Name
                        })
                        .ToListAsync<object>();
                    break;



                case QueryType.TasksByExclusiveDepartment:
                    Results = await _context.Tasks
                        .Where(t =>
                            _context.TaskExecutors
                                .Where(te => te.TaskID == t.TaskID)
                                .Any(te => te.Member.Manager.DepartmentID == DepartmentIdParam)
                            &&
                            !_context.TaskExecutors
                                .Where(te => te.TaskID == t.TaskID)
                                .Any(te => te.Member.Manager.DepartmentID != DepartmentIdParam)
                        )
                        .Select(t => new { t.Title })
                        .ToListAsync<object>();
                    break;

                default:
                    Results = Array.Empty<object>();
                    break;
            }

            return Page();
        }

        private async System.Threading.Tasks.Task LoadSelectListsAsync()
        {
            DepartmentsList = new SelectList(
                await _context.Departments.OrderBy(d => d.DepartmentName).ToListAsync(),
                "DepartmentID", "DepartmentName"
            );
            ManagersList = new SelectList(
                await _context.Managers.OrderBy(m => m.Name).ToListAsync(),
                "ManagerID", "Name"
            );
            MembersList = new SelectList(
                await _context.Members.OrderBy(m => m.Name).ToListAsync(),
                "MemberID", "Name"
            );
            RolesList = new SelectList(
                await _context.Members.Select(m => m.Role).Distinct().OrderBy(r => r).ToListAsync()
            );
            StructuralUnitsList = new SelectList(
                await _context.Members.Select(m => m.StructuralUnit).Distinct().OrderBy(s => s).ToListAsync()
            );
        }
    }
}
