using System.ComponentModel.DataAnnotations;

namespace DemoMVC.Models
{
    public class Employee
    {
        [Key]
        public string EmpId { get; set; } = default!;
        public string FullName { get; set; } = default!;
    }
}