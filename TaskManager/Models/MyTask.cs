using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Threading.Tasks;

namespace TaskManager.Models
{
    public class MyTask
    {
        [Key]
        public long ID { get; set; }
        [Required]
        [StringLength(100, ErrorMessage = "Task Title can't be more than 100 chars")]
        [Display(Name = "Task Title")]
        public string Title { get; set; }

        [StringLength(250, ErrorMessage = "Description can't be more than 250 chars")]
        public string Description { get; set; }
        [Required]
        [Display(Name = "Status")]
        public Status Status { get; set; }
        [Required]
        [Display(Name = "Priority")]
        public Priority Priority { get; set; }
    }

    public enum Status
    {
        Open = 0,
        [Display(Name = "In-Progress")]
        InProgress = 1,
        Completed = 2,
        Canceled = 3,
    }

    public enum Priority
    {
        Low = 0,
        Medium = 1,
        High = 2,
    }
}
