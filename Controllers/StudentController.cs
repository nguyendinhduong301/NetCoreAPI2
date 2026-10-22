using DemoMVC.Data;
using DemoMVC.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;

namespace DemoMVC.Controllers
{
    public class StudentController : Controller
    {
        private readonly ApplicationDbContext _context;

        public StudentController(ApplicationDbContext context)
        {
            _context = context;
        }

        // Đưa danh sách khoa vào View để chọn
        private void LoadDepartments(string? selectedCode = null)
        {
            ViewBag.Departments = new SelectList(
                _context.Departments
                    .OrderBy(d => d.DepartmentName)
                    .ToList(),
                "DepartmentCode",
                "DepartmentName",
                selectedCode
            );
        }

        // GET: Student/Index
        public IActionResult Index()
        {
            var students = _context.Students
                .Include(s => s.Department)
                .ToList();

            return View(students);
        }

        // API: Lấy mã sinh viên tiếp theo theo khoa
        [HttpGet]
        public IActionResult GetNextStudentCode(string departmentCode)
        {
            bool departmentExists = _context.Departments.Any(
                d => d.DepartmentCode == departmentCode);

            if (!departmentExists)
            {
                return BadRequest("Mã khoa không hợp lệ");
            }

            string studentCode = GenerateStudentCode(departmentCode);

            return Json(new { studentCode });
        }

        // GET: Student/Create
        public IActionResult Create()
        {
            var student = new Student
            {
                DepartmentCode = "CNTT",
                StudentCode = GenerateStudentCode("CNTT")
            };

            LoadDepartments(student.DepartmentCode);

            return View(student);
        }

        // POST: Student/Create
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Create(Student student)
        {
            bool departmentExists = _context.Departments.Any(
                d => d.DepartmentCode == student.DepartmentCode);

            if (!departmentExists)
            {
                ModelState.AddModelError(
                    "DepartmentCode",
                    "Vui lòng chọn khoa hợp lệ!");
            }

            // Máy chủ tự sinh mã theo khoa đã chọn
            if (departmentExists)
            {
                student.StudentCode =
                    GenerateStudentCode(student.DepartmentCode);
            }

            // Không nhận đối tượng Department từ form
            ModelState.Remove(nameof(student.Department));
            ModelState.Remove(nameof(student.StudentCode));

            if (ModelState.IsValid)
            {
                _context.Students.Add(student);
                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            LoadDepartments(student.DepartmentCode);

            if (departmentExists)
            {
                student.StudentCode =
                    GenerateStudentCode(student.DepartmentCode);
            }

            return View(student);
        }

        // GET: Student/Edit/{id}
        public IActionResult Edit(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var student = _context.Students.Find(id);

            if (student == null)
            {
                return NotFound();
            }

            LoadDepartments(student.DepartmentCode);

            return View(student);
        }

        // POST: Student/Edit/{id}
        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult Edit(string id, Student student)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            // Tìm sinh viên theo mã cũ trong URL
            var existingStudent = _context.Students.Find(id);

            if (existingStudent == null)
            {
                return NotFound();
            }

            bool departmentExists = _context.Departments.Any(
                d => d.DepartmentCode == student.DepartmentCode);

            if (!departmentExists)
            {
                ModelState.AddModelError(
                    "DepartmentCode",
                    "Vui lòng chọn khoa hợp lệ!");
            }

            // Không nhận đối tượng Department từ form
            ModelState.Remove(nameof(student.Department));

            if (ModelState.IsValid)
            {
                string oldDepartmentCode = existingStudent.DepartmentCode;

                // Nếu chuyển khoa, tạo mã mới theo khoa đích
                if (oldDepartmentCode != student.DepartmentCode)
                {
                    string newStudentCode =
                        GenerateStudentCode(student.DepartmentCode);

                    // Cập nhật khóa chính
                    existingStudent.StudentCode = newStudentCode;
                }

                existingStudent.FullName = student.FullName;
                existingStudent.Address = student.Address;
                existingStudent.Email = student.Email;
                existingStudent.DepartmentCode = student.DepartmentCode;

                _context.SaveChanges();

                return RedirectToAction(nameof(Index));
            }

            LoadDepartments(student.DepartmentCode);

            // Giữ mã cũ để hiển thị lại form khi có lỗi
            student.StudentCode = id;

            return View(student);
        }

        // GET: Student/Delete/{id}
        public IActionResult Delete(string id)
        {
            if (string.IsNullOrEmpty(id))
            {
                return NotFound();
            }

            var student = _context.Students
                .Include(s => s.Department)
                .FirstOrDefault(s => s.StudentCode == id);

            if (student == null)
            {
                return NotFound();
            }

            return View(student);
        }

        // POST: Student/Delete/{id}
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public IActionResult DeleteConfirmed(string id)
        {
            var student = _context.Students.Find(id);

            if (student == null)
            {
                return NotFound();
            }

            _context.Students.Remove(student);
            _context.SaveChanges();

            return RedirectToAction(nameof(Index));
        }

        // Tự sinh mã sinh viên theo khoa
        private string GenerateStudentCode(string departmentCode)
        {
            string yearCode = "28";
            string trainingCode = "21";

            // Quy đổi mã khoa thành mã ngành
            string majorCode = departmentCode switch
            {
                "CNTT" => "05",
                "MO"   => "06",
                "CK"   => "07",
                "MT"   => "08",
                _      => "00"
            };

            string prefix = yearCode + trainingCode + majorCode;

            var existingCodes = _context.Students
                .Where(s => s.StudentCode.StartsWith(prefix))
                .Select(s => s.StudentCode)
                .ToList();

            int maxNumber = -1;

            foreach (var code in existingCodes)
            {
                if (code.Length == 10 &&
                    int.TryParse(code.Substring(6, 4), out int number))
                {
                    if (number > maxNumber)
                    {
                        maxNumber = number;
                    }
                }
            }

            int nextNumber = maxNumber + 1;

            return prefix + nextNumber.ToString("D4");
        }
    }
}
