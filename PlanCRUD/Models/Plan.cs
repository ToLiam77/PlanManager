using System;
using System.ComponentModel.DataAnnotations;

namespace PlanCRUD.Models
{
    public class Plan
    {
        public int Id { get; set; }

        [Required]
        [MaxLength(100)]
        public string? Activity { get; set; }

        public string Days { get; set; }

        [DataType(DataType.Time)]
        public TimeSpan StartTime { get; set; }

        [DataType(DataType.Time)]
        public TimeSpan EndTime { get; set; }

        public TimeSpan Duration { get; set; }

        public decimal Price { get; set; }

        public string Currency { get; set; } = "GBP";

        [StringLength(1000)] 
        public string? Details { get; set; } 
    }
}
