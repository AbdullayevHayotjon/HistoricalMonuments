using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Obidalar.Data;
using Obidalar.Models;

namespace Obidalar.Controllers
{
    public class LoginController : Controller
    {
        private readonly ApplicationDbContext _context;
        public LoginController(ApplicationDbContext context)
        {
            _context = context;
        }

        [HttpGet]
        public IActionResult Register()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Register(string ism, string Email, string password)
        {
            if (await _context.Userlar.AnyAsync(u => u.Email == Email.Trim().ToLower()))
            {
                ViewBag.Error = "Bu email allaqachon ro'yxatdan o'tgan!";
                ViewBag.Ism = ism;
                ViewBag.Email = Email;
                return View(); // Register sahifasini xatolik bilan ko‘rsatadi
            }

            var user = new User
            {
                Ism = ism,
                Email = Email.Trim().ToLower(),
                Parol = password
            };

            _context.Userlar.Add(user);
            await _context.SaveChangesAsync();

            // Tizimga avtomatik kirish
            HttpContext.Session.SetString("UserId", user.Id.ToString());
            HttpContext.Session.SetString("UserName", user.Ism); // Ism saqlanadi
            HttpContext.Session.SetString("UserEmail", user.Email); // Email saqlanadi
            HttpContext.Session.SetString("UserRole", user.Role.ToString());

            return RedirectToAction("Index", "Obida");
        }

        [HttpGet]
        public IActionResult Login()
        {
            return View();
        }

        [HttpPost]
        public async Task<IActionResult> Login(string Email, string Password)
        {
            var user = await _context.Userlar
                .FirstOrDefaultAsync(u => u.Email == Email.Trim().ToLower() && u.Parol == Password);

            if (user == null)
            {
                ViewBag.Error = "Email yoki parol noto‘g‘ri!";
                ViewBag.Email = Email; // Emailni saqlab qoldik
                return View(); // Login.cshtml sahifasini xatolik bilan ko‘rsatadi
            }

            // Sessiyaga ma'lumotlarni saqlaymiz (tartibli holda)
            HttpContext.Session.SetString("UserId", user.Id.ToString());
            HttpContext.Session.SetString("UserName", user.Ism); // Ism saqlanadi
            HttpContext.Session.SetString("UserEmail", user.Email); // Email saqlanadi
            HttpContext.Session.SetString("UserRole", user.Role.ToString()); // Foydalanuvchi rolini string sifatida saqlaymiz

            if(user.Role == UserRole.Admin)
            {
                return RedirectToAction("Index", "Admin");
            }
            return RedirectToAction("Index", "Obida");
        }

        [HttpGet]
        public IActionResult Logout()
        {
            HttpContext.Session.Clear();
            return RedirectToAction("Index", "Obida");
        }

    }
}

