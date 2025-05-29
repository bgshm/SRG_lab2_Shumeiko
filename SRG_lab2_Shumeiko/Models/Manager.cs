using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace SRG_lab2_Shumeiko.Models
{
    public class Manager
    {
        [Key]
        public int ManagerID { get; set; }

        [Required]
        [StringLength(255)]
        public string Name { get; set; }              

        [Required]
        public int EnrollmentYear { get; set; }      

        [Required]
        public string StructuralUnit { get; set; }

        [Required]
        [ForeignKey("Department")]
        public int DepartmentID { get; set; }
        [ValidateNever]
        public Department Department { get; set; }
        [ValidateNever]
        public ICollection<Task> Tasks { get; set; }

        [ValidateNever]
        public ICollection<Member> Members { get; set; }
    }

}
