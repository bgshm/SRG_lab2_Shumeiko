using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace SRG_lab2_Shumeiko.Models
{
    public class Task
    {
        [Key]
        public int TaskID { get; set; }

        [ForeignKey(nameof(Manager))]
        public int? ManagerID { get; set; }
        [ValidateNever]
        public Manager Manager { get; set; }

        [Required, StringLength(255)]
        public string Title { get; set; }

        public string Description { get; set; }

        [Required, DataType(DataType.DateTime)]
        public DateTime CreationDate { get; set; }

        [DataType(DataType.DateTime)]
        public DateTime? Deadline { get; set; }

        [Required, StringLength(50)]
        public string Status { get; set; }
        [ValidateNever]
        public ICollection<TaskExecutor> TaskExecutors { get; set; }
    }
}