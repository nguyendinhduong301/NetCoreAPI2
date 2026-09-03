namespace DemoMVC.Controllers
{

    using Microsoft.AspNetCore.Mvc;

    public class Lesson4Controller : Controller
    {
        public IActionResult Index()
        {
            return View();
        }

    }
}