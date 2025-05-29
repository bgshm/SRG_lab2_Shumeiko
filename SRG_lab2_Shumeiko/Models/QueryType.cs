// Models/QueryType.cs
using System.ComponentModel.DataAnnotations;

namespace SRG_lab2_Shumeiko.Models
{
    public enum QueryType
    {
        // 1. Департаменти, CreatedDate яких пізніше вказаної
        [Display(Name = "1. Департаменти за датою створення")]
        ByDepartmentDate,
        // 2. Менеджери, EnrollmentYear яких раніше вказаного
        [Display(Name = "2. Менеджери за роком вступу")]
        ByManagerEnrollmentYear,
        // 3. Учасники, TasksPerMonth не менше вказаної і TasksTotal не більше вказаної
        [Display(Name = "3. Учасники за кількістю завдань")]
        ByMemberTasksRange,
        // 4. Учасники, з вказаною Role, у яких LastTaskDate дорівнює обраному дню 
        [Display(Name = "4. Учасники за роллю й датою")]
        ByMemberRoleAndDate,
        // 5. Завдання, що створив вказаний Manager і Status яких дорівнює вказаному
        [Display(Name = "5. Завдання керівника за статусом")]
        ByManagerTasksByStatus,
        // 6. Учасники, які виконують точно ті самі завдання, що й заданий учасник
        [Display(Name = "6*. Учасники з тими ж завданнями")]
        SameTasksAsMember,
        // 7. Пари учасників, які мають ідентичні множини виконуваних завдань з вказаного структурного підрозділу
        [Display(Name = "7*. Пари учасників за підрозділом")]
        MemberPairsByStructuralUnit,
        // 8.Завдання, які виконують виключно учасники заданого департаменту
        [Display(Name = "8*. Завдання лише свого департаменту")]
        TasksByExclusiveDepartment
    }
}
