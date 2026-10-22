using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace DemoMVC.Models
{
    [Table("Student")]
    public class Student
    {
        [Key]
        [Required(ErrorMessage = "Mã sinh viên không được để trống !")]
        public string StudentCode { get; set; } = default!;

        [Required(ErrorMessage = "Họ tên không được để trống !")]
        [StringLength(50, MinimumLength = 3,
            ErrorMessage = "Họ tên phải từ 3 đến 50 ký tự !")]
        public string FullName { get; set; } = default!;

        [Required(ErrorMessage = "Địa chỉ không được để trống !")]
        public string? Address { get; set; }

        [Required(ErrorMessage = "Email không được để trống !")]
        [EmailAddress(ErrorMessage = "Email không đúng định dạng")]
        public string Email { get; set; } = default!;
        
        [Required(ErrorMessage = "Vui lòng chọn khoa")]
        [Display(Name = "Khoa")]
        public string DepartmentCode { get; set; } = string.Empty;

        // Khóa ngoại liên kết với bảng Department
        [ForeignKey(nameof(DepartmentCode))]
        public Department? Department { get; set; }

    }
}