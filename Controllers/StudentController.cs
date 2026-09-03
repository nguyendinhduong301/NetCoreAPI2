namespace DemoMVC.Controllers
{
    using DemoMVC.Models;
    using Microsoft.AspNetCore.Mvc;

    public class StudentController : Controller
    {
        public IActionResult Index()
        {
            return View();
        }
        [HttpPost]
        public IActionResult Index(Student student)
        {
            String KetQua = "Xin chào " + student.Fullname + 
                            ", Địa chỉ: " + student.Address +
                            ", Trường: " + student.University + " !";
           
            ViewData["KetQua"] = KetQua;
            return View();
        }        
    }
}