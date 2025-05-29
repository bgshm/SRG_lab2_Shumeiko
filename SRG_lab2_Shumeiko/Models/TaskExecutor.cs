using System.ComponentModel.DataAnnotations.Schema;
using System.ComponentModel.DataAnnotations;
using Microsoft.AspNetCore.Mvc.ModelBinding.Validation;

namespace SRG_lab2_Shumeiko.Models
{
    public class TaskExecutor
    {
        [ForeignKey(nameof(Task))]
        public int TaskID { get; set; }
        [ValidateNever]
        public Task Task { get; set; }

        [ForeignKey(nameof(Member))]
        public int MemberID { get; set; }
        [ValidateNever]
        public Member Member { get; set; }
    }


}
