using Microsoft.AspNetCore.Mvc;

namespace Obidalar.Controllers
{
    public class LoginController : Controller
    {
        [HttpGet]
        public IActionResult Index()
        {
            return View();
        }

        [HttpPost]
        public IActionResult Index(string username, string password)
        {
            if (username == "Hayot" && password == "3003")
            {
                HttpContext.Session.SetString("IsAdmin", "true");
                return RedirectToAction("Index", "Obida");
            }
            else
            {
                ViewBag.ErrorMessage = "Noto‘g‘ri login yoki parol!";
                return View(); // Login sahifasini xatolik bilan qaytaradi
            }
        }

        public IActionResult Logout()
        {
            HttpContext.Session.Remove("IsAdmin");
            return RedirectToAction("Index", "Obida");
        }
    }

}
