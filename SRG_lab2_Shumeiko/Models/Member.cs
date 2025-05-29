using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace SRG_lab2_Shumeiko.Models
{
    public class Member
    {
        [Key]
        public int MemberID { get; set; }

        [Required, StringLength(255)]
        public string Name { get; set; }

        [Required]
        [ForeignKey(nameof(Manager))]
        public int ManagerID { get; set; }
        [ValidateNever]
        public Manager Manager { get; set; }

        [Required, StringLength(100)]
        public string Role { get; set; }

        [DefaultValue(0)]
        public int TasksPerMonth { get; set; }

        [DefaultValue(0)]
        public int TasksTotal { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? LastTaskDate { get; set; }

        [DefaultValue(0)]
        public int EnrollmentYear { get; set; }

        [Required]
        public string StructuralUnit { get; set; }
        [ValidateNever]
        public ICollection<TaskExecutor> TaskExecutors { get; set; }
    }
}
