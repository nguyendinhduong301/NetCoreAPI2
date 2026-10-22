
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoMVC.Models
{
    [Table("Department")]
    public class Department
    {
        [Key]
        [Required]
        [StringLength(10)]
        [Display(Name = "Mã khoa")]
        public string DepartmentCode { get; set; } = string.Empty;

        [Required(ErrorMessage = "Tên khoa không được để trống")]
        [StringLength(100)]
        [Display(Name = "Tên khoa")]
        public string DepartmentName { get; set; } = string.Empty;

        // Một khoa có nhiều sinh viên
        public ICollection<Student> Students { get; set; }
            = new List<Student>();
    }
}
