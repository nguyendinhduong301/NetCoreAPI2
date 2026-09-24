using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoMVC.Models
{
    public class Student
    {
        [Key]
        public string StudentCode { get; set; } = default!;
        public string FullName { get; set; } = default!;
        public string? Address { get; set; }
        public string? University { get; set; }
        public string Email { get; set; } = default!;
    }
}